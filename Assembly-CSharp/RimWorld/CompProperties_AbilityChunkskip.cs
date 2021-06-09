using System;

namespace RimWorld
{
	// Token: 0x02001369 RID: 4969
	public class CompProperties_AbilityChunkskip : CompProperties_AbilityEffect
	{
		// Token: 0x06006C19 RID: 27673 RVA: 0x00049909 File Offset: 0x00047B09
		public CompProperties_AbilityChunkskip()
		{
			this.compClass = typeof(CompAbilityEffect_Chunkskip);
		}

		// Token: 0x040047C6 RID: 18374
		public int chunkCount;

		// Token: 0x040047C7 RID: 18375
		public float scatterRadius;
	}
}
