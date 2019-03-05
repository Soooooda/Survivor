using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMange : MonoBehaviour {

    public BoardManager boardScript;
    public static GameMange instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Debug.Log("this is awake");
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene();

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
