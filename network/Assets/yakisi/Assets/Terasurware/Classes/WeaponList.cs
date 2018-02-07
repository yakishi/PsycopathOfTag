using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		public string ID;
		public string Name;
		public int Bullet;
		public float Load;
		public float Speed;
		public float Range;
		public int Power;
		public float Time;
	}
}