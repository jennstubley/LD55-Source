using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    public Vector2 WeaponOffset;
    public float WeaponDamage;
    public float WeaponSpeed;
    public bool FireSword;
    public GameObject ProjectilePrefab;
    public AudioClip AttackClip;

    private float attackDelay;
    private Move move;
    private Animator anim;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(AttackClip);
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
        }
        attackDelay = WeaponSpeed;

        if (FireSword)
        {
            GameObject proj = GameObject.Instantiate(ProjectilePrefab);
            Vector2 dir = GetForwardDir();
            proj.transform.position = transform.position;
            proj.GetComponent<Projectile>().Direction = dir;
            proj.GetComponent<Projectile>().TargetMask = "Enemies";
        }

    }

    private Vector2 GetForwardDir()
    {
        switch (move.GetFacingDir())
        {
            case Move.FacingDir.Down:
                return Vector2.down;
            case Move.FacingDir.Left:
                return Vector2.left;
            case Move.FacingDir.Right:
                return Vector2.right;
            case Move.FacingDir.Up:
                return Vector2.up;
        }
        return Vector2.zero;
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
