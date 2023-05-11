using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LevelManager
{
    [SerializeField] GameObject playerGameObj;
    [SerializeField] float offset = 0.25f;
    [SerializeField] private Animator anim;
    private Rigidbody rb;
    private float speed = 10f;
    private int countBricks = 0;
    private string currentAnimName;
    private List<GameObject> bricksList = new List<GameObject>();
    public bool isMoveOnBridge = false;
    public DetectSwipe swipe;
    public Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        swipe.onSwipeDown += MoveDown;
        swipe.onSwipeUp += MoveUp;
        swipe.onSwipeLeft += MoveLeft;
        swipe.onSwipeRight += MoveRight;

        inGameLevel = 1;
        coin = 0;
        InstatiateMapLevel(inGameLevel);
        OnInit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
        if (countBricks < 0)
        {
            RePlay();
        }
    }

    public override void OnInit()
    {
        countBricks = 0;
        bricksList.Clear();
        playerGameObj.transform.rotation = Quaternion.Euler(new Vector3(0f, -15f, 0f));
        moveDirection = new Vector3(0, 0, 0);
        UIManager.instance.WinPanel(false);      
    }

    public void Move(Vector2 moveDirection)
    {
        if (isMoveOnBridge)
        {
            return;
        }

        this.moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.y);
    }

    private void MoveDown()
    {
        Move(new Vector2(0, -1));
    }
    private void MoveUp()
    {
        Move(new Vector2(0, 1));
    }
    private void MoveLeft()
    {
        //Debug.Log("qua trai");
        Move(new Vector2(-1, 0));
    }
    private void MoveRight()
    {
        Move(new Vector2(1, 0));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            isMoveOnBridge = true;
        }
        if (other.tag == "Brick")
        {
            //Debug.Log("brick");
            ChangeAnim("Move");

            Invoke(nameof(ResetAnim), 0.15f);
            countBricks++;
            other.gameObject.tag = "StackBrick";
            AddBrick(other.gameObject);
        }
        else if (other.tag == "LineBridge")
        {
            
            if (other.TryGetComponent<LineBridge>(out LineBridge brickOfLineBridge))
            {
                brickOfLineBridge.ShowBrick();
                ChangeAnim("Move");
                Invoke(nameof(ResetAnim), 0.15f);
                other.gameObject.tag = "Untagged";
                //other.enabled = false;
                //RemoveBrick();
                DestroyBrick();
            }
        }
        else if (other.tag == "Winbox")
        {
            isMoveOnBridge = true;
            ChangeAnim("Win");
            Invoke(nameof(ResetAnim), 5f);
            ClearBrick();
            StartCoroutine(DelayShowWinPanel(4f));
        }
               
                  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            isMoveOnBridge = false;
            //Debug.Log("ko di tren cau");
        }
    }

    private void DestroyBrick()
    {
        countBricks--;
        if (countBricks > 0)
        {           
            GameObject brickDestroy = bricksList[countBricks];
            bricksList.Remove(brickDestroy);
            Destroy(brickDestroy);
            playerGameObj.transform.localPosition = new Vector3(0f, 0.15f + offset * (countBricks - 1), -5f);
        }
    }
        

    private void AddBrick(GameObject brick)
    {
        brick.transform.SetParent(transform);
        brick.transform.localPosition = new Vector3(0, 0.15f + offset * (countBricks-1), -5f); 
        playerGameObj.transform.localPosition = new Vector3(0, 0.15f + offset * (countBricks - 1), -5f);
       
        bricksList.Add(brick);

    }
    private void ClearBrick()
    {
        while(countBricks > 1)
        {
            DestroyBrick();            
        }
        playerGameObj.transform.rotation = Quaternion.Euler(new Vector3(0f,-150f, 0f));
    }

    private IEnumerator DelayShowWinPanel(float second)
    {
        yield return new WaitForSeconds(second);

        UIManager.instance.WinPanel(true);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    public void ResetAnim()
    {
        ChangeAnim("Reset");
    }
    public void NextLevel()
    {

        inGameLevel++;
        if (inGameLevel > listMapLevel.Count)
        {
            inGameLevel = 1;
        }
        coin += 50;
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
        UIManager.instance.SetCoin(coin);
        UIManager.instance.SetLevel(inGameLevel);
        //PlayerPrefs.Save();
        DestroyMapLevel();
        InstatiateMapLevel(inGameLevel);
        OnInit();

    }

    public void RePlay()
    {
        DestroyMapLevel();
        InstatiateMapLevel(inGameLevel);
        OnInit();
    }
}
