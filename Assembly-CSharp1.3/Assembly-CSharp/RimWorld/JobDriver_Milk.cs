using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C5 RID: 1733
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x0011D748 File Offset: 0x0011B948
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x0011D74F File Offset: 0x0011B94F
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
