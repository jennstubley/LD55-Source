using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Serializable]
    public struct Monster
    {
        public string Name;
        public GameObject Prefab;
        public Sprite image;
    }

    [Serializable]
    public struct LootType
    {
        public string Name;
        public Sprite image;
    }

    public static GameController Instance;

    public List<Monster> AllMonsters;
    public List<LootType> LootTypes;
    public string[] MonsterSlots = new string[4];
    public GameObject SummonPanel;
    public GameObject LootPrefab;
    public GameObject NextWaveButton;
    public List<GameObject> GroundEffects = new List<GameObject>();

    private List<GameObject> currentMonsters = new List<GameObject>();
    private Dictionary<string, Monster> monstersByName = new Dictionary<string, Monster>();
    private GameObject player;
    private bool IsPlaying;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Move>().gameObject;
        foreach (var monster in AllMonsters)
        {
            monstersByName.Add(monster.Name, monster);
        }
        if (Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < MonsterSlots.Length; i++)
        {
            MonsterSlots[i] = null;
        }
        //Summon(new List<string> { "Rat", "Rat" });
        //SpawnLoot(20);
    }

    // Update is called once per frame
    void Update()
    {
        bool monstersStillAlive = false;
        foreach (var monster in currentMonsters)
        {
            if (monster.gameObject.activeSelf && monster.GetComponent<Hitpoints>().Current <= 0)
            {
                monster.gameObject.SetActive(false);
            }
            else if (monster.gameObject.activeSelf)
            {
                monstersStillAlive = true;
            }
        }

        if (!monstersStillAlive && IsPlaying)
        {
            Debug.Log("You won!");
            SpawnLoot(10);
            IsPlaying = false;
            NextWaveButton.SetActive(true);
            foreach (GameObject obj in GroundEffects)
            {
                Destroy(obj);
            }
            GroundEffects.Clear();
        }
    }

    public void SummonMonsters()
    {
        Dictionary<string, int> mons = new Dictionary<string, int>();
        bool hasMonsters = false;
        foreach (var monName in MonsterSlots)
        {
            if (monName != null && monName != "")
            {
                hasMonsters = true;
                if (mons.ContainsKey(monName))
                {
                    mons[monName] += 1;
                }
                else
                {
                    mons.Add(monName, 1);
                }
            }
        }
        if (hasMonsters)
        {
            Summon(mons);
            SummonPanel.SetActive(false);
            player.GetComponent<Hitpoints>().Reset();
            IsPlaying = true;
        }
    }


    private void Summon(Dictionary<string, int> monsters)
    {
        foreach (var monster in currentMonsters)
        {
            Destroy(monster);
        }
        currentMonsters.Clear();
        foreach (var monster in monsters)
        {
            for (int i=0; i<monster.Value; ++i)
            {
                GameObject monsterObj = Instantiate(monstersByName[monster.Key].Prefab);
                currentMonsters.Add(monsterObj);
                if (monsterObj.GetComponent<AIMove>())
                {
                    monsterObj.GetComponent<AIMove>().StartPosIndex = i;
                }
                if (monsterObj.GetComponent<AIChase>())
                {
                    monsterObj.GetComponent<AIChase>().Target = FindObjectOfType<Move>().gameObject;
                }
                if (monsterObj.GetComponent<AIStartPosition>())
                {
                    monsterObj.GetComponent<AIStartPosition>().StartPosIndex = i;
                }
                if (monsterObj.GetComponent<AIProjectileAttack>())
                {
                    monsterObj.GetComponent<AIProjectileAttack>().Target = FindObjectOfType<Move>().gameObject;
                }
            }
        }
    }

    public Monster GetSlotMonster(int idx)
    {
        return monstersByName[MonsterSlots[idx]];
    }

    public void AddMonsterToNextSlot(Monster monster)
    {
        int idxToUse = -1;
        for (int i = 0; i < MonsterSlots.Length; i++)
        {
            string monString = MonsterSlots[i];
            if (monString == null || monString == "")
            {
                idxToUse = i;
                break;
            }
        }

        if (idxToUse >= 0)
        {
            MonsterSlots[idxToUse] = monster.Name;
        }
    }

    public void NextWave()
    {
        for (int i = 0; i < MonsterSlots.Length; i++)
        {
            MonsterSlots[i] = null;
        }
        SummonPanel.SetActive(true);
        NextWaveButton.SetActive(false);
    }

    public void RemoveMonsterFromSlot(int slotID)
    {
        MonsterSlots[slotID] = null;
    }

    private void SpawnLoot(int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            LootType lootType = LootTypes[UnityEngine.Random.Range(0, LootTypes.Count)];
            GameObject obj = Instantiate(LootPrefab);
            obj.transform.position = Vector3.zero;
            obj.GetComponent<Loot>().Name = lootType.Name;
            obj.GetComponentInChildren<SpriteRenderer>().sprite = lootType.image;
        }
    }
}
