using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class C_Enemy2 : MonoBehaviour
{
    public float speed = 5.0f;
    public float CatchSpeed = 10.0f;
    public GameObject Player;
    public float shootDuration;
    public float DistanceToPlayer = 20.0f;

    public GameObject Shooter;

    private float shootTimer;
    private Vector3 playerDirection;
    private Vector3 shootDirection;

    private Rigidbody drone;

    public int blood = 5;
    // Start is called before the first frame update
    void Start()
    {
        drone = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            playerDirection = (Player.transform.position - this.transform.position).normalized;
            this.transform.RotateAround(Player.transform.position, Vector3.up, speed * Time.deltaTime);

            //shootDirection = (Player.transform.position - Shooter.transform.position).normalized;
            //Shooter.transform.rotation = Quaternion.LookRotation(playerDirection);

            transform.rotation = Quaternion.LookRotation(playerDirection);
        }

        if(Vector3.Distance(this.transform.position, Player.transform.position) > DistanceToPlayer)
        {
            drone.velocity = playerDirection * CatchSpeed;
        }
        else
        {
            drone.velocity = Vector3.zero;
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = shootDuration;
            Shooter.GetComponent<C_Shoot>().Fire();

        }
    }
    public void GetHit()
    {
        Debug.Log("Gethit");
    }

    public void GetHurt(int hurtValue)
    {
        
        if (blood - hurtValue <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            blood -= hurtValue;
        }
    }
}
