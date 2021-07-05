using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001205 RID: 4613
	public class CompProperties_UseEffectArtifact : CompProperties_UseEffect
	{
		// Token: 0x06006EE2 RID: 28386 RVA: 0x002514C9 File Offset: 0x0024F6C9
		public CompProperties_UseEffectArtifact()
		{
			this.compClass = typeof(CompUseEffect_Artifact);
		}

		// Token: 0x04003D5D RID: 15709
		public SoundDef sound;
	}
}
