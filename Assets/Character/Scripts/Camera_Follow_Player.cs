using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow_Player : MonoBehaviour
{

    public GameObject character; //zmienna do której wrzucamy postać, za którą podążą kamera

    public static Camera_Follow_Player Instance;

    public float cameraHeight;

    //Start is called before the first frame update
    void Start()
    {
        cameraHeight = 22;
    }

    //Update is called once per frame
    void Update()
    {
        transform.position = character.transform.position + new Vector3(0, cameraHeight, -cameraHeight / (1.7f));
    }

    private void Awake()
    {
        // Create the singleton instance
        Instance = this;
    }
}
