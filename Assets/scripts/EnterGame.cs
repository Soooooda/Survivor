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
    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;

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
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
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
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
            ClientSocket.instance.SendMessage(content);
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
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
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
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);

        }
    }

    void getMap()
    {
        string roomID = ConstantData.roomID;
        string userid = ConstantData.userID;
        string roomName = ConstantData.roomName;
        string content = "map#get#" + userid + "#" + roomID + "#4#3#1#1#######";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.onGetReceive = ShowReceiveMessage;
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);

        }
    }
	
	// Update is called once per frame
	void Update () {
        //ClientSocket.instance.onGetReceive = ShowReceiveMessage;
        //Debug.Log(m_receiveMessage);
        if (m_receiveMessage.Contains("#412#"))//需要创建地图
        {
            m_receiveMessage = "";//清空消息
            createMap();
            
        }
        if(m_receiveMessage.Contains("#410#"))//地图存在
        {
            m_receiveMessage = "";        
            getMap();
        }
        if (m_receiveMessage.Contains("#400#"))//创建地图成功
        {
            m_receiveMessage = "";
            getMap();
        }
        if (m_receiveMessage.Contains("#401#"))//create重复
        {
            m_receiveMessage = "";
            getMap();
        }
        if (m_receiveMessage.Contains("#420#"))//获取地图成功
        {
            Debug.Log(m_receiveMessage);
            m_receiveMessage = "";
            StartCoroutine(FadeScene());
        }
        if (m_receiveMessage.Contains("#310#"))//创建角色成功
        {
            Debug.Log("role begin to create");
            Debug.Log(m_receiveMessage);
            m_receiveMessage = "";
            OpenMap();
        }
        if (m_receiveMessage.Contains("#312#"))//角色已经存在
        {
            Debug.Log("role exists");
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

}
