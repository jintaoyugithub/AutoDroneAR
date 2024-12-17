using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class C_Game2 : MonoBehaviour
{
    public static C_Game2 Instance;
    // Start is called before the first frame update
    public GameObject Drone;
    public GameObject EnemyDrone1;
    public GameObject EnemyDrone2;

    private float PointsGot;
    private float TimePassed;

    public float Enemy1GenerateDuration = 1.0f;
    public float Enemy2GenerateDuration = 2.0f;
    public float Enemy2ShootDuration = 3.0f;

    private float Enemy1Timer;
    private float Enemy2Timer;
    [HideInInspector]
    public bool isStartGame = false;
    private int DroneBlood = 5;

    public float generateRadiusMin = 10.0f;
    public float generateRadiusMax = 15.0f;

    public Image[] HeartsUI;
    private int HeartIndex = 4;
    private GameObject EnemyParent;
    private int HardLevel = 1;

    public GameObject PointsGotUI;
    public GameObject FinishGameUI;

    public bool generateEnemy1 = true;
    public bool generateEnemy2 = true;

    public GameObject Shield;

    [HideInInspector]
    public int curDamage = 1;

    public GameObject ShieldItem;
    public GameObject HeartItem;
    public GameObject ImprovementItem;


    public float ItemGenerateRadiusMin = 5.0f;
    public float ItemGenerateRadiusMax = 10.0f;
    public float ItemGenerateDuration = 10.0f;

    private float ItemGenerateTimer = 10.0f;
    private GameObject ItemParent;

    public GameObject StartButton1;
    public GameObject StartButton2;

    public GameObject RestButton;
    public GameObject FinalScoreBK;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //EnemyParent = new GameObject("EnemyParent");
        //EnemyParent.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartGame && DroneBlood > 0)
        {
            if (DroneBlood <= 0)
            {
                isStartGame = false;
            }
            PointsGot += Time.deltaTime * HardLevel;
            PointsGotUI.GetComponent<TextMeshProUGUI>().text = "Score:" + Mathf.Round(PointsGot);

            Enemy1Timer -= Time.deltaTime;
            Enemy2Timer -= Time.deltaTime;
            ItemGenerateTimer -= Time.deltaTime;
            TimePassed += Time.deltaTime;
            if (TimePassed >= 20.0f && TimePassed < 40.0f)
            {
                HardLevel = 2;
            }
            else if (TimePassed >= 40.0f && TimePassed <= 60.0f)
            {
                HardLevel = 3;
            }
            else if (TimePassed >= 60.0f && TimePassed <= 80.0f)
            {
                HardLevel = 4;
            }
            else if (TimePassed >= 80.0f && TimePassed <= 100.0f)
            {
                HardLevel = 5;
            }
            if (ItemGenerateTimer <= 0)
            {
                ItemGenerateTimer = ItemGenerateDuration;
                GenerateItem();
            }

            if (Enemy1Timer <= 0 && generateEnemy1)
            {
                Enemy1Timer = Enemy1GenerateDuration;
                switch (HardLevel)
                {
                    case (1):
                        GenerateEnemy1(Drone, 3.0f);
                        break;
                    case (2):
                        GenerateEnemy1(Drone, 3.5f);
                        break;
                    case (3):
                        GenerateEnemy1(Drone, 4.0f);
                        GenerateEnemy1(Drone, 4.0f);
                        break;
                    case (4):
                        GenerateEnemy1(Drone, 4.5f);
                        GenerateEnemy1(Drone, 4.5f);
                        break;
                    case (5):
                        GenerateEnemy1(Drone, 5.0f);
                        GenerateEnemy1(Drone, 5.0f);
                        GenerateEnemy1(Drone, 5.0f);
                        break;
                }
            }
            if (Enemy2Timer <= 0 && generateEnemy2)
            {
                Enemy2Timer = Enemy2GenerateDuration;
                switch (HardLevel)
                {
                    case (1):
                        GenerateEnemy2(Drone, 3.0f, 3.0f);
                        break;
                    case (2):
                        GenerateEnemy2(Drone, 3.0f, 2.5f);
                        break;
                    case (3):
                        GenerateEnemy2(Drone, 3.0f, 2.0f);
                        break;
                    case (4):
                        GenerateEnemy2(Drone, 3.0f, 1.5f);
                        GenerateEnemy2(Drone, 3.0f, 1.5f);
                        break;
                    case (5):
                        GenerateEnemy2(Drone, 3.0f, 1.0f);
                        GenerateEnemy2(Drone, 3.0f, 1.0f);
                        break;
                }
            }

        }
    }

    public void StartGame()
    {
        Drone.GetComponent<C_Drone>().ResetWeapon();
        EnemyParent = new GameObject("EnemyParent");
        ItemParent = new GameObject("ItemParent");

        DroneBlood = 5;
        HeartIndex = 4;
        curDamage = 1;
        HardLevel = 1;
        PointsGot = 0;
        TimePassed = 0;

        EnemyParent.transform.position = Vector3.zero;
        for (int i = 0; i < HeartsUI.Length; i++)
        {
            HeartsUI[i].gameObject.SetActive(true);
            HeartsUI[i].transform.localScale = Vector3.one;
        }
        StartButton1.SetActive(false);
        StartButton1.SetActive(false);
        RestButton.SetActive(true);
        isStartGame = true;
    }

    public Transform GetNearestEnemy()
    {
        float minDistance = float.MaxValue;
        Transform nearestEnemy = null;
        if(EnemyParent != null && EnemyParent.transform.childCount != 0)
        {
            for(int i = 0; i < EnemyParent.transform.childCount; i++)
            {
                float tmp = Vector3.Distance(Drone.transform.position, EnemyParent.transform.GetChild(i).position);
                if (tmp < minDistance)
                {
                    minDistance = tmp;
                    nearestEnemy = EnemyParent.transform.GetChild(i);
                }
            }
        }
        return nearestEnemy;
    }

    private Vector3 GetRandomPosition(Vector3 center, float innerRadius, float outerRadius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(innerRadius, outerRadius);
        Vector3 randomPoint = center + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;
        return randomPoint;
    }

    private void GenerateEnemy1(GameObject Target, float speed)
    {
        GameObject tmpEnemy1 = Instantiate(EnemyDrone1, GetRandomPosition(Target.transform.position, generateRadiusMin, generateRadiusMax), Quaternion.identity);
        tmpEnemy1.GetComponent<C_Enemy1>().Player = Target;
        tmpEnemy1.GetComponent<C_Enemy1>().speed = speed;
        tmpEnemy1.transform.SetParent(EnemyParent.transform);
    }

    private void GenerateEnemy2(GameObject Target, float speed, float shootDuration)
    {
        GameObject tmpEnemy2 = Instantiate(EnemyDrone2, GetRandomPosition(Target.transform.position, generateRadiusMin, generateRadiusMax), Quaternion.identity);
        tmpEnemy2.GetComponent<C_Enemy2>().Player = Target;
        tmpEnemy2.GetComponent<C_Enemy2>().speed = speed;
        tmpEnemy2.GetComponent<C_Enemy2>().shootDuration = shootDuration;
        tmpEnemy2.GetComponent<C_Enemy2>().DistanceToPlayer = 20.0f;

        tmpEnemy2.transform.SetParent(EnemyParent.transform);
    }

    public void GetHurt()
    {
        if (Drone.GetComponent<C_Drone>().hasShield)
        {
            return;
        }
        StartCoroutine(heartDisappear(HeartsUI[HeartIndex], 0.1f));
        if (HeartIndex == 0)
        {
            GameOver();
            return;
        }
        //DroneBlood--;
        HeartIndex--;
    }

    public void GetHeart()
    {
        if (HeartIndex == 4)
        {
            return;
        }
        HeartIndex++;
        StartCoroutine(heartAppear(HeartsUI[HeartIndex], 0.1f));
    }

    public void GameOver()
    {
        for (int i = 0; i < HeartsUI.Length; i++)
        {
            HeartsUI[i].gameObject.SetActive(false);
        }
        //for (int i = 0; i < EnemyParent.transform.childCount; i++)
        //{
        //    Destroy(EnemyParent.transform.GetChild(i));
        //}
        Destroy(EnemyParent);
        Destroy(ItemParent);

        StartButton1.SetActive(true);
        StartButton1.SetActive(true);
        RestButton.SetActive(false);

        isStartGame = false;
        PointsGotUI.GetComponent<TextMeshProUGUI>().text = "";
        FinalScoreBK.SetActive(true);
        FinishGameUI.GetComponent<TextMeshProUGUI>().text = "    GAME OVER!\r\nYOUR FINAL SCORE:\r\n         " + Mathf.Round(PointsGot);
        StartCoroutine(FianlScoreDisappear(3.0f));
    }

    private IEnumerator FianlScoreDisappear(float time)
    {
        yield return new WaitForSeconds(time);
        FinalScoreBK.SetActive(false);
    }

    private IEnumerator heartDisappear(Image heart, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            heart.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.localScale = Vector3.zero;
        heart.gameObject.SetActive(false);
    }

    private IEnumerator heartAppear(Image heart, float time)
    {
        heart.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            heart.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.localScale = Vector3.one;
    }

    private void GenerateItem()
    {
        int index = Random.Range(0, 3);
        GameObject tmp = null;
        Vector3 GeneratePosition = GetRandomPosition(Drone.transform.position, ItemGenerateRadiusMin, ItemGenerateRadiusMax);
        switch (index)
        {
            case 0: tmp = Instantiate(HeartItem, GeneratePosition, Quaternion.Euler(-90f, 0f, 0f)); break;
            case 1: tmp = Instantiate(ImprovementItem, GeneratePosition, Quaternion.identity); break;
            case 2: tmp = Instantiate(ShieldItem, GeneratePosition, Quaternion.identity); break;
        }
        tmp.transform.SetParent(ItemParent.transform);
    }
}
