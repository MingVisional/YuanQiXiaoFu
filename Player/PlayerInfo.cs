using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    private static PlayerInfo instance;
    public static PlayerInfo GetInstance()
    {
        if (instance == null)
            instance = new PlayerInfo();
        return instance;
    }
    private float hp= PlayerPrefs.GetFloat("HP");
    private float maxHp=150f;
    private float attack = PlayerPrefs.GetFloat("attack");
    private float basicAttack;
    private float gold = PlayerPrefs.GetFloat("goldplayer");
    //技能倍率
    private float sprintPower = PlayerPrefs.GetFloat("sprintPower");
    private float soundBulletPower = PlayerPrefs.GetFloat("soundBulletPower");
    private float normalAttackPower = PlayerPrefs.GetFloat("normalAttackPower");

    public float Hp { get => hp;
        set
        {
            hp = value;
            PlayerHpBar.Change(hp / maxHp);
        }
    }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Attack { get => attack; set => attack = value; }
    public float BasicAttack { get => basicAttack; set => basicAttack = value; }
    public float Gold { get => gold; set
        {
            gold = value;
            GoldNum.fixGold(gold);
        }
    }
    public float SprintPower { get => sprintPower; set => sprintPower = value; }
    public float SoundBulletPower { get => soundBulletPower; set => soundBulletPower = value; }
    public float NormalAttackPower { get => normalAttackPower; set => normalAttackPower = value; }

    public void fixData()
    {
        PlayerPrefs.SetFloat("HP", hp);
        PlayerPrefs.SetFloat("attack",attack);
        PlayerPrefs.SetFloat("goldplayer", gold);
        PlayerPrefs.SetFloat("sprintPower", sprintPower);
        PlayerPrefs.SetFloat("soundBulletPower", soundBulletPower);
        PlayerPrefs.SetFloat("normalAttackPower", normalAttackPower);
    }
    public void initData()
    {
        maxHp = 150*(PlayerPrefs.GetFloat("hp")+1);
        hp = PlayerPrefs.GetFloat("HP");
        attack = PlayerPrefs.GetFloat("attack");
        gold = PlayerPrefs.GetFloat("goldplayer");
        sprintPower = PlayerPrefs.GetFloat("sprintPower");
        soundBulletPower = PlayerPrefs.GetFloat("soundBulletPower");
        normalAttackPower = PlayerPrefs.GetFloat("normalAttackPower");
}
}
