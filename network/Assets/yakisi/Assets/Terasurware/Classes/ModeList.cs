using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public string ID;
		public string Name;
		public int Power;
		public float Range;
		public int Bullet;
		public float Speed;
	}
}