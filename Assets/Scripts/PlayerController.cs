﻿using System;
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
    public class WeaponStats
    {
        public WeaponType Type;
        public int Level;
        public float Durability;
    }

    public string nickname = "";
    public int deviceID = -1;
    public Color playerColor;

    public float HP => Bot.HealthPercentage;

    public BotBase Bot { get; set; }

    public Dictionary<WeaponType, WeaponStats> WeaponLevel { get; set; } = new Dictionary<WeaponType, WeaponStats>();

    // needs reference to what weapons this has
    public void AddWeapon(WeaponType weapon)
    {
        if (WeaponLevel.ContainsKey(weapon))
        {
            WeaponLevel[weapon].Level += 1;
        }
        else
        {
			// TODO: figure out how to add a weapon component as child of BotBase dynamically
			// Apparently this means finding the first instance of a child prefab that matches WeaponType
			GetComponentsInChildren<Weapon>(true).First(x => x.TypeOfWeapon == weapon)?.gameObject.SetActive(true);
        }
    }

    public Result ButtonInput(string input)
    {
        switch (input)
        {
            case "buy_flamethrower":
                return BuyWeapon(WeaponType.Flamethrower);
            case "buy_sword":
                return BuyWeapon(WeaponType.Sword);
            case "buy_boxglove":
                return BuyWeapon(WeaponType.BoxGlove);
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
            case "repair_boxglove":
                return BuyRepair(WeaponType.BoxGlove);
            case "repair_cannon":
                return BuyRepair(WeaponType.Cannon);
            case "repair_mines":
                return BuyRepair(WeaponType.Mines);
            case "repair_shurikens":
                return BuyRepair(WeaponType.Shurikens);

            default:
                return Result.Error;
                /*
                    case "right":
                        anim.SetBool("isRunning", true);
                        rightButton = true;
                        spr.flipX = false;
                        break;
                    case "left":
                        anim.SetBool("isRunning", true);
                        leftButton = true;
                        spr.flipX = true;
                        break;
                    case "right-up":
                        rightButton = false;
                        anim.SetBool("isRunning", false);
                        break;
                    case "left-up":
                        leftButton = false;
                        anim.SetBool("isRunning", false);
                        break;
                    case "jump":
                        jumpButton = true;
                        anim.SetBool("isJumping", true);
                        break;
                    case "jump-up":
                        jumpButton = false;
                        break;
                    case "interact":
                        interactButton = true;
                        break;
                */
        }
    }

    private Result BuyWeapon(WeaponType weaponType)
    {
        AddWeapon(weaponType);
        Debug.Log(weaponType.ToString() + " BOUGHT");
        return Result.Success;
    }

    private Result BuyRepair(WeaponType repairType)
    {
        Bot.HealDamage(10f);
        Debug.Log(repairType.ToString() + " BOUGHT");
        return Result.Success;
    }
}
