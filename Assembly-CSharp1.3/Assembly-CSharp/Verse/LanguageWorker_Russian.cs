using System;

namespace Verse
{
	// Token: 0x02000164 RID: 356
	public class LanguageWorker_Russian : LanguageWorker
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00034716 File Offset: 0x00032916
		public override int TotalNumCaseCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00033236 File Offset: 0x00031436
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0003471C File Offset: 0x0003291C
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string text;
			if (!this.TryLookupPluralForm(str, gender, out text, count))
			{
				text = this.PluralizeFallback(str, gender, count);
			}
			if (count == -1)
			{
				return text;
			}
			string formSeveral;
			string formMany;
			if (base.TryLookUp("Case", str, 3, out formSeveral, null) && base.TryLookUp("Case", text, 3, out formMany, null))
			{
				return this.GetFormForNumber(count, str, formSeveral, formMany);
			}
			return text;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00034780 File Offset: 0x00032980
		private string PluralizeFallback(string str, Gender gender, int count = -1)
		{
			char c = str[str.Length - 1];
			char c2 = (str.Length < 2) ? '\0' : str[str.Length - 2];
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					if (gender == Gender.None)
					{
						if (c == 'o')
						{
							return str.Substring(0, str.Length - 1) + "a";
						}
						if (c == 'O')
						{
							return str.Substring(0, str.Length - 1) + "A";
						}
						if (c == 'e' || c == 'E')
						{
							char value = char.ToUpper(c2);
							if ("ГКХЖЧШЩЦ".IndexOf(value) >= 0)
							{
								if (c == 'e')
								{
									return str.Substring(0, str.Length - 1) + "a";
								}
								if (c == 'E')
								{
									return str.Substring(0, str.Length - 1) + "A";
								}
							}
							else
							{
								if (c == 'e')
								{
									return str.Substring(0, str.Length - 1) + "я";
								}
								if (c == 'E')
								{
									return str.Substring(0, str.Length - 1) + "Я";
								}
							}
						}
					}
				}
				else
				{
					if (c == 'я')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'Я')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'Ь')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'a' || c == 'A')
					{
						char value2 = char.ToUpper(c2);
						if ("ГКХЖЧШЩ".IndexOf(value2) >= 0)
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "и";
							}
							return str.Substring(0, str.Length - 1) + "И";
						}
						else
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "ы";
							}
							return str.Substring(0, str.Length - 1) + "Ы";
						}
					}
				}
			}
			else
			{
				if (LanguageWorker_Russian.IsConsonant(c))
				{
					return str + "ы";
				}
				if (c == 'й')
				{
					return str.Substring(0, str.Length - 1) + "и";
				}
				if (c == 'ь')
				{
					return str.Substring(0, str.Length - 1) + "и";
				}
				if (c == 'Й')
				{
					return str.Substring(0, str.Length - 1) + "И";
				}
				if (c == 'Ь')
				{
					return str.Substring(0, str.Length - 1) + "И";
				}
			}
			return str;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00034A66 File Offset: 0x00032C66
		private static bool IsConsonant(char ch)
		{
			return "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСТФХЦЧШЩ".IndexOf(ch) >= 0;
		}
	}
}
