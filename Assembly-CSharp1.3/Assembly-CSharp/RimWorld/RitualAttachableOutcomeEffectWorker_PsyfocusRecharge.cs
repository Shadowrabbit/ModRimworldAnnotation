using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F02 RID: 3842
	public class RitualAttachableOutcomeEffectWorker_PsyfocusRecharge : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BBF RID: 23487 RVA: 0x001FB75C File Offset: 0x001F995C
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = this.def.letterInfoText;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
				if (psychicEntropy != null)
				{
					psychicEntropy.RechargePsyfocus();
				}
			}
		}
	}
}
