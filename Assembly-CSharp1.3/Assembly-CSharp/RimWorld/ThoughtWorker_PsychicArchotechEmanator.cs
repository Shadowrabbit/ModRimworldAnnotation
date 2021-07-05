using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DB RID: 2523
	public class ThoughtWorker_PsychicArchotechEmanator : ThoughtWorker
	{
		// Token: 0x06003E64 RID: 15972 RVA: 0x0015517C File Offset: 0x0015337C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.ArchonexusSuperstructureMajorStudiable);
			for (int i = 0; i < list.Count; i++)
			{
				CompStudiable compStudiable = list[i].TryGetComp<CompStudiable>();
				if (compStudiable != null && !compStudiable.Completed && p.Position.InHorDistOf(list[i].Position, 8f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040020ED RID: 8429
		private const float Radius = 8f;
	}
}
