using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThingInBagControll : MonoBehaviour {

    bool followMouse;
    public GameObject afterCursor;//prefab
    GameObject AC;
    public GameObject targetObject;
    // Use this for initialization
    void Start () {

        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(takeOut);
        followMouse = false;

    }

    void takeOut()
    {
        followMouse = true;
        if(AC == null)
            AC = Instantiate(afterCursor, transform) as GameObject;
        Sprite pickedSprite = targetObject.GetComponent<SpriteRenderer>().sprite;
        AC.GetComponent<Image>().sprite = pickedSprite;
        Debug.Log("AC:" + AC);
        //transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

    }



    // Update is called once per frame
    void Update()
    {
        if (followMouse)
        {
            Debug.Log(AC.transform.position);
            Debug.Log(AC.GetComponent<RectTransform>().sizeDelta.x);
            AC.transform.position = new Vector3(Input.mousePosition.x + (AC.GetComponent<RectTransform>().sizeDelta.x / 2), Input.mousePosition.y - (AC.GetComponent<RectTransform>().sizeDelta.y / 2), 0);

            if (Input.GetMouseButtonUp(0))
            {
                //从摄像机发出到点击坐标的射线
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    //划出射线，只有在scene视图中才能看到
                    Debug.DrawLine(ray.origin, hitInfo.point);
                    GameObject gameObj = hitInfo.collider.gameObject;
                    Debug.Log("click object name is " + gameObj.name);
                    if (gameObj.name.Contains("floor"))
                    {
                        Vector3 PlacePos = hitInfo.point;
                        Debug.Log(PlacePos);
                        //当射线碰撞目标为boot类型的物品，执行拾取操作
                        //if (gameObj.tag == "boot")
                        //{
                        //    Debug.Log("pickup!");
                        //}
                        GameObject x = Instantiate(targetObject, PlacePos, Quaternion.Euler(45, 0, 0)) as GameObject;
                    }
                }
            }
        }
    }
}
