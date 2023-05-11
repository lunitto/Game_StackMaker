using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBridge : MonoBehaviour
{
    [SerializeField] GameObject brickOfLineBridge;

    public void ShowBrick()
    {
        brickOfLineBridge.SetActive(true);
    }
}
