using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000971 RID: 2417
	public class ThoughtWorker_Precept_OutdoorBlindingLight : ThoughtWorker_Precept
	{
		// Token: 0x06003D5A RID: 15706 RVA: 0x00151D08 File Offset: 0x0014FF08
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			return p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Overlit && !p.Position.Roofed(p.Map);
		}
	}
}
