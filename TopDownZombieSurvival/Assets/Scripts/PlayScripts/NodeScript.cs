using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour {

  /*  public bool HasBuilding;
    public bool IsBuildingMode;
    public GameObject Grid;
    public GameObject ChildNode;
    public bool IsSelected;
    public Renderer renderer;
    private Color color;
    // Use this for initialization
    void Start ()
	{
        Grid = GameObject.FindGameObjectWithTag("Grid");
	    //renderer = GetComponentInChildren<Renderer>();
        //ChildNode = this.transform.FindChild("Node").gameObject;
	    renderer.enabled = false;
        ChildNode.SetActive(false);
	}

    void changeAlpha(float _color)
    {
        color.a = _color;
        renderer.material.color = new Color(1,1,1, _color);
    }

    private void IsHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            if (hit.collider.transform == ChildNode.transform)
            {
                IsSelected = true;
            }
            else
            {
                IsSelected = false;
            }
        else
        {
            IsSelected = false;
        }
    }

    // Update is called once per frame
	void Update ()
	{
	    IsBuildingMode = Grid.GetComponent<GenerateGrid>().IsBuildingMode;
        
        if (IsBuildingMode)
	    {
	        ChildNode.SetActive(true);
            IsHit();
	        if (!HasBuilding)
	        {
	            if (IsSelected)
	            {
	                renderer.enabled = true;
	            }
	            else if (!IsSelected)
	            {
	                renderer.enabled = false;
	            }
	        }
	        if (HasBuilding)
	        {
	            
	        }
	    }
	    else if (!IsBuildingMode)
	        ChildNode.SetActive(false);





	}*/
}
