using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float speed = 6;

    public int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(1, (int)speed);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * (randomNumber * 1.0f));
    }
}
