using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}
public class playerControll : MonoBehaviour {

    public float speed;
    //public float tilt;
    public Boundary boundary;
    private Rigidbody rb;
    public static int mywarmth = 0;
    public LayerMask PickableLayer;
    //控制人的动画
    public int moveH = 0;
    public int moveV = 0;
    //private float m_floorHeight;
    //private float m_spriteLowerBound;
    //private float m_spriteHalfWidth;
    //private readonly float m_tan30 = Mathf.Tan(Mathf.PI / 5);

    public bool SpaceKeyDown = true;//是否抬起了space
    public Animator anim;

    Collider close;//离当前物体最近的物体
    bool spaceOverFlag = false;//空格是否抬起了
    bool pickthings = false;//是否是捡东西的状态
    Vector3 closeOffset = new Vector3(0, 0, 0);//物体和最近物体的偏移

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //m_spriteLowerBound = spriteRenderer.bounds.size.y * 0.5f;
        //m_spriteHalfWidth = spriteRenderer.bounds.size.x * 0.5f;
    }


    public void pickThings()
    {
        Debug.Log("Before Waiting 2 seconds");
        pickthings = true;
        
        anim.SetFloat("Y", 2); anim.SetFloat("X", 0);
        //yield return new WaitForSeconds(100);
        anim.SetFloat("Y", 2); anim.SetFloat("X", 0);
        Debug.Log("After Waiting 2 Seconds");
    }

    //吃掉叶子
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("leaf") && spaceOverFlag == false)
        {
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
            spaceOverFlag = true;
            pickThings();
            //StartCoroutine(pickThings());
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("woodFire"))
        {
            mywarmth += 1;
            //Debug.Log(mywarmth);
        }

        if (other.gameObject.CompareTag("leaf") && spaceOverFlag == false)
        {
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
            spaceOverFlag = true;
            pickThings();
            //StartCoroutine(pickThings());
        }

    }


    //测试：去找离得最近的物体
    protected Collider FindClosestGrabbableObject(Vector3 pinch_position)//当前位置
    {
        Collider closest = null;
        //自定义的距离
        float grabObjectDistance = 2f;
        //距离平方，找到最近的距离
        float closest_sqr_distance = grabObjectDistance * grabObjectDistance;
        //用一个sphere来重叠，找到所有collide的
        Collider[] close_things = Physics.OverlapSphere(pinch_position, grabObjectDistance, PickableLayer);
        //遍历所有碰到的物品
        //Debug.Log(pinch_position.ToString());
        Debug.Log(close_things.Length);



        for (int j = 0; j < close_things.Length; ++j)
        {
            
            //计算当前物品和其中的一个物品的距离平方
            float sqr_distance = (pinch_position - close_things[j].transform.position).sqrMagnitude;
            //当前物品可以pick
            Debug.Log(sqr_distance);
            if (close_things[j].GetComponent<Rigidbody>() != null && sqr_distance < closest_sqr_distance && !close_things[j].transform.IsChildOf(transform) && close_things[j].tag != "NotGrabbable")
            {
                //GameObject grabbable = close_things[j];
                //Transform grabbable = close_things[j].GetComponent<Transform>();//这个renderer随便定义的
                //if (grabbable == null)//|| !grabbable.IsGrabbed()
                //{//遍历找距离最短的
                    closest = close_things[j];
                    closest_sqr_distance = sqr_distance;
                //}
            }
        }
        return closest;//返回最短的那个
    } 







    //处理Rigidbody时，需要用FixedUpdate代替Update。例如:给刚体加一个作用力时，你必须应用作用力在FixedUpdate里的固定帧，而不是Update中的帧(两者帧长不同)。
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        //rb.velocity = movement * speed;
        
        //rb.MovePosition(rb.position + speed * movement * Time.deltaTime);
        
        //rb.transform.position


        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            //Debug.Log("Space UP!!");
            spaceOverFlag = true;
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", 0);
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            //anim.SetFloat("X", 0);
            //anim.SetFloat("Y", 0);
            if (spaceOverFlag)
            {
                spaceOverFlag = false;
                close = FindClosestGrabbableObject(transform.position);
                if (close == null)
                {
                    anim.SetFloat("X", 0);
                    anim.SetFloat("Y", 0);
                    //Debug.Log("New set 00");
                    return;
                }
                //Debug.Log(close.transform.position.ToString());
                //rb.MovePosition(rb.position + speed * close.transform.position * Time.deltaTime);
                
                //rb.MovePosition(rb.position + speed * closeOffset);

            }
            if (close == null)
            {
                anim.SetFloat("X", 0);
                anim.SetFloat("Y", 0);
                //Debug.Log("Old set 00");
                return;
            }

            closeOffset = close.transform.position - transform.position;
            if (closeOffset.x > 0.1f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                moveV = 1;
                //anim.SetFloat("Y", 1);
            }
            else if (closeOffset.x < -0.1f)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                moveV = -1;
                //anim.SetFloat("Y", -1);

            }
            else
                moveV = 0;
            if (closeOffset.z < -0.1)
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
                //if(moveV==0)
                    moveH = 1;
                //anim.SetFloat("X", 1);
            }
            else if (closeOffset.z > 0.1)
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
                //if (moveV == 0)
                    moveH = -1;
                //anim.SetFloat("X", -1);
            }
            else
                moveH = 0;
        }
        else
        {
            moveV = 0;
            moveH = 0;
        }

        if (Input.GetKey(KeyCode.W) || (moveH == -1&&moveV==0))
        { anim.SetFloat("X", -1); anim.SetFloat("Y", 0); }
        else if (Input.GetKey(KeyCode.S) || (moveH == 1&&moveV==0 ))
        { anim.SetFloat("X", 1); anim.SetFloat("Y", 0); }
        else if (Input.GetKey(KeyCode.A) || moveV == -1)
        { anim.SetFloat("Y", -1); anim.SetFloat("X", 0); }
        else if (Input.GetKey(KeyCode.D) || moveV == 1)
        { anim.SetFloat("Y", 1); anim.SetFloat("X", 0); }


        //if(moveH==-1&&moveV==0)
        //{
        //    anim.SetFloat("X", -1);
        //    anim.SetFloat("Y", 0);
        //}
        //if (moveH == -1 && moveV == 0)
        //{
        //    anim.SetFloat("X", -1);
        //    anim.SetFloat("Y", 0);
        //}

        if (Input.GetKeyUp(KeyCode.W))
            anim.SetFloat("X", 0);
        else if (Input.GetKeyUp(KeyCode.S))
            anim.SetFloat("X", 0);
        else if (Input.GetKeyUp(KeyCode.A))
            anim.SetFloat("Y", 0);
        else if (Input.GetKeyUp(KeyCode.D))
            anim.SetFloat("Y", 0);
        //else if (Input.GetKeyUp(KeyCode.Space))
        //{

        moveH = 0;
        moveV = 0;
        //}



        //rb.position = new Vector3
        //(
        //    Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
        //    0.0f,
        //    Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        //);

        //rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

}
