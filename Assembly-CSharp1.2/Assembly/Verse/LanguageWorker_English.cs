using System;

namespace Verse
{
	// Token: 0x02000210 RID: 528
	public class LanguageWorker_English : LanguageWorker
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x000104A3 File Offset: 0x0000E6A3
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "a " + str;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000104C9 File Offset: 0x0000E6C9
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			return "the " + str;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000ADF18 File Offset: 0x000AC118
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			if (str.StartsWith("a ", StringComparison.OrdinalIgnoreCase) && str.Length >= 3 && (str.Substring(2) == "hour" || str[2] == 'a' || str[2] == 'e' || str[2] == 'i' || str[2] == 'o' || str[2] == 'u'))
			{
				str = str.Insert(1, "n");
			}
			str = str.Replace(" a a", " an a");
			str = str.Replace(" a e", " an e");
			str = str.Replace(" a i", " an i");
			str = str.Replace(" a o", " an o");
			str = str.Replace(" a u", " an u");
			str = str.Replace(" a hour", " an hour");
			str = str.Replace(" A a", " An a");
			str = str.Replace(" A e", " An e");
			str = str.Replace(" A i", " An i");
			str = str.Replace(" A o", " An o");
			str = str.Replace(" A u", " An u");
			str = str.Replace(" A hour", " An hour");
			str = str.Replace("\na a", "\nan a");
			str = str.Replace("\na e", "\nan e");
			str = str.Replace("\na i", "\nan i");
			str = str.Replace("\na o", "\nan o");
			str = str.Replace("\na u", "\nan u");
			str = str.Replace("\na hour", "\nan hour");
			str = str.Replace("\nA a", "\nAn a");
			str = str.Replace("\nA e", "\nAn e");
			str = str.Replace("\nA i", "\nAn i");
			str = str.Replace("\nA o", "\nAn o");
			str = str.Replace("\nA u", "\nAn u");
			str = str.Replace("\nA hour", "\nAn hour");
			return str;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000104EA File Offset: 0x0000E6EA
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000AE150 File Offset: 0x000AC350
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			int num = number % 10;
			if (number / 10 % 10 != 1)
			{
				if (num == 1)
				{
					return number + "st";
				}
				if (num == 2)
				{
					return number + "nd";
				}
				if (num == 3)
				{
					return number + "rd";
				}
			}
			return number + "th";
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000AE1BC File Offset: 0x000AC3BC
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty() || str[str.Length - 1] == 's')
			{
				return str;
			}
			int num = (int)str[str.Length - 1];
			char c = (str.Length == 1) ? '\0' : str[str.Length - 2];
			bool flag = char.IsLetter(c) && "oaieuyOAIEUY".IndexOf(c) >= 0;
			bool flag2 = char.IsLetter(c) && !flag;
			if (num == 121 && flag2)
			{
				return str.Substring(0, str.Length - 1) + "ies";
			}
			return str + "s";
		}
	}
}
