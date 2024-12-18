using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Heart : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 1.0f;

    private Vector3 startPosition;

    public float rotationSpeed = 50f;
    public float timeOffset = 0f;

    public AudioClip GetHeart;
    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        // 浮动效果
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 旋转效果
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(GetHeart, this.transform.position);
            C_Game2.Instance.GetHeart();
            Destroy(gameObject);
        }
    }
}
