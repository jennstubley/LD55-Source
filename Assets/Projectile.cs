using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;
    public float LifeSpan;
    public float Damage;

    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeSpan = Time.deltaTime;
        if (LifeSpan <= 0)
        {
            Destroy(this.gameObject);
            return;
        }

        Debug.DrawRay(transform.position, Direction);
        transform.position = transform.position + Direction.normalized * Speed * Time.deltaTime;

        CheckForHits();
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
            Destroy(this.gameObject);
        }
    }
}
