using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public Vector2 WeaponOffset;
    public float Range;
    public float Damage;
    public float AttackInterval;

    public GameObject AttackVisual;

    private float attackDelay;
    private float telegraphDelay;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        attackDelay = AttackInterval;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
            if (attackDelay <= 0)
            {
                attackDelay = 0;
                TelegraphAttack();
            }
        }

        if (telegraphDelay > 0)
        {
            telegraphDelay -= Time.deltaTime;
            if (telegraphDelay <= 0)
            {
                telegraphDelay = 0;
                DoAttack();
            }
        }
    }

    private void TelegraphAttack()
    {
        AttackVisual.SetActive(true);
        telegraphDelay = 1.0f;
    }

    private void DoAttack()
    {
        anim.SetTrigger("Attack");
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        Collider2D[] overlaps = new Collider2D[1];
        Collider2D attackCollider = AttackVisual.GetComponent<Collider2D>();
        attackCollider.OverlapCollider(contactFilter, overlaps);
        if (overlaps[0] != null)
        {
            Hitpoints hp = overlaps[0].GetComponent<Hitpoints>();
            hp.Damage(Damage);
        }
        AttackVisual.SetActive(false);
        attackDelay = AttackInterval;
    }
}
