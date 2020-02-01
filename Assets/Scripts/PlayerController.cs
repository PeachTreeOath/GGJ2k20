using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
	Flamethrower,
	Knife,
	Nuke
}

public enum RepairType
{

}

public enum Result
{
	Success,
	NotEnoughMoney,
	NotAllowed,
	Error
}

public class WeaponUpgradeEventArgs : EventArgs
{
	public WeaponType Weapon { get; set; }
	public int Level { get; set; }
}

public class PlayerController : MonoBehaviour
{

    public string nickname = "";
    public int deviceID = -1;

    private int cash; // Use AddRemoveCash
    private int hp;

	public event EventHandler<WeaponUpgradeEventArgs> WeaponUpgraded;

	private Dictionary<WeaponType, int> weaponLevel = new Dictionary<WeaponType, int>();

    public void AddRemoveCash(int changedCash)
    {
        cash += changedCash;

        // todo Send airconsole messages to update phones
    }

	// needs reference to what weapons this has

	private void AddWeapon(WeaponType weapon)
	{
		if (new List<WeaponType>(weaponLevel.Keys).Contains(weapon))
		{
			weaponLevel[weapon] += 1;
			WeaponUpgraded?.Invoke(this, new WeaponUpgradeEventArgs() { Weapon = weapon, Level = weaponLevel[weapon] });
		}
		else
		{
			weaponLevel.Add(weapon, 1);
			WeaponUpgraded?.Invoke(this, new WeaponUpgradeEventArgs() { Weapon = weapon, Level = 1 });
		}
	}

    public Result ButtonInput(string input)
    {
        switch (input)
        {
            case "buy_flamethrower":
                if (cash >= Constants.WeaponCost[WeaponType.Flamethrower][weaponLevel[WeaponType.Flamethrower]])
                {
                    cash -= Constants.WeaponCost[WeaponType.Flamethrower][weaponLevel[WeaponType.Flamethrower]];
					AddWeapon(WeaponType.Flamethrower);
                    Debug.Log("FLAMETHROWER BOUGHT");
					return Result.Success;
                }
				return Result.NotEnoughMoney;
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
}
