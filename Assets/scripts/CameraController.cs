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

    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }

}
