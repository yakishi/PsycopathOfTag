using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public string ID;
		public string Name;
		public double Power;
		public double Range;
		public double Bullet;
		public double Time;
		public double Speed;
	}
}