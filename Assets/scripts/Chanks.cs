using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chanks : MonoBehaviour
{
    public Sprite[] images;

    public int index = 0;

    public bool HideChank = false;

    void ChangeImages()
    {
        if(images.Length > index)
        {
            if ((HideChank) && (index == 1)) GetComponent<SpriteRenderer>().sprite = images[0];
            else
                GetComponent<SpriteRenderer>().sprite = images[index];
        }
    }
    void Start()
    {
        ChangeImages();
    }

   
    void Update()
    {
        ChangeImages();
    }
}
