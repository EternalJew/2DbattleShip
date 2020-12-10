using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject MyMap;
    void OnGUI()
    {
        Rect LocationButton;
        LocationButton = new Rect(new Vector2(10, 10), new Vector2(200, 40));
        if (GUI.Button(LocationButton, "Generate Ships")) MyMap.GetComponent<GameMap>().EnterRandomShip();
        LocationButton = new Rect(new Vector2(10, 50), new Vector2(200, 40));
        if (GUI.Button(LocationButton, "Copy in second rang")) MyMap.GetComponent<GameMap>().CopyMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
