using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameController;

public class GameController : MonoBehaviour
{
    [Serializable]
    public struct Monster
    {
        public string Name;
        public GameObject Prefab;
        public Sprite image;
        public int RewardPoints;
    }

    [Serializable]
    public struct LootType
    {
        public string Name;
        public Sprite image;
        public int UnlockRound;
        public string UnlockMonster;
        public int UnlockSlotsFilled;
    }

    public static GameController Instance;

    public List<Monster> AllMonsters;
    public List<LootType> BasicLoot;
    public List<LootType> SpecialLoot;
    Dictionary<LootType, bool> SpecialLootLocks = new Dictionary<LootType, bool>();
    public string[] MonsterSlots = new string[4];
    public int UnlockedSlots = 1;
    public GameObject SummonPanel;
    public GameObject DeathScreen;
    public GameObject WinScreen;
    public GameObject LootPrefab;
    public GameObject NextWaveButton;
    public List<GameObject> GroundEffects = new List<GameObject>();
    public int RewardMultiplier;
    public int WaveCounter;

    private List<GameObject> currentMonsters = new List<GameObject>();
    private Dictionary<string, Monster> monstersByName = new Dictionary<string, Monster>();
    private Dictionary<Monster, bool> monsterLocks = new Dictionary<Monster, bool>();
    private GameObject player;
    private bool IsPlaying;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Move>().gameObject;
        foreach (var monster in AllMonsters)
        {
            monstersByName.Add(monster.Name, monster);
            monsterLocks.Add(monster, monster.Name == "Rat" ? true : false);
        }
        if (Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < MonsterSlots.Length; i++)
        {
            MonsterSlots[i] = null;
        }
        foreach (var loot in SpecialLoot)
        {
            SpecialLootLocks.Add(loot, false);
        }
        RecalculateRewardMulti();
        NextWave();
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
            SpawnLoot(RewardMultiplier);
            IsPlaying = false;
            NextWaveButton.SetActive(true);
            foreach (GameObject obj in GroundEffects)
            {
                Destroy(obj);
            }
            GroundEffects.Clear();
        }

        if (NextWaveButton.activeSelf && Input.GetKeyUp(KeyCode.Return))
        {
            NextWave();
        }
    }

    public void UnlockMonster(string name)
    {
        monsterLocks[monstersByName[name]] = true;
    }

    public bool IsMonsterUnlocked(string name)
    {
        return monsterLocks[monstersByName[name]];
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

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
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

        // Move the player to the bottom right.
        FindObjectOfType<Move>().transform.position = new Vector3(6, -4.5f, 0);
    }

    public void RecalculateRewardMulti()
    {
        RewardMultiplier = 0;
        List<string> monsterTypes = new List<string>();
        int usedSlots = 0;
        for (int i = 0; i < UnlockedSlots; i++)
        {
            string monName = MonsterSlots[i];
            if (monName != null && monName != "")
            {
                RewardMultiplier += monstersByName[monName].RewardPoints;
                usedSlots++;
                if (!monsterTypes.Contains(monName))
                {
                    monsterTypes.Add(monName);
                }
            }
        }

        if (usedSlots > 0)
        {
            RewardMultiplier += (usedSlots - 1);
        }
        if (monsterTypes.Count > 1)
        {
            RewardMultiplier += (monsterTypes.Count - 1) * 2;
        }
    }


    public Monster GetSlotMonster(int idx)
    {
        return monstersByName[MonsterSlots[idx]];
    }

    public void AddMonsterToNextSlot(Monster monster)
    {
        int idxToUse = -1;
        for (int i = 0; i < UnlockedSlots; i++)
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
            RecalculateRewardMulti();
        }
    }

    public void NextWave()
    {
        WaveCounter++;
        for (int i = 0; i < MonsterSlots.Length; i++)
        {
            MonsterSlots[i] = null;
        }
        RecalculateRewardMulti();
        SummonPanel.SetActive(true);
        DeathScreen.SetActive(false);
        NextWaveButton.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
        foreach (var monster in currentMonsters)
        {
            Destroy(monster.gameObject);
        }
        currentMonsters.Clear();
        IsPlaying = false;
        foreach (GameObject obj in GroundEffects)
        {
            Destroy(obj);
        }
        GroundEffects.Clear();
    }

    public void RemoveMonsterFromSlot(int slotID)
    {
        MonsterSlots[slotID] = null;
        RecalculateRewardMulti();
    }

    private void SpawnLoot(int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            LootType lootType = BasicLoot[UnityEngine.Random.Range(0, BasicLoot.Count - 1)];
            SpawnLootType(lootType);
            // Spawn coins.
            SpawnLootType(BasicLoot[BasicLoot.Count - 1]);
        }
        for (int i = 0; i < 3; ++i)
        {
            // Spawn bonus coins
            SpawnLootType(BasicLoot[BasicLoot.Count - 1]);
        }
        for (int i=0; i< SpecialLoot.Count; ++i)
        {
            LootType loot = SpecialLoot[i];
            if (SpecialLootLocks[loot]) continue;
            if (loot.UnlockRound > 0 && WaveCounter >= loot.UnlockRound)
            {
                SpawnLootType(loot);
                SpecialLootLocks[loot] = true;
                continue;
            }
            if (loot.UnlockSlotsFilled > 0 && NumSlotsFilled() >= loot.UnlockSlotsFilled)
            {
                SpawnLootType(loot);
                SpecialLootLocks[loot] = true;
                continue;
            }
            if (loot.UnlockMonster != "" && MonsterWasSummoned(loot.UnlockMonster))
            {
                SpawnLootType(loot);
                SpecialLootLocks[loot] = true;
                continue;
            }
        }
    }

    private bool MonsterWasSummoned(string monName)
    {
        for (int i = 0; i < UnlockedSlots; i++)
        {
            if (MonsterSlots[i] != null && MonsterSlots[i].Equals(monName)) return true;
        }
        return false;
    }

    private int NumSlotsFilled()
    {
        int res = 0;
        for (int i = 0; i < UnlockedSlots; i++)
        {
            if (MonsterSlots[i] != null) res++;
        }
        return res;
    }

    private void SpawnLootType(LootType loot)
    {
        GameObject obj = Instantiate(LootPrefab);
        obj.transform.position = Vector3.zero;
        obj.GetComponent<Loot>().Name = loot.Name;
        obj.GetComponentInChildren<SpriteRenderer>().sprite = loot.image;
    }

    public void UnlockNextSlot()
    {
        UnlockedSlots++;
    }

    internal void YouWin()
    {
        DeathScreen.SetActive(false);
        WinScreen.SetActive(true);
        SummonPanel.SetActive(false);
        NextWaveButton.SetActive(false);
        foreach (var monster in currentMonsters)
        {
            Destroy(monster.gameObject);
        }
        currentMonsters.Clear();
        IsPlaying = false;
        foreach (GameObject obj in GroundEffects)
        {
            Destroy(obj);
        }
        GroundEffects.Clear();
    }
}
