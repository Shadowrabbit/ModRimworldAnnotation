using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044D RID: 1101
	public class Dialog_InfoCard : Window
	{
		// Token: 0x06002154 RID: 8532 RVA: 0x000D05FE File Offset: 0x000CE7FE
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000D062A File Offset: 0x000CE82A
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000D0656 File Offset: 0x000CE856
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000D0684 File Offset: 0x000CE884
		public static void PushCurrentToHistoryAndClose()
		{
			Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
			if (dialog_InfoCard == null)
			{
				return;
			}
			Dialog_InfoCard.history.Add(new Dialog_InfoCard.Hyperlink(dialog_InfoCard, StatsReportUtility.SelectedStatIndex));
			Find.WindowStack.TryRemove(dialog_InfoCard, false);
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x000D06C2 File Offset: 0x000CE8C2
		private Def Def
		{
			get
			{
				if (this.thing != null)
				{
					return this.thing.def;
				}
				if (this.worldObject != null)
				{
					return this.worldObject.def;
				}
				return this.def;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002159 RID: 8537 RVA: 0x000D06F2 File Offset: 0x000CE8F2
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600215A RID: 8538 RVA: 0x000D06FF File Offset: 0x000CE8FF
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x000D0710 File Offset: 0x000CE910
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				if (this.tab != Dialog_InfoCard.InfoCardTab.Stats)
				{
					return null;
				}
				return StatsReportUtility.QuickSearchWidget;
			}
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000D0721 File Offset: 0x000CE921
		public Dialog_InfoCard(Thing thing, Precept_ThingStyle precept = null)
		{
			this.thing = thing;
			this.precept = precept;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000D0744 File Offset: 0x000CE944
		public Dialog_InfoCard(Def onlyDef, Precept_ThingStyle precept = null)
		{
			this.def = onlyDef;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000D0760 File Offset: 0x000CE960
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff, Precept_ThingStyle precept = null)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000D0783 File Offset: 0x000CE983
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction, Pawn pawn = null)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.pawn = pawn;
			this.Setup();
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000D07A6 File Offset: 0x000CE9A6
		public Dialog_InfoCard(Faction faction)
		{
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000D07BB File Offset: 0x000CE9BB
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000D07D0 File Offset: 0x000CE9D0
		public override void Notify_CommonSearchChanged()
		{
			StatsReportUtility.Notify_QuickSearchChanged();
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000D07D7 File Offset: 0x000CE9D7
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000D07EC File Offset: 0x000CE9EC
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.soundAppear = SoundDefOf.InfoCard_Open;
			this.soundClose = SoundDefOf.InfoCard_Close;
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x000D0842 File Offset: 0x000CEA42
		public void SetTab(Dialog_InfoCard.InfoCardTab infoCardTab)
		{
			this.tab = infoCardTab;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000D084C File Offset: 0x000CEA4C
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true);
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000D08BC File Offset: 0x000CEABC
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			rect.x += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect.x + 9f, rect.y, 34f, 34f);
			if (this.thing != null)
			{
				Widgets.ThingIcon(rect2, this.thing, 1f, null);
			}
			else
			{
				Widgets.DefIcon(rect2, this.def, this.stuff, 1f, null, true, null);
			}
			Rect rect3 = new Rect(inRect);
			rect3.yMin = rect.yMax;
			rect3.yMax -= 38f;
			Rect rect4 = rect3;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), delegate()
			{
				this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			}, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Character;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Health;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Health);
				list.Add(item3);
				if (ModsConfig.RoyaltyActive && this.ThingPawn.RaceProps.Humanlike && this.ThingPawn.Faction == Faction.OfPlayer && !this.ThingPawn.IsQuestLodger() && this.ThingPawn.royalty != null && PermitsCardUtility.selectedFaction != null)
				{
					TabRecord item4 = new TabRecord("TabPermits".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Permits;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Permits);
					list.Add(item4);
				}
				TabRecord item5 = new TabRecord("TabRecords".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Records;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Records);
				list.Add(item5);
			}
			if (list.Count > 1)
			{
				rect4.yMin += 45f;
				TabDrawer.DrawTabs<TabRecord>(rect4, list, 200f);
			}
			this.FillCard(rect4.ContractedBy(18f));
			if (this.def != null && this.def is BuildableDef)
			{
				IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor((BuildableDef)this.def, TechLevel.Undefined);
				if (enumerable.Count<ThingDef>() > 0 && Dialog_InfoCard.ShowMaterialsButton(inRect, Dialog_InfoCard.history.Count > 0))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (ThingDef thingDef in enumerable)
					{
						ThingDef localStuff = thingDef;
						list2.Add(new FloatMenuOption(thingDef.LabelCap, delegate()
						{
							this.stuff = localStuff;
							this.Setup();
						}, thingDef, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (Dialog_InfoCard.history.Count > 0 && Widgets.BackButtonFor(inRect))
			{
				Dialog_InfoCard.Hyperlink hyperlink = Dialog_InfoCard.history[Dialog_InfoCard.history.Count - 1];
				Dialog_InfoCard.history.RemoveAt(Dialog_InfoCard.history.Count - 1);
				Find.WindowStack.TryRemove(typeof(Dialog_InfoCard), false);
				hyperlink.ActivateHyperlink();
			}
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000D0CA8 File Offset: 0x000CEEA8
		protected void FillCard(Rect cardRect)
		{
			if (this.tab == Dialog_InfoCard.InfoCardTab.Stats)
			{
				if (this.thing != null)
				{
					Thing innerThing = this.thing;
					MinifiedThing minifiedThing = this.thing as MinifiedThing;
					if (minifiedThing != null)
					{
						innerThing = minifiedThing.InnerThing;
					}
					StatsReportUtility.DrawStatsReport(cardRect, innerThing);
				}
				else if (this.titleDef != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.titleDef, this.faction, this.pawn);
				}
				else if (this.faction != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.faction);
				}
				else if (this.worldObject != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.worldObject);
				}
				else if (this.def is AbilityDef)
				{
					StatsReportUtility.DrawStatsReport(cardRect, (AbilityDef)this.def);
				}
				else
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.def, this.stuff);
				}
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect));
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Health)
			{
				cardRect.yMin += 8f;
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, false, null);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Permits)
			{
				PermitsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			if (this.executeAfterFillCardOnce != null)
			{
				this.executeAfterFillCardOnce();
				this.executeAfterFillCardOnce = null;
			}
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000D0E2C File Offset: 0x000CF02C
		private string GetTitle()
		{
			if (this.thing != null)
			{
				if (this.precept == null)
				{
					return this.thing.LabelCapNoCount;
				}
				return this.precept.LabelCap;
			}
			else
			{
				if (this.worldObject != null)
				{
					return this.worldObject.LabelCap;
				}
				ThingDef thingDef = this.Def as ThingDef;
				if (thingDef != null)
				{
					if (this.precept == null)
					{
						return GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
					}
					return this.precept.LabelCap;
				}
				else
				{
					AbilityDef abilityDef = this.Def as AbilityDef;
					if (abilityDef != null)
					{
						return abilityDef.LabelCap;
					}
					if (this.titleDef != null)
					{
						return this.titleDef.GetLabelCapForBothGenders();
					}
					if (this.faction != null)
					{
						return this.faction.Name;
					}
					if (this.precept == null)
					{
						return this.Def.LabelCap;
					}
					return this.precept.LabelCap;
				}
			}
		}

		// Token: 0x040014B1 RID: 5297
		private const float ShowMaterialsButtonWidth = 200f;

		// Token: 0x040014B2 RID: 5298
		private const float ShowMaterialsButtonHeight = 40f;

		// Token: 0x040014B3 RID: 5299
		private const float ShowMaterialsMargin = 16f;

		// Token: 0x040014B4 RID: 5300
		private Action executeAfterFillCardOnce;

		// Token: 0x040014B5 RID: 5301
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		// Token: 0x040014B6 RID: 5302
		private Thing thing;

		// Token: 0x040014B7 RID: 5303
		private ThingDef stuff;

		// Token: 0x040014B8 RID: 5304
		private Precept_ThingStyle precept;

		// Token: 0x040014B9 RID: 5305
		private Def def;

		// Token: 0x040014BA RID: 5306
		private WorldObject worldObject;

		// Token: 0x040014BB RID: 5307
		private RoyalTitleDef titleDef;

		// Token: 0x040014BC RID: 5308
		private Faction faction;

		// Token: 0x040014BD RID: 5309
		private Pawn pawn;

		// Token: 0x040014BE RID: 5310
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x02001C74 RID: 7284
		public enum InfoCardTab : byte
		{
			// Token: 0x04006DE6 RID: 28134
			Stats,
			// Token: 0x04006DE7 RID: 28135
			Character,
			// Token: 0x04006DE8 RID: 28136
			Health,
			// Token: 0x04006DE9 RID: 28137
			Records,
			// Token: 0x04006DEA RID: 28138
			Permits
		}

		// Token: 0x02001C75 RID: 7285
		public struct Hyperlink
		{
			// Token: 0x170019FD RID: 6653
			// (get) Token: 0x0600A739 RID: 42809 RVA: 0x003832A8 File Offset: 0x003814A8
			public string Label
			{
				get
				{
					string result = null;
					if (this.worldObject != null)
					{
						result = this.worldObject.Label;
					}
					else if (this.def != null && this.def is ThingDef && this.stuff != null)
					{
						result = (this.def as ThingDef).label;
					}
					else if (this.def != null)
					{
						result = this.def.label;
					}
					else if (this.thing != null)
					{
						result = this.thing.Label;
					}
					else if (this.titleDef != null)
					{
						result = this.titleDef.GetLabelCapForBothGenders();
					}
					else if (this.quest != null)
					{
						result = this.quest.name;
					}
					else if (this.ideo != null)
					{
						result = this.ideo.name;
					}
					return result;
				}
			}

			// Token: 0x0600A73A RID: 42810 RVA: 0x00383370 File Offset: 0x00381570
			public Hyperlink(Dialog_InfoCard infoCard, int statIndex = -1)
			{
				this.def = infoCard.def;
				this.thing = infoCard.thing;
				this.stuff = infoCard.stuff;
				this.worldObject = infoCard.worldObject;
				this.titleDef = infoCard.titleDef;
				this.faction = infoCard.faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
			}

			// Token: 0x0600A73B RID: 42811 RVA: 0x003833DC File Offset: 0x003815DC
			public Hyperlink(Def def, int statIndex = -1)
			{
				this.def = def;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
			}

			// Token: 0x0600A73C RID: 42812 RVA: 0x00383428 File Offset: 0x00381628
			public Hyperlink(RoyalTitleDef titleDef, Faction faction, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = titleDef;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
			}

			// Token: 0x0600A73D RID: 42813 RVA: 0x00383474 File Offset: 0x00381674
			public Hyperlink(Thing thing, int statIndex = -1)
			{
				this.thing = thing;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
			}

			// Token: 0x0600A73E RID: 42814 RVA: 0x003834C0 File Offset: 0x003816C0
			public Hyperlink(Quest quest, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = quest;
				this.ideo = null;
			}

			// Token: 0x0600A73F RID: 42815 RVA: 0x0038350C File Offset: 0x0038170C
			public Hyperlink(Ideo ideo)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = 0;
				this.quest = null;
				this.ideo = ideo;
			}

			// Token: 0x0600A740 RID: 42816 RVA: 0x00383558 File Offset: 0x00381758
			public void ActivateHyperlink()
			{
				if (this.ideo != null)
				{
					Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard != null)
					{
						dialog_InfoCard.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
					IdeoUIUtility.SetSelected(this.ideo);
					return;
				}
				if (this.quest != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					return;
				}
				Dialog_InfoCard dialog_InfoCard2 = null;
				if (this.def == null && this.thing == null && this.worldObject == null && this.titleDef == null)
				{
					dialog_InfoCard2 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
				}
				else
				{
					Dialog_InfoCard.PushCurrentToHistoryAndClose();
					if (this.worldObject != null)
					{
						dialog_InfoCard2 = new Dialog_InfoCard(this.worldObject);
					}
					else if (this.def != null && this.def is ThingDef && (this.stuff != null || GenStuff.DefaultStuffFor((ThingDef)this.def) != null))
					{
						dialog_InfoCard2 = new Dialog_InfoCard(this.def as ThingDef, this.stuff ?? GenStuff.DefaultStuffFor((ThingDef)this.def), null);
					}
					else if (this.def != null)
					{
						dialog_InfoCard2 = new Dialog_InfoCard(this.def, null);
					}
					else if (this.thing != null)
					{
						dialog_InfoCard2 = new Dialog_InfoCard(this.thing, null);
					}
					else if (this.titleDef != null)
					{
						dialog_InfoCard2 = new Dialog_InfoCard(this.titleDef, this.faction, null);
					}
				}
				if (dialog_InfoCard2 == null)
				{
					return;
				}
				int localSelectedStatIndex = this.selectedStatIndex;
				if (this.selectedStatIndex >= 0)
				{
					dialog_InfoCard2.executeAfterFillCardOnce = delegate()
					{
						StatsReportUtility.SelectEntry(localSelectedStatIndex);
					};
				}
				Find.WindowStack.Add(dialog_InfoCard2);
			}

			// Token: 0x04006DEB RID: 28139
			public Thing thing;

			// Token: 0x04006DEC RID: 28140
			public ThingDef stuff;

			// Token: 0x04006DED RID: 28141
			public Def def;

			// Token: 0x04006DEE RID: 28142
			public WorldObject worldObject;

			// Token: 0x04006DEF RID: 28143
			public RoyalTitleDef titleDef;

			// Token: 0x04006DF0 RID: 28144
			public Faction faction;

			// Token: 0x04006DF1 RID: 28145
			public Quest quest;

			// Token: 0x04006DF2 RID: 28146
			public Ideo ideo;

			// Token: 0x04006DF3 RID: 28147
			public int selectedStatIndex;
		}
	}
}
