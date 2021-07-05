using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001338 RID: 4920
	public class ChoiceLetter_ChoosePawn : ChoiceLetter
	{
		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x0600770D RID: 30477 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x0600770E RID: 30478 RVA: 0x0029D000 File Offset: 0x0029B200
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

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x0600770F RID: 30479 RVA: 0x0029D056 File Offset: 0x0029B256
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

		// Token: 0x06007710 RID: 30480 RVA: 0x0029D068 File Offset: 0x0029B268
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

		// Token: 0x06007711 RID: 30481 RVA: 0x0029D0B4 File Offset: 0x0029B2B4
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

		// Token: 0x0400422C RID: 16940
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400422D RID: 16941
		public string chosenPawnSignal;
	}
}
