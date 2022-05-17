using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

public class EnumUtil
{
	public static void GetValuesAndFieldNames<T>(out int[] ids, out string[] names)
	{
		Type type = typeof(T);
		T[] valueArr =(T[]) Enum.GetValues(type);
		List<T> valueList = new List<T>(valueArr);
		valueList.Sort();

		names = new string[valueList.Count];
		ids = new int[valueList.Count];

		for(int i = 0; i < valueList.Count; i ++)
		{

			ids[i] = Convert.ToInt32(valueList[i]);
			names[i] = Enum.GetName(type, valueList[i]);
		}

	}
}
