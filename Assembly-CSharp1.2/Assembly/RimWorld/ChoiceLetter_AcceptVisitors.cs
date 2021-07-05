using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADE RID: 6878
	public class ChoiceLetter_AcceptVisitors : ChoiceLetter
	{
		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x0600977D RID: 38781 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x0600977E RID: 38782 RVA: 0x002C81AC File Offset: 0x002C63AC
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.quest == null || this.quest.State != QuestState.Ongoing)
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanStillAccept(this.pawns[i]))
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x0600977F RID: 38783 RVA: 0x002C820C File Offset: 0x002C640C
		private DiaOption Option_Accept
		{
			get
			{
				DiaOption diaOption = new DiaOption("AcceptButton".Translate());
				diaOption.action = delegate()
				{
					this.pawns.RemoveAll((Pawn x) => !this.CanStillAccept(x));
					if (!this.acceptedSignal.NullOrEmpty())
					{
						object arg = (this.pawns.Count == 1) ? this.pawns[0] : this.pawns;
						Find.SignalManager.SendSignal(new Signal(this.acceptedSignal, arg.Named("SUBJECT")));
					}
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				bool flag = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanStillAccept(this.pawns[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x06009780 RID: 38784 RVA: 0x00064F87 File Offset: 0x00063187
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					yield return this.Option_Accept;
					yield return base.Option_Reject;
					yield return base.Option_Postpone;
				}
				else
				{
					yield return base.Option_Close;
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocationAndPostpone;
				}
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", true);
				}
				yield break;
			}
		}

		// Token: 0x06009781 RID: 38785 RVA: 0x002C8284 File Offset: 0x002C6484
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.acceptedSignal, "acceptedSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009782 RID: 38786 RVA: 0x00064F97 File Offset: 0x00063197
		private bool CanStillAccept(Pawn p)
		{
			return !p.DestroyedOrNull() && p.SpawnedOrAnyParentSpawned && (p.CurJob == null || !p.CurJob.exitMapOnArrival);
		}

		// Token: 0x040060D6 RID: 24790
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040060D7 RID: 24791
		public string acceptedSignal;
	}
}
