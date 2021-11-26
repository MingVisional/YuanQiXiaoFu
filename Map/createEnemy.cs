using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform EnemyPoints;
    private string[] 
        BedRoom = { "fan_green", "fan_pink", "garbage_black", "garbage_yellow" , "kettle_black", "kettle_orange" },
        SportRoom= { "badminton_green", "badminton_white", "ball_blue", "ball_pink", "clock_black", "clock_yellow" },
        ClassRoom = { "bluebook", "classbell_blue", "classbell_green", "pen_blue", "pen_red", "redbook" },
        SchoolRoad= { "flower", "tree" };
    public int ChoiceLevel;
    private int index;
    void Start()
    {
        EnemyPoints = transform.Find("Enemy");
        //getEnemy(EnemyPoints.childCount-1);
        GameObject.Find("TotalControl").GetComponent<TotalControl>().SumOfTheLevel = EnemyPoints.childCount;
        for (int i = 0; i < EnemyPoints.childCount; i++)
        {
            if (ChoiceLevel == 1)
            {
                index = Random.Range(0, BedRoom.Length);
                getEnemy(EnemyPoints.GetChild(i).position,"Room/"+BedRoom[index]);
            }
            else if(ChoiceLevel == 2)
            {
                index = Random.Range(0, SportRoom.Length);
                getEnemy(EnemyPoints.GetChild(i).position, "GYM/" + SportRoom[index]);
            }
            else if (ChoiceLevel == 3)
            {
                index = Random.Range(0, ClassRoom.Length);
                getEnemy(EnemyPoints.GetChild(i).position, "Class/" + ClassRoom[index]);
            }
            else if (ChoiceLevel == 4)
            {
                index = Random.Range(0, SchoolRoad.Length);
                getEnemy(EnemyPoints.GetChild(i).position, "Road/" + SchoolRoad[index]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getEnemy(Vector3 pos,string name)
    {

        /*PoolManager.GetInstance().GetObj("Prefab/Enemy/"+name, (o) =>
        {
            o.transform.position = new Vector3(0,0,0);
            o.transform.position = new Vector3(pos.x,pos.y, 0);
            o.transform.SetParent(transform.parent);

        });*/
        GameObject enemy = Resources.Load<GameObject>("Prefab/Enemy/" + name);
        enemy = Instantiate<GameObject>(enemy, new Vector3(pos.x, pos.y, 0), Quaternion.identity, transform.parent);
        enemy.transform.GetChild(0).position = new Vector3(pos.x, pos.y, 3);
    }

        
    
}
