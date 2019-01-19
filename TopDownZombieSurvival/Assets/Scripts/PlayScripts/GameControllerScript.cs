using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControllerScript : MonoBehaviour {

    public GameObject[] MiscSpawners;
    public GameObject[] ZombieSpawners;

    public Button ShopButton;
    private bool shopOpen;

    public int WaveNumber;

    public GameObject Terrain;

    public int ResourceSpawnAmount;
    public int ZombieSpawnerSpawnAmount;

    public GameObject Zombie;
    public GameObject Boulder;
    public GameObject Tree;
    public GameObject ZombieSpawner;

    public GameObject Grid;
    
    public GameObject Player;
    public GameObject PlayerPrefab;

    public GameObject Base;

    public float zombieSpawn;

    private PlayerStats playerStats;
    private HealthScript healthStats;
    private WaveMenu waveMenu;

    public float time;

    public float DayNightCycleTime;

    public enum GameState
    {
        Play,
        Building,
        Dead
    }

    public GameState EGameState;

    // Use this for initialization
    void Start () {
        MiscSpawner();
        zombieSpawn = 5.0f;
        Grid = GameObject.FindGameObjectWithTag("Grid");
        waveMenu = GameObject.FindGameObjectWithTag("WaveMenu").GetComponent<WaveMenu>();
        ShopButton.onClick.AddListener(delegate { IsClicked(); });

        Player = Instantiate(PlayerPrefab, Base.transform.position, Base.transform.rotation);
        NewPlayer();
        EGameState = GameState.Play;
        StartCoroutine(StateEnumerator());
    }

    public void NewPlayer()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void BasePlaced(GameObject gameObject)
    {
        Base = gameObject;
    }

    void FixedUpdate()
    {
        if (DayNightCycleTime >= 0.0f)
        {

            DayNightCycleTime -= Time.deltaTime;
            waveMenu.UpdateTime(DayNightCycleTime);
        }
        else
            SpawnZombieWaves();
    }

    void SpawnZombieWaves()
    {
        ArrayList zombieSpawners = new ArrayList();
        zombieSpawners.AddRange(GameObject.FindGameObjectsWithTag("ZombieSpawner"));

        foreach (GameObject zomb in zombieSpawners)
        {
            ZombieSpawnerScript script = zomb.GetComponent<ZombieSpawnerScript>();
            script.UpdateSpawnAmount();
            script.SpawnZombie();
        }
        WaveNumber++;
        waveMenu.UpdateWave(WaveNumber);
        DayNightCycleTime = 90.0f;
    }

    IEnumerator StateEnumerator()
    {
        while (true)
        {
            switch (EGameState)
            {
                case GameState.Play:
                    if (Input.GetKeyDown(KeyCode.B))
                       IsClicked();

                    break;
                case GameState.Building:
                    if (Input.GetKeyDown(KeyCode.B))
                        IsClicked();

                    break;
                case GameState.Dead:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return null;
        }
        yield return null;
    }

    public void IsClicked()
    {
        shopOpen = !shopOpen;
        if (shopOpen)
        {
            ShopState(GameState.Building, shopOpen);
            Player.GetComponent<ControllerScript>().EPlayerState =
                ControllerScript.PlayerState.Building;
        }
        if (!shopOpen)
        {
            ShopState(GameState.Play, shopOpen);
            Player.GetComponent<ControllerScript>().EPlayerState =
                ControllerScript.PlayerState.Idle;
            EGameState = GameState.Play;
        }
    }

    private void ShopState(GameState state, bool boolean)
    {
        EGameState = state;
        Grid.GetComponent<GridSystem>().IsBuildingMode = boolean;
    }

    public void RespawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, Base.transform.position, Base.transform.rotation);
        playerStats = GetComponent<PlayerStats>();
        healthStats = GetComponent<HealthScript>();
    }

    public void ZombieSpawn()
    {
        GameObject mySpawn = ZombieSpawners[Random.Range(0, ZombieSpawners.Length)];
        GameObject myZombie = Instantiate(Zombie, mySpawn.transform.position, mySpawn.transform.rotation);

        myZombie.GetComponent<NavMeshAgent>().enabled = true;
        myZombie.GetComponent<NormalZombieScript>().enabled = true;
    }

    public void MiscSpawner()
    {
        GameObject mySpawn = MiscSpawners[Random.Range(0, MiscSpawners.Length)];
        int resourceAmount;
        for (resourceAmount = 0; resourceAmount < ResourceSpawnAmount; resourceAmount++)
        {
            GameObject myTree = Instantiate(Tree, mySpawn.transform.position, mySpawn.transform.rotation);
            GameObject myBoulder = Instantiate(Boulder, mySpawn.transform.position, mySpawn.transform.rotation);
           

            myTree.GetComponent<GenerateSpawnScript>().enabled = true;
            myBoulder.GetComponent<GenerateSpawnScript>().enabled = true;
        }
        
    }

    public void SpawnZombieSpawners()
    {    
        ArrayList zombieSpawners = new ArrayList();
        zombieSpawners.AddRange(GameObject.FindGameObjectsWithTag("ZombieSpawner"));

        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        
        if (baseObject != null)
        {
            if (zombieSpawners.Count < ZombieSpawnerSpawnAmount)
            {
                int zombieSpawnerAmount;

                for (zombieSpawnerAmount = 0; zombieSpawnerAmount < ZombieSpawnerSpawnAmount; zombieSpawnerAmount++)
                {
                    GameObject myZombieSpawner = Instantiate(ZombieSpawner, Base.transform.position,
                        Base.transform.rotation);

                    myZombieSpawner.GetComponent<GenerateSpawnScript>().enabled = true;
                }
            }
        }
    }
}
