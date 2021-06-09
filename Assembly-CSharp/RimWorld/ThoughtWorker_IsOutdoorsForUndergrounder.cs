using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBF RID: 3775
	public class ThoughtWorker_IsOutdoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x060053D0 RID: 21456 RVA: 0x0003A49B File Offset: 0x0003869B
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (p.Position.UsesOutdoorTemperature(p.Map) || !p.Position.Roofed(p.Map));
		}
	}
}
