using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CD RID: 1741
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x0011E043 File Offset: 0x0011C243
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x0011E04A File Offset: 0x0011C24A
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
