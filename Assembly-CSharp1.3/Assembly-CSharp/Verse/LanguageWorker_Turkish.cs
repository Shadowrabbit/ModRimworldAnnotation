using System;

namespace Verse
{
	// Token: 0x02000167 RID: 359
	public class LanguageWorker_Turkish : LanguageWorker
	{
		// Token: 0x060009F5 RID: 2549 RVA: 0x00034CB0 File Offset: 0x00032EB0
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return str + " bir";
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			return str;
		}
	}
}
