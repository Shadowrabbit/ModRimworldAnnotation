using System;

namespace Verse
{
	// Token: 0x0200021F RID: 543
	public class LanguageWorker_Swedish : LanguageWorker
	{
		// Token: 0x06000DEF RID: 3567 RVA: 0x00010851 File Offset: 0x0000EA51
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "en " + str;
			}
			return "ett " + str;
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000AF934 File Offset: 0x000ADB34
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
			char ch = str[str.Length - 1];
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (this.IsVowel(ch))
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (this.IsVowel(ch))
				{
					return str + "t";
				}
				return str + "et";
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00010878 File Offset: 0x0000EA78
		public bool IsVowel(char ch)
		{
			return "aeiouyåäöAEIOUYÅÄÖ".IndexOf(ch) >= 0;
		}
	}
}
