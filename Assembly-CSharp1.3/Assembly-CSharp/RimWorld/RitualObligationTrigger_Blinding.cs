using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4F RID: 3919
	public class RitualObligationTrigger_Blinding : RitualObligationTrigger_EveryMember
	{
		// Token: 0x06005D0A RID: 23818 RVA: 0x001FF294 File Offset: 0x001FD494
		protected override void Recache()
		{
			try
			{
				if (this.ritual.activeObligations != null)
				{
					this.ritual.activeObligations.RemoveAll(delegate(RitualObligation o)
					{
						Pawn pawn2 = o.targetA.Thing as Pawn;
						return pawn2 != null && pawn2.Ideo != this.ritual.ideo;
					});
					foreach (RitualObligation ritualObligation in this.ritual.activeObligations)
					{
						RitualObligationTrigger_Blinding.existingObligations.Add(ritualObligation.targetA.Thing as Pawn);
					}
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
				{
					if (!RitualObligationTrigger_Blinding.existingObligations.Contains(pawn) && pawn.Ideo != null)
					{
						if (pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Eye))
						{
							this.ritual.AddObligation(new RitualObligation(this.ritual, pawn, false));
						}
					}
				}
			}
			finally
			{
				RitualObligationTrigger_Blinding.existingObligations.Clear();
			}
		}

		// Token: 0x040035E0 RID: 13792
		private static List<Pawn> existingObligations = new List<Pawn>();
	}
}
