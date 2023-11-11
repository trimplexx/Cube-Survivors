using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Tooltip("Rozmiar chunka")] public float size; //Ustawia rozmiar pojedynczego chunka
    public Transform plane;
    private Renderer planeRenderer;
    private void Awake()
    {
        planeRenderer = plane.GetChild(0).GetComponent<Renderer>();
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }

    private void Initialize()
    {
        Resize();
    }

    private void Resize()
    {
        plane.localScale = Vector3.one * size; //Oblicza rozmiar powierzchni
    }

    public void Paint(Color color)
    {
        planeRenderer.material.color = color;
    }
}
