using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public float gameTimer = 180f;
    public List<PlayerTimer> timers;
    public List<PlayerGameStateScript> playerGameStates;
    public List<PlayerMovement> movementScripts;
    static public GameManager code;

    public bool timerIsActive = false;

    void Awake()
    {
        code = this;
    }

    void Update ()
    {
        if(timerIsActive)
            UpdateTime();

        CheckPlayerSpeeds();
	}

    void UpdateTime()
    {
        gameTimer -= Time.deltaTime;

        if(gameTimer <= 0)
        {
            timerIsActive = false;
            EndGame("Draw!");
        }
        else
        {
            foreach(PlayerTimer t in timers)
            {
                t.RpcUpdateClientTimer(gameTimer);
            }
        }
    }

    public void AddPlayerTimer(PlayerTimer timer)
    {
        timers.Add(timer);
    }

    public void AddPlayerGameStates(PlayerGameStateScript state)
    {
        playerGameStates.Add(state);
    }

    public void AddPlayerMovement(PlayerMovement move)
    {
        movementScripts.Add(move);
    }

    void EndGame(string winnerName)
    {
        Names.namesHolder.DeclareWinner(winnerName);

        Debug.Log("Server_EndGame" + winnerName);

        foreach (PlayerGameStateScript g in playerGameStates)
        {
            g.RpcEndGame(true);
        }
    }

    void CheckPlayerSpeeds()
    {
        foreach(PlayerMovement m in movementScripts)
        {
            if(m.xSpeed == 0f)
            {
                foreach(PlayerMovement n in movementScripts)
                {
                    if(n.xSpeed != 0)
                    {
                        EndGame(n.gameObject.name);
                    }
                }
            }
        }
    }
}
