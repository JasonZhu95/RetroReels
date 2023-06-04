using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatManager : MonoBehaviour
{
    public int CurrentBalance { get; set; } = 500;

    [SerializeField] private TextMeshProUGUI balanceText;

    private void Update()
    {
        balanceText.text = "$" + CurrentBalance.ToString();
    }
}
