using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000405 RID: 1029
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001ECD RID: 7885
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x000C082C File Offset: 0x000BEA2C
		protected DiaOption Option_Close
		{
			get
			{
				return new DiaOption("Close".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x000C085C File Offset: 0x000BEA5C
		protected DiaOption Option_JumpToLocation
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x000C08CC File Offset: 0x000BEACC
		protected DiaOption Option_JumpToLocationAndPostpone
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000C0933 File Offset: 0x000BEB33
		protected DiaOption Option_Reject
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x000C0964 File Offset: 0x000BEB64
		protected DiaOption Option_Postpone
		{
			get
			{
				DiaOption diaOption = new DiaOption("PostponeLetter".Translate());
				diaOption.resolveTree = true;
				if (base.TimeoutActive && this.disappearAtTick <= Find.TickManager.TicksGame + 1)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000C09B4 File Offset: 0x000BEBB4
		protected DiaOption Option_ViewInQuestsTab(string labelKey = "ViewRelatedQuest", bool postpone = false)
		{
			TaggedString taggedString = labelKey.Translate();
			if (this.title != this.quest.name && !this.quest.hidden)
			{
				taggedString += ": " + this.quest.name;
			}
			DiaOption diaOption = new DiaOption(taggedString);
			diaOption.action = delegate()
			{
				if (this.quest != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					if (!postpone)
					{
						Find.LetterStack.RemoveLetter(this);
					}
				}
			};
			diaOption.resolveTree = true;
			if (this.quest == null || this.quest.hidden)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000C0A60 File Offset: 0x000BEC60
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000C0ABC File Offset: 0x000BECBC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Collections.Look<ThingDef>(ref this.hyperlinkThingDefs, "hyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.hyperlinkHediffDefs, "hyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000C0B4A File Offset: 0x000BED4A
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000C0B58 File Offset: 0x000BED58
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		// Token: 0x040012D6 RID: 4822
		public string title;

		// Token: 0x040012D7 RID: 4823
		public TaggedString text;

		// Token: 0x040012D8 RID: 4824
		public bool radioMode;

		// Token: 0x040012D9 RID: 4825
		public Quest quest;

		// Token: 0x040012DA RID: 4826
		public List<ThingDef> hyperlinkThingDefs;

		// Token: 0x040012DB RID: 4827
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
