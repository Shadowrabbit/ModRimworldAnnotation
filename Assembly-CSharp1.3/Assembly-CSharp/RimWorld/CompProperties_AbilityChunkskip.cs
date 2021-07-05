using System;

namespace RimWorld
{
	// Token: 0x02000D24 RID: 3364
	public class CompProperties_AbilityChunkskip : CompProperties_AbilityEffect
	{
		// Token: 0x06004EEC RID: 20204 RVA: 0x001A6DFF File Offset: 0x001A4FFF
		public CompProperties_AbilityChunkskip()
		{
			this.compClass = typeof(CompAbilityEffect_Chunkskip);
		}

		// Token: 0x04002F73 RID: 12147
		public int chunkCount;

		// Token: 0x04002F74 RID: 12148
		public float scatterRadius;
	}
}
