using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Shoot : MonoBehaviour
{
    public Transform spawnLocator;
    public Transform spawnLocatorMuzzleFlare;
    public Transform shellLocator;

    [System.Serializable]
    public class projectile
    {
        public string name;
        public Rigidbody bombPrefab;
        public GameObject muzzleflare;
        public float min, max;
        public bool rapidFire;
        public float rapidFireCooldown;

        public GameObject shellPrefab;
        public bool hasShells;
    }
    public projectile[] bombList;

    string FauxName;
    public float rapidFireDelay;

    float firingTimer;
    public bool firing;
    public int bombType = 0;

    void Start()
    {

    }
    public void StartFire()
    {
        firing = true;
        Fire();
    }
    public void FireEnd()
    {
        firing = false;
        firingTimer = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Rotate(Vector3.up, -25 * Time.deltaTime);
        //更换子弹种类
        //Switch(1);

        if (bombList[bombType].rapidFire && firing)
        {
            if (firingTimer > bombList[bombType].rapidFireCooldown + rapidFireDelay)
            {
                Fire();
                firingTimer = 0;
            }
        }

        if (firing)
        {
            firingTimer += Time.deltaTime;
        }
    }

    public void Switch(int value)
    {
        bombType += value;
        if (bombType < 0)
        {
            bombType = bombList.Length;
            bombType--;
        }
        else if (bombType >= bombList.Length)
        {
            bombType = 0;
        }
    }

    public void Fire()
    {
        Instantiate(bombList[bombType].muzzleflare, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation);
        //   bombList[bombType].muzzleflare.Play();

        if (bombList[bombType].hasShells)
        {
            Instantiate(bombList[bombType].shellPrefab, shellLocator.position, shellLocator.rotation);
        }

        Rigidbody rocketInstance;
        rocketInstance = Instantiate(bombList[bombType].bombPrefab, spawnLocator.position, spawnLocator.rotation) as Rigidbody;
        // Quaternion.Euler(0,90,0)
        rocketInstance.AddForce(spawnLocator.forward * Random.Range(bombList[bombType].min, bombList[bombType].max));
    }
}
