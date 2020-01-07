using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    Camera PlayerCamera;
    [SerializeField]
    GameObject Controller;

    [Header("Player 1")]
    public GameObject TilesP1;
    public GameObject BoardParentP1;
    public GameObject ShipsP1;
    [Header("Player 2")]
    public GameObject TilesP2;
    public GameObject BoardParentP2;
    public GameObject ShipsP2;
    [Header("Highlighted")]
    [SerializeField]
    private Material Checking;
    [SerializeField]
    private Material Hiting;
    [SerializeField]
    public GameObject selectedObject;
    [Header("BattleShips")]
    public GameObject[] ships;
    [Header("UIPanel")]
    public GameObject GamePanel;
    public GameObject ShipSelectionPanel;
    public GameObject[] TheButton;
    public GameObject Confirmation;
    public GameObject StartPanel;

    public const float Tile_Size = 1.0f;
    public const float Tile_Offset = 0.5f;
    public static bool canshoot;
    public static bool canplace;
    public List<int> values = new List<int>();

    private bool isdone;
    public bool StartGame;
    bool reset;
    public int SelectionX = -1;
    public int SelectionZ = -1;
    int k = 0;
    int j = 0;

    Vector3 TileVectorP1;
    Vector3 TileVectorP2;
    Vector3 ShipVector;
    Material originMat;
    GameObject shipsparent;
    string pname;
    string stags;
    int slayer;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        reset = false;
        shipping();
        ShipSelectionPanel.SetActive(false);
        GamePanel.SetActive(true);
        Confirmation.SetActive(false);
        StartPanel.SetActive(false);
        FindCameras();  
        CreateBoard();        
    }

    // Update is called once per frame
    void Update()
    {
        //DrawChessBoard();
        SelectionUpdate();
        shipping();
        FindCameras();
        if (DragHandler.space)
        {
            Confirmation.SetActive(true);
        }
        shiphide();     
    }
    #endregion

    #region Public Methods

    public void FindCameras()
    {
        if (TurnBasedManager.turnNo == 1)
        {
            Controller = GameObject.Find(PlayerNameInput.player1);
            PlayerCamera = Controller.GetComponentInChildren<Camera>();
        }
        if (TurnBasedManager.turnNo == 2)
        {
            Controller = GameObject.Find(PlayerNameInput.player2);
            PlayerCamera = Controller.GetComponentInChildren<Camera>();
        }
    }    

    public void selectObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            if (obj == selectedObject)
                return;
            ClearSelection();
        }
        selectedObject = obj;
        Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            originMat = r.material;
            if (TurnBasedManager.turnNo == 1)
            {
                if (r.gameObject.tag == "Player1" && r.gameObject.layer == 10)
                {
                    r.GetComponent<Renderer>().material = Checking;
                    canplace = true;
                    canshoot = false;
                }
                if (r.gameObject.tag == "Player2" && r.gameObject.layer == 10)
                {
                    r.GetComponent<Renderer>().material = Hiting;
                    canplace = false;
                    canshoot = true;
                }
            }
            if (TurnBasedManager.turnNo == 2)
            {
                if (r.gameObject.tag == "Player2" && r.gameObject.layer == 10)
                {
                    r.GetComponent<Renderer>().material = Checking;
                    canplace = true;
                    canshoot = false;
                }
                if (r.gameObject.tag == "Player1" && r.gameObject.layer == 10)
                {
                    r.GetComponent<Renderer>().material = Hiting;
                    canplace = false;
                    canshoot = true;
                }
            }
        }
        
    }

    public void ClearSelection()
    {
        if (selectedObject == null)
            return;
        Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            r.GetComponent<Renderer>().material = originMat;
            if (TurnBasedManager.strike && canshoot)
            {
                r.GetComponent<Renderer>().material = Hiting;
                TurnBasedManager.strike = false;
                TurnBasedManager.miss = false;
            }
            if (TurnBasedManager.miss && canshoot)
            {
                r.GetComponent<Renderer>().material = Checking;
                TurnBasedManager.strike = false;
                TurnBasedManager.miss = false;
            }
        }
        selectedObject = null;
    }

    public void DisableGamePanel()
    {
        ShipSelectionPanel.SetActive(true);
        if (reset)
        {
            buttonrecreate();
        }
        GamePanel.SetActive(false);
    }

    public void OnStartPanel()
    {
        StartPanel.SetActive(true);
    }

    public void GenerateShip(int ship)
    {
        GameObject Ship = Instantiate(ships[ship]);
        ShipVector = new Vector3(0.5f, -0.25f, 0.5f);
        Ship.transform.position = ShipVector;
        Ship.transform.SetParent(shipsparent.transform);
        Ship.name = (ships[ship].name + pname);
        Ship.tag = stags;
        Ship.layer = slayer;
        //Ship.GetComponentInChildren<GameObject>().layer = slayer;
        TheButton[ship].SetActive(false);
        ShipSelectionPanel.SetActive(false);
        values.Add(ship+1);               
    }

    public void ShipConfirm()
    {
        DragHandler.space = false;
        for (int i = 1; i < 6; i++)
        {
            if (values.Contains(i))
            {
                isdone = true;
                j += 1;
            }
            else
            {
                isdone = false;
                j -= 1;
            }
        }
        if (isdone)
        {
            if (j == 5)
            {
                ShipSelectionPanel.SetActive(false);                
                Confirmation.SetActive(false);
                //give the player 2 to place the ships
                if (TurnBasedManager.turnNo == 1)
                {
                    j = 0;
                    TurnBasedManager.turnNo = 2;
                    reset = true;
                    values.Clear();
                    GamePanel.SetActive(true);
                }
                if(j==5 && TurnBasedManager.turnNo == 2)
                {
                    StartGame = true;
                    OnStartPanel();
                }
            }
            else
            {
                ShipSelectionPanel.SetActive(true);
                Confirmation.SetActive(false);
            }
        }
        else
        {
                ShipSelectionPanel.SetActive(true);
                Confirmation.SetActive(false);       
        }       
    }

    public void buttonrecreate()
    {
        if (reset)
        {
            for (int i = 0; i < 5; i++)
            {
                TheButton[i].SetActive(true);
            }
        }
    }

    public void shiphide()
    {
        if(TurnBasedManager.turnNo == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject.Find(ships[i].name + "p2").GetComponentInChildren<Renderer>().enabled = false;
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject.Find(ships[i].name + "p1").GetComponentInChildren<Renderer>().enabled = true;
            }
            //ShipsP1.SetActive(true);
            //ShipsP2.SetActive(false);
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject.Find(ships[i].name + "p1").GetComponentInChildren<Renderer>().enabled = false;
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject.Find(ships[i].name + "p2").GetComponentInChildren<Renderer>().enabled = true;
            }
            //ShipsP1.SetActive(false);
            //ShipsP2.SetActive(true);
        }
    }

    public void shipping()
    {
        if (TurnBasedManager.turnNo == 1)
        {
            shipsparent = ShipsP1;
            pname = "p1";
            stags = "Player1";
            slayer = 8;
        }
        else
        {
            shipsparent = ShipsP2;
            pname = "p2";
            stags = "Player2";
            slayer = 9;
        }
    }
    #endregion

    #region Private Methods

    private void CreateBoard()
    {
        //Player 1 board
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject tileP1 = Instantiate(TilesP1);
                TileVectorP1 = new Vector3(0.5f + i, -0.25f, 0.5f + j);
                tileP1.transform.position = TileVectorP1;
                tileP1.transform.SetParent(BoardParentP1.transform);
            }
        }

        //Player 2 board
        for (int i = 20; i < 30; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject tileP2 = Instantiate(TilesP2);
                TileVectorP2 = new Vector3(0.5f + j, -0.25f, 0.5f + i);
                tileP2.transform.position = TileVectorP2;
                tileP2.transform.SetParent(BoardParentP2.transform);
            }
        }
    }

    private void DrawChessBoard()
    {
        Vector3 widthline = Vector3.right * 10;
        Vector3 heightline = Vector3.forward * 10;
        //player 1
        for (int i = 0; i <= 10; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthline);
            for (int j = 0; j <= 10; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightline);
            }
        }

        //player 2
        for (int i = 20; i <= 30; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthline);
            for (int j = 0; j <= 10; j++)
            {
                start = Vector3.right * j;
                Vector3 playervector = new Vector3(0.0f+j, 0.0f, 20.0f);
                Debug.DrawLine(playervector, playervector + heightline, Color.yellow);
            }
        }
    }

    private void SelectionUpdate()
    {
        if (GamePanel.activeSelf == false && ShipSelectionPanel.activeSelf == false)
        {
            if (!PlayerCamera)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, layerMask: ~((1 << 8) | (1 << 9))))
            {
                GameObject hitObject = hit.transform.gameObject;
                selectObject(hitObject);
                SelectionX = (int)hit.point.x;
                SelectionZ = (int)hit.point.z;
            }
            else
            {
                ClearSelection();
                SelectionX = -1;
                SelectionZ = -1;
            }
        }
        else
        {
            return;
        }
    }
    #endregion
}