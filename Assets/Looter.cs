using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Looter : MonoBehaviour
{
    public AudioClip LootClip;
    public AudioClip CoinClip;
    public int TotalCoins;
    public GameObject UnlockMessage;
    public Image SwordImage;
    public Sprite FireSprite;
    public Canvas MainCanvas;

    private Collider2D col;
    private AudioSource audioSource;
    private float lootOffset;
    private float offsetDelay;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (offsetDelay > 0)
        {
            offsetDelay -= Time.deltaTime;
            if (offsetDelay < 0) { offsetDelay = 0; lootOffset = 0; }
        }

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(LayerMask.GetMask("Loot"));
        Collider2D[] overlaps = new Collider2D[5];
        col.OverlapCollider(contactFilter, overlaps);
        foreach (Collider2D collider in overlaps)
        {
            if (collider != null && collider.GetComponent<Loot>() != null && collider.GetComponent<Loot>().CanBePicked)
            {
                audioSource.PlayOneShot(collider.GetComponent<Loot>().Name == "Coin" ? CoinClip : LootClip);
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
        else if (loot.Name == "Coin")
        {
            TotalCoins++;
            if (TotalCoins >= 50)
            {
                GameController.Instance.YouWin();
            }
        }
        else if (loot.Name == "Fire Sword")
        {
            GetComponent<Attack>().FireSword = true;
            transform.Find("Sword").gameObject.SetActive(false);
            transform.Find("Fire Sword").gameObject.SetActive(true);
            ShowUnlockMessage("Fire Sword");
            SwordImage.sprite = FireSprite;
        }
        else if (loot.Name == "Slot 2" || loot.Name == "Slot 3" || loot.Name == "Slot 4")
        {
            GameController.Instance.UnlockNextSlot();
            ShowUnlockMessage(loot.Name);
        }
        else if (loot.Name == "Goo" || loot.Name == "Crystal")
        {
            GameController.Instance.UnlockMonster(loot.Name);
            ShowUnlockMessage(loot.Name);
        }
    }

    private void ShowUnlockMessage(string msg)
    {
        offsetDelay = 10f;
        GameObject obj = Instantiate(UnlockMessage);
        obj.transform.SetParent(MainCanvas.transform, false);
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + lootOffset, obj.transform.position.z);
        obj.GetComponent<TMP_Text>().text = msg + " unlocked";
        lootOffset -= 50;
    }
}
