
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectSwipe : MonoBehaviour
{
    enum SWIPE_PHASE
    {
        NONE = 0,
        BEGAN,
        MOVED,
        ENDED
    }
    
    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;
    // Start is called before the first frame update
    private SWIPE_PHASE currentSwipePhase;

    private bool isTouched = false;
    //private bool isMoved = false;

    private const float SWIPE_THRESHHOLD = 50f;

    public UnityAction onSwipeRight;
    public UnityAction onSwipeLeft;
    public UnityAction onSwipeUp;
    public UnityAction onSwipeDown;

    void Start()
    {
        currentSwipePhase = SWIPE_PHASE.NONE;
        isTouched = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isTouched && Input.GetMouseButtonDown(0))
        {
            currentSwipePhase = SWIPE_PHASE.BEGAN;
            fingerDownPos = Input.mousePosition;
            isTouched = true;
        }

        if (isTouched)
        {
            if (fingerDownPos != (Vector2)Input.mousePosition)
            {
                currentSwipePhase = SWIPE_PHASE.MOVED;
            }
        }

        if (currentSwipePhase == SWIPE_PHASE.MOVED)
        {
            DetectorSwipe(Input.mousePosition);
        }

        if (isTouched && Input.GetMouseButtonUp(0))
        {
            currentSwipePhase = SWIPE_PHASE.ENDED;
            isTouched = false;
        }
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) 
        {
            Touch curTouch = Input.GetTouch(0);
            if (!isTouched && curTouch.phase == TouchPhase.Began) 
            {
                fingerDownPos = curTouch.deltaPosition;
                isTouched = true;
            }

            if (isTouched)
            {
                if (curTouch.phase == TouchPhase.Moved)
                {
                    DetectorSwipe(curTouch.deltaPosition);
                }
            }

            if (isTouched && curTouch.phase == TouchPhase.Ended) 
            {
                fingerDownPos = Vector2.zero;
                isTouched = false;
            }
        }
#endif
    }

    void DetectorSwipe(Vector2 movedPos)
    {
        float xChange = movedPos.x - fingerDownPos.x;
        float yChange = movedPos.y - fingerDownPos.y;

        if(Mathf.Abs(xChange) > Mathf.Abs(yChange))
        {
            if (Mathf.Abs(xChange) > SWIPE_THRESHHOLD)
            {
                if (xChange > 0)
                {
                    //Debug.Log("qua phai");
                    fingerDownPos.x = movedPos.x;
                    //SWIPE RIGHT
                    if (onSwipeRight != null)
                    {
                        onSwipeRight();
                    }
                }

                if (xChange < 0)
                {
                    //Debug.Log("qua trai");
                    fingerDownPos.x = movedPos.x;
                    //SWIPE LEFT
                    if (onSwipeLeft != null)
                    {
                        //Debug.Log("qua trai");
                        onSwipeLeft();
                    }
                }
            }
        }
        else
        {          
                if (Mathf.Abs(yChange) > SWIPE_THRESHHOLD)
                {
                    if (yChange > 0)
                    {
                        fingerDownPos.y = movedPos.y;
                        //SWIPE UP
                        if (onSwipeUp != null)
                        {
                            onSwipeUp();
                        }
                    }
                    else if (yChange < 0)
                    {
                        fingerDownPos.y = movedPos.y;
                        //SWIPE DOWN
                        if (onSwipeDown != null)
                        {
                            onSwipeDown();
                        }
                    }
                }
            
        }
    
    }
}
