using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001336 RID: 4918
	public class ChoiceLetter_AcceptVisitors : ChoiceLetter
	{
		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x060076FC RID: 30460 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x060076FD RID: 30461 RVA: 0x0029CA74 File Offset: 0x0029AC74
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

		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x060076FE RID: 30462 RVA: 0x0029CAD4 File Offset: 0x0029ACD4
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

		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x060076FF RID: 30463 RVA: 0x0029CB4A File Offset: 0x0029AD4A
		private DiaOption Option_RejectWithCharityConfirmation
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Action action = delegate()
						{
							if (!this.rejectedSignal.NullOrEmpty())
							{
								object arg = (this.pawns.Count == 1) ? this.pawns[0] : this.pawns;
								Find.SignalManager.SendSignal(new Signal(this.rejectedSignal, arg.Named("SUBJECT")));
							}
							Find.LetterStack.RemoveLetter(this);
						};
						if (!ModsConfig.IdeologyActive || !this.charity)
						{
							action();
							return;
						}
						IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
						if (source.Any<Pawn>())
						{
							string text = "";
							foreach (IGrouping<Ideo, Pawn> grouping in from c in source
							group c by c.Ideo)
							{
								text += "\n- " + "BelieversIn".Translate(grouping.Key.name, grouping.Select((Pawn f) => f.Label).ToCommaList(false, false));
							}
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmationCharityJoiner".Translate(text), action, false, null));
							return;
						}
						action();
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x06007700 RID: 30464 RVA: 0x0029CB79 File Offset: 0x0029AD79
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					yield return this.Option_Accept;
					yield return this.Option_RejectWithCharityConfirmation;
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

		// Token: 0x06007701 RID: 30465 RVA: 0x0029CB8C File Offset: 0x0029AD8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.acceptedSignal, "acceptedSignal", null, false);
			Scribe_Values.Look<string>(ref this.rejectedSignal, "rejectedSignal", null, false);
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x0029CC20 File Offset: 0x0029AE20
		private bool CanStillAccept(Pawn p)
		{
			if (p.DestroyedOrNull() || !p.SpawnedOrAnyParentSpawned)
			{
				return false;
			}
			if (p.CurJob != null && p.CurJob.exitMapOnArrival)
			{
				return false;
			}
			Lord lord = p.GetLord();
			if (lord != null)
			{
				if (lord.CurLordToil is LordToil_ExitMap)
				{
					return false;
				}
				LordJob_VisitColony lordJob_VisitColony;
				if ((lordJob_VisitColony = (lord.LordJob as LordJob_VisitColony)) != null && lordJob_VisitColony.exitSubgraph.lordToils.Contains(lord.CurLordToil))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004225 RID: 16933
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004226 RID: 16934
		public string acceptedSignal;

		// Token: 0x04004227 RID: 16935
		public string rejectedSignal;

		// Token: 0x04004228 RID: 16936
		public bool charity;
	}
}
