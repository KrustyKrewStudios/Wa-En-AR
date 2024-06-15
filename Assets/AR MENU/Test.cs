using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Object Reference
    public GameObject obj1;
    public GameObject obj2;

    // Hide Function
    public void Hide()
    {
        obj1.gameObject.SetActive(false);
        obj2.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
