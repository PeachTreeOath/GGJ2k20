using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponUpgradeEventArgs : EventArgs
{
    public WeaponType Weapon { get; set; }
    public int Level { get; set; }
}

public class PlayerController : MonoBehaviour
{
    public string nickname = "";
    public int deviceID = -1;
    public Color playerColor;

    public float HP => Bot.HealthPercentage;

    private BotBase bot;
    public BotBase Bot
    {
        get { return bot; }
        set
        {
            bot = value;
            WeaponTypeMap.Clear();
            foreach (Weapon w in bot.GetComponentsInChildren<Weapon>(true))
            {
                w.CurrentDurability = 100f;
                w.gameObject.SetActive(false);
                w.Level = 1;
                WeaponTypeMap.Add(w.TypeOfWeapon, w);
            }
        }
    }

    public Dictionary<WeaponType, Weapon> WeaponTypeMap { get; set; } = new Dictionary<WeaponType, Weapon>();

    public Result ButtonInput(string input)
    {
        switch (input)
        {
            case "buy_flamethrower":
                return BuyWeapon(WeaponType.Flamethrower);
            case "buy_sword":
                return BuyWeapon(WeaponType.Sword);
            //case "buy_boxglove":
            //    return BuyWeapon(WeaponType.BoxGlove);
            case "buy_cannon":
                return BuyWeapon(WeaponType.Cannon);
            case "buy_mines":
                return BuyWeapon(WeaponType.Mines);
            case "buy_shurikens":
                return BuyWeapon(WeaponType.Shurikens);

            case "repair_flamethrower":
                return BuyRepair(WeaponType.Flamethrower);
            case "repair_sword":
                return BuyRepair(WeaponType.Sword);
            //case "repair_boxglove":
            //    return BuyRepair(WeaponType.BoxGlove);
            case "repair_cannon":
                return BuyRepair(WeaponType.Cannon);
            case "repair_mines":
                return BuyRepair(WeaponType.Mines);
            case "repair_shurikens":
                return BuyRepair(WeaponType.Shurikens);

            default:
                return Result.Error;
        }
    }

    private Result BuyWeapon(WeaponType weaponType)
    {
        if (WeaponTypeMap.ContainsKey(weaponType))
        {
            if (WeaponTypeMap[weaponType].gameObject.activeSelf)
            {
                WeaponTypeMap[weaponType].Level = WeaponTypeMap[weaponType].Level + 1;
            }
            else
            {
                WeaponTypeMap[weaponType].gameObject.SetActive(true);
            }
        }

        Debug.Log(weaponType.ToString() + " BOUGHT");
        return Result.Success;
    }

    private Result BuyRepair(WeaponType repairType)
    {
        if (WeaponTypeMap.ContainsKey(repairType))
        {
            WeaponTypeMap[repairType].CurrentDurability = 100f;
        }
        Debug.Log(repairType.ToString() + " REPAIRED");
        return Result.Success;
    }
}
