using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class MonsterListUI : MonoBehaviour
{
    public GameObject MonsterPrefab;
    public Sprite LockedImage;

    public Dictionary<GameController.Monster, GameObject> LockedMonsters = new Dictionary<GameController.Monster, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameController.Monster monster in GameController.Instance.AllMonsters)
        {
            bool isMonsterUnlocked = GameController.Instance.IsMonsterUnlocked(monster.Name);
            GameObject monsterUI = Instantiate(MonsterPrefab);
            monsterUI.transform.SetParent(transform, false);
            monsterUI.transform.Find("Image").GetComponentInChildren<Image>().sprite = isMonsterUnlocked ? monster.image : LockedImage;
            monsterUI.GetComponentInChildren<TMP_Text>().text = isMonsterUnlocked ? monster.Name : "Locked";
            if (isMonsterUnlocked)
            {
                var monsterCapture = monster;
                monsterUI.GetComponent<Button>().onClick.AddListener(() => AddMonster(monsterCapture));
            }
            else
            {
                LockedMonsters.Add(monster, monsterUI);
            }
            monsterUI.GetComponent<Button>().onClick.AddListener(() => AudioController.Instance.PlayClick());
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<GameController.Monster> monstersToUnlock = new List<GameController.Monster>();
        foreach (var monster in LockedMonsters.Keys)
        {
            if (GameController.Instance.IsMonsterUnlocked(monster.Name))
            {
                LockedMonsters[monster].transform.Find("Image").GetComponentInChildren<Image>().sprite = monster.image;
                LockedMonsters[monster].GetComponentInChildren<TMP_Text>().text = monster.Name;
                var monsterCapture = monster;
                LockedMonsters[monster].GetComponent<Button>().onClick.AddListener(() => AddMonster(monsterCapture));
                monstersToUnlock.Add(monster);
            }
        }

        foreach (GameController.Monster monster in monstersToUnlock)
        {
            LockedMonsters.Remove(monster);
        }
    }

    public void AddMonster(GameController.Monster monster)
    {
        GameController.Instance.AddMonsterToNextSlot(monster);
    }
}
