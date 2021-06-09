using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200004A RID: 74
	public static class GenDictionary
	{
		// Token: 0x06000313 RID: 787 RVA: 0x00082328 File Offset: 0x00080528
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
	}
}
