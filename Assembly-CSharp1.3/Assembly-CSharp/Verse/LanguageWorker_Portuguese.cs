using System;

namespace Verse
{
	// Token: 0x02000162 RID: 354
	public class LanguageWorker_Portuguese : LanguageWorker
	{
		// Token: 0x060009DE RID: 2526 RVA: 0x000345D9 File Offset: 0x000327D9
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "umas " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "uma " : "um ") + str;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00034611 File Offset: 0x00032811
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "as " : "os ") + str;
			}
			return ((gender == Gender.Female) ? "a " : "o ") + str;
		}
	}
}
