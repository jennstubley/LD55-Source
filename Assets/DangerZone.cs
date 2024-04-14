using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public float Damage;
    public bool Knockback;

    private float damageDelay;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        damageDelay = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageDelay > 0)
        {
            damageDelay -= Time.deltaTime;
            if (damageDelay <= 0)
            {
                CheckForHits();
            }
        }
        else
        {
            CheckForHits();
        }

    }

    private void CheckForHits()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        Collider2D[] overlaps = new Collider2D[1];
        col.OverlapCollider(contactFilter, overlaps);
        if (overlaps[0] != null)
        {
            Hitpoints hp = overlaps[0].GetComponent<Hitpoints>();
            hp.Damage(Damage);
            if (Knockback)
            {
                Vector3 target = (overlaps[0].transform.position - transform.position).normalized;
                overlaps[0].transform.position = Vector3.MoveTowards(overlaps[0].transform.position, target * 200, 1f);
            }
            damageDelay = 0.5f;
        }
        else
        {
            damageDelay = 0;
        }
    }
}
