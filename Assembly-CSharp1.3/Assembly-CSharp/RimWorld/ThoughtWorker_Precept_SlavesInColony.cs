using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000959 RID: 2393
	public class ThoughtWorker_Precept_SlavesInColony : ThoughtWorker_Precept
	{
		// Token: 0x06003D10 RID: 15632 RVA: 0x00151096 File Offset: 0x0014F296
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.IsColonist && !p.IsSlave && !p.IsPrisoner && !p.IsQuestLodger() && FactionUtility.GetSlavesInFactionCount(p.Faction) > 0;
		}
	}
}
