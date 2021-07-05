using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C0 RID: 2496
	public class ThoughtWorker_PsychicEmanatorSoothe : ThoughtWorker
	{
		// Token: 0x06003E11 RID: 15889 RVA: 0x001543A0 File Offset: 0x001525A0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.PsychicEmanator);
			for (int i = 0; i < list.Count; i++)
			{
				CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
				if ((compPowerTrader == null || compPowerTrader.PowerOn) && p.Position.InHorDistOf(list[i].Position, 15f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040020E7 RID: 8423
		private const float Radius = 15f;
	}
}
