using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject HealtHBar, GameMap;
    GameObject[] LivesBar = new GameObject[20];
    void CreateLivesBar()
    {
        Vector3 GetPositionScreen = this.transform.position;
        float Dx = 0.5f;
        for(int i = 0; i < 20; i++)
        {
            LivesBar[i] = Instantiate(HealtHBar) as GameObject;
            LivesBar[i].transform.position = GetPositionScreen;
            GetPositionScreen.x += Dx;
        }
    }
    void RefresshHealth()
    {
        int L = 0;
        for ( int i = 0; i < 20; i++) LivesBar[i].GetComponent<Chanks>().index = 0;
        if (GameMap != null) L = GameMap.GetComponent<GameMap>().LifeShip();
        for (int i = 0; i < L; i++) LivesBar[i].GetComponent<Chanks>().index = 1;
    }
    void Start()
    {
        if(HealtHBar!=null) CreateLivesBar();

    }
    
    void Update()
    {
        if ((GameMap != null) && (HealtHBar != null)) RefresshHealth();
    }
}
