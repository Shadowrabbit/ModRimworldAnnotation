using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000957 RID: 2391
	public class ThoughtWorker_Precept_LeaderResentment : ThoughtWorker_Precept
	{
		// Token: 0x06003D0C RID: 15628 RVA: 0x00151038 File Offset: 0x0014F238
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			Faction faction = p.Faction;
			Pawn pawn = (faction != null) ? faction.leader : null;
			return pawn != null && p.Ideo != pawn.Ideo;
		}
	}
}
