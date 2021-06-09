using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D43 RID: 3395
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x00036DCD File Offset: 0x00034FCD
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x000320A3 File Offset: 0x000302A3
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
