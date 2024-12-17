using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class C_Game1 : MonoBehaviour
{
    public static C_Game1 Instance;

    private Vector3 startGamePosition;

    private Vector3 startForwardDirection;
    private Vector3 startRightDirection;
    private Vector3 startUpDirection;

    public float routeWidth = 10.0f;
    public float routeHeigh = 10.0f;


    public float WallLength = 50.0f;
    public float WallHeight = 10.0f;

    private float generateTimer = 0;
    private float HardLevel = 1;

    public float TimePassed = 0;

    public int DroneBlood = 5;

    public float generateTimeDuration = 5.0f;
    public float generateDistance = 50.0f;

    public float crossWidthMin = 2.0f;
    public float crossWidthMax = 5.0f;

    public float singleWallMin = 3.0f;
    public float singleWallMax = 8.0f;

    [HideInInspector]
    public float PointsGot;
    [HideInInspector]
    public bool isStartGame = false;

    [Header("���˻�GameObject")]
    public GameObject Drone;

    [Header("Wall")]
    public GameObject WallPrefab;
    public GameObject ObstaclePrefab;

    public Material[] ObstacleMaterials;
    private int MaterialIndex = 0;

    public Image[] HeartsUI;
    private int HeartIndex = 4;

    public GameObject PointsGotUI;
    public GameObject FinishGameUI;
    
    public GameObject Shield;
    public GameObject ShieldGold;

    private GameObject WallsParent;

    public GameObject StartGame1;
    public GameObject StartGame2;

    private Vector3 startDronePosition;
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
    // Start is called before the first frame update
    void Start()
    {
        if (Drone != null)
        {
            startDronePosition = Drone.transform.position;
        }
        //WallsParent = new GameObject("Obstacle's parent");
        //WallsParent.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartGame && DroneBlood > 0)
        {
            if(DroneBlood <= 0)
            {
                isStartGame = false;
            }
            PointsGot += Time.deltaTime * HardLevel;
            PointsGotUI.GetComponent<TextMeshProUGUI>().text = "Points:" + Mathf.Round(PointsGot);
            generateTimer -= Time.deltaTime;
            TimePassed += Time.deltaTime;
            if(TimePassed >= 20.0f && TimePassed < 40.0f)
            {
                HardLevel = 2;
            }
            else if(TimePassed >= 40.0f && TimePassed <= 60.0f)
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
            else if (TimePassed >= 100.0f)
            {
                HardLevel = 6;
            }

            if (generateTimer <= 0)
            {
                generateTimer = generateTimeDuration;
                //GenerateLRObstacles();
                //GenerateUDObstacles();

                //GenerateHorObstacle();
                //GenerateVerObstacle();

                //GenerateTwoHorObstacle();
                //GenerateTwoVerObstacle();
                switch (HardLevel) {
                    case (1): 
                        if(Random.Range(0,2) == 0)
                        {
                            GenerateHorObstacle();
                        }
                        else
                        {
                            GenerateVerObstacle();
                        }
                    break;
                    case (2):
                        int x = Random.Range(0,3);
                        if (x == 0)
                        {
                            GenerateHorObstacle();
                            GenerateVerObstacle();
                        }
                        else if(x == 1)
                        {
                            GenerateTwoHorObstacle();
                        }
                        else
                        {
                            GenerateTwoVerObstacle();
                        }
                        break;
                    case (3):
                        if (Random.Range(0, 2) == 0)
                        {
                            GenerateHorObstacle();
                            GenerateTwoVerObstacle();
                        }
                        else
                        {
                            GenerateVerObstacle();
                            GenerateTwoHorObstacle();
                        }
                        break;
                    case (4):
                        if (Random.Range(0, 2) == 0)
                        {        
                            GenerateVerObstacle();
                            GenerateLRObstacles();
                        }
                        else
                        {
                            GenerateHorObstacle();
                            GenerateUDObstacles();
                        }
                        break;
                    case (5):
                        if (Random.Range(0, 2) == 0)
                        {               
                            GenerateLRObstacles();
                            GenerateTwoVerObstacle();
                        }
                        else
                        {
                            GenerateUDObstacles();
                            GenerateTwoHorObstacle();
                        }
                        break;
                    case (6):
                        GenerateUDObstacles();
                        GenerateLRObstacles();
                        break;
                }
                MaterialChange();
            }


        }
    }
    public void MaterialChange()
    {
        MaterialIndex++;
        if (MaterialIndex == ObstacleMaterials.Length) MaterialIndex = 0;
    }

    public void StartGame()
    {
        isStartGame = true;
        HardLevel = 1;
        PointsGot = 0;
        TimePassed = 0;

        startGamePosition = Drone.transform.position;

        startForwardDirection = Drone.transform.forward;
        startRightDirection = Drone.transform.right;
        startUpDirection = Drone.transform.up;

        for (int i = 0; i < HeartsUI.Length; i++)
        {
            HeartsUI[i].gameObject.SetActive(true);
            HeartsUI[i].transform.localScale = Vector3.one;
        }
        StartGame1.SetActive(false);
        StartGame2.SetActive(false);

        HeartIndex = 4;
        WallsParent = new GameObject("Obstacle's parent");
        WallsParent.transform.position = Vector3.zero;

        GameObject Wall_R = Instantiate(WallPrefab, startGamePosition + startRightDirection * (routeWidth / 2), Quaternion.identity);
        Wall_R.transform.localScale = new Vector3(0.1f, WallHeight, WallLength);
        Wall_R.transform.rotation = Quaternion.LookRotation(startForwardDirection);

        GameObject Wall_L = Instantiate(WallPrefab, startGamePosition - startRightDirection * (routeWidth / 2), Quaternion.identity);
        Wall_L.transform.localScale = new Vector3(0.1f, WallHeight, WallLength);
        Wall_L.transform.rotation = Quaternion.LookRotation(startForwardDirection);

        Wall_R.transform.SetParent(WallsParent.transform);
        Wall_L.transform.SetParent(WallsParent.transform);

        //GameObject Wall_U = Instantiate(WallPrefab, startGamePosition + startUpDirection * (WallHeight / 2), Quaternion.identity);
        //Wall_U.transform.localScale = new Vector3(routeWidth, 0.1f, WallLength);
        //Wall_U.transform.rotation = Quaternion.LookRotation(startForwardDirection);

        //GameObject Wall_D = Instantiate(WallPrefab, startGamePosition - startUpDirection * (WallHeight / 2), Quaternion.identity);
        //Wall_D.transform.localScale = new Vector3(routeWidth, 0.1f, WallLength);
        //Wall_D.transform.rotation = Quaternion.LookRotation(startForwardDirection);
    }

    public void GenerateHorObstacle()
    {
        float Width = Random.Range(singleWallMin, singleWallMax);
        Vector3 generatePosition = Random.Range(-routeWidth / 2 + Width / 2, routeWidth / 2 - Width / 2) * startRightDirection + startGamePosition + generateDistance * startForwardDirection;

        GameObject HorObstacle = Instantiate(ObstaclePrefab, generatePosition, Quaternion.identity);
        HorObstacle.transform.localScale = new Vector3(0.2f, WallHeight, Width);
        HorObstacle.transform.rotation = Quaternion.LookRotation(startRightDirection);
        HorObstacle.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        HorObstacle.transform.SetParent(WallsParent.transform);

        HorObstacle.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
    }

    public void GenerateVerObstacle()
    {
        float Width = Random.Range(singleWallMin, singleWallMax);
        Vector3 generatePosition = Random.Range(-routeHeigh / 2 + Width / 2, routeHeigh / 2 + Width / 2) * startUpDirection + startGamePosition + generateDistance * startForwardDirection;

        GameObject VerObstacle = Instantiate(ObstaclePrefab, generatePosition, Quaternion.identity);
        VerObstacle.transform.localScale = new Vector3(0.2f, Width, routeWidth);
        VerObstacle.transform.rotation = Quaternion.LookRotation(startRightDirection);
        VerObstacle.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        VerObstacle.transform.SetParent(WallsParent.transform);

        VerObstacle.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
    }

    public void GenerateTwoHorObstacle()
    {
        float Width1 = Random.Range(singleWallMin, singleWallMax);
        float Width2 = Random.Range(singleWallMin, singleWallMax);

        Vector3 generatePosition1 = Random.Range(-routeWidth / 2 + Width1 / 2, -Width1 / 2) * startRightDirection + startGamePosition + generateDistance * startForwardDirection;
        Vector3 generatePosition2 = Random.Range(Width2 / 2, routeWidth / 2 - Width2 / 2) * startRightDirection + startGamePosition + generateDistance * startForwardDirection;

        GameObject HorObstacle1 = Instantiate(ObstaclePrefab, generatePosition1, Quaternion.identity);
        GameObject HorObstacle2 = Instantiate(ObstaclePrefab, generatePosition2, Quaternion.identity);

        HorObstacle1.transform.localScale = new Vector3(1, WallHeight, Width1);
        HorObstacle1.transform.rotation = Quaternion.LookRotation(startRightDirection);
        HorObstacle1.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        HorObstacle1.transform.SetParent(WallsParent.transform);

        HorObstacle2.transform.localScale = new Vector3(1, WallHeight, Width2);
        HorObstacle2.transform.rotation = Quaternion.LookRotation(startRightDirection);
        HorObstacle2.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        HorObstacle2.transform.SetParent(WallsParent.transform);

        HorObstacle1.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
        HorObstacle2.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
    }

    public void GenerateTwoVerObstacle()
    {
        float Width1 = Random.Range(singleWallMin, singleWallMax);
        float Width2 = Random.Range(singleWallMin, singleWallMax);

        Vector3 generatePosition1 = Random.Range(-routeHeigh / 2 + Width1 / 2, -Width1 / 2) * startUpDirection + startGamePosition + generateDistance * startForwardDirection;
        Vector3 generatePosition2 = Random.Range(Width2 / 2, routeHeigh / 2 - Width2 / 2) * startUpDirection + startGamePosition + generateDistance * startForwardDirection;


        GameObject VerObstacle1 = Instantiate(ObstaclePrefab, generatePosition1, Quaternion.identity);
        GameObject VerObstacle2 = Instantiate(ObstaclePrefab, generatePosition2, Quaternion.identity);

        VerObstacle1.transform.localScale = new Vector3(0.2f, Width1, routeWidth);
        VerObstacle1.transform.rotation = Quaternion.LookRotation(startRightDirection);
        VerObstacle1.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        VerObstacle1.transform.SetParent(WallsParent.transform);

        VerObstacle2.transform.localScale = new Vector3(0.2f, Width2, routeWidth);
        VerObstacle2.transform.rotation = Quaternion.LookRotation(startRightDirection);
        VerObstacle2.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        VerObstacle2.transform.SetParent(WallsParent.transform);

        VerObstacle1.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
        VerObstacle2.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
    }

    public void GenerateLRObstacles()
    {
        float crossDistanceToL = Random.Range(1.0f, routeWidth - 1.0f);
        Vector3 crossPosition = (crossDistanceToL - routeWidth / 2) * startRightDirection + startGamePosition + generateDistance * startForwardDirection;
        float crossWidth = Random.Range(crossWidthMin, crossWidthMax);

        float leftWallLength = crossDistanceToL - (crossWidth / 2);
        float rightWallLength = routeWidth - crossDistanceToL - (crossWidth / 2);

        Vector3 leftWallPosition = crossPosition - ((crossWidth / 2) + (leftWallLength / 2)) * startRightDirection;
        Vector3 rightWallPosition = crossPosition + ((crossWidth / 2) + (rightWallLength / 2)) * startRightDirection;


        GameObject leftObstacle = Instantiate(ObstaclePrefab, leftWallPosition, Quaternion.identity);
        leftObstacle.transform.localScale = new Vector3(0.2f, WallHeight, leftWallLength);
        leftObstacle.transform.rotation = Quaternion.LookRotation(startRightDirection);
        leftObstacle.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        leftObstacle.transform.SetParent(WallsParent.transform);

        GameObject RightObstacle = Instantiate(ObstaclePrefab, rightWallPosition, Quaternion.identity);
        RightObstacle.transform.localScale = new Vector3(0.2f, WallHeight, rightWallLength);
        RightObstacle.transform.rotation = Quaternion.LookRotation(startRightDirection);
        RightObstacle.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        RightObstacle.transform.SetParent(WallsParent.transform);

        leftObstacle.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
        RightObstacle.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
    }

    public void GenerateUDObstacles()
    {
        float crossDistanceToB = Random.Range(WallHeight / 2 - routeHeigh / 2, WallHeight / 2 + routeHeigh / 2);
        Vector3 crossPosition = (crossDistanceToB - WallHeight / 2) * startUpDirection + startGamePosition + generateDistance * startForwardDirection;
        float crossWidth = Random.Range(crossWidthMin, crossWidthMax);

        float upWallLength = crossDistanceToB - (crossWidth / 2);
        float downWallLength = WallHeight - crossDistanceToB - (crossWidth / 2);

        Vector3 upWallPosition = crossPosition - ((crossWidth / 2) + (upWallLength / 2)) * startUpDirection;
        Vector3 downWallPosition = crossPosition + ((crossWidth / 2) + (downWallLength / 2)) * startUpDirection;


        GameObject upObstacle = Instantiate(ObstaclePrefab, upWallPosition, Quaternion.identity);
        upObstacle.transform.localScale = new Vector3(0.2f, upWallLength, routeWidth);
        upObstacle.transform.rotation = Quaternion.LookRotation(startRightDirection);
        upObstacle.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        upObstacle.transform.SetParent(WallsParent.transform);

        GameObject downObstacle = Instantiate(ObstaclePrefab, downWallPosition, Quaternion.identity);
        downObstacle.transform.localScale = new Vector3(0.2f, downWallLength, routeWidth);
        downObstacle.transform.rotation = Quaternion.LookRotation(startRightDirection);
        downObstacle.GetComponent<C_Obstacle>().MoveDirection = -startForwardDirection;
        downObstacle.transform.SetParent(WallsParent.transform);

        upObstacle.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
        downObstacle.GetComponent<MeshRenderer>().material = ObstacleMaterials[MaterialIndex];
    }

    public void GetHurt()
    {
        StartCoroutine(heartDisappear(HeartsUI[HeartIndex], 0.1f));
        if (HeartIndex == 0)
        {
            GameOver();
            return;
        }
        HeartIndex--;
    }

    public void GetHeart()
    {
        if(HeartIndex == 4)
        {
            return;
        }
        HeartIndex++;
        StartCoroutine(heartAppear(HeartsUI[HeartIndex], 0.1f));
    }

    public void GameOver()
    {
        isStartGame = false;
        //for(int i = 0; i < WallsParent.transform.childCount; i++)
        //{
        //    Destroy(WallsParent.transform.GetChild(i).gameObject);
        //}
        Destroy(WallsParent);
        for (int i = 0; i < HeartsUI.Length; i++)
        {
            HeartsUI[i].gameObject.SetActive(false);
        }
        StartGame1.SetActive(true);
        StartGame2.SetActive(true);

        PointsGotUI.GetComponent<TextMeshProUGUI>().text = "";
        FinishGameUI.GetComponent<TextMeshProUGUI>().text = "Points Got Last Time: " + Mathf.Round(PointsGot);
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

    public void resetDronePosition()
    {
        Drone.transform.position = startDronePosition + Drone.transform.forward * 1.0f;
    }
}
