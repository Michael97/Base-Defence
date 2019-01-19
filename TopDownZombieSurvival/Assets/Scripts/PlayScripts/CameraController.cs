using UnityEngine;

namespace Assets.Scripts.PlayScripts
{
    public class CameraController : MonoBehaviour {

        public GameObject player;  //The offset of the camera to centrate the player in the X axis

        public Vector2 camRange;

        public Vector3 target;

        public void NewPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            MoveCameraWithPlayer();
        }

        public void MoveCameraWithPlayer()
        {
            target = new Vector3(player.transform.position.x, 15, player.transform.position.z -11);
            
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.5f);
            float d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                if (GetComponent<Camera>().fieldOfView > camRange.x)
                {
                    GetComponent<Camera>().fieldOfView -= 2;
                }
            }
            else if (d < 0f)
            {
                if (GetComponent<Camera>().fieldOfView < camRange.y)
                {
                    GetComponent<Camera>().fieldOfView += 2;
                }
            }

        }
    }
}
