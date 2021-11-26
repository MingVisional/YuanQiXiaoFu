using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play_HP : MonoBehaviour
{
    public static float Health_max = 10f;
    public static float Health_current;
    private Image HealthBar;
    //float i = 1f;
    void Start()
    {
        HealthBar = GetComponent<Image>();
        Health_current = Health_max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void Change(float res)
    {
        HealthBar.fillAmount = res;
    }
}
