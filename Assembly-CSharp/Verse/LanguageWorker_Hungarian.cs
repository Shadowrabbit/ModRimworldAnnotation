using System;

namespace Verse
{
	// Token: 0x02000213 RID: 531
	public class LanguageWorker_Hungarian : LanguageWorker
	{
		// Token: 0x06000DB8 RID: 3512 RVA: 0x00010589 File Offset: 0x0000E789
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "egy " + str;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000AED3C File Offset: 0x000ACF3C
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "az " + str;
			}
			return "a " + str;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0001059C File Offset: 0x0000E79C
		public bool IsVowel(char ch)
		{
			return "eéöőüűiíaáoóuúEÉÖŐÜŰIÍAÁOÓUÚ".IndexOf(ch) >= 0;
		}
	}
}
