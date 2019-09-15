using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CursorScript : MonoBehaviour
{
    [DllImport("Dll1", EntryPoint = "?SetMousePos@@YA_NHH@Z")]
    public static extern bool SetMousePos(int x, int y);

    [DllImport("Dll1", EntryPoint = "?GetMousePosX@@YAHXZ")]
    public static extern int GetMousePosX();

    [DllImport("Dll1", EntryPoint = "?GetMousePosY@@YAHXZ")]
    public static extern int GetMousePosY();

    public int updateFrequency = 5; //Number of frames between cursor updates
    public float smoothTime = 0.2f;
    [SerializeField] Vector2[] pastPositions;
    [SerializeField] Vector2 newPos;

    static int frameCount = 0;

    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        pastPositions = new Vector2[updateFrequency];
    }

    
    void Update()
    {
        frameCount++;

        Vector2 currentPos = new Vector2(GetMousePosX(), GetMousePosY());
        Vector2 averagedPos;

        addNewPosition(currentPos);

        averagedPos = getAveragePos();

        newPos = Vector2.SmoothDamp(currentPos, averagedPos, ref velocity, smoothTime);

        SetMousePos((int)newPos.x, (int)newPos.y);
    }

    void addNewPosition(Vector2 newPos)
    {
        for (int i = 1; i < pastPositions.Length; i++)
        {
            pastPositions[i - 1] = pastPositions[i];
        }

        pastPositions[pastPositions.Length - 1] = newPos;
    }

    Vector3 getAveragePos()
    {
        Vector2 average = Vector2.zero;

        for (int i = 0; i < pastPositions.Length; i++)
        {
            average += pastPositions[i];
        }

        average /= updateFrequency;

        return average;
    }
}
