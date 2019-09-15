using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CursorTest2 : MonoBehaviour
{
    [DllImport("Dll1", EntryPoint = "?SetMousePos@@YA_NHH@Z")]
    public static extern bool SetMousePos(int x, int y);

    [DllImport("Dll1", EntryPoint = "?GetMousePosX@@YAHXZ")]
    public static extern int GetMousePosX();

    [DllImport("Dll1", EntryPoint = "?GetMousePosY@@YAHXZ")]
    public static extern int GetMousePosY();

    [SerializeField] private int numPredictions = 5;   //Number of frames between cursor updates

    [Range(0, 1)]
    [SerializeField] private float lerpSpeed = 1.0f;

    [SerializeField] private Vector2[] pastPositions;
    [SerializeField] private Vector2 newPos;

    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        pastPositions = new Vector2[numPredictions];
    }


    void FixedUpdate()
    {
        Vector2 currentPos = new Vector2(GetMousePosX(), GetMousePosY());
        Vector2 averageFuturePos = new Vector2();
        Vector2 targetPos = new Vector2();

        addNewPosition(currentPos);

        newPos = pastPositions[0];
        averageFuturePos = getAverage();

        targetPos = Vector2.LerpUnclamped(newPos, averageFuturePos, lerpSpeed);

        SetMousePos((int)targetPos.x, (int)targetPos.y);
    }

    void addNewPosition(Vector2 newPos)
    {
        for (int i = 1; i < pastPositions.Length; i++)
        {
            pastPositions[i - 1] = pastPositions[i];
        }

        pastPositions[pastPositions.Length - 1] = newPos;
    }

    Vector2 getAverage()
    {
        Vector2 total = new Vector2();

        for(int i = 1; i < pastPositions.Length; i++)
        {
            total += pastPositions[i];
        }

        return total / (pastPositions.Length - 1);
    }
}
