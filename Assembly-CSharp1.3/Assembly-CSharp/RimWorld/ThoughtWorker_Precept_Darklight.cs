using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000973 RID: 2419
	public class ThoughtWorker_Precept_Darklight : ThoughtWorker_Precept
	{
		// Token: 0x06003D5E RID: 15710 RVA: 0x00151DEB File Offset: 0x0014FFEB
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			return DarklightUtility.IsDarklightAt(p.Position, p.Map);
		}
	}
}
