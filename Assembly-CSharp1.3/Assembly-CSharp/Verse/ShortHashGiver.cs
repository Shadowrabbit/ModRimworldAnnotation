using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200007D RID: 125
	public static class ShortHashGiver
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x00019468 File Offset: 0x00017668
		public static void GiveAllShortHashes()
		{
			ShortHashGiver.takenHashesPerDeftype.Clear();
			List<Def> list = new List<Def>();
			foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
			{
				IEnumerable enumerable = (IEnumerable)typeof(DefDatabase<>).MakeGenericType(new Type[]
				{
					type
				}).GetProperty("AllDefs").GetGetMethod().Invoke(null, null);
				list.Clear();
				foreach (object obj in enumerable)
				{
					Def item = (Def)obj;
					list.Add(item);
				}
				list.SortBy((Def d) => d.defName);
				for (int i = 0; i < list.Count; i++)
				{
					ShortHashGiver.GiveShortHash(list[i], type);
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001958C File Offset: 0x0001778C
		private static void GiveShortHash(Def def, Type defType)
		{
			if (def.shortHash != 0)
			{
				Log.Error(def + " already has short hash.");
				return;
			}
			HashSet<ushort> hashSet;
			if (!ShortHashGiver.takenHashesPerDeftype.TryGetValue(defType, out hashSet))
			{
				hashSet = new HashSet<ushort>();
				ShortHashGiver.takenHashesPerDeftype.Add(defType, hashSet);
			}
			ushort num = (ushort)(GenText.StableStringHash(def.defName) % 65535);
			int num2 = 0;
			while (num == 0 || hashSet.Contains(num))
			{
				num += 1;
				num2++;
				if (num2 > 5000)
				{
					Log.Message("Short hashes are saturated. There are probably too many Defs.");
				}
			}
			def.shortHash = num;
			hashSet.Add(num);
		}

		// Token: 0x04000199 RID: 409
		private static Dictionary<Type, HashSet<ushort>> takenHashesPerDeftype = new Dictionary<Type, HashSet<ushort>>();
	}
}
