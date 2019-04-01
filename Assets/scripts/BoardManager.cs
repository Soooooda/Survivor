using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Tool;

public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count
    {
        public int minmum;
        public int maximum;

        public Count(int min,int max)
        {
            minmum = min;
            maximum = max;
        }
    }

    //地图大小
    public int columns = 8;
    public int rows = 8;

    //物品放置的格子大小
    public int gridCol = 100;
    public int gridRow = 100;

    //用于背包初始化
    public GameObject ThingsInBag;
    public GameObject PlayerBag;


    //在2d游戏里有个wall，wall的数量在5到9之间
    //还有食物的数量
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public Count leafCount = new Count(50, 80);
    //public GameObject exit;//在2d游戏中有个出口
    //这里是铺地图了
    public GameObject[] floorTiles;
    //下面还有一些其它的tiles
    //public GameObject[] floorTiles;
    //public GameObject[] wallTiles;
    //public GameObject[] foodTiles;
    //public GameObject[] enemyTiles;
    //public GameObject[] outerWallTiles;
    public GameObject[] leaves;

    //keep clear
    private Transform boardHolder;
    //keep track of positions
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<Vector3> itemPositions = new List<Vector3>();


    //连接服务器用
    string m_receiveMessage = "wait...";


    //把所有可以放物品的位置初始化
    void InitialiseList()
    {
        gridPositions.Clear();
        for(int x= 1; x < gridCol - 1; x++)
        {
            for(int y = 1; y < gridRow - 1; y++)
            {
                gridPositions.Add(new Vector3(x/10, 62.9f, y/10 - 854.5f));
            }
        }

        //itemPositions.Clear();
        //for (int x = 1; x < gridCol - 1; x++)
        //{
        //    for (int y = 1; y < gridRow - 1; y++)
        //    {
        //        itemPositions.Add(new Vector3(x * 5, 62.9f, y * 5 - 854.5f));
        //    }
        //}
    }

    //初始化地图
    void BoardSetup()
    {
        boardHolder = new GameObject("board").transform;
        //这里x和y是坐标
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                //把floor们实例化，这个之后再改，先看效果
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //Quaternion是用来控制rotation的，之后应该也要改
                GameObject instance = Instantiate(toInstantiate, new Vector3(x*5, 62.9f, y*5 - 854.5f),toInstantiate.transform.rotation) as GameObject;
                //把初始化的作为boardHolder的子对象
                instance.transform.SetParent(boardHolder);
            }
        }


    }



    //随机选一个的位置放地图，确保都在gridposition中不重复
    Vector3 RandomPostion()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //在postion上放除了地图之外的东西，花花草草
    void LayoutObjectAtRandom(GameObject[] tileArray, int minmum, int maxmum)
    {
        int objectCount = Random.Range(minmum, maxmum + 1);
        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPostion();//随机找一个位置放
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.Euler(45, 0, 0));
        }
    }

    public void SetupScene()
    {
        ClientSocket.instance.onGetReceive = ShowReceiveMessage;
        BoardSetup();
        InitialiseList();
        
        LayoutObjectAtRandom(leaves, leafCount.minmum, leafCount.maximum);
        //int enemyCuunt = (int)
    }

    //展示有没有连上
    void ShowReceiveMessage(string message)
    {
        m_receiveMessage = message;
    }

}
