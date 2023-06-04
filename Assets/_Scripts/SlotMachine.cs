using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ----------------------------------------------------------------------------
 * Enum: SymbolType
 * Description: The types of symbols that appear on the reel
 * ---------------------------------------------------------------------------- */
public enum SymbolType
{
    Cherry,
    Grape,
    Bar,
    Seven
}

/* ----------------------------------------------------------------------------
 * Class: SlotMachine
 * Description: Manages the main functionality of spinning the wheel, referencing
 * the payout table, and keeping track of game over.
 * ---------------------------------------------------------------------------- */
public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Reel[] reels;                      // Contains all the reels of the machine
    [SerializeField] private float spinSpeed = 3f;              // The speed of an individual spin
    [SerializeField] private int numberOfSpins = 30;            // How much the machine should spin
    [SerializeField] private PlayerStatManager playerStats;     // References the player balance
    [SerializeField] private GameObject handle;                 // Clickable handle to start spin
    [SerializeField] private GameObject handleInactive;         // Inactive handle to show while spinning
    [SerializeField] private GameObject gameOverCanvas;

    private bool isSpinning = false;
    private Vector3[] initialSlotPosition;

    // The potential combinations of payouts.
    private Dictionary<SymbolType[], int> payoutTable = new Dictionary<SymbolType[], int>()
    {
        { new SymbolType[] {SymbolType.Cherry, SymbolType.Cherry, SymbolType.Seven}, 1 },
        { new SymbolType[] {SymbolType.Cherry, SymbolType.Cherry, SymbolType.Bar}, 1 },
        { new SymbolType[] {SymbolType.Cherry, SymbolType.Cherry, SymbolType.Cherry}, 3 },
        { new SymbolType[] {SymbolType.Grape, SymbolType.Grape, SymbolType.Seven}, 2 },
        { new SymbolType[] {SymbolType.Grape, SymbolType.Grape, SymbolType.Bar}, 2 },
        { new SymbolType[] {SymbolType.Grape, SymbolType.Grape, SymbolType.Cherry}, 2 },
        { new SymbolType[] {SymbolType.Bar, SymbolType.Bar, SymbolType.Bar}, 15 },
        { new SymbolType[] {SymbolType.Seven, SymbolType.Seven, SymbolType.Seven}, 25 },
    };

    private void Start()
    {
        // Keep track of the initial position of the reels to reference when determining which
        // symbol to output
        initialSlotPosition = new Vector3[reels.Length];
        for (int i = 0; i < reels.Length; i++)
        {
            initialSlotPosition[i] = reels[i].transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the handle is clickable with a raycast
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject && !isSpinning && playerStats.CurrentBalance >= playerStats.BetSize)
            {
                playerStats.PlaceWager();
                StartSpinning();
            }
        }
    }

    /* ------------------------------------------------------------------------
    * Function: StartSpinning
    * Description: Determines which symbol should be output and begin spinning
    * the wheel.
    * ---------------------------------------------------------------------- */
    private void StartSpinning()
    {
        isSpinning = true;

        // Turn the handle off while the machine is spinning
        handle.SetActive(false);
        handleInactive.SetActive(true);

        // Play spinning sounds.
        SoundManager.instance.Play("PullLever");
        SoundManager.instance.Play("StartReel");

        // For each reel, determine which symbol to output and start spinning
        for (int i = 0; i < reels.Length; i++)
        {
            //Random index to stop the reel
            int randomIndex = Random.Range(0, reels[i].numberOfSymbolsOnWheel);

            // Set the reel symbol
            reels[i].SetCurrentSymbol(randomIndex);

            //Calculate target position to stop reel at
            Vector3 targetPosition = new Vector3(initialSlotPosition[i].x, initialSlotPosition[i].y - 1 + randomIndex, initialSlotPosition[i].z);
            
            //Start Spinning
            StartCoroutine(SpinReel(reels[i], targetPosition, i));
        }
    }

    /* ------------------------------------------------------------------------
    * Coroutine: SpinReel
    * Description: Given the reel, the target symbol position, and which reel
    * in the array it is.  Contains the logic to spin the wheel over time using
    * the Vector3.Lerp function.
    * ---------------------------------------------------------------------- */
    private IEnumerator SpinReel(Reel reel, Vector3 targetPosition, int reelIndex)
    {
        // Add delay based on the reelIndex
        if (reelIndex > 0)
        {
            yield return new WaitForSeconds(.25f * reelIndex);
        }

        // Keep the child count for the number of symbols in a wheel
        float childCount = reel.transform.childCount;

        // Spin the wheel the targeted number of times
        for (int i = 0; i < numberOfSpins; i++)
        {
            
            // Begin Lerping the reel position based off of the spinSpeed
            Vector3 startPosition = reel.transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < spinSpeed)
            {
                float t = elapsedTime / spinSpeed;

                // If the position of the reel is at an endpoint essentially loop the symbols back to the bottom.
                if (reel.transform.position.y <= -1.6f)
                {
                    reel.transform.position = new Vector3(reel.transform.position.x, .4f + childCount - 5f, reel.transform.position.z);
                    startPosition = reel.transform.position;
                }

                reel.transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.down, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // If the reel is close to the target symbol and has already spun
            // a certain number of times exit the loop
            if (Mathf.Abs(reel.transform.position.y - targetPosition.y) < 1f && i >= numberOfSpins - 10)
            {
                break;
            }
        }

        // Calculate the distance between the reel and target position
        float distance = Vector3.Distance(reel.transform.position, targetPosition);

        // Use the MoveTowards function to get an exact position because of floating point precision error while using Lerp
        while (distance > 0.01f)
        {
            reel.transform.position = Vector3.MoveTowards(reel.transform.position, targetPosition, 3f * Time.deltaTime);
            distance = Vector3.Distance(reel.transform.position, targetPosition);
            yield return null;
        }

        reel.transform.position = targetPosition;

        // Check the payout table when the final reel is completed spinning
        if (reel == reels[reels.Length - 1])
        {
            CheckPayout();
        }
    }

    /* ------------------------------------------------------------------------
    * Function: CheckPayout
    * Description: Check the hashtable of the payout combinations.  If a 
    * successful combination is found, then reference the playerStats and
    * increase the player's balance.
    * ---------------------------------------------------------------------- */
    private void CheckPayout()
    {
        SymbolType symbolReelOne = reels[0].currentSymbol;
        SymbolType symbolReelTwo = reels[1].currentSymbol;
        SymbolType symbolReelThree = reels[2].currentSymbol;

        // Search through the payout hashmap
        int payout = GetPayout(symbolReelOne, symbolReelTwo, symbolReelThree);

        // If the player won money
        if (payout > 0)
        {
            playerStats.WinMoney(payout);
        }
        
        // If the player loses and has no more money after the final spin
        if (payout <= 0 && playerStats.CurrentBalance == 0)
        {
            StartCoroutine(StartGameOver());
        }

        isSpinning = false;

        // Stop looping sound
        SoundManager.instance.StopPlay("StartReel");

        // Set the handle back to being clickable.
        handle.SetActive(true);
        handleInactive.SetActive(false);
    }

    /* ------------------------------------------------------------------------
    * Function: GetPayout
    * Description: Given the symbol combination, check if the combination 
    * exists in the payoutTable dictionary.
    * ---------------------------------------------------------------------- */
    private int GetPayout(SymbolType symbolOne, SymbolType symbolTwo, SymbolType symbolThree)
    {
        foreach (KeyValuePair<SymbolType[], int> kvp in payoutTable)
        {
            SymbolType[] combination = kvp.Key;
            int payout = kvp.Value;

            if (combination[0] == symbolOne && combination[1] == symbolTwo && combination[2] == symbolThree)
            {
                return payout;
            }
        }
        return 0;
    }

    /* ------------------------------------------------------------------------
    * Coroutine: StartGameOver
    * Description: After a second, Enable the game over canvas.
    * ---------------------------------------------------------------------- */
    private IEnumerator StartGameOver()
    {
        yield return new WaitForSeconds(1f);
        gameOverCanvas.SetActive(true);
    }
}