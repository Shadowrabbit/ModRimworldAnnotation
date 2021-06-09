using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000724 RID: 1828
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002E13 RID: 11795
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002E14 RID: 11796 RVA: 0x00024417 File Offset: 0x00022617
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

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002E15 RID: 11797 RVA: 0x001368B0 File Offset: 0x00134AB0
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

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002E16 RID: 11798 RVA: 0x00136920 File Offset: 0x00134B20
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

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002E17 RID: 11799 RVA: 0x00024446 File Offset: 0x00022646
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

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002E18 RID: 11800 RVA: 0x00136988 File Offset: 0x00134B88
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

		// Token: 0x06002E19 RID: 11801 RVA: 0x001369D8 File Offset: 0x00134BD8
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

		// Token: 0x06002E1A RID: 11802 RVA: 0x00136A84 File Offset: 0x00134C84
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x00136AE0 File Offset: 0x00134CE0
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

		// Token: 0x06002E1C RID: 11804 RVA: 0x00024475 File Offset: 0x00022675
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x00136B70 File Offset: 0x00134D70
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		// Token: 0x04001F78 RID: 8056
		public string title;

		// Token: 0x04001F79 RID: 8057
		public TaggedString text;

		// Token: 0x04001F7A RID: 8058
		public bool radioMode;

		// Token: 0x04001F7B RID: 8059
		public Quest quest;

		// Token: 0x04001F7C RID: 8060
		public List<ThingDef> hyperlinkThingDefs;

		// Token: 0x04001F7D RID: 8061
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
