using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000B65 RID: 2917
	public class QuestPart_Bestowing_TargetChangedTitle : QuestPart
	{
		// Token: 0x0600443C RID: 17468 RVA: 0x0016A4D0 File Offset: 0x001686D0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				RoyalTitleDef titleAwardedWhenUpdating = this.pawn.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.pawn.royalty.GetFavor(this.bestower.Faction));
				string key;
				string key2;
				LetterDef textLetterDef;
				if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.seniority > this.currentTitle.seniority)
				{
					key = "LetterLabelBestowingCeremonyTitleUpdated";
					key2 = "LetterTextBestowingCeremonyTitleUpdated";
					textLetterDef = LetterDefOf.NeutralEvent;
					SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
				}
				else
				{
					key = "LetterQuestFailedLabel";
					key2 = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
				}
				Find.LetterStack.ReceiveLetter(key.Translate(this.pawn.Named("TARGET")), key2.Translate(this.quest.name.CapitalizeFirst(), this.pawn.Named("TARGET")), textLetterDef, this.pawn, null, this.quest, null, null);
				this.quest.End(QuestEndOutcome.Unknown, false);
			}
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0016A5F8 File Offset: 0x001687F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.currentTitle, "currentTitle");
		}

		// Token: 0x04002967 RID: 10599
		public string inSignal;

		// Token: 0x04002968 RID: 10600
		public Pawn pawn;

		// Token: 0x04002969 RID: 10601
		public Pawn bestower;

		// Token: 0x0400296A RID: 10602
		public RoyalTitleDef currentTitle;
	}
}
