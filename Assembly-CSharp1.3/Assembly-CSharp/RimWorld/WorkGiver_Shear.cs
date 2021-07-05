using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000809 RID: 2057
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060036E6 RID: 14054 RVA: 0x001370A3 File Offset: 0x001352A3
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x0011E04A File Offset: 0x0011C24A
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
