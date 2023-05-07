using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform spawnBrickTransform;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject playerGameObj;
    [SerializeField] float offset = 0.2f;
    [SerializeField] private GameObject StackParent;
    [SerializeField] private GameObject BrickPoint;
    [SerializeField] private GameObject Bridge;
    [SerializeField] private GameObject LinePoint;
    [SerializeField] private Animator anim;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float speed = 10f;
    private Vector3 startPosition;
    private int countBricks = 0;
    private int countLine = 0;
    private string currentAnimName;

    private List<GameObject> bricksList = new List<GameObject>();

    public bool isMoveOnBridge = false;
    public DetectSwipe swipe;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        swipe.onSwipeDown += MoveDown;
        swipe.onSwipeUp += MoveUp;
        swipe.onSwipeLeft += MoveLeft;
        swipe.onSwipeRight += MoveRight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
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

            Invoke(nameof(ResetAnim), 0.25f);
            countBricks++;
            other.gameObject.tag = "StackBrick";
            AddBrick(other.gameObject);
        }
        else if (other.tag == "BridgeBrick")
        {
            ChangeAnim("Move");

            Invoke(nameof(ResetAnim), 0.25f);
            other.gameObject.tag = "Untagged";
            //other.enabled = false;
            RemoveBrick();

        }
        else if (other.tag == "Winbox")
        {
            isMoveOnBridge = true;
            ChangeAnim("Win");
        }

        if (other.tag == "Chest")
        {
            ChangeAnim("Win");
            Debug.Log("nhay");           
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

    private void InstantiateBrick()
    {
        Vector3 offsetBrick = new Vector3(0f, offset * (countBricks - 1), 0f);

        GameObject newBrick = Instantiate(brickPrefab, spawnBrickTransform);
        newBrick.transform.position += offsetBrick;
        bricksList.Add(newBrick);
        playerGameObj.transform.localPosition = new Vector3(0f, 0.5f + offsetBrick.y, -5f);
    }

    private void DestroyBrick()
    {
        countBricks--;
        GameObject brickDestroy = bricksList[countBricks];
        bricksList.Remove(brickDestroy);
        Destroy(brickDestroy);
        playerGameObj.transform.localPosition = new Vector3(0f, 0.5f + offset * (countBricks - 1), -5f);
    }

    private void AddBrick(GameObject brick)
    {
        //countBricks++;
        //Vector3 offsetBrick = new Vector3(0f, offset * (countBricks - 0) + 0.5f, -5f);
        brick.transform.SetParent(StackParent.transform);
        brick.transform.localPosition = new Vector3(BrickPoint.transform.localPosition.x, BrickPoint.transform.localPosition.y + offset * countBricks, BrickPoint.transform.localPosition.z);
        playerGameObj.transform.localPosition = new Vector3(playerGameObj.transform.localPosition.x, playerGameObj.transform.localPosition.y + offset, playerGameObj.transform.localPosition.z);

    }
    private void RemoveBrick()
    {
        countBricks--;
        if (StackParent.transform.GetChild(countBricks + 1).gameObject.CompareTag("StackBrick"))
        {

            //Destroy(StackParent.transform.GetChild(countBricks+ 1).gameObject);
            StackParent.transform.GetChild(countBricks + 1).gameObject.transform.SetParent(Bridge.transform);
            Bridge.transform.GetChild(countLine + 1).gameObject.transform.position = new Vector3(LinePoint.transform.position.x, LinePoint.transform.position.y, LinePoint.transform.position.z + (float)countLine);
            // StackParent.transform.GetChild(countBricks + 0).gameObject.transform.position=new Vector3(LinePoint.transform.position.x, 0f,LinePoint.transform.position.z - (float)countLine);
            countLine++;
            playerGameObj.transform.localPosition = new Vector3(playerGameObj.transform.localPosition.x, playerGameObj.transform.localPosition.y - offset, playerGameObj.transform.localPosition.z);
        }

    }

    private void ClearBrick()
    {

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

    private void ResetAnim()
    {
        ChangeAnim("Reset");
    }
}
