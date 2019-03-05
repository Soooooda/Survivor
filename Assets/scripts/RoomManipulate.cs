using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;

public class RoomManipulate: MonoBehaviour {

    [SerializeField]
    Button m_disconnectBtn;


    [SerializeField]
    InputField m_RoomName;
    [SerializeField]
    InputField m_RoomPwd;
    [SerializeField]
    Button m_CreateRoomBtn;
    [SerializeField]
    Button m_SearchRoomBtn;
    [SerializeField]
    InputField m_RoomNum;


    string m_receiveMessage = "wait...";

    void Start()
    {
        //ClientSocket.instance.ConnectServer("188.131.178.149", 8078);
        //m_connectBtn.onClick.AddListener(SocketConnect);
        //m_sendBtn.onClick.AddListener(SocketSendMessage);
        m_CreateRoomBtn.onClick.AddListener(CreateRoom);
        m_SearchRoomBtn.onClick.AddListener(SearchRoom);
        //m_disconnectBtn.onClick.AddListener(SocketDisconnect);

        //m_LoginBtn.onClick.AddListener(UserLogin);
        //m_RegisterBtn.onClick.AddListener(UserRegister);

        //似乎是个代理。。反正这句话可以用来收消息
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("firstWorld");
    }

    void CreateRoom()
    {
        string roomname = m_RoomName.text;
        string roompwd = m_RoomPwd.text;
        string userid = ConstantData.userID;
        string content = "room#create#" + roomname + "#" +userid +"#"+ roompwd+"#5person";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
            Debug.Log(m_receiveMessage);
            //if (m_receiveMessage == "user logged")
                //StartCoroutine(FadeScene());
        }
    }

    void SearchRoom()
    {
        string roomnum = m_RoomNum.text;
        string userid = ConstantData.userID;
        string content = "room#list#" + userid;
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
            Debug.Log(m_receiveMessage);
            //if (m_receiveMessage == "user logged")
            //StartCoroutine(FadeScene());
        }
    }



    void SocketDisconnect()
    {
        ClientSocket.instance.Disconnect();
        m_receiveMessage = "已断开连接";
    }

    //展示有没有连上
    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;
    }

    //展示文字
    void Update()
    {
        //m_receiveText.text = m_receiveMessage;
    }


}
