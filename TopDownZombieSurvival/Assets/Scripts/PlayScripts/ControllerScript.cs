using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts;
using Assets.Scripts.PlayScripts;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(HealthScript))]
public class ControllerScript : MonoBehaviour
{
    public Animator MyAnimator;
    public Vector3 NewPosition;
    public float Speed;
    public float WalkRange;

    public bool IsAlive;
    public GameObject Gun;

    public GameObject Projectile;
    public GameObject Target;

    private HealthScript healthScript;

    //Harvesting
    public int HarvestAmount;

    public float HarvestRange;
    public float HarvestCoolDown;

    private float time;

    public enum PlayerState
    {
        Idle,
        Moving,
        Shooting,
        Dead,
        Harvesting,
        Building
    }

    public PlayerState EPlayerState;
   

    public NavMeshAgent NavMeshAgent;
    public ParticleSystem Flash;

    public MeshRenderer GunMesh;

    public PlayerStats playerStats;
    private PlayerShootScript shootScript;
    public GrabPlayerStats grabPlayerStats;
    private CameraController cameraController;

    // Use this for initialization
    void Start ()
    {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        cameraController.NewPlayer();
	    Flash = Gun.transform.GetComponentInChildren<ParticleSystem>();
        NewPosition = transform.position;
	    shootScript = GetComponent<PlayerShootScript>();
	    GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
	    healthScript = player.GetComponent<HealthScript>();
        grabPlayerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<GrabPlayerStats>();
        grabPlayerStats.NewPlayer();
        Flash.Stop();
        //Grabbing Necassary Components
        MyAnimator = GetComponent<Animator>();
        NavMeshAgent = GetComponent<NavMeshAgent>();

        //Setting state to Idle
	    EPlayerState = PlayerState.Idle;

	    StartCoroutine(Alive());

       
	}

	// Update is called once per frame
	void Update () {
        
	    bool rightMouseButton = Input.GetMouseButton(1);

        
	    if (healthScript.CurrentHealth <= 0)
	    {
	        IsAlive = false;
	        EPlayerState = PlayerState.Dead;
	    }
	    HarvestCoolDown -= Time.deltaTime;
	    if (EPlayerState != PlayerState.Building)
	    {
	        if (rightMouseButton)
	        {
	            RaycastHit hit;
	            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	            //If ground hit move
	            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
	            {
	                if (hit.transform.tag == "Ground")
	                {
	                    Target = hit.transform.gameObject;
	                    NewPosition = hit.point;
                        
	                    EPlayerState = PlayerState.Moving;
	                }
	                if (hit.transform.tag == "Zombie")
	                {
	                    Target = hit.transform.gameObject;
	                    EPlayerState = PlayerState.Shooting;
	                }
	                if (hit.transform.tag == "Resource")
	                {
	                    Target = hit.transform.gameObject;
	                    EPlayerState = PlayerState.Harvesting;
	                }
	                if (hit.transform.tag == null)
	                    return;
	            }
	        }
	    }
	    



        //Use A* pathfinding method for player
    }

    IEnumerator Alive()
    {
        while (IsAlive)
        {
            switch (EPlayerState)
            {
                case PlayerState.Idle:

                    if (canShoot())
                    {
                        if(!shootScript)
                            shootScript = GetComponent<PlayerShootScript>();
                        shootScript.Shoot();
                    }

                    MyAnimator.SetFloat("VSpeed", 0);
                    break;
                case PlayerState.Moving:
                    MyAnimator.SetFloat("VSpeed", 1);
                    if (Vector3.Distance(NewPosition, transform.position) > WalkRange)
                    {
                        Move();
                    }
                    else
                    {
                        MyAnimator.SetFloat("VSpeed", 0);
                        EPlayerState = PlayerState.Idle;
                    }
                    break;
                case PlayerState.Shooting:
                    if (canShoot())
                    {
                        if(!shootScript)
                            shootScript = GetComponent<PlayerShootScript>();
                        shootScript.Shoot();
                    }
                    else
                        EPlayerState = PlayerState.Idle;

                    break;
                case PlayerState.Building:
                    break;
                case PlayerState.Harvesting:
                    if (Target != null)
                    {
                        //if within range of resource
                        if (Vector3.Distance(Target.transform.position, transform.position) <= HarvestRange)
                        {
                            MyAnimator.SetFloat("VSpeed", 0);
                            //if harvest cool down is 0
                            if (HarvestCoolDown <= 0.0f)
                            {
                                //run harvest animation
                                Target.transform.gameObject.GetComponent<ResourceScript>().ResourceCount -= playerStats.HarvestAmount;
                                Target.transform.gameObject.GetComponent<ResourceScript>().Hit();
                                if (Target.transform.Find("Wood"))
                                {
                                    Target.GetComponent<AudioSource>().Play();
                                    grabPlayerStats.UpdateWood();
                                    playerStats.Wood += playerStats.HarvestAmount;
                                }
                                if (Target.transform.Find("Stone"))
                                {
                                    grabPlayerStats.UpdateStone();
                                    playerStats.Stone += playerStats.HarvestAmount;
                                }
                                HarvestCoolDown = 0.5f;
                            }
                        }
                        else //If out of range, run towards it
                        {
                            MyAnimator.SetFloat("VSpeed", 1);
                            NewPosition = Target.transform.position;
                            Move();
                        }
            
                    }
                    else
                        EPlayerState = PlayerState.Idle;
                    break;
                case PlayerState.Dead:
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    

    private bool isMoving()
    {
        if (Vector3.Distance(NewPosition, transform.position) <= 0)
            return true;
        return false;
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, NewPosition, Speed * Time.deltaTime);
        GunMesh.enabled = false;
        Quaternion transformRotationQuaternion =
            Quaternion.LookRotation(NewPosition - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transformRotationQuaternion, transform.rotation, 0.8f);
        cameraController.MoveCameraWithPlayer();
    }

    public void SetGunRenderer(bool set)
    {
        GunMesh.enabled = set;
    }

    private bool canShoot()
    {
        if (Target != null)
        {
            if (Target.tag == "Zombie")
            {
                if (Target.GetComponent<HealthScript>().IsAlive)
                {
                    return true;
                }
                SetGunRenderer(false);
                Target = null;
                return false;
            }
            return false;
        }
        return false;
    }
}
