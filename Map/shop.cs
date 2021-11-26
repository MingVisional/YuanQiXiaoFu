using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject talkpanel,shopmain,joy,shoplist;
    private string[] StuffNames = { "stuff1", "stuff2", "stuff3", "stuff4" ,"BoxingGloves","Horn","Other","Fan"};
    void Start()
    {
        GetStuffs(6);
        joy=transform.parent.parent.parent.transform.FindChild("UiButton").gameObject;
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
    public void OpenShop()
    {
        shopmain.SetActive(true);
        joy.SetActive(false);
        Time.timeScale = 0;
        talkpanel.SetActive(false);
    }
    public void OutShop()
    {
        shopmain.SetActive(false);
        Time.timeScale = 1f;
        joy.SetActive(true);
        talkpanel.SetActive(true);
    }
    public void GetStuffs(int stuffNum)
    {
        for (int i = 0; i < stuffNum; i++)
        {
            int shopKey;
            shopKey = Random.Range(0,StuffNames.Length);
            PoolManager.GetInstance().GetObj("prefabs/Commodity/"+StuffNames[shopKey], (o) =>
            {
                o.transform.SetParent(shoplist.transform);
                o.transform.FindChild("Cost").GetComponent<Text>().text = o.GetComponent<ShopStuff>().TheItem.ItemCost.ToString();
            });
        }
    }
    
}
