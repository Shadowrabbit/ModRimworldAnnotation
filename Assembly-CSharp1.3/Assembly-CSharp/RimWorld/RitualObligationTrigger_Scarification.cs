using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4D RID: 3917
	public class RitualObligationTrigger_Scarification : RitualObligationTrigger_EveryMember
	{
		// Token: 0x06005D05 RID: 23813 RVA: 0x001FF0B4 File Offset: 0x001FD2B4
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
						RitualObligationTrigger_Scarification.existingObligations.Add(ritualObligation.targetA.Thing as Pawn);
					}
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
				{
					if (!RitualObligationTrigger_Scarification.existingObligations.Contains(pawn) && pawn.Ideo != null && pawn.Ideo == this.ritual.ideo)
					{
						int hediffCount = pawn.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
						if (pawn.Ideo.RequiredScars > hediffCount)
						{
							this.ritual.AddObligation(new RitualObligation(this.ritual, pawn, false));
						}
					}
				}
			}
			finally
			{
				RitualObligationTrigger_Scarification.existingObligations.Clear();
			}
		}

		// Token: 0x040035DF RID: 13791
		private static List<Pawn> existingObligations = new List<Pawn>();
	}
}
