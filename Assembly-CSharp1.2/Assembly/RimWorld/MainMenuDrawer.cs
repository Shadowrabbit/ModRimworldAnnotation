using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	// Token: 0x02001A8B RID: 6795
	[StaticConstructorOnStartup]
	public static class MainMenuDrawer
	{
		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x060095FC RID: 38396 RVA: 0x000640E5 File Offset: 0x000622E5
		private static UI_BackgroundMain BackgroundMain
		{
			get
			{
				return (UI_BackgroundMain)UIMenuBackgroundManager.background;
			}
		}

		// Token: 0x060095FD RID: 38397 RVA: 0x002B8C0C File Offset: 0x002B6E0C
		public static void Init()
		{
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			MainMenuDrawer.anyMapFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
			foreach (ExpansionDef expansionDef in ModLister.AllExpansions)
			{
				if (expansionDef.Status != ExpansionStatus.NotInstalled && !expansionDef.isCore)
				{
					MainMenuDrawer.BackgroundMain.overrideBGImage = expansionDef.BackgroundImage;
					break;
				}
			}
		}

		// Token: 0x060095FE RID: 38398 RVA: 0x002B8C94 File Offset: 0x002B6E94
		public static void MainMenuOnGUI()
		{
			VersionControl.DrawInfoInCorner();
			Rect rect = new Rect((float)(UI.screenWidth / 2) - MainMenuDrawer.PaneSize.x / 2f, (float)(UI.screenHeight / 2) - MainMenuDrawer.PaneSize.y / 2f + 50f, MainMenuDrawer.PaneSize.x, MainMenuDrawer.PaneSize.y);
			rect.x = (float)UI.screenWidth - rect.width - 30f;
			Rect rect2 = new Rect(0f, rect.y - 30f, (float)UI.screenWidth - 85f, 30f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperRight;
			string text = "MainPageCredit".Translate();
			if (UI.screenWidth < 990)
			{
				Rect position = rect2;
				position.xMin = position.xMax - Text.CalcSize(text).x;
				position.xMin -= 4f;
				position.xMax += 4f;
				GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
				GUI.DrawTexture(position, BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			Widgets.Label(rect2, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			Vector2 vector = MainMenuDrawer.TitleSize;
			if (vector.x > (float)UI.screenWidth)
			{
				vector *= (float)UI.screenWidth / vector.x;
			}
			vector *= 0.5f;
			GUI.DrawTexture(new Rect((float)UI.screenWidth - vector.x - 50f, rect2.y - vector.y, vector.x, vector.y), MainMenuDrawer.TexTitle, ScaleMode.StretchToFill, true);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUI.DrawTexture(new Rect((float)(UI.screenWidth - 8) - MainMenuDrawer.LudeonLogoSize.x, 8f, MainMenuDrawer.LudeonLogoSize.x, MainMenuDrawer.LudeonLogoSize.y), MainMenuDrawer.TexLudeonLogo, ScaleMode.StretchToFill, true);
			GUI.color = Color.white;
			rect.yMin += 17f;
			MainMenuDrawer.DoMainMenuControls(rect, MainMenuDrawer.anyMapFiles);
			MainMenuDrawer.DoTranslationInfoRect(new Rect(8f, 100f, 300f, 400f));
			MainMenuDrawer.DoExpansionIcons();
		}

		// Token: 0x060095FF RID: 38399 RVA: 0x002B8F14 File Offset: 0x002B7114
		public static void DoMainMenuControls(Rect rect, bool anyMapFiles)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 170f, rect.height);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, 145f, rect.height);
			Text.Font = GameFont.Small;
			List<ListableOption> list = new List<ListableOption>();
			if (Current.ProgramState == ProgramState.Entry)
			{
				string label;
				if (!"Tutorial".CanTranslate())
				{
					label = "LearnToPlay".Translate();
				}
				else
				{
					label = "Tutorial".Translate();
				}
				list.Add(new ListableOption(label, delegate()
				{
					MainMenuDrawer.InitLearnToPlay();
				}, null));
				list.Add(new ListableOption("NewColony".Translate(), delegate()
				{
					Find.WindowStack.Add(new Page_SelectScenario());
				}, null));
			}
			if (Current.ProgramState == ProgramState.Playing && !Current.Game.Info.permadeathMode)
			{
				list.Add(new ListableOption("Save".Translate(), delegate()
				{
					MainMenuDrawer.CloseMainTab();
					Find.WindowStack.Add(new Dialog_SaveFileList_Save());
				}, null));
			}
			ListableOption item;
			if (anyMapFiles && (Current.ProgramState != ProgramState.Playing || !Current.Game.Info.permadeathMode))
			{
				item = new ListableOption("LoadGame".Translate(), delegate()
				{
					MainMenuDrawer.CloseMainTab();
					Find.WindowStack.Add(new Dialog_SaveFileList_Load());
				}, null);
				list.Add(item);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				list.Add(new ListableOption("ReviewScenario".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Find.Scenario.GetFullInformationText(), null, null, null, null, Find.Scenario.name, false, null, null));
				}, null));
			}
			item = new ListableOption("Options".Translate(), delegate()
			{
				MainMenuDrawer.CloseMainTab();
				Find.WindowStack.Add(new Dialog_Options());
			}, "MenuButton-Options");
			list.Add(item);
			if (Current.ProgramState == ProgramState.Entry)
			{
				item = new ListableOption("Mods".Translate(), delegate()
				{
					Find.WindowStack.Add(new Page_ModsConfig());
				}, null);
				list.Add(item);
				if (Prefs.DevMode && LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage && LanguageDatabase.activeLanguage.anyError)
				{
					item = new ListableOption("SaveTranslationReport".Translate(), delegate()
					{
						LanguageReportGenerator.SaveTranslationReport();
					}, null);
					list.Add(item);
				}
				item = new ListableOption("Credits".Translate(), delegate()
				{
					Find.WindowStack.Add(new Screen_Credits());
				}, null);
				list.Add(item);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Current.Game.Info.permadeathMode)
				{
					item = new ListableOption("SaveAndQuitToMainMenu".Translate(), delegate()
					{
						LongEventHandler.QueueLongEvent(delegate()
						{
							GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
							MemoryUtility.ClearAllMapsAndWorld();
						}, "Entry", "SavingLongEvent", false, null, false);
					}, null);
					list.Add(item);
					item = new ListableOption("SaveAndQuitToOS".Translate(), delegate()
					{
						LongEventHandler.QueueLongEvent(delegate()
						{
							GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
							LongEventHandler.ExecuteWhenFinished(delegate
							{
								Root.Shutdown();
							});
						}, "SavingLongEvent", false, null, false);
					}, null);
					list.Add(item);
				}
				else
				{
					Action action = delegate()
					{
						if (GameDataSaveLoader.CurrentGameStateIsValuable)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), delegate
							{
								GenScene.GoToMainMenu();
							}, true, null));
							return;
						}
						GenScene.GoToMainMenu();
					};
					item = new ListableOption("QuitToMainMenu".Translate(), action, null);
					list.Add(item);
					Action action2 = delegate()
					{
						if (GameDataSaveLoader.CurrentGameStateIsValuable)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), delegate
							{
								Root.Shutdown();
							}, true, null));
							return;
						}
						Root.Shutdown();
					};
					item = new ListableOption("QuitToOS".Translate(), action2, null);
					list.Add(item);
				}
			}
			else
			{
				item = new ListableOption("QuitToOS".Translate(), delegate()
				{
					Root.Shutdown();
				}, null);
				list.Add(item);
			}
			OptionListingUtility.DrawOptionListing(rect2, list);
			Text.Font = GameFont.Small;
			List<ListableOption> list2 = new List<ListableOption>();
			ListableOption item2 = new ListableOption_WebLink("FictionPrimer".Translate(), "https://rimworldgame.com/backstory", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("LudeonBlog".Translate(), "https://ludeon.com/blog", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("Forums".Translate(), "https://ludeon.com/forums", TexButton.IconForums);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("OfficialWiki".Translate(), "https://rimworldwiki.com", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("TynansTwitter".Translate(), "https://twitter.com/TynanSylvester", TexButton.IconTwitter);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("TynansDesignBook".Translate(), "https://tynansylvester.com/book", TexButton.IconBook);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("HelpTranslate".Translate(), MainMenuDrawer.TranslationsContributeURL, TexButton.IconForums);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("BuySoundtrack".Translate(), "http://www.lasgameaudio.co.uk/#!store/t04fw", TexButton.IconSoundtrack);
			list2.Add(item2);
			float num = OptionListingUtility.DrawOptionListing(rect3, list2);
			GUI.BeginGroup(rect3);
			if (Current.ProgramState == ProgramState.Entry && Widgets.ButtonText(new Rect(0f, num + 10f, rect3.width, 50f), LanguageDatabase.activeLanguage.FriendlyNameNative, true, true, true))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				foreach (LoadedLanguage localLang2 in LanguageDatabase.AllLoadedLanguages)
				{
					LoadedLanguage localLang = localLang2;
					list3.Add(new FloatMenuOption(localLang.DisplayName, delegate()
					{
						LanguageDatabase.SelectLanguage(localLang);
						Prefs.Save();
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			GUI.EndGroup();
			GUI.EndGroup();
		}

		// Token: 0x06009600 RID: 38400 RVA: 0x002B95C4 File Offset: 0x002B77C4
		public static void DoExpansionIcons()
		{
			List<ExpansionDef> allExpansions = ModLister.AllExpansions;
			int num = -1;
			int num2 = 64;
			int num3 = allExpansions.Count((ExpansionDef e) => !e.isCore);
			int num4 = num2 / 2 + num2 * num3 + (num3 - 1) * 8;
			int num5 = num2 + num2 / 2;
			Rect rect = new Rect(8f, (float)(UI.screenHeight - num5 - 8), (float)num4, (float)num5);
			Widgets.DrawWindowBackground(rect);
			GUI.BeginGroup(rect.ContractedBy((rect.height - (float)num2) / 2f));
			float num6 = 0f;
			for (int i = 0; i < allExpansions.Count; i++)
			{
				if (!allExpansions[i].isCore)
				{
					Rect rect2 = new Rect(num6, 0f, (float)num2, (float)num2);
					num6 += (float)num2;
					if (Widgets.ButtonImage(rect2, allExpansions[i].Icon, (allExpansions[i].Status != ExpansionStatus.NotInstalled) ? Color.white : MainMenuDrawer.PurchasedColor, true) && !allExpansions[i].StoreURL.NullOrEmpty())
					{
						SteamUtility.OpenUrl(allExpansions[i].StoreURL);
					}
					GUI.color = Color.white;
					if (Mouse.IsOver(rect2))
					{
						if (allExpansions[i].Status == ExpansionStatus.NotInstalled)
						{
							MainMenuDrawer.BackgroundMain.SetOverlayImage(allExpansions[i].BackgroundImage);
						}
						num = i;
					}
				}
			}
			GUI.EndGroup();
			if (num < 0)
			{
				MainMenuDrawer.BackgroundMain.FadeOut();
				return;
			}
			MainMenuDrawer.DoExpansionInfo(num, rect.yMax);
		}

		// Token: 0x06009601 RID: 38401 RVA: 0x002B9758 File Offset: 0x002B7958
		private static void DoExpansionInfo(int index, float yOffset)
		{
			ExpansionDef expansionDef = ModLister.AllExpansions[index];
			List<Texture2D> previewImages = expansionDef.PreviewImages;
			float num = 350f;
			float num2 = 16f;
			float num3 = 200f;
			float num4 = num3 * 2f + num2 * 2f;
			float num5 = previewImages.NullOrEmpty<Texture2D>() ? 0f : (num3 * 3f + num2 * 2.5f);
			Text.Font = GameFont.Medium;
			float num6 = Text.CalcHeight(expansionDef.label, num - num2 * 2f);
			Text.Font = GameFont.Small;
			string text = "ClickForMoreInfo".Translate();
			float num7 = Text.CalcHeight(expansionDef.description, num - num2 * 2f);
			float num8 = Text.CalcHeight(text, num - num2 * 2f);
			num5 = Mathf.Max(num6 + num8 + num2 + num7 + num2 * 2f, num5);
			Rect rect = new Rect(8f, yOffset - num5, num, num5);
			Widgets.DrawWindowBackground(new Rect(rect.x, rect.y, previewImages.NullOrEmpty<Texture2D>() ? rect.width : (rect.width + num4), rect.height));
			Rect position = rect.ContractedBy(num2);
			GUI.BeginGroup(position);
			float num9 = 0f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, num9, position.width, num6), new GUIContent(" " + expansionDef.label, expansionDef.Icon));
			Text.Font = GameFont.Small;
			num9 += num6;
			GUI.color = Color.grey;
			Widgets.Label(new Rect(0f, num9, position.width, num8), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			num9 += num8 + num2;
			Widgets.Label(new Rect(0f, num9, position.width, position.height - num9), expansionDef.description);
			GUI.EndGroup();
			if (!previewImages.NullOrEmpty<Texture2D>())
			{
				Rect position2 = new Rect(rect.x + rect.width, rect.y, num4, rect.height).ContractedBy(num2);
				GUI.BeginGroup(position2);
				float num10 = 0f;
				float num11 = 0f;
				for (int i = 0; i < previewImages.Count; i++)
				{
					float num12 = num3 - num2 / 2f;
					GUI.DrawTexture(new Rect(num10, num11, num12, num12), previewImages[i]);
					num10 += num3 + num2 / 2f;
					if (num10 >= position2.width)
					{
						num10 = 0f;
						num11 += num3 + num2 / 2f;
					}
				}
				GUI.EndGroup();
			}
		}

		// Token: 0x06009602 RID: 38402 RVA: 0x002B9A10 File Offset: 0x002B7C10
		public static void DoTranslationInfoRect(Rect outRect)
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return;
			}
			Widgets.DrawWindowBackground(outRect);
			Rect rect = outRect.ContractedBy(8f);
			GUI.BeginGroup(rect);
			rect = rect.AtZero();
			Rect rect2 = new Rect(5f, rect.height - 25f, rect.width - 10f, 25f);
			rect.height -= 29f;
			Rect rect3 = new Rect(5f, rect.height - 25f, rect.width - 10f, 25f);
			rect.height -= 29f;
			Rect rect4 = new Rect(5f, rect.height - 25f, rect.width - 10f, 25f);
			rect.height -= 29f;
			string text = "";
			foreach (CreditsEntry creditsEntry in LanguageDatabase.activeLanguage.info.credits)
			{
				CreditRecord_Role creditRecord_Role = creditsEntry as CreditRecord_Role;
				if (creditRecord_Role != null)
				{
					text = text + creditRecord_Role.creditee + "\n";
				}
			}
			text = text.TrimEndNewlines();
			string label = "TranslationThanks".Translate(text) + "\n\n" + "TranslationHowToContribute".Translate();
			Widgets.LabelScrollable(rect, label, ref MainMenuDrawer.translationInfoScrollbarPos, false, false, false);
			if (Widgets.ButtonText(rect4, "LearnMore".Translate(), true, true, true))
			{
				Application.OpenURL(MainMenuDrawer.TranslationsContributeURL);
			}
			if (Widgets.ButtonText(rect3, "SaveTranslationReport".Translate(), true, true, true))
			{
				LanguageReportGenerator.SaveTranslationReport();
			}
			if (Widgets.ButtonText(rect2, "CleanupTranslationFiles".Translate(), true, true, true))
			{
				TranslationFilesCleaner.CleanupTranslationFiles();
			}
			GUI.EndGroup();
		}

		// Token: 0x06009603 RID: 38403 RVA: 0x000640F1 File Offset: 0x000622F1
		private static void DoDevBuildWarningRect(Rect outRect)
		{
			Widgets.DrawWindowBackground(outRect);
			Widgets.Label(outRect.ContractedBy(17f), "DevBuildWarning".Translate());
		}

		// Token: 0x06009604 RID: 38404 RVA: 0x002B9C24 File Offset: 0x002B7E24
		private static void InitLearnToPlay()
		{
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Tutorial.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Tutor, DifficultyDefOf.Easy);
			Page next = Current.Game.Scenario.GetFirstConfigPage().next;
			next.prev = null;
			Find.WindowStack.Add(next);
		}

		// Token: 0x06009605 RID: 38405 RVA: 0x00064113 File Offset: 0x00062313
		private static void CloseMainTab()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
			}
		}

		// Token: 0x04005F8D RID: 24461
		private static bool anyMapFiles;

		// Token: 0x04005F8E RID: 24462
		private static Vector2 translationInfoScrollbarPos;

		// Token: 0x04005F8F RID: 24463
		private const float PlayRectWidth = 170f;

		// Token: 0x04005F90 RID: 24464
		private const float WebRectWidth = 145f;

		// Token: 0x04005F91 RID: 24465
		private const float RightEdgeMargin = 50f;

		// Token: 0x04005F92 RID: 24466
		private static readonly Vector2 PaneSize = new Vector2(450f, 450f);

		// Token: 0x04005F93 RID: 24467
		private static readonly Vector2 TitleSize = new Vector2(1032f, 146f);

		// Token: 0x04005F94 RID: 24468
		private static readonly Texture2D TexTitle = ContentFinder<Texture2D>.Get("UI/HeroArt/GameTitle", true);

		// Token: 0x04005F95 RID: 24469
		private const float TitleShift = 50f;

		// Token: 0x04005F96 RID: 24470
		private static readonly Vector2 LudeonLogoSize = new Vector2(200f, 58f);

		// Token: 0x04005F97 RID: 24471
		private static readonly Texture2D TexLudeonLogo = ContentFinder<Texture2D>.Get("UI/HeroArt/LudeonLogoSmall", true);

		// Token: 0x04005F98 RID: 24472
		private static readonly string TranslationsContributeURL = "https://rimworldgame.com/helptranslate";

		// Token: 0x04005F99 RID: 24473
		private static readonly Color PurchasedColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x04005F9A RID: 24474
		private const int NumColumns = 2;

		// Token: 0x04005F9B RID: 24475
		private const int NumRows = 3;
	}
}
