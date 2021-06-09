using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A1F RID: 2591
	public class MentalBreakWorker_WildDecree : MentalBreakWorker
	{
		// Token: 0x06003DDE RID: 15838 RVA: 0x0002E99B File Offset: 0x0002CB9B
		public override bool BreakCanOccur(Pawn pawn)
		{
			return base.BreakCanOccur(pawn) && pawn.IsColonist && !pawn.IsPrisoner && pawn.royalty != null && pawn.royalty.PossibleDecreeQuests.Any<QuestScriptDef>();
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00176D98 File Offset: 0x00174F98
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

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00176DF0 File Offset: 0x00174FF0
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.royalty.IssueDecree(true, reason);
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, causedByMood, null, true);
			}
			else if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, causedByMood, null, true);
			}
			return true;
		}
	}
}
