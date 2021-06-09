using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020018DE RID: 6366
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06008D01 RID: 36097 RVA: 0x0005E844 File Offset: 0x0005CA44
		private CompProperties_UseEffectPlaySound Props
		{
			get
			{
				return (CompProperties_UseEffectPlaySound)this.props;
			}
		}

		// Token: 0x06008D02 RID: 36098 RVA: 0x0005E851 File Offset: 0x0005CA51
		public override void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap && this.Props.soundOnUsed != null)
			{
				this.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(usedBy, MaintenanceType.None));
			}
		}
	}
}
