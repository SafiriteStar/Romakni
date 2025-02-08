using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{

    public string[] tags;
    public GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerCollision(collision);
    }

    protected virtual void OnTriggerCollision(Collider2D collision)
    {
        foreach (string tag in tags)
        {
            if (collision.CompareTag(tag))
            {
                Destroy(gameObject);

                if (effect)
                {
                    Instantiate(effect, transform.position, transform.rotation);
                }
            }
        }
    }
}
