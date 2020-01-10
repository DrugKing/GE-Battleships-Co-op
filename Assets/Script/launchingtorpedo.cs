using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launchingtorpedo : MonoBehaviour
{
    public GameObject[] PlayerLauncher;
    public float speed = 20.0f;
    public GameObject Launchs;
    public GameObject playerPrefab;
    public GameObject hitObject;
    Camera PlayerCamera;
    public Vector3 target;

    // Start is called before the first frame update
    void Awake()
    {
        if (TurnBasedManager.turnNo == 1)
        {
            playerPrefab = GameObject.Find(PlayerNameInput.player1);
            PlayerCamera = playerPrefab.GetComponentInChildren<Camera>();
        }
        else
        {
            playerPrefab = GameObject.Find(PlayerNameInput.player2);
            PlayerCamera = playerPrefab.GetComponentInChildren<Camera>();
        }
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.ScreenPointToRay(Input.mousePosition), out hit, 100.0f, layerMask: ~((1 << 10))))
        {
            hitObject = hit.transform.gameObject;
            target = new Vector3(hitObject.transform.position.x, -5.0f, hitObject.transform.position.z);
        }
    }
    void Start()
    {
        PlayerLauncher = GameObject.FindGameObjectsWithTag("Launch");              
        for (int i = 0; i < PlayerLauncher.Length; i++)
        {
            Transform Parent = PlayerLauncher[i].GetComponentInParent<Transform>();
            Debug.Log(Parent.parent.name);
            if (TurnBasedManager.turnNo == 1 && Parent.parent.name == PlayerNameInput.player1)
            {
                Launchs = PlayerLauncher[i];
                transform.position = PlayerLauncher[i].transform.position;
            }
            if (TurnBasedManager.turnNo == 2 && Parent.parent.name == PlayerNameInput.player2)
            {
                Launchs = PlayerLauncher[i];
                transform.position = PlayerLauncher[i].transform.position;
            }
        }
        transform.SetParent(Launchs.transform);
    }

    // Update is called once per frame
    void Update()
    {        
        transform.position = Vector3.MoveTowards(Launchs.transform.position, target, Time.deltaTime * speed);
        if (Vector3.Distance(Launchs.transform.position, target) < 0.001f)
        {
            Destroy(this);
        }
    }
}
        