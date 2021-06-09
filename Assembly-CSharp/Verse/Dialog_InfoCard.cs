using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000798 RID: 1944
	public class Dialog_InfoCard : Window
	{
		// Token: 0x060030FC RID: 12540 RVA: 0x000269BA File Offset: 0x00024BBA
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000269E6 File Offset: 0x00024BE6
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x00026A12 File Offset: 0x00024C12
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x00143C6C File Offset: 0x00141E6C
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

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x00026A3E File Offset: 0x00024C3E
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

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x00026A6E File Offset: 0x00024C6E
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x00026A7B File Offset: 0x00024C7B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x00026A8C File Offset: 0x00024C8C
		public Dialog_InfoCard(Thing thing)
		{
			this.thing = thing;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x00026AA8 File Offset: 0x00024CA8
		public Dialog_InfoCard(Def onlyDef)
		{
			this.def = onlyDef;
			this.Setup();
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x00026ABD File Offset: 0x00024CBD
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.Setup();
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x00026AD9 File Offset: 0x00024CD9
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x00026AF5 File Offset: 0x00024CF5
		public Dialog_InfoCard(Faction faction)
		{
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x00026B0A File Offset: 0x00024D0A
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00026B1F File Offset: 0x00024D1F
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x00143CAC File Offset: 0x00141EAC
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

		// Token: 0x0600310C RID: 12556 RVA: 0x00026B32 File Offset: 0x00024D32
		public void SetTab(Dialog_InfoCard.InfoCardTab infoCardTab)
		{
			this.tab = infoCardTab;
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x00143D04 File Offset: 0x00141F04
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true);
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x00143D74 File Offset: 0x00141F74
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
				Widgets.ThingIcon(rect2, this.thing, 1f);
			}
			else
			{
				Widgets.DefIcon(rect2, this.def, this.stuff, 1f, true);
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
				TabDrawer.DrawTabs(rect4, list, 200f);
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
						}, thingDef, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (Dialog_InfoCard.history.Count > 0 && Widgets.BackButtonFor(inRect))
			{
				Dialog_InfoCard.Hyperlink hyperlink = Dialog_InfoCard.history[Dialog_InfoCard.history.Count - 1];
				Dialog_InfoCard.history.RemoveAt(Dialog_InfoCard.history.Count - 1);
				Find.WindowStack.TryRemove(typeof(Dialog_InfoCard), false);
				hyperlink.OpenDialog();
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x00144148 File Offset: 0x00142348
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
					StatsReportUtility.DrawStatsReport(cardRect, this.titleDef, this.faction);
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

		// Token: 0x06003110 RID: 12560 RVA: 0x001442C4 File Offset: 0x001424C4
		private string GetTitle()
		{
			if (this.thing != null)
			{
				return this.thing.LabelCapNoCount;
			}
			if (this.worldObject != null)
			{
				return this.worldObject.LabelCap;
			}
			ThingDef thingDef = this.Def as ThingDef;
			if (thingDef != null)
			{
				return GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
			}
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
			return this.Def.LabelCap;
		}

		// Token: 0x040021B6 RID: 8630
		private const float ShowMaterialsButtonWidth = 200f;

		// Token: 0x040021B7 RID: 8631
		private const float ShowMaterialsButtonHeight = 40f;

		// Token: 0x040021B8 RID: 8632
		private const float ShowMaterialsMargin = 16f;

		// Token: 0x040021B9 RID: 8633
		private Action executeAfterFillCardOnce;

		// Token: 0x040021BA RID: 8634
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		// Token: 0x040021BB RID: 8635
		private Thing thing;

		// Token: 0x040021BC RID: 8636
		private ThingDef stuff;

		// Token: 0x040021BD RID: 8637
		private Def def;

		// Token: 0x040021BE RID: 8638
		private WorldObject worldObject;

		// Token: 0x040021BF RID: 8639
		private RoyalTitleDef titleDef;

		// Token: 0x040021C0 RID: 8640
		private Faction faction;

		// Token: 0x040021C1 RID: 8641
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x02000799 RID: 1945
		public enum InfoCardTab : byte
		{
			// Token: 0x040021C3 RID: 8643
			Stats,
			// Token: 0x040021C4 RID: 8644
			Character,
			// Token: 0x040021C5 RID: 8645
			Health,
			// Token: 0x040021C6 RID: 8646
			Records,
			// Token: 0x040021C7 RID: 8647
			Permits
		}

		// Token: 0x0200079A RID: 1946
		public struct Hyperlink
		{
			// Token: 0x17000756 RID: 1878
			// (get) Token: 0x06003117 RID: 12567 RVA: 0x00144370 File Offset: 0x00142570
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
					return result;
				}
			}

			// Token: 0x06003118 RID: 12568 RVA: 0x00144408 File Offset: 0x00142608
			public Hyperlink(Dialog_InfoCard infoCard, int statIndex = -1)
			{
				this.def = infoCard.def;
				this.thing = infoCard.thing;
				this.stuff = infoCard.stuff;
				this.worldObject = infoCard.worldObject;
				this.titleDef = infoCard.titleDef;
				this.faction = infoCard.faction;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x06003119 RID: 12569 RVA: 0x00026B74 File Offset: 0x00024D74
			public Hyperlink(Def def, int statIndex = -1)
			{
				this.def = def;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x0600311A RID: 12570 RVA: 0x00026BA7 File Offset: 0x00024DA7
			public Hyperlink(RoyalTitleDef titleDef, Faction faction, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = titleDef;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x0600311B RID: 12571 RVA: 0x00026BDA File Offset: 0x00024DDA
			public Hyperlink(Thing thing, int statIndex = -1)
			{
				this.thing = thing;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x0600311C RID: 12572 RVA: 0x00144464 File Offset: 0x00142664
			public void OpenDialog()
			{
				Dialog_InfoCard dialog_InfoCard = null;
				if (this.def == null && this.thing == null && this.worldObject == null && this.titleDef == null)
				{
					dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
				}
				else
				{
					Dialog_InfoCard.PushCurrentToHistoryAndClose();
					if (this.worldObject != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.worldObject);
					}
					else if (this.def != null && this.def is ThingDef && (this.stuff != null || GenStuff.DefaultStuffFor((ThingDef)this.def) != null))
					{
						dialog_InfoCard = new Dialog_InfoCard(this.def as ThingDef, this.stuff ?? GenStuff.DefaultStuffFor((ThingDef)this.def));
					}
					else if (this.def != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.def);
					}
					else if (this.thing != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.thing);
					}
					else if (this.titleDef != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.titleDef, this.faction);
					}
				}
				if (dialog_InfoCard == null)
				{
					return;
				}
				int localSelectedStatIndex = this.selectedStatIndex;
				if (this.selectedStatIndex >= 0)
				{
					dialog_InfoCard.executeAfterFillCardOnce = delegate()
					{
						StatsReportUtility.SelectEntry(localSelectedStatIndex);
					};
				}
				Find.WindowStack.Add(dialog_InfoCard);
			}

			// Token: 0x040021C8 RID: 8648
			public Thing thing;

			// Token: 0x040021C9 RID: 8649
			public ThingDef stuff;

			// Token: 0x040021CA RID: 8650
			public Def def;

			// Token: 0x040021CB RID: 8651
			public WorldObject worldObject;

			// Token: 0x040021CC RID: 8652
			public RoyalTitleDef titleDef;

			// Token: 0x040021CD RID: 8653
			public Faction faction;

			// Token: 0x040021CE RID: 8654
			public int selectedStatIndex;
		}
	}
}
