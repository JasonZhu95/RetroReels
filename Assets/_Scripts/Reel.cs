using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ----------------------------------------------------------------------------
 * Class: Reel
 * Description: The gameobject that will be spun by the slot machine.
 * ---------------------------------------------------------------------------- */
public class Reel : MonoBehaviour
{
    public int numberOfSymbolsOnWheel;              // Number of total symbols on reel
    public SymbolType[] symbolArray;                // Create an array based on the number of symbols
    public SymbolType currentSymbol { get; set; }   // The current Symbol to be set while spinning

    /* ------------------------------------------------------------------------
    * Function: SetCurrentSymbol
    * Description: Access the symbol array, and set a symbol to be output
    * when the player spins the reel.
    * ---------------------------------------------------------------------- */
    public void SetCurrentSymbol(int symbolIndex)
    {
        currentSymbol = symbolArray[symbolIndex];
    }
}
