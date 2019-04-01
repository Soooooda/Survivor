using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThingInBagControll : MonoBehaviour {

    //int order;//记录在第几个
    GameObject ThisThing;//记录记下来的物品
    int count;//有多少个
    Sprite thingSprite;//放在包包里的样子
    bool followMouse;
    public GameObject afterCursor;//prefab
    GameObject AC;//跟在鼠标后面的真实物体
    //public GameObject targetObject;//放到地上的物体

    //得到一个物品
    public void CreateThing(GameObject thing)
    {
        setEnable();
        ThisThing = thing;
        count = 1;
        thingSprite = thing.transform.Find("PickableThing").gameObject.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Image>().sprite = thingSprite;
    }

    public void setEnable()
    {
        gameObject.GetComponent<Image>().enabled = true;
        transform.Find("num").gameObject.GetComponent<Text>().enabled = true;
    }
    public void setDisable()
    {
        gameObject.GetComponent<Image>().enabled = false;
        transform.Find("num").gameObject.GetComponent<Text>().enabled = false;
    }

    //增加一个物品
    public void addCount()
    {
        count++;
        transform.Find("num").gameObject.GetComponent<Text>().text = count.ToString();
        //ThisThing.GetComponent<Image>().sprite = component.GetComponent<SpriteRenderer>().sprite;
    }

    //检查这位置存的是什么
    public GameObject showThing()
    {
        return gameObject;
    }

    public int showCount()
    {
        return count;
    }

    public void reduceCount()
    {
        count--;
        transform.Find("num").gameObject.GetComponent<Text>().text = count.ToString();
        if (count == 0)
        {
            followMouse = false;
            ThisThing = null;//记录记下来的物品
            thingSprite = null;//放在包包里的样子
            //afterCursor = null;//prefab
            Destroy(AC);//跟在鼠标后面的真实物体
            setDisable();
        }
    }

    public Sprite showSprite()
    {
        return thingSprite;
    }

    // Use this for initialization
    void Start () {
        count = 0;
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(takeOut);
        followMouse = false;

    }

    void takeOut()
    {
        followMouse = true;
        if(AC == null)
            AC = Instantiate(afterCursor, transform) as GameObject;
        Sprite pickedSprite = ThisThing.GetComponent<SpriteRenderer>().sprite;
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
                        GameObject x = Instantiate(ThisThing, PlacePos, Quaternion.Euler(45, 0, 0)) as GameObject;
                        reduceCount();
                    }
                }
            }
        }
    }



}
