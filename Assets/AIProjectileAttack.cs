using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProjectileAttack : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject Target;
    public float FireRate;
    public AudioClip ShootClip;

    private float fireDelay;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        fireDelay = FireRate;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay -= Time.deltaTime;
        if (fireDelay <= 0 )
        {
            fireDelay = FireRate;
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        GameObject proj = GameObject.Instantiate(ProjectilePrefab);
        proj.transform.position = transform.position;
        proj.GetComponent<Projectile>().Direction = Target.transform.position - transform.position;
        proj.GetComponent<Projectile>().TargetMask = "Player";
        audioSource.PlayOneShot(ShootClip);
    }
}
