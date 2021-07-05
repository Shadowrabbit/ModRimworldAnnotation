using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C3 RID: 1475
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06002AF5 RID: 10997 RVA: 0x00101880 File Offset: 0x000FFA80
		public override bool BreakCanOccur(Pawn pawn)
		{
			if (!pawn.IsColonistPlayerControlled || pawn.Downed || !pawn.Spawned || pawn.IsQuestLodger() || !base.BreakCanOccur(pawn))
			{
				return false;
			}
			if (pawn.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
			{
				return false;
			}
			float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(pawn.Map.Tile);
			return seasonalTemp >= pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 7f && seasonalTemp <= pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + 7f;
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x00101924 File Offset: 0x000FFB24
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "RanWild", pawn.Named("SUBJECT"));
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null, null);
			if (pawn.Spawned && !pawn.Downed)
			{
				pawn.jobs.StopAll(false, true);
			}
			return true;
		}
	}
}
