using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoldNum : MonoBehaviour
{
    // Start is called before the first frame update
    private static Text goldnum;
    void Start()
    {
        goldnum = transform.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void fixGold(float num)
    {
        goldnum.text = num.ToString();
    }
}
