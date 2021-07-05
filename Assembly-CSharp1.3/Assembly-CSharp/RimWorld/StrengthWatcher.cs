using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFC RID: 3324
	public class StrengthWatcher
	{
		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06004DAB RID: 19883 RVA: 0x001A0FD0 File Offset: 0x0019F1D0
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

		// Token: 0x06004DAC RID: 19884 RVA: 0x001A10CC File Offset: 0x0019F2CC
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x04002EDC RID: 11996
		private Map map;
	}
}
