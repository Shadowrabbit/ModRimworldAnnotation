using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D42 RID: 3394
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x00036DBE File Offset: 0x00034FBE
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x0003208C File Offset: 0x0003028C
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
