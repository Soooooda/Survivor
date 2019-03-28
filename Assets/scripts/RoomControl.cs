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

    }

    void EnterRoom()
    {
        Debug.Log("in enter room");
        ConstantData.roomID = RoomID;
        ConstantData.roomName = RoomName;
        ConstantData.roomOwner = RoomOwner;

        Debug.Log(transform.parent.gameObject.name);
        if (transform.parent.gameObject.name== "GridMyRooms")
        {
            Debug.Log("进入选择角色啦！");
            StartCoroutine(FadeScene("ChooseCharacter"));
        }
        else
            PwdBar.transform.position = PwdBarPos;
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
    //void Update () {

    //   }
}
