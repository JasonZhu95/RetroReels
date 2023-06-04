using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public int numberOfSymbolsOnWheel;
    public SymbolType[] symbolArray;
    private SymbolType currentSymbol;

    public void SetCurrentSymbol(int symbolIndex)
    {
        currentSymbol = symbolArray[symbolIndex];
        Debug.Log(currentSymbol);
    }
}
