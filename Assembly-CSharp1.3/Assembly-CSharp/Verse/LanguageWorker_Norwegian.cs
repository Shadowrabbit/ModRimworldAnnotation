using System;

namespace Verse
{
	// Token: 0x02000161 RID: 353
	public class LanguageWorker_Norwegian : LanguageWorker
	{
		// Token: 0x060009DB RID: 2523 RVA: 0x00034542 File Offset: 0x00032742
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
			return "et " + str;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0003456C File Offset: 0x0003276C
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
			char c = str[str.Length - 1];
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (c == 'e')
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (c == 'e')
				{
					return str + "t";
				}
				return str + "et";
			}
		}
	}
}
