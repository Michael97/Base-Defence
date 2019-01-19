using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSystem : MonoBehaviour {

    public float CellSize = 1.0f;

    public Vector3 NewPosition;
    public GameObject PlacementObjects;

    public GameObject SelectedBuilding;
    public GameObject PrefabBase;
    public GameObject PrefabCannon;
    public GameObject PrefabWall;
    public GameObject PrefabDoor;

    public GameObject placementObject;

    public bool IsBuildingMode;

    public float x, y, z;

    public GameObject SellMenu;
    public GrabBuildingStats GrabBuildingStats;
    private GrabPlayerStats grabPlayerStats;
    private GameControllerScript gameControllerScript;
    private PlayerStats playerStats;


    void Start()
    {
        GrabBuildingStats = SellMenu.GetComponent<GrabBuildingStats>();
        SelectedBuilding = PrefabBase;

        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        grabPlayerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<GrabPlayerStats>();
    }

    void FixedUpdate()
    {
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ChangePlacementObject(SelectedBuilding);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        { 
            CheckForBuilding(hit);

            if (IsBuildingMode)
            {
                BuildMode();
            
                placementObject.SetActive(true);
                NewPosition = hit.point;

                x = NewPosition.x;
                z = NewPosition.z;

                x = Mathf.Round(x / CellSize) * CellSize;
                y = -0.5f;
                z = Mathf.Round(z / CellSize) * CellSize;

                placementObject.transform.position = new Vector3(x, y, z);

                if (Input.GetMouseButton(0))
                {
                    //Add this in when i add the new UI to unity, this will stop the button pressing effect the ingame world
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }

                    if (hit.transform.tag == "Ground")
                    {
                        if (CanBuy())
                        {
                            GameObject newBuilding = Instantiate(SelectedBuilding, new Vector3(x, y, z), Quaternion.identity);
                            newBuilding.transform.parent = transform;
                            
                            grabPlayerStats.BuildingBoughtSoldRepairedUpgraded();

                            if (SelectedBuilding.tag == "Base")
                                gameControllerScript.SpawnZombieSpawners();

                            /*if (SelectedBuilding.tag == "Door")
                                Physics.IgnoreCollision(SelectedBuilding.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());*/
                        }
                    }
                }
            }
            else if (!IsBuildingMode)
                placementObject.SetActive(false);
        }
    }

    public void CheckForBuilding(RaycastHit hit)
    {
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.transform.tag == "Cannon Tower" || hit.transform.tag == "Wall" ||
                         hit.transform.tag == "Door" || hit.transform.tag == "Base")
                {
                    GrabBuildingStats.BuildingPrefab = hit.transform.gameObject;
                    GrabBuildingStats.Child.SetActive(true);
                    GrabBuildingStats.SellMenuStats();
                }
                else
                {
                    GrabBuildingStats.BuildingPrefab = null;
                    GrabBuildingStats.Child.SetActive(false);
                }
            }
        }
    }

    public bool CanBuild()
    {
        //Adds the selected building type to an array
        ArrayList buildings = new ArrayList();
        buildings.AddRange(GameObject.FindGameObjectsWithTag(SelectedBuilding.tag));

        BuildingStats buildingStats = SelectedBuilding.GetComponent<BuildingStats>();

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            //checks to see if the building limit has been reached
            if (buildingStats.BuildingLimit > buildings.Count)
                return true;
            Debug.Log("building limit reached");
            return false;
        }
        return false;
    }

    public bool CanBuy()
    {
        BuildingStats buildingStats = SelectedBuilding.GetComponent<BuildingStats>();
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            if (buildingStats.Stone <= playerStats.Stone && buildingStats.Wood <= playerStats.Wood && CanBuild())
            {
                playerStats.Stone -= buildingStats.Stone;
                playerStats.Wood -= buildingStats.Wood;
                return true;
            }
                return false;
        }
            return false;
    }

    void BuildMode()
    {
        if (IsBuildingMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SelectedBuilding = PrefabBase;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SelectedBuilding = PrefabCannon;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SelectedBuilding = PrefabWall;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                SelectedBuilding = PrefabDoor;
            }
        }
    }

    void ChangePlacementObject(GameObject gameObject)
    {
        placementObject.SetActive(false);

        placementObject = PlacementObjects.transform.Find(gameObject.tag).gameObject;
        placementObject.SetActive(true);
    }
}
