using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private HUD hud;
    int level = 1;
    int requiredExp;
    int storedExp = 0;
    int storedCoin = 0;

    //void Start()
    //{
    //    hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
    //    requiredExp = (int)(level * 100 * 1.25);
    //    hud.SetupXP(requiredExp);
    //}

    //void Update()
    //{
    //    hud.UpdateXP(storedExp);
    //    requiredExp = (int)(level * 100 * 1.25);
    //    if(storedExp >= requiredExp)
    //    {
    //        storedExp -= requiredExp;
    //        hud.SetupXP(requiredExp);
    //        level++;
    //    }
    //}

    //public int GetLevel()
    //{
    //    return level;
    //}
    
    //public int GetStoredXP()
    //{
    //    return storedExp;
    //}

    //public void AddEXP(int exp)
    //{
    //    storedExp += exp;
    //    hud.UpdateXP(storedExp);
    //}

    //public void AddCoin(int coin)
    //{
    //    storedCoin += coin;
    //}

    //public void DeductEXP(int exp)
    //{
    //    storedExp -= exp;
    //    hud.UpdateXP(storedExp);
    //}

    //public void DeductCoin(int coin)
    //{
    //    storedCoin -= coin;
    //}
}
