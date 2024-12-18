using DroneController.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Enemy1 : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject Player;

    private Rigidbody drone;

    public int blood = 3;

    public AudioClip Explosion;
    void Start()
    {
        drone = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Player != null)
        {
            Vector3 playerDirection = (Player.transform.position - this.transform.position).normalized;
            drone.velocity = playerDirection * speed;
            transform.rotation = Quaternion.LookRotation(playerDirection);
        }   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            C_Game2.Instance.GetHurt();
            Destroy(this.gameObject);
        }
    }

    public void GetHit()
    {
        Debug.Log("Gethit");
    }

    public void GetHurt(int hurtValue)
    {
        if(blood - hurtValue <= 0)
        {
            AudioSource.PlayClipAtPoint(Explosion, this.transform.position);

            Destroy(this.gameObject);
        }
        else
        {
            blood -= hurtValue;
        }
    }
}
