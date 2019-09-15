using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WinnerNamePlacer : NetworkBehaviour
{
    public Text winMessage;

    [SyncVar]
    public string winnerName;

	void Start ()
    {
        winnerName = Names.namesHolder.winnerName;
        if(winnerName == "")
        {
            winMessage.text = "Draw!";
        }
        else
        {
            winMessage.text = winnerName + " wins!";
        }
    }
}
