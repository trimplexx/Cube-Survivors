using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Tooltip("Rozmiar chunka")] public float size; //Ustawia rozmiar pojedynczego chunka
    public Transform plane;
    private void Awake()
    {
        Resize();
    }

    private void OnValidate()
    {
        Resize();
    }

    private void Resize()
    {
        plane.localScale = Vector3.one * size; //Oblicza rozmiar powierzchni
    }
}
