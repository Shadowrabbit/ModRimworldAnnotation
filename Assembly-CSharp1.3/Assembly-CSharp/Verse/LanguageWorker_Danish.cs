using System;

namespace Verse
{
	// Token: 0x02000158 RID: 344
	public class LanguageWorker_Danish : LanguageWorker
	{
		// Token: 0x060009A9 RID: 2473 RVA: 0x00032E80 File Offset: 0x00031080
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
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

		// Token: 0x060009AA RID: 2474 RVA: 0x00032EB4 File Offset: 0x000310B4
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
