using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020018D4 RID: 6356
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06008CD5 RID: 36053 RVA: 0x0005E69C File Offset: 0x0005C89C
		public CompProperties_UseEffectArtifact Props
		{
			get
			{
				return (CompProperties_UseEffectArtifact)this.props;
			}
		}

		// Token: 0x06008CD6 RID: 36054 RVA: 0x0028DB3C File Offset: 0x0028BD3C
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
