using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static string COIN_KEY = "Coin";
    [SerializeField] Text levelText;
    [SerializeField] Text coinText;
    [SerializeField] GameObject winPanel;
    private void Awake()
    {
        instance = this;
    }

    public void WinPanel(bool isActive)
    {
        winPanel.SetActive(isActive);
    }

    public void SetLevel(int level)
    {
        levelText.text = "Level " + level.ToString();
    }

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}
