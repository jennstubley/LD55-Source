using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looter : MonoBehaviour
{
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(LayerMask.GetMask("Loot"));
        Collider2D[] overlaps = new Collider2D[5];
        col.OverlapCollider(contactFilter, overlaps);
        foreach (Collider2D collider in overlaps)
        {
            if (collider != null && collider.GetComponent<Loot>() != null)
            {
                ApplyLoot(collider.GetComponent<Loot>());
                Destroy(collider.gameObject);
            }
        }
    }

    private void ApplyLoot(Loot loot)
    {
        if (loot.Name == "HP")
        {
            GetComponent<Hitpoints>().Max++;
            GetComponent<Hitpoints>().Current++;
        }
        else if (loot.Name == "Damage")
        {
            GetComponent<Attack>().WeaponDamage++;
        }
    }
}
