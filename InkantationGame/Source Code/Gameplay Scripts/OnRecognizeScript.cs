using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GestureRecognizer;
using System.Linq;

public class OnRecognizeScript : MonoBehaviour
{
    private Vector3 spawnOffset;

    private GameObject player;
    private GameObject malPrefab;
    private GameObject kekPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnOffset = new Vector3();

        // Find demon prefabs
        malPrefab = Resources.Load("Prefabs/World_Kit/Demons/Placeholder_Mal") as GameObject;
        kekPrefab = Resources.Load("Prefabs/World_Kit/Demons/Placeholder_Kek") as GameObject;
    }

    public void OnRecognize(RecognitionResult result, DrawDetector detector)
    {
        if (result != RecognitionResult.Empty)
        {
            StartCoroutine(DeleteCoroutine(detector));

            spawnOffset = transform.forward * 0.1f;

            Debug.Log(result.gesture.id + "\n" + Mathf.RoundToInt(result.score.score * 100) + "%");


            if (result.gesture.id == "Glyph1")
            {
                Instantiate(kekPrefab, transform.position + spawnOffset, Quaternion.identity);
            }
            else
            {
                player.GetComponent<PlayerScript>().GetDamageBuff();
                Instantiate(malPrefab, transform.position + spawnOffset, Quaternion.identity);
            }
        }
        else
        {
            detector.ClearLines();
            Debug.Log("? ? ?");
        }
    }

    IEnumerator DeleteCoroutine(DrawDetector det)
    {
        yield return new WaitForSeconds(1.0f);
        det.ClearLines();
    }
}