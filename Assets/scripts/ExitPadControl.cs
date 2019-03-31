using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;

public class ExitPadControl : MonoBehaviour {

    string m_receiveMessage = "wait...";
    bool showPadChange = false;
    Button Cancel;
    Button Return2Menu;
    Button Return2Room;
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
        Cancel = pad.transform.Find("Cancel").GetComponent<Button>();
        Return2Menu = pad.transform.Find("Return2Menu").GetComponent<Button>();
        Return2Room = pad.transform.Find("Return2Room").GetComponent<Button>();
        Cancel.onClick.AddListener(returnToGame);
        Return2Menu.onClick.AddListener(returnToHome);
        Return2Room.onClick.AddListener(returnToRoom);
        pad.transform.position = new Vector3(10000f, 0f, 0f);
    }

    void returnToGame()
    {
        showPadChange = true;
    }

    void returnToHome()
    {
        StartCoroutine(FadeScene("start"));
    }

    void returnToRoom()
    {
        //room name roomid userid 250退出成功 251退出失败
        string content = "room#exit#" + ConstantData.roomName + "#" + ConstantData.roomID + "#" + ConstantData.userID + "#####";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);
        }
        //发送消息验证房间
    }

    IEnumerator FadeScene(string scene)
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene);
        
    }

    // Update is called once per frame
    void Update () {

        //StartCoroutine(FadeScene("Room"));
        if (m_receiveMessage.Contains("#250#"))
        {
            StartCoroutine(FadeScene("Room"));
        }

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
