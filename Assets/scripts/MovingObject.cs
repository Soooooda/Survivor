using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract是必须要继承的
public abstract class MovingObject : MonoBehaviour {

    //物体移动一次花的秒数
    public float moveTime = 0.1f;
    //检测碰撞，这个地方可不可以移动
    public LayerMask blockingLayer;
    //player，enemy和wall的prefabs在这个layer，完蛋当时没有注意这个。。

    //教程里是2d的，不知道会不会有影响，反正老子是3d的hhh
    private BoxCollider boxCollider;
    //rb使用来保存我们移动的当前物体
    private Rigidbody rb;
    //iMT用来让移动的计算更有效率
    private float inverseMoveTime;

    //out是指针的意思
    protected bool Move(int xDir, int yDir, out bool hit)
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(xDir, yDir);

        //把Collider设置为false，抛弃自己的ray的时候不会撞到自己的collider？没懂
        boxCollider.enabled = false;
        hit = Physics.Raycast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit == false)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    //控制对方
    protected virtual void attemptMove <T>(int xDir,int yDir)
        where T: Component
    {
        bool hit;
        //是否可以移动:可以移动，直接return
        //bool canMove = Move(xDir, yDir, out hit);
        //if (hit == false)
        //    return;
        ////如果被撞到了，把对方物体设为T
        //T hitComponent = 

    }


    //virtual可以被继承的子类覆盖，这样子类可以有不同的start function
	// Use this for initialization
	protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        //因为乘法比除法更有效率
        inverseMoveTime = 1f / moveTime;
    }

    //平滑位移协同程序，用来把物体从一个地方移动到另外一个地方(end)
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        
        //检查剩余位移是否大于一个很小的数
        while(sqrRemainingDistance>float.Epsilon)
        {
            //计算挪多少
            Vector3 newPosition = Vector3.MoveTowards(rb.position,end,inverseMoveTime*Time.deltaTime);
            //往前挪一挪
            rb.MovePosition(newPosition);
            //更新剩余距离
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            //在frame切换中等到剩余位移很小才return
            yield return null;
        }

    }

    protected abstract void onCantMove<T>(T component)
        where T : Component;

}
