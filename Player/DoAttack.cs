using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAttack : MonoBehaviour
{
    public GameObject attack;
    public void Attack()
    {
        attack.SetActive(true);
        Invoke("FixAtt", 0.15f);
    }
    void FixAtt()
    {
        attack.SetActive(false);
    }
}
