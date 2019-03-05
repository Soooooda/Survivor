using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogButton : MonoBehaviour {

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * 0.01f, Screen.height * 0.01f, Screen.width * 0.1f, Screen.height * 0.1f), "play1"))
        {
            StartCoroutine(FadeScene());
            //Application.LoadLevel("");
            //SceneManager.LoadScene("firstWorld");
        }
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("firstWorld");
    }

}
