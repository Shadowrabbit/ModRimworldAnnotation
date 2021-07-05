using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC7 RID: 4039
	public class PawnTableDef : Def
	{
		// Token: 0x04003A59 RID: 14937
		public List<PawnColumnDef> columns;

		// Token: 0x04003A5A RID: 14938
		public Type workerClass = typeof(PawnTable);

		// Token: 0x04003A5B RID: 14939
		public int minWidth = 998;
	}
}
