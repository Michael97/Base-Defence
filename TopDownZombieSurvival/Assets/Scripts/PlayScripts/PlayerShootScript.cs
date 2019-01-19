using UnityEngine;

namespace Assets.Scripts.PlayScripts
{
    public class PlayerShootScript : MonoBehaviour
    {

        public float AttackRange;
        public float AttackCoolDown;
        //public GameObject Target;

        public GameObject Projectile;

        public AudioSource GunShot;

        private ControllerScript controllerScript;

        // Use this for initialization
        void Start()
        {
            controllerScript = GetComponent<ControllerScript>();
            controllerScript.SetGunRenderer(false);

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            AttackCoolDown -= Time.deltaTime;
        }

        public void Shoot()
        {
            if (Vector3.Distance(controllerScript.Target.transform.position, transform.position) <= AttackRange)
            {
                if (AttackCoolDown <= 0.0f)
                {
                    GunShot.Play();
                    controllerScript.MyAnimator.SetFloat("VSpeed", 0);
                    transform.rotation =
                        Quaternion.LookRotation(controllerScript.Target.transform.position - transform.position, Vector3.up);
                    controllerScript.Flash.Play();
                    controllerScript.SetGunRenderer(true);
                    //insert shoot animation
                    GameObject myProjectile = Instantiate(Projectile, transform.position, transform.rotation);
                    myProjectile.GetComponent<AttackScript>().Target = controllerScript.Target.transform;
                    myProjectile.GetComponent<AttackScript>().Origin = transform.position;
                    AttackCoolDown = 0.5f;
                }


            }
            //if out of attack range, run towards the target
            else if (Vector3.Distance(controllerScript.Target.transform.position, transform.position) >= AttackRange)
            {
                controllerScript.MyAnimator.SetFloat("VSpeed", 1);
                controllerScript.NewPosition = controllerScript.Target.transform.position;
                controllerScript.Move();
            }
        }
    }
}
