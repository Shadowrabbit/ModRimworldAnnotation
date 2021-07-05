using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000054 RID: 84
	public static class GenString
	{
		// Token: 0x06000378 RID: 888 RVA: 0x00083B00 File Offset: 0x00081D00
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000093D2 File Offset: 0x000075D2
		public static string ToStringCached(this int num)
		{
			if (num < -4999)
			{
				return num.ToString();
			}
			if (num > 4999)
			{
				return num.ToString();
			}
			return GenString.numberStrings[num + 5000];
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00009401 File Offset: 0x00007601
		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (str.NullOrEmpty())
			{
				yield break;
			}
			if (chunkLength < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength > str.Length - i)
				{
					chunkLength = str.Length - i;
				}
				yield return str.Substring(i, chunkLength);
			}
			yield break;
		}

		// Token: 0x04000184 RID: 388
		private static string[] numberStrings = new string[10000];
	}
}
