using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;


public class EnterGame : MonoBehaviour {

    string m_receiveMessage = "wait...";

    // Use this for initialization
    void Start () {
        Button b = gameObject.GetComponent<Button>();
		b.onClick.AddListener(EnterTheGame);
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;
    }

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("firstWorld");
    }

    void EnterTheGame()//如果有角色了就创建，创建角色成功以后openmap
    {
        if (ConstantData.hasRole == true)
        {
            OpenMap();
        }
        else CreateRole();
    }

    void CreateRole()
    {
        //role#create#test4#836226#test4@163.com#xia2#1
        string roomID = ConstantData.roomID;
        string userid = ConstantData.userID;
        string roomName = ConstantData.roomName;
        string content = "role#create#" + roomName + "#" + roomID + "#" + userid + "#"+ ConstantData.role +"##########";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);

        }
    }

    void OpenMap()
    {
        //map#open#836226#test4@163.com
        string roomID = ConstantData.roomID;
        string userid = ConstantData.userID;
        string content = "map#open#" + roomID + "#" + userid + "############";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);
            
        }

    }

    void createMap()
    {
        string roomID = ConstantData.roomID;
        string userid = ConstantData.userID;
        string roomName = ConstantData.roomName;
        string content = "map#create#" + roomID + "#" + userid + "#"+ roomName + "#0#0#########";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);

        }
    }

    void getMap()
    {
        string roomID = ConstantData.roomID;
        string userid = ConstantData.userID;
        string roomName = ConstantData.roomName;
        string content = "map#get#" + userid + "#" + roomID + "#" + "#4#3#1#1#######";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);

        }
    }
	
	// Update is called once per frame
	void Update () {
        if (m_receiveMessage == "412")//创建的地图
        {
            createMap();
            m_receiveMessage = "";//清空消息

        }
        else if(m_receiveMessage == "410")//加载地图
        {

            m_receiveMessage = "";        
            getMap();
        }
        else if (m_receiveMessage == "400")//创建地图成功
        {

            m_receiveMessage = "";
            getMap();
        }
        else if (m_receiveMessage == "420")//获取地图成功
        {
            Debug.Log(m_receiveMessage);
            m_receiveMessage = "";
            StartCoroutine(FadeScene());
        }
        else if (m_receiveMessage == "310")//创建角色成功
        {
            Debug.Log(m_receiveMessage);
            m_receiveMessage = "";
            OpenMap();
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
}
