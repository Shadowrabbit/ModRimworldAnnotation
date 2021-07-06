using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001274 RID: 4724
	public static class ResolveParamsUtility
	{
		// Token: 0x06006703 RID: 26371 RVA: 0x001FACB8 File Offset: 0x001F8EB8
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

		// Token: 0x06006704 RID: 26372 RVA: 0x00046681 File Offset: 0x00044881
		public static void RemoveCustom(ref Dictionary<string, object> custom, string name)
		{
			if (custom == null)
			{
				return;
			}
			custom = new Dictionary<string, object>(custom);
			custom.Remove(name);
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x001FAD08 File Offset: 0x001F8F08
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

		// Token: 0x06006706 RID: 26374 RVA: 0x001FAD3C File Offset: 0x001F8F3C
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
