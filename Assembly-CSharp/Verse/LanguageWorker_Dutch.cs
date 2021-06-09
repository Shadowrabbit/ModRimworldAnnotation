using System;

namespace Verse
{
	// Token: 0x0200020F RID: 527
	public class LanguageWorker_Dutch : LanguageWorker
	{
		// Token: 0x06000D9F RID: 3487 RVA: 0x00010455 File Offset: 0x0000E655
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

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0001046D File Offset: 0x0000E66D
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
