using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatManager : MonoBehaviour
{
    public float CurrentBalance { get; set; } = 500;
    public float BetSize { get; set; } = 10;

    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI betSizeText;
    [SerializeField] private TextMeshProUGUI payoutText;
    [SerializeField] private Animator payoutAnimator;

    private void Update()
    {
        balanceText.text = "$" + CurrentBalance.ToString();
        betSizeText.text = "Bet Size\n$" + BetSize.ToString();
    }

    public void WinMoney(float amount)
    {
        SoundManager.instance.Play("Payout");
        float payoutValue = amount * BetSize;
        CurrentBalance += payoutValue;
        payoutText.text = "+$" + payoutValue;
        payoutAnimator.SetBool("start", true);
    }

    public void ResetBalanceOnGameOver()
    {
        CurrentBalance = 500;
    }

    public void PlaceWager()
    {
        CurrentBalance -= BetSize;
    }
}
