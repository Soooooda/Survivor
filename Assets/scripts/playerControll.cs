using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}
public class playerControll : MonoBehaviour {

    public Image bloodBar;
    public Image magicBar;
    public float magic = 0f;
    public float blood = 0f;
    //控制捡东西的时间
    public float picktime;
    public float speed;
    public Boundary boundary;
    private Rigidbody rb;
    public static int mywarmth = 0;
    public LayerMask PickableLayer;
    //控制人的动画
    public int moveH = 0;
    public int moveV = 0;
    public float pickUpTimer = 0f;//定时器用来看是否捡东西

    public bool SpaceKeyDown = true;//是否抬起了space
    Animator anim;

    Collider close;//离当前物体最近的物体
    bool spaceOverFlag = false;//空格是否抬起了
    bool pickthings = false;//是否是捡东西的状态
    Vector3 closeOffset = new Vector3(0, 0, 0);//物体和最近物体的偏移
    public GameObject PlayerAction;

    //背包组件
    public GameObject PlayerBag;
    public GameObject ThingsInBag;//还在地上的Thing
    GameObject[] Things;//放进包里的Thing
    int[] ThingsNum;//每个栏里有多少个东西

    //初始化
    void Start()
    {
        ThingsNum = new int[10];
        Things = new GameObject[10];
        magicBar.fillAmount = 0f;
        bloodBar.fillAmount = 0f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = PlayerAction.GetComponent<Animator>();
    }


    //开始捡东西,并且存起来
    public void pickThings(Collider other)
    {
        bool pickedflag = false;
        if (other.gameObject.transform.Find("PickableThing") == null)
            return;
        GameObject component = other.gameObject.transform.Find("PickableThing").gameObject;
        Sprite pickedSprite = component.GetComponent<SpriteRenderer>().sprite;
        //如果包里一开始有
        Debug.Log(pickedSprite.name);
        for (int i = 0;i<10;i++)
        {
            Debug.Log(Things[i]);
            if(Things[i] != null &&pickedSprite==Things[i].GetComponent<Image>().sprite)
            {
                string num = Things[i].transform.Find("num").gameObject.GetComponent<Text>().text;
                //int n = int.Parse(num);
                //n++;
                ThingsNum[i]++;
                Things[i].transform.Find("num").gameObject.GetComponent<Text>().text = ThingsNum[i].ToString();
                pickedflag = true;
                Destroy(component);
            }
        }
        //如果包里一开始没有东西,就装进去
        if(!pickedflag)
        {
            int index = -1;
            for(int i=0;i<10;i++)
            {
                if (ThingsNum[i] == 0)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                Things[index] = Instantiate(ThingsInBag, PlayerBag.transform) as GameObject;
                Things[index].GetComponent<Image>().sprite = pickedSprite;
                ThingsNum[index] = 1;
                Destroy(component);
            }
            
        }
        
        
        //component.transform.parent = PlayerBag.transform;
        //Destroy(component);
        if (magic < 100)
            magic++;
        if (blood < 100 )
            blood++;        
    }

    //吃掉叶子
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("leaf") && spaceOverFlag == false)
        {
            //开始捡东西
            if (picktime == pickUpTimer)
                pickthings = true;
            if (pickUpTimer < 0.00001f)
            {
                anim.SetBool("pick", false);
                
                pickThings(other);
                pickUpTimer = picktime;
                spaceOverFlag = true;//捡起来叶子后可以重新判断一次叶子在哪里
            }
            else
            { anim.SetBool("pick", true); pickUpTimer -= Time.deltaTime; }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("woodFire"))
        {
            mywarmth += 1;
        }
        //空格键摁着并且碰到了叶子
        if (other.gameObject.CompareTag("leaf") && spaceOverFlag == false)
        {
            
            //开始捡东西
            if (picktime == pickUpTimer)
                pickthings = true;

            if(pickUpTimer<0.00001f)
            {
                anim.SetBool("pick", false);
                pickThings(other);
                pickUpTimer = picktime;
                spaceOverFlag = true;//捡起来叶子后可以重新判断一次叶子在哪里
            }
            else
            { anim.SetBool("pick", true); pickUpTimer -= Time.deltaTime; }

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


        for (int j = 0; j < close_things.Length; ++j)
        {
            
            //计算当前物品和其中的一个物品的距离平方
            float sqr_distance = (pinch_position - close_things[j].transform.position).sqrMagnitude;
            //当前物品可以pick
            if (close_things[j].GetComponent<Rigidbody>() != null && sqr_distance < closest_sqr_distance && !close_things[j].transform.IsChildOf(transform) && close_things[j].tag != "NotGrabbable")
            {
                //遍历找距离最短的
                closest = close_things[j];
                closest_sqr_distance = sqr_distance;
            }
        }
        return closest;//返回最短的那个
    } 




    //处理Rigidbody时，需要用FixedUpdate代替Update。例如:给刚体加一个作用力时，你必须应用作用力在FixedUpdate里的固定帧，而不是Update中的帧(两者帧长不同)。
    void FixedUpdate()
    {
        //Debug.Log(pickUpTimer);
        magicBar.fillAmount = magic / 100f;
        bloodBar.fillAmount = blood / 100f;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

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

        //if (anim.GetBool("pick") == true)
        //{
        //    if (pickUpTimer > 0)
        //        pickUpTimer -= Time.deltaTime;
        //    else
        //    {
        //        pickUpTimer = picktime;
        //        anim.SetBool("pick", false);
        //    }
        //}

        if (Input.GetKeyUp(KeyCode.Space))
        {
            pickUpTimer = picktime;
            anim.SetBool("pick", false);
            spaceOverFlag = true;
            anim.SetFloat("Walk", -1f);
        }
        //抬起空格后再摁下要重新计算一次最近的坐标
        else if (Input.GetKey(KeyCode.Space))
        {
            if (spaceOverFlag)
            {
                spaceOverFlag = false;
                close = FindClosestGrabbableObject(transform.position);
                if (close == null)
                {
                    anim.SetFloat("Walk", -1f);
                    return;
                }
            }
            if (close == null)
            {
                anim.SetFloat("Walk", -1f);
                return;
            }

            closeOffset = close.transform.position - transform.position;
            if (anim.GetBool("pick")==false)
            {


                if (closeOffset.x > 0.01f)
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    moveV = 1;
                }
                else if (closeOffset.x < -0.01f)
                {
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    moveV = -1;

                }
                else
                    moveV = 0;
                if (closeOffset.z < -0.01)
                {
                    transform.position += Vector3.back * speed * Time.deltaTime;
                    moveH = 1;
                }
                else if (closeOffset.z > 0.01)
                {
                    transform.position += Vector3.forward * speed * Time.deltaTime;
                    moveH = -1;
                }
                else
                    moveH = 0;
            }
        }
        else
        {
            moveV = 0;
            moveH = 0;
        }

        if (Input.GetKey(KeyCode.W) || (moveH == -1&&moveV==0))
        { anim.SetFloat("Walk", 1f); anim.SetBool("pick", false); }
        else if (Input.GetKey(KeyCode.S) || (moveH == 1&&moveV==0 ))
        { anim.SetFloat("Walk", 1f); anim.SetBool("pick", false); }
        else if (Input.GetKey(KeyCode.A) || (moveV == -1))
        { anim.SetFloat("Walk", 1f); anim.SetBool("pick", false); }
        else if (Input.GetKey(KeyCode.D) || (moveV == 1 ))
        { anim.SetFloat("Walk", 1f); anim.SetBool("pick", false); }

        if (Input.GetKeyUp(KeyCode.W))
            anim.SetFloat("Walk", 0f);
        else if (Input.GetKeyUp(KeyCode.S))
            anim.SetFloat("Walk", 0f);
        else if (Input.GetKeyUp(KeyCode.A))
            anim.SetFloat("Walk", 0f);
        else if (Input.GetKeyUp(KeyCode.D))
            anim.SetFloat("Walk", 0f);
        moveH = 0;
        moveV = 0;
    }

}
