using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class PlayerGameStateScript : NetworkBehaviour
{
    public GameObject winPanel;
    public GameObject instructionCanvas;
    public Text countdownText;

    public float startTimer = 10f;
    public bool startTimerIsActive = true;

    void Update()
    {
        if (startTimerIsActive)
            UpdateStartTime();
    }

    [Command]
    void CmdDisableInstructions(bool disabled)
    {
        RpcDisableInstructions(disabled);
    }
    
    [ClientRpc]
    public void RpcDisableInstructions(bool disabled)
    {
        if (isLocalPlayer)
            return;
        else
            DisableInstructions(disabled);
    }

    public void DisableInstructions(bool disable)
    {
        if (isLocalPlayer)
            CmdDisableInstructions(disable);

        Debug.Log("Client Disable Instructions");

        if (disable)
        {
            instructionCanvas.SetActive(false);
            CmdDisableInstructions(instructionCanvas);
        }
    }

    void UpdateStartTime()
    {
        startTimer -= Time.deltaTime;

        if(startTimer <= 0)
        {
            startTimerIsActive = false;
            DisableInstructions(true);
            GameManager.code.timerIsActive = true;
        }
        else
        {
            countdownText.text = ((int)startTimer).ToString();
        }
    }

    [ClientRpc]
    public void RpcEndGame(bool hasWinner)
    {
        Debug.Log("Client RPC EndGame");

        if (hasWinner)
        {
            winPanel.GetComponent<Image>().enabled = true;
            winPanel.GetComponentInChildren<Text>().enabled = true;
            StartCoroutine("Disconnect");
        }
    }

    IEnumerator Disconnect()
    {
        yield return new WaitForSeconds(3f);
        if (isServer)
        {
            LobbyManager.s_Singleton.StopHostClbk();
        }
        else if (isClient)
        {
            LobbyManager.s_Singleton.StopClientClbk();
        }
    }

    public override void OnStartClient()
    {
        instructionCanvas = GameObject.Find("SelfCanvas");
        countdownText = GameObject.Find("CountdownText").GetComponent<Text>();
    }
}
