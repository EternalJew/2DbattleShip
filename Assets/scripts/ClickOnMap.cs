using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnMap : MonoBehaviour
{
    public GameObject WhoParents = null;
    public int posX, posY;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
     void OnMouseDown()
    {
       if(WhoParents != null)
        {
            WhoParents.GetComponent<GameMap>().OnClick(posX,posY);
        } 
    }
}
