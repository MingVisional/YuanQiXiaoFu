using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    private static Image HealthBar;
    void Start()
    {
        HealthBar = GetComponent<Image>();
        HealthBar.fillAmount = 1f;
    }
    public static void Change(float res)
    {
        HealthBar.fillAmount = res;
    }
}