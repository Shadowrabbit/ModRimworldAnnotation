using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE4 RID: 6884
	public class ChoiceLetter_ChoosePawn : ChoiceLetter
	{
		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x060097A1 RID: 38817 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x060097A2 RID: 38818 RVA: 0x002C86A4 File Offset: 0x002C68A4
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.chosenPawnSignal.NullOrEmpty())
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x060097A3 RID: 38819 RVA: 0x00065071 File Offset: 0x00063271
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					int num;
					for (int i = 0; i < this.pawns.Count; i = num + 1)
					{
						if (!this.pawns[i].DestroyedOrNull())
						{
							yield return this.Option_ChoosePawn(this.pawns[i]);
						}
						num = i;
					}
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

		// Token: 0x060097A4 RID: 38820 RVA: 0x002C86FC File Offset: 0x002C68FC
		private DiaOption Option_ChoosePawn(Pawn p)
		{
			return new DiaOption(p.LabelCap)
			{
				action = delegate()
				{
					if (!this.chosenPawnSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.chosenPawnSignal, p.Named("CHOSEN")));
					}
					Find.LetterStack.RemoveLetter(this);
				},
				resolveTree = true
			};
		}

		// Token: 0x060097A5 RID: 38821 RVA: 0x002C8748 File Offset: 0x002C6948
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040060E5 RID: 24805
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040060E6 RID: 24806
		public string chosenPawnSignal;
	}
}
