using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016CC RID: 5836
	public class QuestNode_LendColonistsToFaction : QuestNode
	{
		// Token: 0x06008720 RID: 34592 RVA: 0x00305ED8 File Offset: 0x003040D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false);
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = new QuestPart_LendColonistsToFaction
			{
				inSignalEnable = inSignal,
				shuttle = this.shuttle.GetValue(slate),
				lendColonistsToFaction = this.lendColonistsToFactionOf.GetValue(slate).Faction,
				returnLentColonistsInTicks = this.returnLentColonistsInTicks.GetValue(slate),
				returnMap = slate.Get<Map>("map", null, false).Parent
			};
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_LendColonistsToFaction.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			if (!this.outSignalColonistsDied.GetValue(slate).NullOrEmpty())
			{
				questPart_LendColonistsToFaction.outSignalColonistsDied = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalColonistsDied.GetValue(slate));
			}
			QuestGen.quest.AddPart(questPart_LendColonistsToFaction);
			QuestGen.quest.TendPawnsWithMedicine(ThingDefOf.MedicineIndustrial, true, null, this.shuttle.GetValue(slate), inSignal);
		}

		// Token: 0x06008721 RID: 34593 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04005517 RID: 21783
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005518 RID: 21784
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04005519 RID: 21785
		[NoTranslate]
		public SlateRef<string> outSignalColonistsDied;

		// Token: 0x0400551A RID: 21786
		public SlateRef<Thing> shuttle;

		// Token: 0x0400551B RID: 21787
		public SlateRef<Pawn> lendColonistsToFactionOf;

		// Token: 0x0400551C RID: 21788
		public SlateRef<int> returnLentColonistsInTicks;
	}
}
