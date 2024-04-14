using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class SummonSlotUI : MonoBehaviour
{
    public Sprite EmptyImage;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.Find("Image").GetComponent<Image>().sprite = EmptyImage;
            child.Find("Slot Text").Find("Slot ID").GetComponent<TMP_Text>().text = "Slot " + ( i + 1 );
            child.Find("Slot Text").Find("Name").GetComponent<TMP_Text>().text = "Empty";
            var index = i;
            child.GetComponent<Button>().onClick.AddListener(() => RemoveMonster(index));
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (GameController.Instance.MonsterSlots[i] == null)
            {
                child.Find("Image").GetComponent<Image>().sprite = EmptyImage;
                child.Find("Slot Text").Find("Name").GetComponent<TMP_Text>().text = "Empty";
            }
            else
            {
                GameController.Monster monster = GameController.Instance.GetSlotMonster(i);
                child.Find("Image").GetComponent<Image>().sprite = monster.image;
                child.Find("Slot Text").Find("Name").GetComponent<TMP_Text>().text = monster.Name;
            }
            i++;
        }
    }

    public void RemoveMonster(int idx)
    {
        GameController.Instance.RemoveMonsterFromSlot(idx);
    }
}
