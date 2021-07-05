using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000294 RID: 660
	public class HediffComp_EntropyLink : HediffComp
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0006A2FF File Offset: 0x000684FF
		public HediffCompProperties_EntropyLink Props
		{
			get
			{
				return (HediffCompProperties_EntropyLink)this.props;
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0006A30C File Offset: 0x0006850C
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, source);
			HediffComp_Link hediffComp_Link = this.parent.TryGetComp<HediffComp_Link>();
			if (hediffComp_Link != null && hediffComp_Link.other != source && hediffComp_Link.other.psychicEntropy != null)
			{
				hediffComp_Link.other.psychicEntropy.TryAddEntropy(baseAmount * this.Props.entropyTransferAmount, this.parent.pawn, false, false);
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, hediffComp_Link.other);
			}
		}
	}
}
