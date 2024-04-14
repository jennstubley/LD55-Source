using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public Looter playerLooter;

    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        playerLooter = FindObjectOfType<Looter>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerLooter.TotalCoins.ToString();
    }
}
