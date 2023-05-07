using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Player player;
    private Vector2 initialPos;
    private Vector2 targetPos;

    // Start is called before the first frame update
    void Start()
    {
           player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            initialPos = Input.mousePosition;
        }
        if( Input.GetMouseButtonUp(0))
        {       
            targetPos = Input.mousePosition;
            Calculate(targetPos);           
        }
        

    }

    void Calculate(Vector2 finalPos)
    {
        float disX = Mathf.Abs(initialPos.x - finalPos.x);
        float disY = Mathf.Abs(initialPos.y - finalPos.y);
        if(disX>0 || disY>0)
        {
                if (disX > disY) 
                {
                    if (initialPos.x > finalPos.x)
                    {
                        //Debug.Log("Left");
                        player.Move(new Vector2(-1, 0));
                        
                    }
                    else
                    {
                        //Debug.Log("Right");
                        player.Move(new Vector2(1, 0));
                        
                    }
                }
                else 
                {   
                    if (initialPos.y > finalPos.y )
                    {
                        //Debug.Log("Down");
                        player.Move(new Vector2(0, -1));
                       
                    } 
                    else
                    {
                        //Debug.Log("Up");
                        player.Move(new Vector2(0, 1));
    
                    }
                }
        }
    }
}
