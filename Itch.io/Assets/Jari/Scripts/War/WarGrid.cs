using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarGrid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject gridParent;

    [Header("Grid Settings")]
    [SerializeField] private int collumLength;
    [SerializeField] private int rowLength;

    [Header("Grid Offset")]
    [SerializeField] private float xStart;
    [SerializeField] private float yStart;
    [SerializeField] private float xSpace, ySpace;

    public void CreateGrid()
    {
        for (int i = 0; i < collumLength * rowLength; i++)
        {
            Instantiate(tile, new Vector3(xStart + (xSpace * (i % collumLength)), 0, yStart + (ySpace * (i / collumLength))), Quaternion.identity, gridParent.transform);
        }
    }
}