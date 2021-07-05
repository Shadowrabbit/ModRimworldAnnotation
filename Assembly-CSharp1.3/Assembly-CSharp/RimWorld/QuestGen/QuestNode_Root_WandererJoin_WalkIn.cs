using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001708 RID: 5896
	public class QuestNode_Root_WandererJoin_WalkIn : QuestNode_Root_WandererJoin
	{
		// Token: 0x06008828 RID: 34856 RVA: 0x0030E59C File Offset: 0x0030C79C
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06008829 RID: 34857 RVA: 0x0030E5EC File Offset: 0x0030C7EC
		public override Pawn GeneratePawn()
		{
			Slate slate = QuestGen.slate;
			Gender? fixedGender = null;
			PawnGenerationRequest request;
			if (!slate.TryGet<PawnGenerationRequest>("overridePawnGenParams", out request, false))
			{
				request = new PawnGenerationRequest(PawnKindDefOf.Villager, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, null, false, false);
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600882A RID: 34858 RVA: 0x0030E694 File Offset: 0x0030C894
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			quest.Signal(this.signalAccept, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.PawnsArrive(Gen.YieldSingle<Pawn>(pawn), null, map.Parent, null, false, null, null, null, null, null, false, false, true);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(this.signalReject, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600882B RID: 34859 RVA: 0x0030E71C File Offset: 0x0030C91C
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString text = "LetterWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref text);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(label, text, LetterDefOf.AcceptJoiner, null, null);
			choiceLetter_AcceptJoiner.signalAccept = this.signalAccept;
			choiceLetter_AcceptJoiner.signalReject = this.signalReject;
			choiceLetter_AcceptJoiner.quest = quest;
			choiceLetter_AcceptJoiner.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner, null);
		}

		// Token: 0x0600882C RID: 34860 RVA: 0x0030E7E0 File Offset: 0x0030C9E0
		public static void AppendCharityInfoToLetter(TaggedString charityInfo, ref TaggedString letterText)
		{
			if (ModsConfig.IdeologyActive)
			{
				IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
				if (source.Any<Pawn>())
				{
					letterText += "\n\n" + charityInfo + "\n\n" + "PawnsHaveCharitableBeliefs".Translate() + ":";
					foreach (IGrouping<Ideo, Pawn> grouping in from c in source
					group c by c.Ideo)
					{
						letterText += "\n  - " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.Color), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false));
					}
				}
			}
		}

		// Token: 0x04005614 RID: 22036
		private const int TimeoutTicks = 60000;

		// Token: 0x04005615 RID: 22037
		public const float RelationWithColonistWeight = 20f;

		// Token: 0x04005616 RID: 22038
		private string signalAccept;

		// Token: 0x04005617 RID: 22039
		private string signalReject;
	}
}
