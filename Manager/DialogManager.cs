using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private GameObject dialog;
    private Text dialogText;
    private GameObject nameDialog;
    private Text nameText;
    private Image image;
    private static DialogManager instance;

    public GameObject Dialog { get => dialog; set => dialog = value; }
    public Text DialogText { get => dialogText; set => dialogText = value; }
    public GameObject NameDialog { get => nameDialog; set => nameDialog = value; }
    public Text NameText { get => nameText; set => nameText = value; }
    public Image Image { get => image; set => image = value; }

    private void Awake()
    {
        dialog = transform.Find("Dialog").gameObject;
        dialogText = transform.Find("Dialog").Find("Text").GetComponent<Text>();
        nameDialog = transform.Find("Dialog").Find("Name").gameObject;
        nameText = transform.Find("Dialog").Find("Name").Find("NameText").GetComponent<Text>();
        image = transform.Find("Dialog").Find("DialogImage").GetComponent<Image>();
        instance = this;
    }
    public static DialogManager GetInstance()
    {
        return instance;
    }
}