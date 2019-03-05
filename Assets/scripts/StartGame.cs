using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;

public class StartGame : MonoBehaviour {

    [SerializeField]
    Button m_startGame;

    // Use this for initialization
    void Start ()
    {
		m_startGame.onClick.AddListener(UserStart);
    }

    IEnumerator FadeScene()
    {
        Debug.Log("inside coroutine");
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Login");
    }

    void UserStart()
    {
        StartCoroutine(FadeScene());
    }

    // Update is called once per frame
    void Update () {
        
    }
}
