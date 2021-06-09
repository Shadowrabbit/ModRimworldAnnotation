using System;

namespace Verse
{
	// Token: 0x0200020D RID: 525
	public class LanguageWorker_Danish : LanguageWorker
	{
		// Token: 0x06000D9B RID: 3483 RVA: 0x00010424 File Offset: 0x0000E624
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

		// Token: 0x06000D9C RID: 3484 RVA: 0x000ADEA8 File Offset: 0x000AC0A8
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
