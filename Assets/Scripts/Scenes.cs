using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public static IEnumerator LoadSceneAsync(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
