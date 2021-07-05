using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001625 RID: 5669
	public static class QuestNodeEqualUtility
	{
		// Token: 0x060084D5 RID: 34005 RVA: 0x002FBACC File Offset: 0x002F9CCC
		public static bool Equal(object value1, object value2, Type compareAs)
		{
			if (value1 == value2)
			{
				return true;
			}
			if (compareAs == null)
			{
				if (value1 == null)
				{
					return value2 == null;
				}
				Type type = value1.GetType();
				if (!ConvertHelper.CanConvert(value2, type))
				{
					return false;
				}
				object obj = ConvertHelper.Convert(value2, type);
				return value1.Equals(obj);
			}
			else
			{
				if (!ConvertHelper.CanConvert(value1, compareAs) || !ConvertHelper.CanConvert(value2, compareAs))
				{
					return false;
				}
				object obj2 = ConvertHelper.Convert(value1, compareAs);
				object obj3 = ConvertHelper.Convert(value2, compareAs);
				if (obj2 == null)
				{
					return obj3 == null;
				}
				return obj2.Equals(obj3);
			}
		}
	}
}
