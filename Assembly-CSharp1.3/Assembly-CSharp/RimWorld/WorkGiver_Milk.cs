using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060036DD RID: 14045 RVA: 0x00136FA2 File Offset: 0x001351A2
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x0011D74F File Offset: 0x0011B94F
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
