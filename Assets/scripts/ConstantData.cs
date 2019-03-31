using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData : MonoBehaviour {

    public static string userID = "";
    public static string roomID = "";
    public static string roomOwner = "";
    public static string roomName = "";
    public static int role = -1;
    public static bool hasRole = false;
    public static Dictionary<Sprite, int> BagDict = new Dictionary<Sprite, int>();
    public static void ResetAllData()
    {
        userID = "";
        roomID = "";
        roomOwner = "";
        roomName = "";
    }

    void Start () {
        DontDestroyOnLoad(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
