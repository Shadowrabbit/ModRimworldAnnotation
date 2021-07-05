using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F06 RID: 3846
	public class RitualAttachableOutcomeEffectWorker_RandomRecruit : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BC9 RID: 23497 RVA: 0x001FBB08 File Offset: 0x001F9D08
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			if (Rand.Chance(0.5f))
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", jobRitual.Map, false);
				slate.Set<PawnGenerationRequest>("overridePawnGenParams", new PawnGenerationRequest(PawnKindDefOf.Villager, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, jobRitual.Ritual.ideo, false, false), false);
				QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.WandererJoins, slate);
				extraOutcomeDesc = this.def.letterInfoText;
				return;
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x0400357D RID: 13693
		public const float RecruitChance = 0.5f;
	}
}
