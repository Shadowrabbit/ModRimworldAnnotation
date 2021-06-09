using System;

namespace Verse
{
	// Token: 0x02000219 RID: 537
	public class LanguageWorker_Romanian : LanguageWorker
	{
		// Token: 0x06000DD3 RID: 3539 RVA: 0x000106DD File Offset: 0x0000E8DD
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

		// Token: 0x06000DD4 RID: 3540 RVA: 0x000AF2A0 File Offset: 0x000AD4A0
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

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0001071A File Offset: 0x0000E91A
		public bool IsVowel(char ch)
		{
			return "aeiouâîAEIOUÂÎ".IndexOf(ch) >= 0;
		}
	}
}
