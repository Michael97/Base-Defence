using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public int Width;
    public int Height;
    public float y;
    public GameObject Node;
    public GameObject[] Nodes;
    public Vector3 Size;
    public bool IsBuildingMode;

	// Use this for initialization
	void Awake ()
	{
	    CreateGrid();
	}

    public void CreateGrid()
    {
        for (int x = -15; x < Size.x; x++)
        {
            for (int z = -15; z < Size.z; z++)
            {
                GameObject node = Instantiate(Node, new Vector3(x, y, z), Quaternion.identity);
                node.transform.parent = transform;
            }
        }

        Nodes = GameObject.FindGameObjectsWithTag("Node");
    }
}
