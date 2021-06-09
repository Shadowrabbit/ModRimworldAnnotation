using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003D7 RID: 983
	public class HediffComp_EntropyLink : HediffComp
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x000170DE File Offset: 0x000152DE
		public HediffCompProperties_EntropyLink Props
		{
			get
			{
				return (HediffCompProperties_EntropyLink)this.props;
			}
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x000DEBE0 File Offset: 0x000DCDE0
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
