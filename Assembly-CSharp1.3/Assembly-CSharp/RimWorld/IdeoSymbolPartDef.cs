using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A77 RID: 2679
	public class IdeoSymbolPartDef : Def
	{
		// Token: 0x06004029 RID: 16425 RVA: 0x0015B588 File Offset: 0x00159788
		public bool CanBeChosenForIdeo(Ideo ideo)
		{
			if (this.memes == null && this.cultures == null)
			{
				return true;
			}
			if (this.cultures != null && this.cultures.Contains(ideo.culture))
			{
				return true;
			}
			if (this.memes != null)
			{
				for (int i = 0; i < ideo.memes.Count; i++)
				{
					if (this.memes.Contains(ideo.memes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04002471 RID: 9329
		public List<MemeDef> memes;

		// Token: 0x04002472 RID: 9330
		public List<CultureDef> cultures;
	}
}
