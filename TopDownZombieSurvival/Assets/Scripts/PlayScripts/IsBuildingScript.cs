using UnityEngine;

namespace Assets.Scripts
{
    public class IsBuildingScript : MonoBehaviour
    {

        public GridSystem GridSystem;
        public GameObject Child;


        // Use this for initialization
        void Start ()
        {
            GridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        }
	
        // Update is called once per frame
        void Update () {
            if (GridSystem.IsBuildingMode)
            {
                Child.SetActive(true);
            }
            else if (!GridSystem.IsBuildingMode)
            {
                Child.SetActive(false);
            }
        }
    }
}
