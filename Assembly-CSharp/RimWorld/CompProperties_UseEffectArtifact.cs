using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D3 RID: 6355
	public class CompProperties_UseEffectArtifact : CompProperties_UseEffect
	{
		// Token: 0x06008CD4 RID: 36052 RVA: 0x0005E684 File Offset: 0x0005C884
		public CompProperties_UseEffectArtifact()
		{
			this.compClass = typeof(CompUseEffect_Artifact);
		}

		// Token: 0x04005A02 RID: 23042
		public SoundDef sound;
	}
}
