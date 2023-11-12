    using System.Collections.Generic;
    using UnityEngine;
    
    [System.Serializable]
    public class BiomeSettings
    {
        [Tooltip("Nazwa biomu")] public string Name;
        [Tooltip("Obiekt")] public List<GameObject> Prefabs = new List<GameObject>();
    }