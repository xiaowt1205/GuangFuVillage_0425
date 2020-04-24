using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("--------------------------------");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("rotate"+this.gameObject.transform.rotation);
        Debug.Log("eul"+this.gameObject.transform.eulerAngles);
    }
}
