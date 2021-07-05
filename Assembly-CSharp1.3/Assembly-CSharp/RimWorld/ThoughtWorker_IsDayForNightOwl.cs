using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B6 RID: 2486
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06003DFC RID: 15868 RVA: 0x00153E48 File Offset: 0x00152048
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
