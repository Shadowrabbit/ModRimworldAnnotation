using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001206 RID: 4614
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x06006EE3 RID: 28387 RVA: 0x002514E1 File Offset: 0x0024F6E1
		public CompProperties_UseEffectArtifact Props
		{
			get
			{
				return (CompProperties_UseEffectArtifact)this.props;
			}
		}

		// Token: 0x06006EE4 RID: 28388 RVA: 0x002514F0 File Offset: 0x0024F6F0
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(this.parent.Position, usedBy.MapHeld, false));
			}
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
