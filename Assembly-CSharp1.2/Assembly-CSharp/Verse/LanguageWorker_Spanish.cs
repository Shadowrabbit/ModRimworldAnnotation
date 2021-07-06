using System;

namespace Verse
{
	// Token: 0x0200021E RID: 542
	public class LanguageWorker_Spanish : LanguageWorker
	{
		// Token: 0x06000DE9 RID: 3561 RVA: 0x000107F0 File Offset: 0x0000E9F0
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0001080E File Offset: 0x0000EA0E
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return ((gender == Gender.Female) ? "la " : "el ") + str;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0001082C File Offset: 0x0000EA2C
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".º";
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x000AF850 File Offset: 0x000ADA50
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
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

		// Token: 0x06000DED RID: 3565 RVA: 0x0001083E File Offset: 0x0000EA3E
		public bool IsVowel(char ch)
		{
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}
	}
}
