using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTagging : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        this.tag = transform.parent.tag;
        this.gameObject.layer = transform.parent.gameObject.layer;
    }
}
