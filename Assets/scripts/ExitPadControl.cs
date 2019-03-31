using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;

public class ExitPadControl : MonoBehaviour {


    bool showPadChange = false;
    Button Cancel;
    Button Exit;
    Vector3 padPos;
    public static ExitPadControl instance;
    Image pad;

    void Awake()
    {
        if (instance != null) { return; }
        else
        {
            instance = this;           //避免场景加载时该对象销毁            
            DontDestroyOnLoad(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        Canvas canvas = gameObject.transform.Find("Canvas").GetComponent<Canvas>();
        pad = canvas.transform.Find("exitPad").GetComponent<Image>();
        padPos = pad.transform.position;
        Cancel = pad.transform.Find("Return").GetComponent<Button>();
        Exit = pad.transform.Find("Exit").GetComponent<Button>();
        Cancel.onClick.AddListener(returnToGame);
        Exit.onClick.AddListener(returnToHome);
        pad.transform.position = new Vector3(10000f, 0f, 0f);
    }

    void returnToGame()
    {
        showPadChange = true;
    }

    void returnToHome()
    {
        StartCoroutine(FadeScene());
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("start");
        
    }

    // Update is called once per frame
    void Update () {

        if (SceneManager.GetActiveScene().name != "start")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                showPadChange = true;
                //Debug.Log("Esc KeyDown");
            }

            if (showPadChange)
            {
                if (pad.transform.position.x > 5000f)
                    pad.transform.position = padPos;
                else
                    pad.transform.position = new Vector3(10000f, 0f, 0f);

                showPadChange = false;
            }

        }
        else
            pad.transform.position = new Vector3(10000f, 0f, 0f);
    }
}
