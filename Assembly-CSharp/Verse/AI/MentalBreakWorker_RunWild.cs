using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A1E RID: 2590
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06003DDB RID: 15835 RVA: 0x00176C64 File Offset: 0x00174E64
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

		// Token: 0x06003DDC RID: 15836 RVA: 0x00176D08 File Offset: 0x00174F08
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "RanWild", pawn.Named("SUBJECT"));
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null);
			if (pawn.Spawned && !pawn.Downed)
			{
				pawn.jobs.StopAll(false, true);
			}
			return true;
		}
	}
}
