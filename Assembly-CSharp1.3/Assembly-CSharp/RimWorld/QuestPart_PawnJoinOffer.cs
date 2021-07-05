using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8B RID: 2955
	public class QuestPart_PawnJoinOffer : QuestPartActivable
	{
		// Token: 0x06004517 RID: 17687 RVA: 0x0016E780 File Offset: 0x0016C980
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.letterSent || this.pawn == null || this.pawn.needs == null || this.pawn.needs.mood == null || !this.pawn.IsHashIntervalTick(2500))
			{
				return;
			}
			float curLevelPercentage = this.pawn.needs.mood.CurLevelPercentage;
			if (curLevelPercentage < 0.5f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(QuestPart_PawnJoinOffer.JoinMTBbyMoodCurve.Evaluate(curLevelPercentage), 60000f, 2500f))
			{
				this.SendLetter();
			}
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x0016E818 File Offset: 0x0016CA18
		private void SendLetter()
		{
			if (this.letterSent)
			{
				return;
			}
			this.letter = (ChoiceLetter_AcceptVisitors)LetterMaker.MakeLetter(this.letterLabel, this.letterText, LetterDefOf.AcceptVisitors, null, this.quest);
			this.letter.title = this.letterTitle;
			this.letter.pawns.Add(this.pawn);
			this.letter.quest = this.quest;
			this.letter.acceptedSignal = this.outSignalPawnAccepted;
			this.letter.rejectedSignal = this.outSignalPawnRejected;
			this.letter.lookTargets = new LookTargets(this.pawn);
			this.letter.charity = this.charity;
			Find.LetterStack.ReceiveLetter(this.letter, null);
			this.letterSent = true;
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x0016E8F9 File Offset: 0x0016CAF9
		public override void Cleanup()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x0016E8F9 File Offset: 0x0016CAF9
		protected override void Disable()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x0016E901 File Offset: 0x0016CB01
		private void RemoveLetterIfActive()
		{
			if (this.letter != null && Find.LetterStack.LettersListForReading.Contains(this.letter))
			{
				Find.LetterStack.RemoveLetter(this.letter);
			}
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x0016E934 File Offset: 0x0016CB34
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled || this.letterSent || this.pawn == null)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, string.Format("Add Join Letter for {0} ", this.pawn.NameShortColored) + base.GetType().Name, true, true, true))
			{
				this.SendLetter();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x0016E9C4 File Offset: 0x0016CBC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<bool>(ref this.letterSent, "letterSent", false, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnAccepted, "outSignalPawnAccepted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnRejected, "outSignalPawnRejected", null, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<string>(ref this.letterText, "letterText", null, false);
			Scribe_Values.Look<string>(ref this.letterTitle, "letterTitle", null, false);
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
		}

		// Token: 0x040029FB RID: 10747
		public Pawn pawn;

		// Token: 0x040029FC RID: 10748
		public bool letterSent;

		// Token: 0x040029FD RID: 10749
		public string outSignalPawnAccepted;

		// Token: 0x040029FE RID: 10750
		public string outSignalPawnRejected;

		// Token: 0x040029FF RID: 10751
		public string letterLabel;

		// Token: 0x04002A00 RID: 10752
		public string letterText;

		// Token: 0x04002A01 RID: 10753
		public string letterTitle;

		// Token: 0x04002A02 RID: 10754
		public bool charity;

		// Token: 0x04002A03 RID: 10755
		private ChoiceLetter_AcceptVisitors letter;

		// Token: 0x04002A04 RID: 10756
		private const float MinMoodPercentage = 0.5f;

		// Token: 0x04002A05 RID: 10757
		private static readonly SimpleCurve JoinMTBbyMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.5f, 60f),
				true
			},
			{
				new CurvePoint(1f, 15f),
				true
			}
		};

		// Token: 0x04002A06 RID: 10758
		private const int CheckInterval = 2500;
	}
}
