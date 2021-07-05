using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001335 RID: 4917
	public class ChoiceLetter_AcceptJoiner : ChoiceLetter
	{
		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x060076F5 RID: 30453 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x060076F6 RID: 30454 RVA: 0x0029C9C1 File Offset: 0x0029ABC1
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && this.quest != null && (this.quest.State == QuestState.Ongoing || this.quest.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x060076F7 RID: 30455 RVA: 0x0029C9F3 File Offset: 0x0029ABF3
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
				}
				else
				{
					DiaOption diaOption = new DiaOption("AcceptButton".Translate());
					DiaOption optionReject = new DiaOption("RejectLetter".Translate());
					diaOption.action = delegate()
					{
						Find.SignalManager.SendSignal(new Signal(this.signalAccept));
						Find.LetterStack.RemoveLetter(this);
					};
					diaOption.resolveTree = true;
					optionReject.action = delegate()
					{
						Find.SignalManager.SendSignal(new Signal(this.signalReject));
						Find.LetterStack.RemoveLetter(this);
					};
					optionReject.resolveTree = true;
					yield return diaOption;
					yield return optionReject;
					yield return base.Option_Postpone;
					optionReject = null;
				}
				yield break;
			}
		}

		// Token: 0x060076F8 RID: 30456 RVA: 0x0029CA03 File Offset: 0x0029AC03
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
		}

		// Token: 0x04004223 RID: 16931
		public string signalAccept;

		// Token: 0x04004224 RID: 16932
		public string signalReject;
	}
}
