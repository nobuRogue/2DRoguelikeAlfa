using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_CharacterData : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int ID;
		public int nameID;
		public string spriteName;
		public int HP;
		public int Attack;
		public int Defense;
	}
}

