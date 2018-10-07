using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerTimer : NetworkBehaviour
{
    Text minutesText;
    Text secondsText;
	
	void Start ()
    {
    }

    [ClientRpc]
    public void RpcUpdateClientTimer(float time)
    {
        minutesText.text = ((int)(time / 60)).ToString();

        if(time >= 10f)
        {
            secondsText.text = ((int)(time % 60)).ToString();
        }
        else
        {
            secondsText.text = "0" + ((int)(time % 60)).ToString();
        }
    }

    public override void OnStartClient()
    {
        minutesText = GameObject.Find("MinutesText").GetComponent<Text>();
        secondsText = GameObject.Find("SecondsText").GetComponent<Text>();
    }

}
