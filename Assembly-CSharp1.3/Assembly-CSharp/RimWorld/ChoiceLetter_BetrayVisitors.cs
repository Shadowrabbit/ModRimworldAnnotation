using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001337 RID: 4919
	public class ChoiceLetter_BetrayVisitors : ChoiceLetter
	{
		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x06007708 RID: 30472 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x06007709 RID: 30473 RVA: 0x0029CECC File Offset: 0x0029B0CC
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
				if (this.requiresAliveAsker && (this.asker == null || this.asker.Dead))
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

		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x0600770A RID: 30474 RVA: 0x0029CF59 File Offset: 0x0029B159
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

		// Token: 0x0600770B RID: 30475 RVA: 0x0029CF6C File Offset: 0x0029B16C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_Values.Look<bool>(ref this.requiresAliveAsker, "requiresAliveAsker", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04004229 RID: 16937
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400422A RID: 16938
		public Pawn asker;

		// Token: 0x0400422B RID: 16939
		public bool requiresAliveAsker;
	}
}
