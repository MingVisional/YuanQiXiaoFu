using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doattack : MonoBehaviour
{
    public GameObject attack;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getattack()
    {
        attack.SetActive(true);
        Invoke("fixatt", 0.15f);
    }
    void fixatt()
    {
        attack.SetActive(false);
    }
    

}
