﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopowner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject talkpanel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            talkpanel.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            talkpanel.SetActive(false);
        }
    }
}
