using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockText : MonoBehaviour
{
    public float Lifespan = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 100, transform.position.z);
        Lifespan -= Time.deltaTime;
        if (Lifespan < 0 )
        {
            Destroy(gameObject);
        }
    }
}
