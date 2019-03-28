using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRooms : MonoBehaviour, IBeginDragHandler,IEndDragHandler {

    private ScrollRect scrollRect;
    private float[] pageArray = new float[] { 0, 0.3333333f, 0.44444f, 1 };
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        //throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float posX = scrollRect.horizontalNormalizedPosition;
        Debug.Log(posX);
        int index = 0;
        float offset = Math.Abs(pageArray[index] - posX);
        for(int i = 1;i<pageArray.Length;i++)
        {
            float offsetTemp = Math.Abs(pageArray[i] - posX);
            if(offsetTemp<offset)
            {
                index = i;
                offset = offsetTemp;
            }
        }
        scrollRect.horizontalNormalizedPosition = pageArray[index];

        //throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        scrollRect = GetComponent<ScrollRect>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
