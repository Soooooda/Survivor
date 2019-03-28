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


    //选择房间后输入密码
    [SerializeField]
    Button m_EnterRoom;
    [SerializeField]
    Button m_CancelRoom;
    [SerializeField]
    InputField m_InputRoomPwd;


    public GameObject Rooms;
    public GameObject CreatedRoom;
    private bool changeMyRoomListFlag;
    public GameObject myRoomsPanel;
    private bool changetoRoomPanel;

    string m_receiveMessage = "wait...";

    void Start()
    {
        changeMyRoomListFlag = true;
        changetoRoomPanel = true;
        //findMyRooms();
        //ClientSocket.instance.ConnectServer("188.131.178.149", 8078);
        //m_connectBtn.onClick.AddListener(SocketConnect);
        //m_sendBtn.onClick.AddListener(SocketSendMessage);
        m_CreateRoomBtn.onClick.AddListener(CreateRoom);
        m_SearchRoomBtn.onClick.AddListener(findMyRooms);//findMyRooms()SearchRoom
        //m_disconnectBtn.onClick.AddListener(SocketDisconnect);

        //m_LoginBtn.onClick.AddListener(UserLogin);
        //m_RegisterBtn.onClick.AddListener(UserRegister);
        m_EnterRoom.onClick.AddListener(CheckPwd);
        m_CancelRoom.onClick.AddListener(CancelPad);

        //似乎是个代理。。反正这句话可以用来收消息
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;


    }

    IEnumerator FadeScene(string Scene)
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(Scene);
    }

    void CreateRoom()
    {
        string roomname = m_RoomName.text;
        string roompwd = m_RoomPwd.text;
        string userid = ConstantData.userID;//name+id+pwd+info string status 0/1/2 opt 0/1
        string content = "room#create#" + roomname + "#" +userid +"#"+ roompwd+"############";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);
            //if (m_receiveMessage == "user logged")
                //StartCoroutine(FadeScene());
        }
    }

    //输入房间密码后验证
    void CheckPwd()
    {
        Debug.Log("BeginCheck");
        string content = "room#enter#"+ ConstantData.roomName+"#"+ConstantData.roomID+"#"+ConstantData.userID+"#"+m_InputRoomPwd.text+"#####";
        Debug.Log(content);

        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);
        }
        //发送消息验证房间
    }
    
    //取消输入房间密码
    void CancelPad()
    {
        GameObject.Find("PwdEnterBar").transform.position = new Vector3(10000f, 0f, 0f);
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
            Debug.Log(m_receiveMessage);
            //if (m_receiveMessage == "user logged")
            //StartCoroutine(FadeScene());
        }
    }

    void findMyRooms()
    {
        changeMyRoomListFlag = true;
        string roomnum = m_RoomNum.text;
        string userid = ConstantData.userID;
        string content = "room#list#" + userid;
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
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

    void ChangePage()
    {

    }

    //展示文字
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {


        if (myRoomsPanel.activeInHierarchy == true&& changetoRoomPanel == true)
        {
            changetoRoomPanel = false;
            findMyRooms();
        }

        if (myRoomsPanel.activeInHierarchy == false)
            changetoRoomPanel = true;

        if (changeMyRoomListFlag == true && m_receiveMessage.Contains("#210#"))
        {
            changeMyRoomListFlag = false;
            Debug.Log(m_receiveMessage);
            if (m_receiveMessage.Contains("no room"))
                return;

            int childCount = Rooms.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {

                Destroy(Rooms.transform.GetChild(i).gameObject);

            }

            string[] ss = m_receiveMessage.Split(new string[] { "#" }, System.StringSplitOptions.None);

            Debug.Log(ss.Length);
            GameObject[] Myrooms = new GameObject[ss.Length-2];
            for (int i = 1; i < ss.Length-1; i++)
            {
                Debug.Log(ss[i]);
                string[] sss = ss[i].Split(' ');
                if (sss.Length > 1)
                {
                    string roomid = sss[0];
                    string roomname = sss[1];
                    string roomOwner = sss[2];
                    CreatedRoom.transform.Find("RoomName").GetComponent<UnityEngine.UI.Text>().text = roomid;
                    CreatedRoom.transform.Find("RoomID").GetComponent<UnityEngine.UI.Text>().text = roomname;
                    Myrooms[i-1] = Instantiate(CreatedRoom, Rooms.transform) as GameObject;
                    //RoomControl roomAttribute = Myrooms[i-1].transform.GetComponent<RoomControl>();
                    //roomAttribute.RoomID = roomid;
                    //roomAttribute.RoomName = roomname;

                }

            }
        }
        //if (m_receiveMessage == "user logged")
        //StartCoroutine(FadeScene());
        //如果成功登录房间
        if (m_receiveMessage.Contains("#220#"))
        {
            m_receiveMessage = "";
            Debug.Log("进入选择角色啦！");
            StartCoroutine(FadeScene("ChooseCharacter"));
        }


    }


}
