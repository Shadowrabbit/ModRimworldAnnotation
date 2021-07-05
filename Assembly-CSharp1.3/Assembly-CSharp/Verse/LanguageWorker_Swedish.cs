using System;

namespace Verse
{
	// Token: 0x02000166 RID: 358
	public class LanguageWorker_Swedish : LanguageWorker
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x00034C01 File Offset: 0x00032E01
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

		// Token: 0x060009F2 RID: 2546 RVA: 0x00034C28 File Offset: 0x00032E28
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

		// Token: 0x060009F3 RID: 2547 RVA: 0x00034C9D File Offset: 0x00032E9D
		public bool IsVowel(char ch)
		{
			return "aeiouyåäöAEIOUYÅÄÖ".IndexOf(ch) >= 0;
		}
	}
}
