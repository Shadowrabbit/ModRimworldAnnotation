using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3B RID: 2875
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06004383 RID: 17283 RVA: 0x0003209C File Offset: 0x0003029C
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x000320A3 File Offset: 0x000302A3
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
