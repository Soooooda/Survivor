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

    //背包里的物品
    //public static GameObject[] Things = new GameObject[10];
    //public static int[] ThingsNum = new int[10];

    //public static ThingInBagControll[] ThingsInBag = new ThingInBagControll[10];
    public static GameObject[] ThingsInPackage = new GameObject[10];
    //public static Dictionary<Sprite, int> BagDict = new Dictionary<Sprite, int>();
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
