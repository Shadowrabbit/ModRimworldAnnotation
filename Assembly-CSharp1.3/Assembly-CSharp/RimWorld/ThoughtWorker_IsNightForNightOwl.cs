using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x06003DFE RID: 15870 RVA: 0x00153E71 File Offset: 0x00152071
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
