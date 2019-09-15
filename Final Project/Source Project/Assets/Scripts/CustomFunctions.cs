using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Functions2DAndUI
{
    public class CustomFunctions : MonoBehaviour
    {
        public static void LookAt2D(Transform trans, Vector2 target)
        {
            Vector2 current = trans.position;
            Vector2 dir = target - current;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void LookAt2D(Transform trans, Vector2 orig, Vector2 target)
        {
            Vector2 dir = target - orig;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        public static void LoadSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
