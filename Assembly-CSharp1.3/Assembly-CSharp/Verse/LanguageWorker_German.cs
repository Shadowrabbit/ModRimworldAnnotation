using System;

namespace Verse
{
	// Token: 0x0200015D RID: 349
	public class LanguageWorker_German : LanguageWorker
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x00033CE8 File Offset: 0x00031EE8
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "ein " + str;
			case Gender.Male:
				return "ein " + str;
			case Gender.Female:
				return "eine " + str;
			default:
				return str;
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00033D34 File Offset: 0x00031F34
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "das " + str;
			case Gender.Male:
				return "der " + str;
			case Gender.Female:
				return "die " + str;
			default:
				return str;
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00033D80 File Offset: 0x00031F80
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".";
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00033D94 File Offset: 0x00031F94
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
			switch (gender)
			{
			case Gender.None:
				if (c == 'r' && c2 == 'e')
				{
					return str;
				}
				if (c == 'l' && c2 == 'e')
				{
					return str;
				}
				if (c == 'n' && c2 == 'e')
				{
					return str;
				}
				if (c == 'R' && c2 == 'E')
				{
					return str;
				}
				if (c == 'L' && c2 == 'E')
				{
					return str;
				}
				if (c == 'N' && c2 == 'E')
				{
					return str;
				}
				if (char.IsUpper(c))
				{
					return str + "EN";
				}
				return str + "en";
			case Gender.Male:
				if (c == 'r' && c2 == 'e')
				{
					return str;
				}
				if (c == 'l' && c2 == 'e')
				{
					return str;
				}
				if (c == 'R' && c2 == 'E')
				{
					return str;
				}
				if (c == 'L' && c2 == 'E')
				{
					return str;
				}
				if (char.IsUpper(c))
				{
					return str + "E";
				}
				return str + "e";
			case Gender.Female:
				if (c == 'e')
				{
					return str + "n";
				}
				if (c == 'E')
				{
					return str + "N";
				}
				if (c == 'n' && c2 == 'i')
				{
					return str + "nen";
				}
				if (c == 'N' && c2 == 'I')
				{
					return str + "NEN";
				}
				if (char.IsUpper(c))
				{
					return str + "EN";
				}
				return str + "en";
			default:
				return str;
			}
		}
	}
}
