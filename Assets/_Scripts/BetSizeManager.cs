using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetSizeManager : MonoBehaviour
{
    [SerializeField] private PlayerStatManager playerStats;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private Sprite unclickedSprite;

    private float[] betSizeValues = { .01f, .10f, .25f, .50f, 1, 5, 10, 25, 50, 100 };
    private int currentIndex = 4;

    private void Start()
    {
        UpdateValue();
        increaseButton.onClick.AddListener(IncreaseValue);
        decreaseButton.onClick.AddListener(DecreaseValue);
    }

    private void IncreaseValue()
    {
        currentIndex++;
        StartCoroutine(ChangeButtonSprite(increaseButton));
    }

    private void DecreaseValue()
    {
        currentIndex--;
        StartCoroutine(ChangeButtonSprite(decreaseButton));
    }

    private IEnumerator ChangeButtonSprite(Button btn)
    {
        playerStats.BetSize = betSizeValues[currentIndex];
        btn.image.sprite = clickedSprite;
        btn.interactable = false;
        yield return new WaitForSeconds(0.5f);

        btn.image.sprite = unclickedSprite;
        btn.interactable = true;
        UpdateValue();
    }
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
