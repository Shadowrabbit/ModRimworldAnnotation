using System;

namespace Verse
{
	// Token: 0x0200015A RID: 346
	public class LanguageWorker_Dutch : LanguageWorker
	{
		// Token: 0x060009AD RID: 2477 RVA: 0x00032F21 File Offset: 0x00031121
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "een " + str;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00032F39 File Offset: 0x00031139
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "de " + str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "de " + str;
			}
			return "het " + str;
		}
	}
}
