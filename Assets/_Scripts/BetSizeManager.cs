using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* ----------------------------------------------------------------------------
 * Class: BetSizeManager
 * Description: Class responsible for changing the player bet size on button
 * clicks.
 * ---------------------------------------------------------------------------- */
public class BetSizeManager : MonoBehaviour
{
    [SerializeField] private PlayerStatManager playerStats;     // Player balance reference
    [SerializeField] private Button increaseButton;             // Button to increase wager
    [SerializeField] private Button decreaseButton;             // Button to decrease wager
    [SerializeField] private Sprite clickedSprite;              // Sprite to show when button can NOT be clicked
    [SerializeField] private Sprite unclickedSprite;            // Sprite to show when button is clickable

    private float[] betSizeValues = {1, 5, 10, 25, 50, 100, 300, 500 }; //Different available bet sizes
    private int currentIndex = 2;   // Starting bet size (10)

    private void Start()
    {
        // Add functionality to buttons through script
        playerStats.BetSize = betSizeValues[currentIndex];
        increaseButton.onClick.AddListener(IncreaseValue);
        decreaseButton.onClick.AddListener(DecreaseValue);
    }

    /* ------------------------------------------------------------------------
    * Function: IncreaseValue
    * Description: Increases the index of the bet size array and increases
    * player bet size
    * ---------------------------------------------------------------------- */
    private void IncreaseValue()
    {
        currentIndex++;
        StartCoroutine(ChangeButtonSprite(increaseButton));
    }

    /* ------------------------------------------------------------------------
    * Function: IncreaseValue
    * Description: Decreases the index of the bet size array and decreases
    * player bet size
    * ---------------------------------------------------------------------- */
    private void DecreaseValue()
    {
        currentIndex--;
        StartCoroutine(ChangeButtonSprite(decreaseButton));
    }

    /* ------------------------------------------------------------------------
    * Coroutine: ChangeButtonSprite
    * Description: Based on which button is clicked, handles the button
    * functionality
    * ---------------------------------------------------------------------- */
    private IEnumerator ChangeButtonSprite(Button btn)
    {
        // Play button clicking sound
        SoundManager.instance.Play("UIClick");

        // Update the player bet size
        playerStats.BetSize = betSizeValues[currentIndex];

        // Turn the buton on and off after .5 seconds to emulate a button press
        btn.image.sprite = clickedSprite;
        btn.interactable = false;
        yield return new WaitForSeconds(0.5f);
        btn.image.sprite = unclickedSprite;
        btn.interactable = true;

        UpdateValue();
    }

    /* ------------------------------------------------------------------------
    * Function: UpdateValue
    * Description: Check if a button is at the ends of the betsize array.
    * Turn those buttons to NOT interactable so the player can't click it
    * ---------------------------------------------------------------------- */
    private void UpdateValue()
    {
        increaseButton.interactable = (currentIndex < betSizeValues.Length - 1);
        decreaseButton.interactable = (currentIndex > 0);
        if (!increaseButton.interactable)
        {
            increaseButton.image.sprite = clickedSprite;
        }
        else
        {
            increaseButton.image.sprite = unclickedSprite;
        }

        if (!decreaseButton.interactable)
        {
            decreaseButton.image.sprite = clickedSprite;
        }
        else
        {
            decreaseButton.image.sprite = unclickedSprite;
        }
    }
}
