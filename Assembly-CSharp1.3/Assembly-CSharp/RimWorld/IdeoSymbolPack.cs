using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A99 RID: 2713
	public class IdeoSymbolPack
	{
		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x0015D346 File Offset: 0x0015B546
		public string PrimarySymbol
		{
			get
			{
				if (!this.ideoName.NullOrEmpty())
				{
					return this.ideoName;
				}
				return this.theme;
			}
		}

		// Token: 0x04002557 RID: 9559
		[MustTranslate]
		public string ideoName;

		// Token: 0x04002558 RID: 9560
		[MustTranslate]
		public string theme;

		// Token: 0x04002559 RID: 9561
		[MustTranslate]
		public string adjective;

		// Token: 0x0400255A RID: 9562
		[MustTranslate]
		public string member;

		// Token: 0x0400255B RID: 9563
		public bool prefix;
	}
}
