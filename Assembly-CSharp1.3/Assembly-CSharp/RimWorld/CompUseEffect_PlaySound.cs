using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200120F RID: 4623
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06006F00 RID: 28416 RVA: 0x00251AF5 File Offset: 0x0024FCF5
		private CompProperties_UseEffectPlaySound Props
		{
			get
			{
				return (CompProperties_UseEffectPlaySound)this.props;
			}
		}

		// Token: 0x06006F01 RID: 28417 RVA: 0x00251B02 File Offset: 0x0024FD02
		public override void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap && this.Props.soundOnUsed != null)
			{
				this.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(usedBy, MaintenanceType.None));
			}
		}
	}
}
