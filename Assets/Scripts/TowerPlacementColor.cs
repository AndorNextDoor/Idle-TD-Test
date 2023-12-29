using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementColor : MonoBehaviour
{
    [SerializeField] private GameObject[] materialsToChange;
    

    public void SetMaterial(Material material)
    {
        foreach (var mat in materialsToChange)
        {
            mat.GetComponent<Renderer>().material = material;
        }
    }
}
