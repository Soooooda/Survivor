using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonManage : MonoBehaviour {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.SetActive(true);
    }

    public void OnHoverEvent(GameObject go)
    {
        //Debug.Log("调用了！！想让物体变大");
        RectTransform trans = go.GetComponent<RectTransform>();
        float mX = 5f;
        float mY = 5f;
        trans.sizeDelta = new Vector2(trans.sizeDelta.x + mX, trans.sizeDelta.y + mY);
        Transform text = go.transform.GetChild(0);
        Text m_Text = text.GetComponent<Text>();
        //RectTransform m_RectTransform = text.GetComponent<RectTransform>();
        //Change the Font Size to 16
        m_Text.fontSize = 50;
        //Change the RectTransform size to allow larger fonts and sentences
        //m_RectTransform.sizeDelta = new Vector2(m_Text.fontSize * 10, 100);
    }
    public void OnHoverEvent2(GameObject go)
    {
        //Debug.Log("调用了！！想让物体变小");
        RectTransform trans = go.GetComponent<RectTransform>();
        float mX = 5f;
        float mY = 5f;
        trans.sizeDelta = new Vector2(trans.sizeDelta.x - mX, trans.sizeDelta.y - mY);
        Transform text = go.transform.GetChild(0);
        Text m_Text = text.GetComponent<Text>();
        //RectTransform m_RectTransform = text.GetComponent<RectTransform>();
        //Change the Font Size to 16
        m_Text.fontSize = 43;
        //Change the RectTransform size to allow larger fonts and sentences
        //m_RectTransform.sizeDelta = new Vector2(m_Text.fontSize / 10, 100);
    }

    public static ButtonManage instance = null;



    //public void ChangeScene()
    //{

    //    if (instance == null)
    //        instance = this;
    //    gameObject.SetActive(true);
    //    Debug.Log("whether object is active " + gameObject.activeInHierarchy);
    //    StartCoroutine(FadeScene());
    //}

    //IEnumerator FadeScene()
    //{
    //    Debug.Log("inside coroutine");
    //    float time = GameObject.Find("Fade").GetComponent<FadeScene>().BeginFade(1);
    //    yield return new WaitForSeconds(time);
    //    SceneManager.LoadScene("Login");
    //}

}
