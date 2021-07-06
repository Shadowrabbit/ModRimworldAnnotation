using System;

namespace Verse
{
	// Token: 0x02000220 RID: 544
	public class LanguageWorker_Turkish : LanguageWorker
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x0001088B File Offset: 0x0000EA8B
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return str + " bir";
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0001037D File Offset: 0x0000E57D
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			return str;
		}
	}
}
