using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Obstacle : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MoveDirection;
    public float DisppearTime = 30.0f;
    public float moveSpeed = 5.0f;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = DisppearTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        this.transform.position += moveSpeed * Time.deltaTime * MoveDirection;
        if(timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
