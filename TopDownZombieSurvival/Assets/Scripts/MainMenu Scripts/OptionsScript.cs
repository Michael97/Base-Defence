using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class OptionsScript : MonoBehaviour
{

    public GameObject Child;
    public bool IsActive;

    void Start()
    {
        Child.SetActive(false);
    }

    public void ChangeActive()
    {
        IsActive = !IsActive;
        Child.SetActive(IsActive);
    }
}
