using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitSplat : MonoBehaviour
{
    private AudioSource audioSource;
    private List<SpriteRenderer> sr;

    private float splatTimer;
    public AudioClip SplatClip;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentsInChildren<SpriteRenderer>().ToList();
        audioSource = GetComponent<AudioSource>();
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
                foreach (var renderer in sr)
                {
                    renderer.color = Color.white;
                }
                transform.position -= new Vector3(0, 0.1f, 0);
            }
        }
    }

    public void Splat()
    {
        foreach (var renderer in sr)
        {
            renderer.color = Color.red;
        }
        splatTimer = 0.1f;
        audioSource.PlayOneShot(SplatClip);
        transform.position += new Vector3(0, 0.1f, 0);
    }
}
