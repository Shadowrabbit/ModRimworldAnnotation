using System;

namespace Verse
{
	// Token: 0x02000163 RID: 355
	public class LanguageWorker_Romanian : LanguageWorker
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x00034649 File Offset: 0x00032849
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (!plural)
			{
				return ((gender == Gender.Female) ? "a " : "un ") + str;
			}
			if (gender != Gender.Male)
			{
				return str + "e";
			}
			return str + "i";
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00034688 File Offset: 0x00032888
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
			if (plural)
			{
				if (gender != Gender.Male)
				{
					return str + "e";
				}
				return str + "i";
			}
			else
			{
				if (!this.IsVowel(ch))
				{
					return str + "ul";
				}
				if (gender == Gender.Male)
				{
					return str + "le";
				}
				return str + "a";
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00034703 File Offset: 0x00032903
		public bool IsVowel(char ch)
		{
			return "aeiouâîAEIOUÂÎ".IndexOf(ch) >= 0;
		}
	}
}
