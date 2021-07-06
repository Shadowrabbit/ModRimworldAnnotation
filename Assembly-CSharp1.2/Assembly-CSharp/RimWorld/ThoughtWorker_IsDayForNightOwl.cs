using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC0 RID: 3776
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x060053D2 RID: 21458 RVA: 0x0003A4D7 File Offset: 0x000386D7
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
