using System;

namespace Verse
{
	// Token: 0x02000165 RID: 357
	public class LanguageWorker_Spanish : LanguageWorker
	{
		// Token: 0x060009EB RID: 2539 RVA: 0x00034A79 File Offset: 0x00032C79
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unas " : "unos ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00034AB1 File Offset: 0x00032CB1
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "las " : "los ") + str;
			}
			return ((gender == Gender.Female) ? "la " : "el ") + str;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00034AE9 File Offset: 0x00032CE9
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".º";
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00034AFC File Offset: 0x00032CFC
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			if (this.IsVowel(c))
			{
				if (str == "sí")
				{
					return "síes";
				}
				if (c == 'í' || c == 'ú' || c == 'Í' || c == 'Ú')
				{
					return str + "es";
				}
				return str + "s";
			}
			else
			{
				if ((c == 'y' || c == 'Y') && this.IsVowel(c2))
				{
					return str + "es";
				}
				if ("lrndzjsxLRNDZJSX".IndexOf(c) >= 0 || (c == 'h' && c2 == 'c'))
				{
					return str + "es";
				}
				return str + "s";
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00034BEE File Offset: 0x00032DEE
		public bool IsVowel(char ch)
		{
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}
	}
}
