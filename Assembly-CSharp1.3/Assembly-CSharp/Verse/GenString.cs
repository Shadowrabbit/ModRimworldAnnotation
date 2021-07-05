using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200002D RID: 45
	public static class GenString
	{
		// Token: 0x06000285 RID: 645 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D202 File Offset: 0x0000B402
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

		// Token: 0x06000287 RID: 647 RVA: 0x0000D231 File Offset: 0x0000B431
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

		// Token: 0x06000288 RID: 648 RVA: 0x0000D248 File Offset: 0x0000B448
		public static StringBuilder AppendLineIfNotEmpty(this StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			return sb;
		}

		// Token: 0x04000070 RID: 112
		private static string[] numberStrings = new string[10000];
	}
}
