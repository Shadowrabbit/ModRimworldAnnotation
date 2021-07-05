using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B4 RID: 2484
	public class ThoughtWorker_IsUndergroundForUndergrounder : ThoughtWorker
	{
		// Token: 0x06003DF8 RID: 15864 RVA: 0x00153DF0 File Offset: 0x00151FF0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && flag;
		}
	}
}
