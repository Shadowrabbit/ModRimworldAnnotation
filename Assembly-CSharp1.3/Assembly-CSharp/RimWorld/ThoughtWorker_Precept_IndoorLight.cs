using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000972 RID: 2418
	public class ThoughtWorker_Precept_IndoorLight : ThoughtWorker_Precept
	{
		// Token: 0x06003D5C RID: 15708 RVA: 0x00151D70 File Offset: 0x0014FF70
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			return p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Lit && p.Position.Roofed(p.Map) && !DarklightUtility.IsDarklightAt(p.Position, p.Map);
		}
	}
}
