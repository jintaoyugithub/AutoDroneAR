using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class C_Drone : MonoBehaviour
{
    private Collider collider;

    private bool onProtected = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !onProtected)
        {
            C_Game1.Instance.GetHurt();
            StartCoroutine(getHurt(1.0f));
        }
    }

    private IEnumerator getHurt(float time)
    {
        onProtected = true;
        collider.enabled = false;
        GameObject tmp = Instantiate(C_Game1.Instance.Shield, this.transform.position, Quaternion.identity);
        tmp.transform.SetParent(this.transform);

        yield return new WaitForSeconds(time);

        onProtected = false;
        collider.enabled = true;
        Destroy(tmp);
    }
}
