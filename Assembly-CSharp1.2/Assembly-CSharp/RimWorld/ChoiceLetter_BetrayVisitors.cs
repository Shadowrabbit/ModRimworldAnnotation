using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE1 RID: 6881
	public class ChoiceLetter_BetrayVisitors : ChoiceLetter
	{
		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x06009791 RID: 38801 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x06009792 RID: 38802 RVA: 0x002C84DC File Offset: 0x002C66DC
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
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Spawned && !this.pawns[i].Destroyed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x06009793 RID: 38803 RVA: 0x00065018 File Offset: 0x00063218
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
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

		// Token: 0x06009794 RID: 38804 RVA: 0x002C854C File Offset: 0x002C674C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040060DE RID: 24798
		public List<Pawn> pawns = new List<Pawn>();
	}
}
