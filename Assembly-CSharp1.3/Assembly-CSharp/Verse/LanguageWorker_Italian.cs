using System;

namespace Verse
{
	// Token: 0x0200015F RID: 351
	public class LanguageWorker_Italian : LanguageWorker
	{
		// Token: 0x060009CB RID: 2507 RVA: 0x00033F90 File Offset: 0x00032190
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			char c = str[0];
			char c2 = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "un'" + str;
				}
				return "una " + str;
			}
			else
			{
				char c3 = char.ToLower(c);
				char c4 = char.ToLower(c2);
				if ((c == 's' || c == 'S') && !this.IsVowel(c2))
				{
					return "uno " + str;
				}
				if ((c3 == 'p' && c4 == 's') || (c3 == 'p' && c4 == 'n') || c3 == 'z' || c3 == 'x' || c3 == 'y' || (c3 == 'g' && c4 == 'n'))
				{
					return "uno " + str;
				}
				return "un " + str;
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00034058 File Offset: 0x00032258
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
			char c = str[0];
			char ch = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "la " + str;
			}
			else
			{
				if (c == 'z' || c == 'Z')
				{
					return "lo " + str;
				}
				if ((c == 's' || c == 'S') && !this.IsVowel(ch))
				{
					return "lo " + str;
				}
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "il " + str;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003410B File Offset: 0x0003230B
		public bool IsVowel(char ch)
		{
			return "aeiouAEIOU".IndexOf(ch) >= 0;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0003411E File Offset: 0x0003231E
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + "°";
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00034130 File Offset: 0x00032330
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
			char ch = str[str.Length - 1];
			if (!this.IsVowel(ch))
			{
				return str;
			}
			if (gender == Gender.Female)
			{
				return str.Substring(0, str.Length - 1) + "e";
			}
			return str.Substring(0, str.Length - 1) + "i";
		}
	}
}
