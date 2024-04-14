using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitpointsUI : MonoBehaviour
{
    public Hitpoints hp;

    private Slider slider;
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = hp.Current / hp.Max;
        if (text != null )
        {
            text.text = hp.Current.ToString() + " / " + hp.Max.ToString();
        }
    }
}
