using System.Collections;
using UnityEngine;

public class SceneIni : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        if (Camera.main != null)
        {
            Camera.main.allowHDR = true;
        }
    }
}
