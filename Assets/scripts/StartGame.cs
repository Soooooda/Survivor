using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;

public class StartGame : MonoBehaviour {

    [SerializeField]
    Button m_startGame;
    [SerializeField]
    Button m_endGame;
    // Use this for initialization
    string m_receiveMessage = "wait...";

    void Start ()
    {
		m_startGame.onClick.AddListener(startGame);
        m_endGame.onClick.AddListener(endGame);
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;
    }

    IEnumerator FadeScene()
    {
        Debug.Log("inside coroutine");
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        if(ConstantData.userID == "")
            SceneManager.LoadScene("Login");
        else
            SceneManager.LoadScene("Room");
    }

    void startGame()
    {

        StartCoroutine(FadeScene());
    }


    void SocketDisconnect()
    {
        ClientSocket.instance.Disconnect();
        m_receiveMessage = "已断开连接";
    }

    void endGame()
    {
        if(ClientSocket.instance.isConnected &&ConstantData.userID!="")
        {
            Debug.Log("User: " + ConstantData.userID + " Log out.");
            string content = "sign#out#" + ConstantData.userID;
            ClientSocket.instance.SendMessage(content);
            ConstantData.ResetAllData();
        }
        //if(ConstantData.userID!="")
        //{

        //}
    }

    // Update is called once per frame
    void Update () {
        if(m_receiveMessage == "true")
        {
            SocketDisconnect();
            Application.Quit();
            //收到user unlogged的消息再disconnect，然后application.quit();
        }
    }

    //展示有没有连上
    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;
    }

    void userLogOut()
    {

    }
}
