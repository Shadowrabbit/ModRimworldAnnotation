using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EE RID: 4334
	public class QuestPart_PawnJoinOffer : QuestPartActivable
	{
		// Token: 0x06005EAB RID: 24235 RVA: 0x001E01E8 File Offset: 0x001DE3E8
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

		// Token: 0x06005EAC RID: 24236 RVA: 0x001E0280 File Offset: 0x001DE480
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
			this.letter.lookTargets = new LookTargets(this.pawn);
			Find.LetterStack.ReceiveLetter(this.letter, null);
			this.letterSent = true;
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x0004186D File Offset: 0x0003FA6D
		public override void Cleanup()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x0004186D File Offset: 0x0003FA6D
		protected override void Disable()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x00041875 File Offset: 0x0003FA75
		private void RemoveLetterIfActive()
		{
			if (this.letter != null && Find.LetterStack.LettersListForReading.Contains(this.letter))
			{
				Find.LetterStack.RemoveLetter(this.letter);
			}
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x001E0340 File Offset: 0x001DE540
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

		// Token: 0x06005EB1 RID: 24241 RVA: 0x001E03D0 File Offset: 0x001DE5D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<bool>(ref this.letterSent, "letterSent", false, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnAccepted, "outSignalPawnAccepted", null, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<string>(ref this.letterText, "letterText", null, false);
			Scribe_Values.Look<string>(ref this.letterTitle, "letterTitle", null, false);
		}

		// Token: 0x04003F49 RID: 16201
		public Pawn pawn;

		// Token: 0x04003F4A RID: 16202
		public bool letterSent;

		// Token: 0x04003F4B RID: 16203
		public string outSignalPawnAccepted;

		// Token: 0x04003F4C RID: 16204
		public string letterLabel;

		// Token: 0x04003F4D RID: 16205
		public string letterText;

		// Token: 0x04003F4E RID: 16206
		public string letterTitle;

		// Token: 0x04003F4F RID: 16207
		private ChoiceLetter_AcceptVisitors letter;

		// Token: 0x04003F50 RID: 16208
		private const float MinMoodPercentage = 0.5f;

		// Token: 0x04003F51 RID: 16209
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

		// Token: 0x04003F52 RID: 16210
		private const int CheckInterval = 2500;
	}
}
