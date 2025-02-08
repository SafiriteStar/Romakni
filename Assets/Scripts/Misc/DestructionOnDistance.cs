using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionOnDistance : MonoBehaviour
{
    public Vector3 target;
    public GameObject effect;

    public void SetTarget (Vector3 targetPosition)
    {
        target = targetPosition;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target) < 0.1)
        {
            Destroy(gameObject);

            if (effect)
            {
                Instantiate(effect, transform.position, transform.rotation);
            }
        }
    }
}
