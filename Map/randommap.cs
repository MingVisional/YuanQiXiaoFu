using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randommap : MonoBehaviour
{
    // Start is called before the first frame update
    private float[] posx = { -23.1f ,-6.9f,9.3f,25.5f};
    private float[] posy = { -10.8f ,-23.76f,};
    private string[] parts = { "part1","part2","part3","part4","part5"};
    private int index;
    void Start()
    {
        for(int i = 0; i < posx.Length; i++)
        {
            for(int j = 0; j < posy.Length; j++)
            {
                index = Random.Range(0, parts.Length);
                getpart(posx[i], posy[j], parts[index]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void getpart(float posx,float posy,string name)
    {
        PoolManager.GetInstance().GetObj("prefabs/mappart/"+name, (o) =>
         {
             o.transform.position = transform.position + new Vector3(posx, posy, 0);
             o.transform.localScale = new Vector3(2, 1.6f, 0);
             o.transform.SetParent(this.transform.parent);
         });
            //Debug.Log(xpos.ToString()+ypos.ToString());
    }
}
