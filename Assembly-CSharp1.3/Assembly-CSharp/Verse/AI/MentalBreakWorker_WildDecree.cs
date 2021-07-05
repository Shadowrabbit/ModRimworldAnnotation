using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005C4 RID: 1476
	public class MentalBreakWorker_WildDecree : MentalBreakWorker
	{
		// Token: 0x06002AF8 RID: 11000 RVA: 0x001019B4 File Offset: 0x000FFBB4
		public override bool BreakCanOccur(Pawn pawn)
		{
			return base.BreakCanOccur(pawn) && pawn.IsColonist && !pawn.IsPrisoner && pawn.royalty != null && pawn.royalty.PossibleDecreeQuests.Any<QuestScriptDef>();
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x001019EC File Offset: 0x000FFBEC
		public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			if (pawn.royalty == null)
			{
				return 0f;
			}
			float num = 0f;
			List<RoyalTitle> allTitlesInEffectForReading = pawn.royalty.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				num = Mathf.Max(num, allTitlesInEffectForReading[i].def.decreeMentalBreakCommonality);
			}
			return num;
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x00101A44 File Offset: 0x000FFC44
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.royalty.IssueDecree(true, reason);
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, causedByMood, null, true, false, false);
			}
			else if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, causedByMood, null, true, false, false);
			}
			return true;
		}
	}
}
