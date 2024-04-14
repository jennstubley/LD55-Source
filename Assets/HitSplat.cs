using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSplat : MonoBehaviour
{

    private SpriteRenderer sr;

    private float splatTimer;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (splatTimer > 0)
        {
            splatTimer -= Time.deltaTime;
            if (splatTimer < 0)
            {
                splatTimer = 0;
                sr.color = Color.white;
                transform.position -= new Vector3(0, 0.1f, 0);
            }
        }
    }

    public void Splat()
    {
        sr.color = Color.red;
        splatTimer = 0.1f;
        transform.position += new Vector3(0, 0.1f, 0);
    }
}
