using System;

namespace Verse
{
	// Token: 0x0200015E RID: 350
	public class LanguageWorker_Hungarian : LanguageWorker
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x00033F22 File Offset: 0x00032122
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "egy " + str;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00033F38 File Offset: 0x00032138
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

		// Token: 0x060009C9 RID: 2505 RVA: 0x00033F7D File Offset: 0x0003217D
		public bool IsVowel(char ch)
		{
			return "eéöőüűiíaáoóuúEÉÖŐÜŰIÍAÁOÓUÚ".IndexOf(ch) >= 0;
		}
	}
}
