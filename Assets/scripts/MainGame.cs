using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public int GameMode = 0;
    public GameObject PlayerMap, ComputerMap, Player;
    bool whoseMove = true;
    void OnGUI()
    {
        Camera cam;
        float CenterScreenX = Screen.width / 2;
        float CenterScreenY = Screen.height / 2;
        Rect LocationButton;
        GameMap PLayerMapControl = PlayerMap.GetComponent<GameMap>();
        switch (GameMode)
        {
            case 0:
                // налаштовуємо камеру
                cam = GetComponent<Camera>();
                cam.orthographicSize = 8;
                this.transform.position = new Vector3(0, 0, -10);
                // Створюємо прямокутник для заднього фона
                LocationButton = new Rect(new Vector2(CenterScreenX - 150, CenterScreenY - 50), new Vector2(300, 200));
                GUI.Box(LocationButton, "");
                LocationButton = new Rect(new Vector2(CenterScreenX - 40, CenterScreenY - 40), new Vector2(200, 30));
                GUI.Label(LocationButton, "SpaceSich Menu");
                //KNOPKA
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, CenterScreenY), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "START"))
                    GameMode = 1;
                //KNOPKA EXITY
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, CenterScreenY + 40), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "EXIT"))
                    Application.Quit();
                break;
            case 1:
                cam = GetComponent<Camera>();
                cam.orthographicSize = 8;
                this.transform.position = new Vector3(50, 0, -10);
                // Створюємо прямокутник для заднього фона
                LocationButton = new Rect(new Vector2(CenterScreenX - 150, 0), new Vector2(300, 200));

                GUI.Box(LocationButton, ""); LocationButton = new Rect(new Vector2(CenterScreenX - 10, 10), new Vector2(200, 20));
                GUI.Label(LocationButton, "Angar");
                //Повертаємося у меню гри
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 50), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "Exit to main menu!"))
                {
                    PLayerMapControl.ClearMap();
                    GameMode = 0;
                }
                //генератор поля
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 90), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "To place a fleet!"))
                {
                    PLayerMapControl.EnterRandomShip();
                }
                if (PLayerMapControl.LifeShip() == 20)
                {
                    LocationButton = new Rect(new Vector2(CenterScreenX - 100, 130), new Vector2(200, 30));
                    if (GUI.Button(LocationButton, "Fight!"))
                    {
                        GameMode = 3;
                        PlayerMap.GetComponent<GameMap>().CopyMap();

                        ComputerMap.GetComponent<GameMap>().EnterRandomShip();
                    }
                }
                break;
            case 3:
                this.transform.position = new Vector3(50, -30, -10);
                cam = GetComponent<Camera>();
                cam.orthographicSize = 9;
                break;
            case 4:
                this.transform.position = new Vector3(100, 0, -10);
                LocationButton = new Rect(new Vector2(CenterScreenX - 150, 0), new Vector2(300, 200));

                GUI.Box(LocationButton, "");
                LocationButton = new Rect(new Vector2(CenterScreenX - 10, 10), new Vector2(200, 30));

                GUI.Box(LocationButton, "Menu");
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 50), new Vector2(200, 30));
                if(GUI.Button(LocationButton, "Exit to main menu"))
                {
                    PLayerMapControl.ClearMap();
                    GameMode = 0;
                }
                break;

            case 5:
                this.transform.position = new Vector3(150, 0, -10);
                LocationButton = new Rect(new Vector2(CenterScreenX - 150, 0), new Vector2(300, 200));

                GUI.Box(LocationButton, "");
                LocationButton = new Rect(new Vector2(CenterScreenX - 10, 10), new Vector2(200, 30));

                GUI.Box(LocationButton, "Menu");
                LocationButton = new Rect(new Vector2(CenterScreenX - 100, 50), new Vector2(200, 30));
                if (GUI.Button(LocationButton, "Exit to main menu"))
                {
                    PLayerMapControl.ClearMap();
                    GameMode = 0;
                }
                break;
        }
    }
    public void UserClick(int x, int y)
    {
        Debug.Log("Click");
        if (whoseMove)
        {
            whoseMove = ComputerMap.GetComponent<GameMap>().ShootController(x, y);
        }
    }

    GameMap.TestCoord Homming()
    {
        GameMap.TestCoord xy = new GameMap.TestCoord();
        xy.x = -1;
        xy.x = -1;
        foreach(GameMap.Ship Test in Player.GetComponent<GameMap>().ListShip)
        {
            foreach(GameMap.TestCoord Paluba in Test.ShipCoord)
            {
                int index = Player.GetComponent<GameMap>().GetIndexBlock(Paluba.x, Paluba.y);
                if(index == 1)
                {
                    return Paluba;
                }
            }
        }
        return xy;
    }
    int ShootCount = 0;
    // створюємо бота якаий буде грати проти нас
    void ArtificialIntelligence()
    {
        if (!whoseMove)
        {
            int ShootX = Random.RandomRange(0, 9);

            int ShootY = Random.RandomRange(0, 9);

            int PC_Ship = ComputerMap.GetComponent<GameMap>().LifeShip();
            
            if (PC_Ship < 10)
            {
                if (ShootCount == 0)
                {
                    GameMap.TestCoord xy = Homming();
                    if ((xy.x >= 0) && (xy.y >= 0))
                    {
                        ShootX = xy.x;
                        ShootY = xy.y;
                    }
                    ShootCount++;
                }
                else
                {
                    ShootCount = 0;
                }
            }
            // перевірка чи попав бот. Якщо попав залишаємо хід за ним
            whoseMove = !Player.GetComponent<GameMap>().ShootController(ShootX, ShootY);
           
        }
    }
    void TestWhoWin()
    {
        int PC_Ship = ComputerMap.GetComponent<GameMap>().LifeShip();
        int Player_Ship = Player.GetComponent<GameMap>().LifeShip();
        if (PC_Ship == 0) GameMode = 4;
        if (Player_Ship == 0) GameMode = 5;
    }
    void Start()
    {

    }


    void Update()
    {
        if(GameMode == 3)
        {
            TestWhoWin();
            ArtificialIntelligence();
        }
    }
}
