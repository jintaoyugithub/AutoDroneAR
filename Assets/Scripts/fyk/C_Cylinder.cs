using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Cylinder : MonoBehaviour
{

    public Material M_Green;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(2);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(1);
            this.transform.GetComponent<MeshRenderer>().material = M_Green;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(2);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(1);
            this.transform.GetComponent<MeshRenderer>().material = M_Green;
        }
    }
}
