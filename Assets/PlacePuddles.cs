using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePuddles : MonoBehaviour
{
    public float PuddleFrequency;
    public GameObject puddlePrefab;
    public AudioClip plopClip;


    private float puddleDelay;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        puddleDelay = PuddleFrequency;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puddleDelay > 0)
        {
            puddleDelay -= Time.deltaTime;
            if (puddleDelay < 0 )
            {
                PlacePuddle();
                puddleDelay = PuddleFrequency;
            }
        }
    }

    private void PlacePuddle()
    {
        GameObject puddle = Instantiate( puddlePrefab );
        GameController.Instance.GroundEffects.Add(puddle );
        puddle.transform.position = transform.position;
        //audioSource.PlayOneShot(plopClip);
    }
}
