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
	Shield,
	Body
}

public enum Result
{
	Success,
	NotEnoughMoney,
	NotAllowed,
	Error
}

public class Constants : Singleton<Constants>
{
	public static readonly Dictionary<WeaponType, Dictionary<int, int>> WeaponCost = new Dictionary<WeaponType, Dictionary<int, int>>()
	{
		{ WeaponType.Flamethrower, new Dictionary<int, int>() {
			{ 1, 10 },
			{ 2, 20 },
			{ 3, 30 } } },
		{ WeaponType.Knife, new Dictionary<int, int>() {
			{ 1, 10 },
			{ 2, 20 },
			{ 3, 30 } } },
		{ WeaponType.Nuke, new Dictionary<int, int>() {
			{ 1, 10 },
			{ 2, 20 },
			{ 3, 30 } } }
	};

	public static readonly Dictionary<RepairType, int> RepairCost = new Dictionary<RepairType, int>()
	{
		{ RepairType.Shield, 1 },
		{ RepairType.Body, 1 }
	};
}
