using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterListUI : MonoBehaviour
{
    public GameObject MonsterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameController.Monster monster in GameController.Instance.AllMonsters)
        {
            GameObject monsterUI = Instantiate(MonsterPrefab);
            monsterUI.transform.SetParent(transform, false);
            monsterUI.transform.Find("Image").GetComponentInChildren<Image>().sprite = monster.image;
            monsterUI.GetComponentInChildren<TMP_Text>().text = monster.Name;
            var monsterCapture = monster;
            monsterUI.GetComponent<Button>().onClick.AddListener(() => AddMonster(monsterCapture));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMonster(GameController.Monster monster)
    {
        GameController.Instance.AddMonsterToNextSlot(monster);
    }
}
