using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class C_Drone : MonoBehaviour
{
    private Collider collider;

    private bool onProtected = false;
    private Vector3 enemyDirection = Vector3.zero;

    public GameObject ShootComponent;
    public bool hasAngleLimitation = false;
    public float AngleLimitation = 30.0f;
    [HideInInspector]
    public bool hasShield = false;

    [HideInInspector]
    public Transform NearestEnemy;

    private int getImproveTimes = 0;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        if (ShootComponent == null)
        {
            Debug.Log("Find no shoot component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        NearestEnemy = C_Game2.Instance.GetNearestEnemy();
        if (C_Game2.Instance.isStartGame && NearestEnemy != null)
        {
            ShootComponent.GetComponent<C_Shoot>().firing = true;
            enemyDirection = (NearestEnemy.position - ShootComponent.transform.position).normalized;
            ShootComponent.transform.rotation = Quaternion.LookRotation(enemyDirection);
        }
        else
        {
            ShootComponent.GetComponent<C_Shoot>().firing = false;
        }   
    }

    public void getShield()
    {
        StartCoroutine(GetShield(10.0f));
    }

    private IEnumerator GetShield(float time)
    {
        hasShield = true;
        GameObject tmp = Instantiate(C_Game2.Instance.Shield, this.transform.position, Quaternion.identity);
        tmp.transform.SetParent(this.transform);

        yield return new WaitForSeconds(time);

        hasShield = false;
        Destroy(tmp);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !onProtected)
        {
            C_Game1.Instance.GetHurt();
            AudioSource.PlayClipAtPoint(C_Game2.Instance.GetWaterShield, this.transform.position);
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
    public void GetSkill()
    {
        if(getImproveTimes < 5)
        {
            switch (getImproveTimes)
            {
                case 0: getImprovement(); break;
                case 1: getWeaponChanged(); break;
                case 2: getImprovement(); break;
                case 3: getWeaponChanged(); break;
                case 4: getImprovement(); break;
            }
            getImproveTimes++;
        }
    }

    public void getImprovement()
    {
        ShootComponent.GetComponent<C_Shoot>().bombList[ShootComponent.GetComponent<C_Shoot>().bombType].rapidFireCooldown = 0.12f;
    }
    public void getWeaponChanged()
    {
        C_Game2.Instance.curDamage++;
        ShootComponent.GetComponent<C_Shoot>().Switch(1);
    }

    public void ResetWeapon()
    {
        getImproveTimes = 0;
        for (int i = 0; i < ShootComponent.GetComponent<C_Shoot>().bombList.Length; i++)
        {
            ShootComponent.GetComponent<C_Shoot>().bombList[i].rapidFireCooldown = 0.24f;
        }
        ShootComponent.GetComponent<C_Shoot>().bombType = 0;
    }
}
