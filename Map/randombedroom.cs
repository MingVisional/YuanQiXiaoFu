using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randombedroom : MonoBehaviour
{
    // Start is called before the first frame update
    private float[] posx = { -15f,-6.9f,1.2f,9.3f};
    private float[] posy = { -5.1f,-9.9f};
    private string[] parts = { "part1", "part2", "part3", "part4", "part5", "part6" ,"part7","part8","part9"};
    private int index;
    void Start()
    {
        for (int i = 0; i < posx.Length; i++)
        {
            for (int j = 0; j < posy.Length; j++)
            {
                index = Random.Range(0, parts.Length);
                getpart(posx[i], posy[j], parts[index]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            loadNext();
        }
    }
    void getpart(float posx, float posy, string name)
    {
        PoolManager.GetInstance().GetObj("prefabs/mappart/bedroompart/" + name, (o) =>
        {
            o.transform.position = transform.position + new Vector3(posx, posy, 0);
            o.transform.localScale = new Vector3(1, 1, 0);
            o.transform.SetParent(this.transform);
        });
        //Debug.Log(xpos.ToString()+ypos.ToString());
    }
    public void loadNext()
    {
        transform.parent.GetComponent<LevelManner>().getNextAfter(2);
        Destroy(gameObject);
        
    }

}
