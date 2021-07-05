using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001325 RID: 4901
	[StaticConstructorOnStartup]
	public class Page_SelectScenario : Page
	{
		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x06007672 RID: 30322 RVA: 0x002910C1 File Offset: 0x0028F2C1
		public override string PageTitle
		{
			get
			{
				return "ChooseScenario".Translate();
			}
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x002910D2 File Offset: 0x0028F2D2
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
			ScenarioLister.MarkDirty();
			this.EnsureValidSelection();
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x002910F0 File Offset: 0x0028F2F0
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			GUI.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoScenarioSelectionList(rect2);
			ScenarioUI.DrawScenarioInfo(new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded(), this.curScen, ref this.infoScrollPosition);
			GUI.EndGroup();
			base.DoBottomButtons(rect, null, "ScenarioEditor".Translate(), new Action(this.GoToScenarioEditor), true, true);
		}

		// Token: 0x06007675 RID: 30325 RVA: 0x002911BB File Offset: 0x0028F3BB
		private bool CanEditScenario(Scenario scen)
		{
			return scen.Category == ScenarioCategory.CustomLocal || scen.CanToUploadToWorkshop();
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x002911D4 File Offset: 0x0028F3D4
		private void GoToScenarioEditor()
		{
			Page_ScenarioEditor page_ScenarioEditor = new Page_ScenarioEditor(this.CanEditScenario(this.curScen) ? this.curScen : this.curScen.CopyForEditing());
			page_ScenarioEditor.prev = this;
			Find.WindowStack.Add(page_ScenarioEditor);
			this.Close(true);
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x00291224 File Offset: 0x0028F424
		private void DoScenarioSelectionList(Rect rect)
		{
			rect.xMax += 2f;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f - 2f, this.totalScenarioListHeight + 250f);
			Widgets.BeginScrollView(rect, ref this.scenariosScrollPosition, rect2, true);
			Rect rect3 = rect2.AtZero();
			rect3.height = 999999f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect2.width;
			listing_Standard.Begin(rect3);
			Text.Font = GameFont.Small;
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosCustom".Translate(), -1f, null);
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.CustomLocal));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosSteamWorkshop".Translate(), -1f, null);
			if (listing_Standard.ButtonText("OpenSteamWorkshop".Translate(), null))
			{
				SteamUtility.OpenSteamWorkshopPage();
			}
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.SteamWorkshop));
			listing_Standard.End();
			this.totalScenarioListHeight = listing_Standard.CurHeight;
			Widgets.EndScrollView();
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x00291360 File Offset: 0x0028F560
		private void ListScenariosOnListing(Listing_Standard listing, IEnumerable<Scenario> scenarios)
		{
			bool flag = false;
			foreach (Scenario scenario in scenarios)
			{
				if (scenario.showInUI)
				{
					if (flag)
					{
						listing.Gap(12f);
					}
					Scenario scen = scenario;
					Rect rect = listing.GetRect(62f);
					this.DoScenarioListEntry(rect, scen);
					flag = true;
				}
			}
			if (!flag)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				listing.Label("(" + "NoneLower".Translate() + ")", -1f, null);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x0029142C File Offset: 0x0028F62C
		private void DoScenarioListEntry(Rect rect, Scenario scen)
		{
			bool flag = this.curScen == scen;
			Widgets.DrawOptionBackground(rect, flag);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = rect.ContractedBy(4f);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.height = Text.CalcHeight(scen.name, rect3.width);
			Widgets.Label(rect3, scen.name);
			Text.Font = GameFont.Tiny;
			Rect rect4 = rect2;
			rect4.yMin = rect3.yMax;
			Widgets.Label(rect4, scen.GetSummary());
			if (scen.enabled)
			{
				WidgetRow widgetRow = new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenDown, 99999f, 4f);
				if (scen.Category == ScenarioCategory.CustomLocal && widgetRow.ButtonIcon(TexButton.DeleteX, "Delete".Translate(), new Color?(GenUI.SubtleMouseoverColor), null, null, true))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(scen.File.Name), delegate
					{
						scen.File.Delete();
						ScenarioLister.MarkDirty();
					}, true, null));
				}
				if (scen.Category == ScenarioCategory.SteamWorkshop && widgetRow.ButtonIcon(TexButton.DeleteX, "Unsubscribe".Translate(), new Color?(GenUI.SubtleMouseoverColor), null, null, true))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribe".Translate(scen.File.Name), delegate
					{
						scen.enabled = false;
						if (this.curScen == scen)
						{
							this.curScen = null;
							this.EnsureValidSelection();
						}
						Workshop.Unsubscribe(scen);
					}, true, null));
				}
				if (scen.GetPublishedFileId() != PublishedFileId_t.Invalid)
				{
					if (widgetRow.ButtonIcon(ContentSource.SteamWorkshop.GetIcon(), "WorkshopPage".Translate(), null, null, null, true))
					{
						SteamUtility.OpenWorkshopPage(scen.GetPublishedFileId());
					}
					if (scen.CanToUploadToWorkshop())
					{
						widgetRow.Icon(Page_SelectScenario.CanUploadIcon, "CanBeUpdatedOnWorkshop".Translate());
					}
				}
				if (!flag && Widgets.ButtonInvisible(rect, true))
				{
					this.curScen = scen;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x002916BD File Offset: 0x0028F8BD
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
			return true;
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x002916E0 File Offset: 0x0028F8E0
		public static void BeginScenarioConfiguration(Scenario scen, Page originPage)
		{
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = scen;
			Current.Game.Scenario.PreConfigure();
			Find.GameInitData.startedFromEntry = true;
			Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
			if (firstConfigPage == null)
			{
				PageUtility.InitGameStart();
				return;
			}
			originPage.next = firstConfigPage;
			firstConfigPage.prev = originPage;
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x00291752 File Offset: 0x0028F952
		private void EnsureValidSelection()
		{
			if (this.curScen == null || !ScenarioLister.ScenarioIsListedAnywhere(this.curScen))
			{
				this.curScen = ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef).FirstOrDefault<Scenario>();
			}
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x0029177C File Offset: 0x0028F97C
		internal void Notify_ScenarioListChanged()
		{
			PublishedFileId_t selModId = this.curScen.GetPublishedFileId();
			this.curScen = ScenarioLister.AllScenarios().FirstOrDefault((Scenario sc) => sc.GetPublishedFileId() == selModId);
			this.EnsureValidSelection();
		}

		// Token: 0x0600767E RID: 30334 RVA: 0x002917C2 File Offset: 0x0028F9C2
		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.curScen != null && this.curScen.GetPublishedFileId() == pfid)
			{
				this.curScen = null;
			}
			this.EnsureValidSelection();
		}

		// Token: 0x040041CA RID: 16842
		private Scenario curScen;

		// Token: 0x040041CB RID: 16843
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x040041CC RID: 16844
		private const float ScenarioEntryHeight = 62f;

		// Token: 0x040041CD RID: 16845
		private static readonly Texture2D CanUploadIcon = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/CanUpload", true);

		// Token: 0x040041CE RID: 16846
		private Vector2 scenariosScrollPosition = Vector2.zero;

		// Token: 0x040041CF RID: 16847
		private float totalScenarioListHeight;
	}
}
