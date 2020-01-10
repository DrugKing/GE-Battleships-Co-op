using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlainHide : MonoBehaviour
{
    public GameObject[] plane;
    // Start is called before the first frame update
    void Start()
    {
        plane[0].SetActive(false);
        plane[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (BoardManager.StartGame)
        {
            if (TurnBasedManager.turnNo == 1)
            {
                plane[0].SetActive(true);
                plane[1].SetActive(false);
            }
            if (TurnBasedManager.turnNo == 2)
            {
                plane[0].SetActive(false);
                plane[1].SetActive(true);
            }
        }
    }
}
