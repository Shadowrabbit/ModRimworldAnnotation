using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3D RID: 3901
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification : RitualObligationTargetWorker_AnyRitualSpotOrAltar
	{
		// Token: 0x06005CC4 RID: 23748 RVA: 0x001FE819 File Offset: 0x001FCA19
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification()
		{
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x001FE821 File Offset: 0x001FCA21
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x001FE82C File Offset: 0x001FCA2C
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn;
			if ((pawn = (obligation.targetA.Thing as Pawn)) == null)
			{
				return false;
			}
			if (pawn.Dead)
			{
				return false;
			}
			int hediffCount = pawn.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
			return pawn.Ideo != null && pawn.Ideo.RequiredScars > hediffCount;
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x001FE887 File Offset: 0x001FCA87
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return obligation.targetA.Thing.LabelShort;
		}
	}
}
