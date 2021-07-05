using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095A RID: 2394
	public class ThoughtWorker_Precept_Slavery_NoSlavesInColony : ThoughtWorker_Precept
	{
		// Token: 0x06003D12 RID: 15634 RVA: 0x001510CE File Offset: 0x0014F2CE
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.IsColonist && !p.IsSlave && !p.IsPrisoner && !p.IsQuestLodger() && FactionUtility.GetSlavesInFactionCount(p.Faction) <= 0;
		}
	}
}
