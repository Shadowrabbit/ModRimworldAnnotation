using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F04 RID: 3844
	public class RitualAttachableOutcomeEffectWorker_InsectJellyTunnels : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BC4 RID: 23492 RVA: 0x001FB8AC File Offset: 0x001F9AAC
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				points = (float)(outcome.BestPositiveOutcome(jobRitual) ? 320 : 210)
			};
			if (IncidentDefOf.Infestation_Jelly.Worker.TryExecute(parms))
			{
				extraOutcomeDesc = this.def.letterInfoText;
			}
		}

		// Token: 0x0400357A RID: 13690
		public const int PositiveOutcomePoints = 210;

		// Token: 0x0400357B RID: 13691
		public const int BestOutcomePoints = 320;
	}
}
