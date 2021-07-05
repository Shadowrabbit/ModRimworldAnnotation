using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001707 RID: 5895
	public class QuestNode_Root_WandererJoinAbasia : QuestNode_Root_WandererJoin
	{
		// Token: 0x06008823 RID: 34851 RVA: 0x0030E350 File Offset: 0x0030C550
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06008824 RID: 34852 RVA: 0x0030E3A0 File Offset: 0x0030C5A0
		public override Pawn GeneratePawn()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Refugee, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
			pawn.health.AddHediff(HediffDefOf.Abasia, null, null, null);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x06008825 RID: 34853 RVA: 0x0030E44C File Offset: 0x0030C64C
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			base.AddSpawnPawnQuestParts(quest, map, pawn);
			this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			quest.Signal(this.signalAccept, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(this.signalReject, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06008826 RID: 34854 RVA: 0x0030E4E0 File Offset: 0x0030C6E0
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelWandererJoinsAbasia".Translate(pawn);
			TaggedString text = "LetterTextWandererJoinsAbasia".Translate(pawn, HediffDefOf.Abasia);
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("AfflictedJoinerChartityInfo".Translate(pawn, this.AllowKilledBeforeTicks.ToStringTicksToPeriod(true, false, true, true)), ref text);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(label, text, LetterDefOf.AcceptJoiner, null, null);
			choiceLetter_AcceptJoiner.signalAccept = this.signalAccept;
			choiceLetter_AcceptJoiner.signalReject = this.signalReject;
			choiceLetter_AcceptJoiner.quest = quest;
			choiceLetter_AcceptJoiner.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner, null);
		}

		// Token: 0x04005610 RID: 22032
		private const int TimeoutTicks = 60000;

		// Token: 0x04005611 RID: 22033
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04005612 RID: 22034
		private string signalAccept;

		// Token: 0x04005613 RID: 22035
		private string signalReject;
	}
}
