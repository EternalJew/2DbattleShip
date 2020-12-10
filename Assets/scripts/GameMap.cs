using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public GameObject GamMain; //головний скріпт який приймає всі рішення
    [SerializeField] private GameObject eLitters, eNumbers, eMap, eState;

    public GameObject MapDestination;

    public bool HideShip = false;


    GameObject[] Litters;

    GameObject[] Numbers;
    public
    GameObject[,] Map;
    int Time = 50, DeltaTime = 0;
    private int LenghtMap = 10;
    public int[] ShipCount = { 0, 4, 3, 2, 1 };
    
    public
    struct TestCoord
    {
        public int x, y;
    }
    public
    struct Ship
    {
        public TestCoord[] ShipCoord;
    }
    public
    List<Ship> ListShip = new List<Ship>();

    public void CopyMap()
    {
        if(MapDestination != null)
        {
            for(int y = 0; y < LenghtMap; y++)
            {
                for(int x = 0; x< LenghtMap; x++)
                {
                    // Читаємо що записано в нашому полі і записуємо дані в інше поле
                    MapDestination.GetComponent<GameMap>().Map[x, y].GetComponent<Chanks>().index = Map[x, y].GetComponent<Chanks>().index;
                    
                }
            }
            // Обнуляємо список
            MapDestination.GetComponent<GameMap>().ListShip.Clear();
            //Записуємо згенеровані кораблі
            MapDestination.GetComponent<GameMap>().ListShip.AddRange(ListShip);
        }
    }
    // Створюємо ігрове поле
    void CreateMap()
    {
        Vector3 StartPosition = transform.position;
        float Xposition = StartPosition.x + 1;
        float Yposition = StartPosition.y - 1;

        Litters = new GameObject[LenghtMap];
        for (int i = 0; i < LenghtMap; i++)
        {
            Litters[i] = Instantiate(eLitters);
            Litters[i].transform.position = new Vector3(Xposition, StartPosition.y, StartPosition.z);
            Litters[i].GetComponent<Chanks>().index = i;
            Xposition++;
        }
        Numbers = new GameObject[LenghtMap];
        for (int j = 0; j < LenghtMap; j++)
        {
            Numbers[j] = Instantiate(eNumbers);
            Numbers[j].transform.position = new Vector3(StartPosition.x, Yposition, StartPosition.z);
            Numbers[j].GetComponent<Chanks>().index = j;
            Yposition--;
        }

        //Генерація мапи
        Xposition = StartPosition.x + 1;
        Yposition = StartPosition.y - 1;
        Map = new GameObject[LenghtMap, LenghtMap];
        // Малювання поля по Y
        for (int y = 0; y < LenghtMap; y++)
        {
            // Малювання поля по X
            for (int x = 0; x < LenghtMap; x++)
            {
                Map[x, y] = Instantiate(eMap);
                Map[x, y].GetComponent<Chanks>().index = 0;
                Map[x, y].GetComponent<Chanks>().HideChank = HideShip;
                Map[x, y].transform.position = new Vector3(Xposition, Yposition, StartPosition.z);
                // Перевірка, щоб гравець не стреляв по своєму полю
                if(HideShip)
                Map[x, y].GetComponent<ClickOnMap>().WhoParents = this.gameObject;
                Map[x, y].GetComponent<ClickOnMap>().posX = x;
                Map[x, y].GetComponent<ClickOnMap>().posY = y;

                Xposition++;
            }
            Xposition = StartPosition.x + 1;
            Yposition--;
        }
    }
    public void OnClick(int x, int y)
    {
        // ShootController(x, y);
        if (GamMain != null) GamMain.GetComponent<MainGame>().UserClick(x, y);
    }
    //Перевірка на палуби.
    bool ChekEnterDeck(int x, int y)
    {
        if ((x > -1) && (y > -1) && (x < 10) && (y < 10))
        {
            int[] arrX = new int[9], arrY = new int[9];
            arrX[0] = x + 1; arrX[1] = x; arrX[2] = x - 1;
            arrY[0] = y + 1; arrY[1] = y + 1; arrY[2] = y + 1;


            arrX[3] = x + 1; arrX[4] = x; arrX[5] = x - 1;
            arrY[3] = y; arrY[4] = y; arrY[5] = y;


            arrX[6] = x + 1; arrX[7] = x; arrX[8] = x - 1;
            arrY[6] = y - 1; arrY[7] = y - 1; arrY[8] = y - 1;

            for (int i = 0; i < 9; i++)
            {
                if ((arrX[i] > -1) && (arrY[i] > -1) && (arrX[i] < 10) && (arrY[i] < 10))
                {
                    if (Map[arrX[i], arrY[i]].GetComponent<Chanks>().index != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
    //Перевірка встановлення палуб у визначене положення
    //ShipType - тип корабля к-сть палуб
    //  XD YD Зміщення по осям перевірки
    // X Y початкові координати корабля
    TestCoord[] TestEnterShipDirect(int ShipType, int xd, int yd, int x, int y)
    {
        // масив для сортування кораблів по палубам
        TestCoord[] ResultCord = new TestCoord[ShipType];
        for (int i = 0; i < ShipType; i++)
        {
            // перевіряємо чи можна поставити палубу
            if (ChekEnterDeck(x, y))
            {
                //запамятовуємо значення
                ResultCord[i].x = x;
                ResultCord[i].y = y;
            }
            else
                return null;
            // зміщуємо перевірку в указаному напрямку
            x += xd;
            y += yd;
        }
        return ResultCord;
    }
    // повідомляємо який корабель ми хочемо поставити у  визначене місце.
    //ShipType - тип корабля к-сть палуб
    //Direction напрямлення 0 вертикально 1 горизонтально
    // X Y початкові координати корабля
    public
    TestCoord[] TestEnterShip(int ShipType, int Direction, int x, int y)
    {
        TestCoord[] ResultCord = new TestCoord[ShipType];
        if (ChekEnterDeck(x, y))
        {
            switch (Direction)
            {
                case 0:
                    ResultCord = TestEnterShipDirect(ShipType, 1, 0, x, y);
                    if(ResultCord == null) ResultCord = TestEnterShipDirect(ShipType, -1, 0, x, y);
                    break;
                case 1:
                    ResultCord = TestEnterShipDirect(ShipType, 0, 1, x, y);
                    if (ResultCord == null) ResultCord = TestEnterShipDirect(ShipType, 0, -1, x, y);
                    break;
            }
            return ResultCord;
        }
        return null;
    }
    //Встановлення корабля на поле
    bool EnterDeck(int ShipType, int Direction, int x, int y)
    {
        TestCoord[] i = TestEnterShip(ShipType, Direction, x, y);
        if(i != null)
        {
            foreach(TestCoord j in i)
            {
                Map[j.x,j.y].GetComponent<Chanks>().index = 1;
            }
            Ship Deck;
            //зберігаємо координати корабля
            Deck.ShipCoord = i;
            //зберігаємо корабель у список
            ListShip.Add(Deck);
            return true;
        }
        return false;
    }
    //функція повертає правду якщо такий корабель знаходиться у ангарі
    bool CountShips()
    {
        int Amount = 0;
        foreach (int Ship in ShipCount) Amount += Ship;
        if (Amount != 0) return true;
        return false;
    }
    public 
    void ClearMap()
    {
        ShipCount = new int[]{ 0, 4, 3, 2, 1 };
        ListShip.Clear();
        for (int y = 0; y < LenghtMap; y++)
        {
            for(int x = 0; x < LenghtMap; x++)
            {
                Map[x, y].GetComponent<Chanks>().index = 0;
            }
        }
    }
    public
    void EnterRandomShip()
    {
        ClearMap();
        int SelectShip = 4;
        int x, y, Direction;

        while (CountShips())
        {
            x = Random.RandomRange(0, 10);
            y = Random.RandomRange(0, 10);
            Direction = Random.RandomRange(0, 2);
            if(EnterDeck(SelectShip, Direction, x, y))
            {
                ShipCount[SelectShip]--;
                if (ShipCount[SelectShip] == 0)
                {
                    SelectShip--;
                }
            }
        }
    }
    public
    bool ShootController(int x,int y)
    {
       if(eState!=null) eState.GetComponent<Chanks>().index = 0;
        int MapSelection = Map[x, y].GetComponent<Chanks>().index;
        bool Result = false;
        switch (MapSelection)
        {
            case 0:
                Map[x, y].GetComponent<Chanks>().index = 2;
                Result = false;
                eState.GetComponent<Chanks>().index = 3;
                break;
            case 1:
                Map[x, y].GetComponent<Chanks>().index = 3;
                Result = true;
                if (TestShoot(x, y))
                {
                    //вбитий корабель
                    if (eState != null) eState.GetComponent<Chanks>().index = 2;
                }
                else
                {
                    if (eState != null) eState.GetComponent<Chanks>().index = 1;
                }
                break;
        }
        return Result;
    }
    //Перевірка попадання по кораблю
    bool TestShoot(int x, int y)
    {
        bool Result = false;
        //перебираємо кораблі і вибираємо той в який ми попали
        foreach(Ship Test in ListShip)
        {
            //Перебираємо палуби корабля і перевіряємо чи попали в неї
            foreach(TestCoord Paluba in Test.ShipCoord)
            {
                if((Paluba.x == x) && (Paluba.y == y)){
                    int CountKills = 0;
                    //перевірка скільки палуб підбито у кораблі
                    foreach(TestCoord KillPaluba in Test.ShipCoord)
                    {
                        int TestBlock = Map[KillPaluba.x, KillPaluba.y].GetComponent<Chanks>().index;
                        if (TestBlock == 3) CountKills++;
                    }
                    if (CountKills == Test.ShipCoord.Length)
                        Result = false;
                    else
                        Result = true;

                    return Result;
                }
            }
        }
        return Result;
    }
    public int LifeShip()
    {
        // лічильник живих кораблів
        int CountLife = 0;
        // перебираємо кораблі і дивимося скільки робочих
        foreach(Ship Test in ListShip)
        {
            // перебираємо палуби і перевіряємо чи живі вони
            foreach(TestCoord Paluba in Test.ShipCoord)
            {
                // дивимося що з палубою
                int TestBlock = Map[Paluba.x, Paluba.y].GetComponent<Chanks>().index;
                // якщо 1 то палуба жива
                if (TestBlock == 1) CountLife++;
            }
            
        }
        return CountLife;
    }
    public int GetIndexBlock(int x, int y)
    {
        return Map[x, y].GetComponent<Chanks>().index;
    }
    void Start()
    {
        CreateMap();
        if(HideShip) EnterRandomShip();
    }

    void Update()
    {
        DeltaTime++;
        if(DeltaTime > Time)
        {
            if (eState != null) eState.GetComponent<Chanks>().index = 0;
            DeltaTime = 0;
        }
    }
}
