using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000C86 RID: 3206
	public static class ResolveParamsUtility
	{
		// Token: 0x06004ACC RID: 19148 RVA: 0x0018B554 File Offset: 0x00189754
		public static void SetCustom<T>(ref Dictionary<string, object> custom, string name, T obj, bool inherit = false)
		{
			if (custom == null)
			{
				custom = new Dictionary<string, object>();
			}
			else
			{
				custom = new Dictionary<string, object>(custom);
			}
			if (!custom.ContainsKey(name))
			{
				custom.Add(name, obj);
				return;
			}
			if (!inherit)
			{
				custom[name] = obj;
			}
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x0018B5A1 File Offset: 0x001897A1
		public static void RemoveCustom(ref Dictionary<string, object> custom, string name)
		{
			if (custom == null)
			{
				return;
			}
			custom = new Dictionary<string, object>(custom);
			custom.Remove(name);
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0018B5BC File Offset: 0x001897BC
		public static bool TryGetCustom<T>(Dictionary<string, object> custom, string name, out T obj)
		{
			object obj2;
			if (custom == null || !custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				return false;
			}
			obj = (T)((object)obj2);
			return true;
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x0018B5F0 File Offset: 0x001897F0
		public static T GetCustom<T>(Dictionary<string, object> custom, string name)
		{
			object obj;
			if (custom == null || !custom.TryGetValue(name, out obj))
			{
				return default(T);
			}
			return (T)((object)obj);
		}
	}
}
