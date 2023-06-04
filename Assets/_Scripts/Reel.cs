using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public int numberOfSymbolsOnWheel;
    public SymbolType[] symbolArray;
    public SymbolType currentSymbol { get; set; }

    public void SetCurrentSymbol(int symbolIndex)
    {
        currentSymbol = symbolArray[symbolIndex];
    }
}
