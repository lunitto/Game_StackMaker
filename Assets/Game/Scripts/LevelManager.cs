using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] protected List<GameObject> listMapLevel;
    [SerializeField] GameObject player;

    protected int inGameLevel;
    protected int coin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void OnInit()
    {

    }
    public void InstatiateMapLevel(int level)
    {
        Instantiate(listMapLevel[level - 1], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        for (int i = 0; i < listMapLevel[level - 1].gameObject.transform.childCount; i++)
        {
            if (listMapLevel[level - 1].gameObject.transform.GetChild(i).name == "StartPoint")
            {
                Vector3 offset = listMapLevel[level - 1].gameObject.transform.GetChild(i).position;
                if (level == 1 || level ==3)
                {
                    player.transform.position = new Vector3(offset.x, offset.y - 0.55f, offset.z + 5f);
                }
                else
                {
                    player.transform.position = new Vector3(offset.x, offset.y, offset.z + 5f); ;
                }
                
            }
        }

    }

    public void DestroyMapLevel()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Level");
        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }
    }

}
