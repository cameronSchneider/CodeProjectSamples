using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Names : NetworkBehaviour
{
    static public Names namesHolder;
    public List<string> allNames;

    [SyncVar(hook = "OnWinnerName")]
    public string winnerName = "";
	
	void Awake ()
    {
        namesHolder = this;
        DontDestroyOnLoad(this);
	}

    public void DeclareWinner(string name)
    {
        foreach(string nm in allNames)
        {
            if(nm == name)
            {
                winnerName = nm;
            }
        }
    }

    public void OnWinnerName(string name)
    {
        winnerName = name;
    }
}
