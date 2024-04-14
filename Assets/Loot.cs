using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Vector3 SpawnDirection;
    public string Name;
    public bool CanBePicked;

    private float bounceTime;
    private float bounceSpeed;
    private float pickUpDelay;

    // Start is called before the first frame update
    void Start()
    {
        pickUpDelay = 1f;
        CanBePicked = false;
        SpawnDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(1f, -1f));
        bounceTime = 5f;
        bounceSpeed = UnityEngine.Random.Range(0.2f, 0.4f);
        GetComponentInChildren<Animator>().speed = 1 + (bounceSpeed - 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (bounceTime > 0)
        {
            bounceTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, SpawnDirection * 200, bounceSpeed * Time.deltaTime * bounceTime);
        }
        if (pickUpDelay > 0)
        {
            pickUpDelay -= Time.deltaTime;
            if (pickUpDelay <= 0)
            {
                CanBePicked = true;
            }
        }
    }
}
