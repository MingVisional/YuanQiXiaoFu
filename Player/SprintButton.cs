using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SprintButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject character;
    public bool isView;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        if(isView)character.GetComponent<PlayerControl>().sprint1();
        else character.GetComponent<HorizontalControl>().sprint1();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if(isView)character.GetComponent<PlayerControl>().sprint2();
        else character.GetComponent<HorizontalControl>().sprint2();
    }

}