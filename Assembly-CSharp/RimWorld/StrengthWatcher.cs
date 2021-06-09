using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001331 RID: 4913
	public class StrengthWatcher
	{
		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06006A94 RID: 27284 RVA: 0x0020ECF4 File Offset: 0x0020CEF4
		public float StrengthRating
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in this.map.mapPawns.FreeColonists)
				{
					float num2 = 1f;
					num2 *= pawn.health.summaryHealth.SummaryHealthPercent;
					if (pawn.Downed)
					{
						num2 *= 0.3f;
					}
					num += num2;
				}
				foreach (Building building in this.map.listerBuildings.allBuildingsColonistCombatTargets)
				{
					if (building.def.building != null && building.def.building.IsTurret)
					{
						num += 0.3f;
					}
				}
				return num;
			}
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x00048715 File Offset: 0x00046915
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x040046E7 RID: 18151
		private Map map;
	}
}
