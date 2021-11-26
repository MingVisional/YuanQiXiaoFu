using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flower_attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().Hurt(transform.parent.GetComponent<flower>().parameter.attack, transform.parent.localScale.x);
        }
    }
}
