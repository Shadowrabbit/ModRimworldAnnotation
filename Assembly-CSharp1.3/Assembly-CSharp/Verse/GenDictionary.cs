using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200002A RID: 42
	public static class GenDictionary
	{
		// Token: 0x06000245 RID: 581 RVA: 0x0000BB40 File Offset: 0x00009D40
		public static string ToStringFullContents<K, V>(this Dictionary<K, V> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> keyValuePair in dict)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				K key = keyValuePair.Key;
				string str = key.ToString();
				string str2 = ": ";
				V value = keyValuePair.Value;
				stringBuilder2.AppendLine(str + str2 + value.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		public static bool NullOrEmpty<K, V>(this Dictionary<K, V> dict)
		{
			return dict == null || dict.Count == 0;
		}
	}
}
