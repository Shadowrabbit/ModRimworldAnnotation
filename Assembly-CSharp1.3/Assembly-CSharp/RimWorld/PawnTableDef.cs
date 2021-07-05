using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA1 RID: 2721
	public class PawnTableDef : Def
	{
		// Token: 0x040025A2 RID: 9634
		public List<PawnColumnDef> columns;

		// Token: 0x040025A3 RID: 9635
		public Type workerClass = typeof(PawnTable);

		// Token: 0x040025A4 RID: 9636
		public int minWidth = 998;
	}
}
