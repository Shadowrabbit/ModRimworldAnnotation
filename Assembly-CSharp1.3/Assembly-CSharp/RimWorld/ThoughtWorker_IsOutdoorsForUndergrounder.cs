using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B5 RID: 2485
	public class ThoughtWorker_IsOutdoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06003DFA RID: 15866 RVA: 0x00153E0C File Offset: 0x0015200C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (p.Position.UsesOutdoorTemperature(p.Map) || !p.Position.Roofed(p.Map));
		}
	}
}
