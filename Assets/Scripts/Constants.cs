using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
