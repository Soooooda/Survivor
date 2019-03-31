using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;
public class RoomControl : MonoBehaviour {

    public string RoomName = "";
    public string RoomID = "";
    public string RoomOwner = "";
    GameObject PwdBar;
    Vector3 PwdBarPos;
    string m_receiveMessage = "wait...";

    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;
    }

    void Start () {
        //PwdBar = ToggleChoice.transform.Find("PwdEnterBar");//.transform.Find("PwdEnterBar");
        PwdBar = GameObject.Find("PwdEnterBar");
        PwdBarPos = PwdBar.transform.position;
        PwdBar.transform.position = new Vector3(10000f, 0f, 0f);
        Button Enter = gameObject.transform.Find("EnterRoomButton").GetComponent<Button>();
        Enter.onClick.AddListener(EnterRoom);
        RoomName = transform.Find("RoomName").GetComponent<UnityEngine.UI.Text>().text;
        RoomID = transform.Find("RoomID").GetComponent<UnityEngine.UI.Text>().text;
        Debug.Log(RoomName + " start!");
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;

    }

    void EnterRoom()
    {
        Debug.Log("in enter room");
        ConstantData.roomID = RoomID;
        ConstantData.roomName = RoomName;
        ConstantData.roomOwner = RoomOwner;

        Debug.Log(transform.parent.gameObject.name);
        if (transform.parent.gameObject.name== "GridMyRooms")//再判断一下房主是不是自己
        {
            roomEnterWithoutPwd();
        }
        else
            PwdBar.transform.position = PwdBarPos;
    }

    void roomEnterWithoutPwd()
    {
        Debug.Log("Enter room without password");
        string content = "room#enter2#" + ConstantData.roomName + "#" + ConstantData.roomID + "#" + ConstantData.userID + "#####";
        Debug.Log(content);

        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);
        }
        //发送消息验证房间

    }

    IEnumerator FadeScene(string Scene)
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(Scene);
    }

    //void CheckPwd()
    //{
    //    InputField pwd = gameObject.transform.Find("EnterPwd").GetComponent<InputField>();

    //}

    //// Update is called once per frame
    void Update()
    {
        if(m_receiveMessage.Contains("#230#"))
        {
            Debug.Log("进入选择角色啦！");
            StartCoroutine(FadeScene("ChooseCharacter"));
        }
    }


}
