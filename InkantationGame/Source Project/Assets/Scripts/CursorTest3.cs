using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;

public class CursorTest3 : MonoBehaviour
{
    [DllImport("Dll1", EntryPoint = "?SetMousePos@@YA_NHH@Z")]
    public static extern bool SetMousePos(int x, int y);

    [DllImport("Dll1", EntryPoint = "?GetMousePosX@@YAHXZ")]
    public static extern int GetMousePosX();

    [DllImport("Dll1", EntryPoint = "?GetMousePosY@@YAHXZ")]
    public static extern int GetMousePosY();


    bool threadRunning;
    Thread thread;


    [SerializeField] private int numPredictions = 5;   //Number of frames between cursor updates
    [SerializeField] private float lerpSpeed = 1.0f;

    [SerializeField] private Vector2[] pastPositions;
    [SerializeField] private Vector2 newPos;
    public Vector2 targetPos;

    float dT = 0.0f;

    void Start()
    {
        targetPos = new Vector2();
        pastPositions = new Vector2[numPredictions];
        StartCoroutine(stabilizeCursor());

        //thread = new Thread(stabilizeCursor);
        //thread.Start();
    }

    void Update()
    {
        dT = Time.deltaTime;
    }

    IEnumerator stabilizeCursor()
    {
        threadRunning = true;

        while (threadRunning && Application.IsPlaying(this))
        {
            Vector2 currentPos = new Vector2(GetMousePosX(), GetMousePosY());
            Vector2 averageFuturePos = new Vector2();

            addNewPosition(currentPos);

            newPos = pastPositions[0];
            averageFuturePos = getAverage();

            targetPos = Vector2.Lerp(newPos, averageFuturePos, lerpSpeed * dT);

            SetMousePos((int)targetPos.x, (int)targetPos.y);
            yield return null;

        }

        threadRunning = false;
    }

    private void OnDisable()
    {
        if(threadRunning && thread != null)
        {
            threadRunning = false;

            thread.Join();
        }
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

        for (int i = 1; i < pastPositions.Length; i++)
        {
            total += pastPositions[i];
        }

        return total / (pastPositions.Length - 1);
    }
}
