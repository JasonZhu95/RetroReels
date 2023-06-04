using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* ----------------------------------------------------------------------------
 * Class: PlayerStatManager
 * Description: Handles the player stats and updates the player canvas based
 * on those stats.
 * ---------------------------------------------------------------------------- */
public class PlayerStatManager : MonoBehaviour
{
    public float CurrentBalance { get; set; } = 500;        // The player's balance
    public float BetSize { get; set; } = 10;                // Current size of a bet

    [SerializeField] private TextMeshProUGUI balanceText;   // Text UI for player balance
    [SerializeField] private TextMeshProUGUI betSizeText;   // Text UI for player bet size
    [SerializeField] private TextMeshProUGUI payoutText;    // Text UI for won money
    [SerializeField] private Animator payoutAnimator;       // Animator for when money is won

    private void Update()
    {
        // Possible Improvement: Should be calling these in a function or
        // using an event action to update the values
        balanceText.text = "$" + CurrentBalance.ToString();
        betSizeText.text = "Bet Size\n$" + BetSize.ToString();
    }

    /* ------------------------------------------------------------------------
    * Function: WinMoney
    * Description: Takes in an amount and adds it the player balance.
    * ---------------------------------------------------------------------- */
    public void WinMoney(float amount)
    {
        // Play payout sound
        SoundManager.instance.Play("Payout");

        // Multiply won amount with the betsize and add to balance
        float payoutValue = amount * BetSize;
        CurrentBalance += payoutValue;

        // Update text and animate payout
        payoutText.text = "+$" + payoutValue;
        payoutAnimator.SetBool("start", true);
    }

    /* ------------------------------------------------------------------------
    * Function: ResetBalanceOnGameOver
    * Description: When the player loses, reset balance to inital amount
    * ---------------------------------------------------------------------- */
    public void ResetBalanceOnGameOver()
    {
        CurrentBalance = 500;
    }

    /* ------------------------------------------------------------------------
    * Function: PlaceWager
    * Description: Based on betsize, reduce the player balance when called
    * ---------------------------------------------------------------------- */
    public void PlaceWager()
    {
        CurrentBalance -= BetSize;
    }
}
