using System;

namespace Verse
{
	// Token: 0x02000218 RID: 536
	public class LanguageWorker_Portuguese : LanguageWorker
	{
		// Token: 0x06000DD0 RID: 3536 RVA: 0x0001066D File Offset: 0x0000E86D
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

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000106A5 File Offset: 0x0000E8A5
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
