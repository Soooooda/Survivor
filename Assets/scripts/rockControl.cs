using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockControl : MonoBehaviour {

    public Sprite Stage1;
    public Sprite Stage2;

    public int currentStage = 0;
    public Transform stonesObj;

    private SpriteRenderer spt;

    // Use this for initialization
    void Start () {
        spt = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		if(currentStage==4)
        {
            spt.sprite = Stage1;
        }
        else if(currentStage==7)
        {
            spt.sprite = Stage2;
        }
        else if(currentStage>10)
        {
            Instantiate(stonesObj, transform.position, stonesObj.rotation);
            Destroy(gameObject);
        }
            
	}

    void OnMouseDown()
    {
        currentStage += 1;
    }
}
