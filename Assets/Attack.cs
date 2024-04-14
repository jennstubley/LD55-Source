using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Vector2 WeaponOffset;
    public float WeaponDamage;
    public float WeaponSpeed;


    private float attackDelay;
    private Move move;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
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
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (attackDelay <= 0)
            {
                DoAttack();
            }
        }
    }

    private void DoAttack()
    {
        anim.SetTrigger("Attack");
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Enemies"));
        contactFilter.useTriggers = true;
        Collider2D[] overlaps = new Collider2D[1];
        Collider2D attackCollider = GetAttackCollider();
        attackCollider.OverlapCollider(contactFilter, overlaps);
        Debug.Log(attackCollider);
        if (overlaps[0] != null)
        {
            Debug.Log(overlaps[0]);
            Hitpoints hp = overlaps[0].GetComponent<Hitpoints>();
            hp.Damage(WeaponDamage);
            attackDelay = WeaponSpeed;
       }

    }

    private Collider2D GetAttackCollider()
    {
        switch (move.GetFacingDir())
        {
            case Move.FacingDir.Down:
                return transform.Find("Attack Down").GetComponent<Collider2D>();
            case Move.FacingDir.Left:
                return transform.Find("Attack Left").GetComponent<Collider2D>();
            case Move.FacingDir.Right:
                return transform.Find("Attack Right").GetComponent<Collider2D>();
            case Move.FacingDir.Up:
                return transform.Find("Attack Up").GetComponent<Collider2D>();
        }
        return null;
    }
}
