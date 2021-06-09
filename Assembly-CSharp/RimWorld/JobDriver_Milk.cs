using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3A RID: 2874
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06004380 RID: 17280 RVA: 0x00032085 File Offset: 0x00030285
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0003208C File Offset: 0x0003028C
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
