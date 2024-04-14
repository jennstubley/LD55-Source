using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Reward Multiplier: " + GameController.Instance.RewardMultiplier;
    }
}
