using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SymbolType
{
    Cherry,
    Grape,
    Bar,
    Seven
}

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Reel[] reels;
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private float delayBetweenReels = 0.5f;
    [SerializeField] private int numberOfSpins = 30;

    private bool isSpinning = false;
    private Vector3[] initialSlotPosition;
    private Dictionary<SymbolType[], int> payoutTable = new Dictionary<SymbolType[], int>()
    {
        { new SymbolType[] {SymbolType.Cherry, SymbolType.Cherry, SymbolType.Seven}, 10 },
        { new SymbolType[] {SymbolType.Cherry, SymbolType.Cherry, SymbolType.Bar}, 10 },
        { new SymbolType[] {SymbolType.Cherry, SymbolType.Cherry, SymbolType.Cherry}, 30 },
        { new SymbolType[] {SymbolType.Grape, SymbolType.Grape, SymbolType.Seven}, 20 },
        { new SymbolType[] {SymbolType.Grape, SymbolType.Grape, SymbolType.Bar}, 20 },
        { new SymbolType[] {SymbolType.Grape, SymbolType.Grape, SymbolType.Cherry}, 20 },
        { new SymbolType[] {SymbolType.Bar, SymbolType.Bar, SymbolType.Bar}, 150 },
        { new SymbolType[] {SymbolType.Seven, SymbolType.Seven, SymbolType.Seven}, 250 },
    };

    private void Start()
    {
        initialSlotPosition = new Vector3[reels.Length];
        for (int i = 0; i < reels.Length; i++)
        {
            initialSlotPosition[i] = reels[i].transform.position;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning)
        {
            StartSpinning();
        }
    }

    private void StartSpinning()
    {
        isSpinning = true;

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

    private IEnumerator SpinReel(Reel reel, Vector3 targetPosition, int reelIndex)
    {
        if (reelIndex > 0)
        {
            yield return new WaitForSeconds(.75f * reelIndex);
        }

        float childCount = reel.transform.childCount;

        for (int i = 0; i < numberOfSpins; i++)
        {
            Vector3 startPosition = reel.transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < spinDuration)
            {
                float t = elapsedTime / spinDuration;

                if (reel.transform.position.y <= -1.6)
                {
                    reel.transform.position = new Vector3(reel.transform.position.x, .4f + childCount - 5f, reel.transform.position.z);
                    startPosition = reel.transform.position;
                }
                reel.transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.down, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (Mathf.Abs(reel.transform.position.y - targetPosition.y) < 1f && i >= numberOfSpins - 10)
            {
                break;
            }
        }

        float distance = Vector3.Distance(reel.transform.position, targetPosition);
        while (distance > 0.01f)
        {
            reel.transform.position = Vector3.MoveTowards(reel.transform.position, targetPosition, 3f * Time.deltaTime);
            distance = Vector3.Distance(reel.transform.position, targetPosition);
            yield return null;
        }
        reel.transform.position = targetPosition;
        Debug.Log("Finished Moving");

        StartCoroutine(CheckPayout());
    }

    private IEnumerator CheckPayout()
    {
        yield return new WaitForSeconds(1f);
        isSpinning = false;
    }
}