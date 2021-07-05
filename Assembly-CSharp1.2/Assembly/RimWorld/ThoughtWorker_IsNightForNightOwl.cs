using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC1 RID: 3777
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x060053D4 RID: 21460 RVA: 0x0003A500 File Offset: 0x00038700
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
