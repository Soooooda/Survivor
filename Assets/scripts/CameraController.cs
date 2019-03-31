using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    Vector3 mousePosition, targetPosition;

    //To Instantiate TargetObject at mouse position
    //public GameObject targetObject;

    void Update()
    {



        //If Left Button is clicked
        //if (Input.GetMouseButtonUp(0))
        //{
        //    //create the instance of targetObject and place it at given position.
        //    Instantiate(targetObject, targetObject.transform.position, Quaternion.Euler(45, 0, 0));
        //}

    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }

}
