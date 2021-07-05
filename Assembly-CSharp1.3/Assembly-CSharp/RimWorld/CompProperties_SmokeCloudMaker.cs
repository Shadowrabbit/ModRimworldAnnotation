using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001198 RID: 4504
	public class CompProperties_SmokeCloudMaker : CompProperties
	{
		// Token: 0x06006C77 RID: 27767 RVA: 0x0024682B File Offset: 0x00244A2B
		public CompProperties_SmokeCloudMaker()
		{
			this.compClass = typeof(CompSmokeCloudMaker);
		}

		// Token: 0x04003C46 RID: 15430
		public EffecterDef sourceStreamEffect;

		// Token: 0x04003C47 RID: 15431
		public float cloudRadius;

		// Token: 0x04003C48 RID: 15432
		public FleckDef cloudFleck;

		// Token: 0x04003C49 RID: 15433
		public float fleckScale = 1f;

		// Token: 0x04003C4A RID: 15434
		public float fleckSpawnMTB;
	}
}
