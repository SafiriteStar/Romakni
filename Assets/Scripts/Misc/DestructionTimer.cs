using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionTimer : MonoBehaviour
{
    public float time = 10.0f;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = time;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}