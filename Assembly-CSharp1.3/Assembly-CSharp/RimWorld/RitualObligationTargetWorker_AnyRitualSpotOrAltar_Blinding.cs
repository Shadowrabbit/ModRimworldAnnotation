using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3F RID: 3903
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding : RitualObligationTargetWorker_AnyRitualSpotOrAltar
	{
		// Token: 0x06005CCE RID: 23758 RVA: 0x001FE819 File Offset: 0x001FCA19
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding()
		{
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x001FE821 File Offset: 0x001FCA21
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x001FE980 File Offset: 0x001FCB80
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
			return pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Eye);
		}
	}
}
