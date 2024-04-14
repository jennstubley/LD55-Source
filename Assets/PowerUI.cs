using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUI : MonoBehaviour
{

    public Attack PlayerPower;

    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = PlayerPower.WeaponDamage.ToString();
    }
}
