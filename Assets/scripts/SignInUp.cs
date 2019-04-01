using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tool;

public class SignInUp : MonoBehaviour {

    //[SerializeField]
    //Button m_connectBtn;
    //[SerializeField]
    //InputField m_input;
    //[SerializeField]
    //Button m_sendBtn;

    //[SerializeField]
    //Text m_receiveText;

    [SerializeField]
    Button m_LoginBtn;
    [SerializeField]
    InputField m_UserPassword;
    [SerializeField]
    InputField m_UserName;
    [SerializeField]
    Button m_RegisterBtn;


    [SerializeField]
    InputField m_Register_Email;
    [SerializeField]
    InputField m_Register_UserPassword;
    [SerializeField]
    InputField m_Register_UserName;
    private bool logInFlag;

    string m_receiveMessage = "wait...";


    void Start()
    {
        logInFlag = true;
        if(!ClientSocket.instance.isConnected)
            ClientSocket.instance.ConnectServer("10.136.88.189", 8078);// 10.136.2.134 188.131.178.149
        //m_connectBtn.onClick.AddListener(SocketConnect);
        //m_sendBtn.onClick.AddListener(SocketSendMessage);
        //m_disconnectBtn.onClick.AddListener(SocketDisconnect);

        m_LoginBtn.onClick.AddListener(UserLogin);
        m_RegisterBtn.onClick.AddListener(UserRegister);

        //似乎是个代理。。反正这句话可以用来收消息
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;
    }

    //void SocketConnect()
    //{
    //    ClientSocket.instance.ConnectServer("188.131.178.149", 8078);
    //}

    //void SocketSendMessage()
    //{
    //    string content = m_input.text;
    //    if (!string.IsNullOrEmpty(content))
    //    {
    //        ClientSocket.instance.SendMessage(content);
    //    }
    //}

    IEnumerator FadeScene()
    {
        float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Room");
    }
    void UserLogin()
    {
        //记得删掉
        ClientSocket.instance.SendMessage("sign#out#123@qq.com");
        string username = m_UserName.text;
        string userpassword = m_UserPassword.text;
        string content = "sign#in#"+username+"#"+userpassword;
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);


        }
    }

    void UserRegister()
    {
        string email = m_Register_Email.text;
        string username = m_Register_UserName.text;
        string userpassword = m_Register_UserPassword.text;
        string content = "sign#up#" + email + "#" + username + "#" + userpassword+"#test";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);

        }
    }




    //展示有没有连上
    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;
    }

    //展示文字
    void Update()
    {
        
        if (logInFlag==true&& m_receiveMessage.Contains("successfully"))
        {
            string username = m_UserName.text;
            logInFlag = false;
            Debug.Log(m_receiveMessage);
            if (m_receiveMessage.Contains("successfully"))
            {
                Debug.Log("进入下一个啦啊啦啦啦啦啦啦啦啦");
                ConstantData.userID = username;
                StartCoroutine(FadeScene());
            }
        }
    }


}
