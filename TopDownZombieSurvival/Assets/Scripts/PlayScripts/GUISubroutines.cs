using UnityEngine;

namespace Assets.Scripts
{
    public class GUISubroutines : MonoBehaviour
    {

        public GameObject Prefab;
        public GridSystem GridSystem;

        private ShopMenuBuildings shopMenuBuildings;

        void Start()
        {
            shopMenuBuildings = GetComponent<ShopMenuBuildings>();
        }

        public void OnClick()
        {
            GridSystem.SelectedBuilding = Prefab;
        }

        public void OnHover()
        {
            if (!shopMenuBuildings.BuildingPrefab == Prefab)
            {
                shopMenuBuildings.Child.SetActive(true);
                shopMenuBuildings.BuildingPrefab = Prefab;
                shopMenuBuildings.ShowStats();
            }
            else if (shopMenuBuildings.BuildingPrefab == Prefab)
                shopMenuBuildings.Child.SetActive(true);
        }

        public void OffHover()
        {
            shopMenuBuildings.Child.SetActive(false);
        }

    }
}
