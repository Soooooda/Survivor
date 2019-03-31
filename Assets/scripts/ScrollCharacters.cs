using System;
using System.Collections;
using System.Collections.Generic;
using Tool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollCharacters : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    private ScrollRect scrollRect;
    private float[] pageArray = new float[] { 0, 0.3333f, 0.6666f, 1 };
    string m_receiveMessage = "wait...";

    public void OnBeginDrag(PointerEventData eventData)
    {


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ConstantData.hasRole == false)
        {
            float posX = scrollRect.horizontalNormalizedPosition;
            Debug.Log(posX);
            int index = 0;
            float offset = Mathf.Abs(pageArray[index] - posX);
            for (int i = 1; i < pageArray.Length; i++)
            {
                float offsetTemp = Mathf.Abs(pageArray[i] - posX);
                if (offsetTemp < offset)
                { offset = offsetTemp; index = i; }
            }
            scrollRect.horizontalNormalizedPosition = pageArray[index];
            ConstantData.role = index;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = pageArray[ConstantData.role];
        }
    }

    // Use this for initialization
    void Start () {
        scrollRect = GetComponent<ScrollRect>();
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;
        roleExistOrNot();
    }

    void roleExistOrNot()
    {
        string roomid = ConstantData.roomID;
        string userid = ConstantData.userID;
        string content = "role#show#" + roomid +"#"+ userid+"#####";
        Debug.Log(content);
        if (!string.IsNullOrEmpty(content))
        {
            ClientSocket.instance.SendMessage(content);
            Debug.Log(m_receiveMessage);
        }
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(ConstantData.role);
        if (m_receiveMessage.Contains("#320#"))
        {
            string[] temp = m_receiveMessage.Split('#');
            int role = int.Parse(temp[2]);
            ConstantData.role = role;
            ConstantData.hasRole = true;
            m_receiveMessage = "";
        }

    }
    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;
    }
}
