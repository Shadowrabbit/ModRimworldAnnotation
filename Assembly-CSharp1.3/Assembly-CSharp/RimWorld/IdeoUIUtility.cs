using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000ED9 RID: 3801
	[StaticConstructorOnStartup]
	public static class IdeoUIUtility
	{
		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06005A22 RID: 23074 RVA: 0x001EEBF5 File Offset: 0x001ECDF5
		public static Ideo FallbackSelectedIdeo
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				Ideo ideo;
				if (ofPlayerSilentFail == null)
				{
					ideo = null;
				}
				else
				{
					FactionIdeosTracker ideos = ofPlayerSilentFail.ideos;
					ideo = ((ideos != null) ? ideos.PrimaryIdeo : null);
				}
				return ideo ?? Find.IdeoManager.IdeosInViewOrder.FirstOrFallback(null);
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x001EEC28 File Offset: 0x001ECE28
		public static string ClickToEdit
		{
			get
			{
				return "ClickToEdit".Translate().CapitalizeFirst().Colorize(ColorLibrary.Green);
			}
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x001EEC51 File Offset: 0x001ECE51
		public static bool TutorAllowsInteraction(IdeoEditMode editMode)
		{
			return editMode == IdeoEditMode.Dev || TutorSystem.AllowAction("ConfiguringIdeo");
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x001EEC68 File Offset: 0x001ECE68
		public static void SetSelected(Ideo ideo)
		{
			IdeoUIUtility.selected = ideo;
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x001EEC70 File Offset: 0x001ECE70
		public static void UnselectCurrent()
		{
			IdeoUIUtility.selected = null;
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x001EEC78 File Offset: 0x001ECE78
		public static void DoIdeoIcon(Rect rect, Ideo ideo, bool doTooltip = true, Action extraAction = null)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				if (doTooltip)
				{
					TooltipHandler.TipRegion(rect, ideo.name);
				}
			}
			GUI.color = ideo.Color;
			GUI.DrawTexture(rect, ideo.Icon);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				IdeoUIUtility.OpenIdeoInfo(ideo);
				if (extraAction != null)
				{
					extraAction();
				}
			}
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x001EECE0 File Offset: 0x001ECEE0
		public static void OpenIdeoInfo(Ideo ideo)
		{
			IdeoUIUtility.selected = ideo;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
				return;
			}
			Find.WindowStack.Add(new Dialog_IdeosDuringLanding());
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x001EED10 File Offset: 0x001ECF10
		public static void DoIdeoListAndDetails(Rect fillRect, ref Vector2 scrollPosition_list, ref float scrollViewHeight_list, ref Vector2 scrollPosition_details, ref float scrollViewHeight_details, bool editMode = false, bool showCreateIdeoButton = false, List<Pawn> pawns = null, Ideo onlyEditIdeo = null)
		{
			Text.Font = GameFont.Small;
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(fillRect.width - 250f, 0f, 120f, 24f), "Dev: Show all", ref IdeoUIUtility.showAll, false, null, null, false);
				Widgets.CheckboxLabeled(new Rect(fillRect.width - 125f, 0f, 120f, 24f), "Dev: Edit mode", ref IdeoUIUtility.devEditMode, false, null, null, false);
			}
			else
			{
				IdeoUIUtility.showAll = false;
			}
			Ideo ideo = null;
			Rect fillRect2 = new Rect(fillRect.x, fillRect.y, (float)Mathf.FloorToInt(fillRect.width * 0.25f), fillRect.height);
			IdeoUIUtility.DoIdeoList(fillRect2, ref scrollPosition_list, ref scrollViewHeight_list, out ideo, showCreateIdeoButton, pawns);
			if (IdeoUIUtility.selected != null && !Find.IdeoManager.IdeosListForReading.Contains(IdeoUIUtility.selected))
			{
				IdeoUIUtility.selected = null;
			}
			Ideo ideo2;
			if ((ideo2 = IdeoUIUtility.selected) == null)
			{
				ideo2 = (ideo ?? (showCreateIdeoButton ? null : IdeoUIUtility.FallbackSelectedIdeo));
			}
			Ideo ideo3 = ideo2;
			Rect rect = new Rect(fillRect2.xMax, fillRect.y, fillRect.width - fillRect2.width, fillRect.height);
			if (ideo3 != null)
			{
				bool editMode2 = editMode && (onlyEditIdeo == null || onlyEditIdeo == ideo3);
				IdeoUIUtility.DoIdeoDetails(rect.ContractedBy(17f), ideo3, ref scrollPosition_details, ref scrollViewHeight_details, editMode2);
				return;
			}
			if (showCreateIdeoButton)
			{
				IdeoUIUtility.DoInitialIdeoSelection(rect.ContractedBy(17f));
			}
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x001EEE88 File Offset: 0x001ED088
		private static void DoInitialIdeoSelection(Rect fillRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect = new Rect(fillRect.center.x - IdeoUIUtility.ButtonSize.x, fillRect.center.y, IdeoUIUtility.ButtonSize.x * 2f, IdeoUIUtility.ButtonSize.y * 2f);
			if (Widgets.ButtonText(rect, "CreateCustomIdeoligion".Translate() + "...", true, true, true))
			{
				Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
				if (page_ConfigureIdeo != null)
				{
					page_ConfigureIdeo.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
				}
			}
			Widgets.Label(new Rect(fillRect.x + 26f, rect.y - Text.LineHeight, fillRect.width - 52f, Text.LineHeight), "SelectAnIdeo".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x001EEF68 File Offset: 0x001ED168
		public static void DoIdeoList(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight, out Ideo mouseoverIdeo, bool showCreateNewButton, List<Pawn> pawns = null)
		{
			mouseoverIdeo = null;
			GUI.BeginGroup(fillRect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = fillRect.AtZero();
			outRect.yMin += 17f;
			Rect rect = new Rect(0f, 0f, fillRect.width - 16f, scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect, true);
			float num = 0f;
			int num2 = 0;
			if (showCreateNewButton)
			{
				if (Widgets.ButtonText(new Rect(0f, num + 5f, rect.width, 36f), "CreateCustom".Translate() + "...", true, true, true))
				{
					Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
					if (page_ConfigureIdeo != null)
					{
						page_ConfigureIdeo.SelectOrMakeNewIdeo(null);
						IdeoUIUtility.selected = page_ConfigureIdeo.ideo;
					}
				}
				num += 46f;
				num2++;
			}
			foreach (Ideo ideo in Find.IdeoManager.IdeosInViewOrder)
			{
				if (pawns != null)
				{
					IdeoUIUtility.tmpPawns.Clear();
					for (int i = 0; i < pawns.Count; i++)
					{
						if (pawns[i].Ideo == ideo)
						{
							IdeoUIUtility.tmpPawns.Add(pawns[i]);
						}
					}
				}
				bool flag;
				IdeoUIUtility.DrawIdeoRow(ideo, ref num, rect, out flag, num2, IdeoUIUtility.tmpPawns);
				if (flag)
				{
					mouseoverIdeo = ideo;
				}
				num2++;
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			IdeoUIUtility.tmpPawns.Clear();
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x001EF11C File Offset: 0x001ED31C
		private static void DrawIdeoRow(Ideo ideo, ref float curY, Rect fillRect, out bool mouseover, int row, List<Pawn> pawns = null)
		{
			Rect rect = new Rect(44f, curY, 0f, 46f);
			rect.width = fillRect.width - rect.x;
			Rect rect2 = new Rect(7f, curY + 7f, 30f, 30f);
			float height = (pawns.Count > 0) ? 92f : 46f;
			Rect rect3 = new Rect(0f, curY, fillRect.width, height);
			if (row % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect3);
			}
			if (ideo == IdeoUIUtility.selected)
			{
				Widgets.DrawHighlightSelected(rect3);
			}
			else
			{
				Widgets.DrawHighlightIfMouseover(rect3);
			}
			Text.Font = GameFont.Small;
			IdeoUIUtility.DoIdeoIcon(rect2, ideo, true, null);
			Rect rect4 = rect;
			rect4.y += 3f;
			Widgets.Label(rect4, ideo.name.Truncate(rect4.width, null));
			float num = rect.y + rect.height / 2f - 2f;
			IdeoUIUtility.DoFactionIcons(ideo, rect.x, ref num, rect.width, 18f, 18f, null);
			curY += 46f;
			if (pawns != null && pawns.Count > 0)
			{
				num = curY;
				IdeoUIUtility.DoPawnIcons(pawns, rect2.x, ref num, rect.width, 22f);
				curY += 46f;
			}
			mouseover = Mouse.IsOver(rect3);
			if (Widgets.ButtonInvisible(rect3, true) && TutorSystem.AllowAction("ConfiguringIdeo"))
			{
				IdeoUIUtility.selected = ideo;
				SoundDefOf.DialogBoxAppear.PlayOneShotOnCamera(null);
				Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
				if (page_ConfigureIdeo != null)
				{
					page_ConfigureIdeo.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
				}
			}
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x001EF2D4 File Offset: 0x001ED4D4
		public static void DoIdeoDetails(Rect inRect, Ideo ideo, ref Vector2 scrollPosition, ref float viewHeight, bool editMode = false)
		{
			if (!ModLister.CheckIdeology("Ideoligion"))
			{
				return;
			}
			IdeoEditMode ideoEditMode;
			if (IdeoUIUtility.devEditMode)
			{
				ideoEditMode = IdeoEditMode.Dev;
			}
			else if (editMode)
			{
				ideoEditMode = IdeoEditMode.GameStart;
			}
			else
			{
				ideoEditMode = IdeoEditMode.None;
			}
			if (ideo.createdFromNoExpansionGame && ideoEditMode == IdeoEditMode.None && !ideo.memes.Any<MemeDef>())
			{
				Widgets.NoneLabelCenteredVertically(inRect, null);
				return;
			}
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			float b = 0f;
			IdeoUIUtility.DoName(ref num, viewRect.width, ideo, ideoEditMode);
			IdeoUIUtility.DoFactions(ref b, viewRect.width, ideo, ideoEditMode);
			num = Mathf.Max(num, b);
			IdeoUIUtility.DoMemes(ref num, viewRect.width, ideo, ideoEditMode);
			IdeoUIUtility.DoDescription(ref num, viewRect.width, ideo, ideoEditMode);
			if (ideo.foundation != null)
			{
				IdeoUIUtility.DoFoundationInfo(ref num, viewRect.width, ideo, ideoEditMode);
			}
			IdeoUIUtility.tmpMouseOverPrecept = null;
			IdeoUIUtility.DoPrecepts(ref num, viewRect.width, ideo, ideoEditMode);
			IdeoUIUtility.DoAppearanceItems(ideo, ideoEditMode, ref num, viewRect.width);
			if (Prefs.DevMode)
			{
				IdeoUIUtility.DoDebugButtons(ref num, viewRect.width, ideo);
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num + inRect.height / 2f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x001EF418 File Offset: 0x001ED618
		private static void DoName(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			IdeoUIUtility.<>c__DisplayClass56_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass56_0();
			CS$<>8__locals1.ideo = ideo;
			Text.Font = GameFont.Medium;
			float num = Text.CalcSize(CS$<>8__locals1.ideo.name).x;
			Text.Font = GameFont.Small;
			string text = CS$<>8__locals1.ideo.adjective.CapitalizeFirst() + " / " + CS$<>8__locals1.ideo.memberName.CapitalizeFirst();
			num = Mathf.Max(num, Text.CalcSize(text).x);
			num = Mathf.Min(num, 300f);
			float num2 = 87f + num;
			float x3 = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num3 = width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num4 = (width - num2) / 2f;
			MemeDef structureMeme = CS$<>8__locals1.ideo.StructureMeme;
			Rect rect = new Rect(x3, curY, 35f, 35f);
			if (structureMeme != null)
			{
				GUI.DrawTexture(rect, structureMeme.Icon);
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TaggedString str = string.Concat(new string[]
					{
						("StructureMeme".Translate() + ": " + structureMeme.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
						"\n\n",
						structureMeme.description,
						"\n\n",
						"StructureMemeTip".Translate().Resolve().Colorize(ColoredText.SubtleGrayColor),
						(editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty
					});
					TooltipHandler.TipRegion(rect, str);
				}
				if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					Find.WindowStack.Add(new Dialog_ChooseMemes(CS$<>8__locals1.ideo, MemeCategory.Structure, false, null));
				}
			}
			Rect rect2 = new Rect(rect.xMax + 4f, curY, 35f, 35f);
			if (CS$<>8__locals1.ideo.culture != null)
			{
				GUI.color = CS$<>8__locals1.ideo.culture.iconColor;
				GUI.DrawTexture(rect2, CS$<>8__locals1.ideo.culture.Icon);
				GUI.color = Color.white;
				if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect2, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (CultureDef culture2 in from c in DefDatabase<CultureDef>.AllDefs
					orderby base.<DoName>g__CultureAllowed|0(c) descending, c.label
					select c)
					{
						CultureDef culture = culture2;
						string text2 = culture.LabelCap;
						Action action = null;
						if (!CS$<>8__locals1.<DoName>g__CultureAllowed|0(culture))
						{
							text2 += " (" + "NotAllowedForFaction".Translate() + ")";
						}
						else
						{
							action = delegate()
							{
								if (CS$<>8__locals1.ideo.culture != culture)
								{
									CS$<>8__locals1.ideo.culture = culture;
									CS$<>8__locals1.ideo.foundation.RandomizeStyles();
									CS$<>8__locals1.ideo.style.RecalculateAvailableStyleItems();
									IdeoFoundation_Deity ideoFoundation_Deity;
									if ((ideoFoundation_Deity = (CS$<>8__locals1.ideo.foundation as IdeoFoundation_Deity)) != null)
									{
										ideoFoundation_Deity.GenerateDeities();
									}
									CS$<>8__locals1.ideo.RegenerateDescription(true);
								}
							};
						}
						list.Add(new FloatMenuOption(text2, action, culture.Icon, culture.iconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list, "ChooseCulture".Translate(), false));
				}
			}
			if (CS$<>8__locals1.ideo.culture != null && Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TaggedString str2 = string.Concat(new string[]
				{
					("Culture".Translate() + ": " + CS$<>8__locals1.ideo.culture.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
					"\n\n",
					CS$<>8__locals1.ideo.culture.description,
					"\n\n",
					"CultureTip".Translate().Resolve().Colorize(ColoredText.SubtleGrayColor),
					(editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty
				});
				TooltipHandler.TipRegion(rect2, str2);
			}
			IdeoUIUtility.tmpStyleCategories.Clear();
			IdeoUIUtility.tmpStyleCategories.AddRange(CS$<>8__locals1.ideo.thingStyleCategories);
			float num5 = rect.xMin;
			for (int i = 0; i < 3; i++)
			{
				Rect rect3 = new Rect(num5, rect.yMax + 10f, 28f, 28f);
				int index = i;
				if (i < IdeoUIUtility.tmpStyleCategories.Count)
				{
					GUI.DrawTexture(rect3.ContractedBy(4f), IdeoUIUtility.tmpStyleCategories[i].category.Icon);
					if (Mouse.IsOver(rect3))
					{
						Widgets.DrawHighlight(rect3);
						TooltipHandler.TipRegion(rect3, string.Concat(new string[]
						{
							("StyleCategory".Translate() + ": " + IdeoUIUtility.tmpStyleCategories[i].category.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
							"\n\n",
							"StyleCategoryDescStyleDominance".Translate(CS$<>8__locals1.ideo.Named("IDEO")).Resolve(),
							"\n\n",
							CS$<>8__locals1.<DoName>g__GetStyleCategoryDescription|4(IdeoUIUtility.tmpStyleCategories[i].category),
							(editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty,
							"\n\n",
							"StyleCategoryDescription".Translate(CS$<>8__locals1.ideo.Named("IDEO")).Resolve().Colorize(ColoredText.SubtleGrayColor)
						}));
					}
					if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect3, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
					{
						List<FloatMenuOption> list2 = new List<FloatMenuOption>();
						list2.Add(new FloatMenuOption("Remove".Translate(), delegate()
						{
							CS$<>8__locals1.ideo.thingStyleCategories.RemoveAt(index);
							CS$<>8__locals1.ideo.SortStyleCategories();
							CS$<>8__locals1.ideo.style.ResetStylesForThingDef();
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
						Func<StyleCategoryDef, bool> predicate;
						if ((predicate = CS$<>8__locals1.<>9__9) == null)
						{
							predicate = (CS$<>8__locals1.<>9__9 = ((StyleCategoryDef x) => !CS$<>8__locals1.ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority y) => y.category == x)));
						}
						using (IEnumerator<StyleCategoryDef> enumerator2 = allDefs.Where(predicate).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								StyleCategoryDef s = enumerator2.Current;
								list2.Add(new FloatMenuOption(s.LabelCap, delegate()
								{
									CS$<>8__locals1.ideo.thingStyleCategories.RemoveAt(index);
									CS$<>8__locals1.ideo.thingStyleCategories.Insert(index, new ThingStyleCategoryWithPriority(s, (float)(3 - index)));
									CS$<>8__locals1.ideo.SortStyleCategories();
									CS$<>8__locals1.ideo.style.ResetStylesForThingDef();
								}, s.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
						Find.WindowStack.Add(new FloatMenu(list2));
					}
				}
				else if (editMode != IdeoEditMode.None)
				{
					GUI.DrawTexture(rect3.ContractedBy(4f), IdeoUIUtility.PlusTex);
					if (Mouse.IsOver(rect3))
					{
						Widgets.DrawHighlight(rect3);
						TooltipHandler.TipRegion(rect3, "AddStyleCategory".Translate() + "\n\n" + "StyleCategoryDescription".Translate(CS$<>8__locals1.ideo.Named("IDEO")).Resolve().Colorize(ColoredText.SubtleGrayColor) + "\n\n" + IdeoUIUtility.ClickToEdit);
					}
					if (!Widgets.ButtonInvisible(rect3, true) || !IdeoUIUtility.TutorAllowsInteraction(editMode))
					{
						break;
					}
					List<FloatMenuOption> list3 = new List<FloatMenuOption>();
					IEnumerable<StyleCategoryDef> allDefs2 = DefDatabase<StyleCategoryDef>.AllDefs;
					Func<StyleCategoryDef, bool> predicate2;
					if ((predicate2 = CS$<>8__locals1.<>9__12) == null)
					{
						predicate2 = (CS$<>8__locals1.<>9__12 = ((StyleCategoryDef x) => !CS$<>8__locals1.ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority y) => y.category == x)));
					}
					using (IEnumerator<StyleCategoryDef> enumerator2 = allDefs2.Where(predicate2).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							StyleCategoryDef s = enumerator2.Current;
							list3.Add(new FloatMenuOption(s.LabelCap, delegate()
							{
								CS$<>8__locals1.ideo.thingStyleCategories.Add(new ThingStyleCategoryWithPriority(s, (float)(3 - index)));
								CS$<>8__locals1.ideo.SortStyleCategories();
								CS$<>8__locals1.ideo.style.ResetStylesForThingDef();
							}, s.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
					if (list3.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list3));
						break;
					}
					break;
				}
				num5 += 28f;
			}
			if (Current.ProgramState == ProgramState.Entry)
			{
				if (IdeoUIUtility.currentRitualAmbiencePreviewIdeo != CS$<>8__locals1.ideo)
				{
					IdeoUIUtility.currentRitualAmbiencePreview = null;
				}
				if (CS$<>8__locals1.ideo.SoundOngoingRitual != null)
				{
					num5 += 10f;
					Rect rect4 = new Rect(num5 + 28f + 10f, rect.yMax + 10f, 32f, 32f);
					TooltipHandler.TipRegion(rect4, () => "TipPreviewRitualAmbienceSound".Translate(), 45834593);
					if (Widgets.ButtonImage(rect4, (IdeoUIUtility.currentRitualAmbiencePreview != null) ? IdeoUIUtility.PreviewRitualAmbienceOn : IdeoUIUtility.PreviewRitualAmbience, true))
					{
						if (IdeoUIUtility.currentRitualAmbiencePreview == null)
						{
							SoundInfo info = SoundInfo.OnCamera(MaintenanceType.PerFrame);
							info.forcedPlayOnCamera = true;
							info.testPlay = true;
							IdeoUIUtility.currentRitualAmbiencePreview = CS$<>8__locals1.ideo.SoundOngoingRitual.TrySpawnSustainer(info);
							IdeoUIUtility.currentRitualAmbiencePreviewIdeo = CS$<>8__locals1.ideo;
						}
						else
						{
							IdeoUIUtility.currentRitualAmbiencePreview = null;
						}
					}
				}
				if (IdeoUIUtility.currentRitualAmbiencePreview != null)
				{
					IdeoUIUtility.currentRitualAmbiencePreview.Maintain();
					Find.MusicManagerEntry.MaintainSilence();
				}
			}
			Rect rect5 = new Rect(num4, curY, 70f, 70f);
			GUI.color = CS$<>8__locals1.ideo.Color;
			GUI.DrawTexture(rect5, CS$<>8__locals1.ideo.Icon);
			GUI.color = Color.white;
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect5, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				Find.WindowStack.Add(new Dialog_ChooseIdeoSymbols(CS$<>8__locals1.ideo));
			}
			float num6 = curY;
			Rect rect6 = new Rect(rect5.xMax + 17f, num6 + 5f, 999f, 30f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect6, CS$<>8__locals1.ideo.name.Truncate(300f, null));
			Text.Font = GameFont.Small;
			num6 += 39f;
			GUI.color = new Color(0.65f, 0.65f, 0.65f);
			Widgets.Label(new Rect(rect5.xMax + 17f, num6, Text.CalcSize(text).x, 30f), text.Truncate(300f, null));
			num6 += 18f;
			GUI.color = Color.white;
			Rect rect7 = new Rect(rect5.xMin, rect5.yMin, num + 70f + 17f + 4f, 70f);
			if (Mouse.IsOver(rect7))
			{
				Widgets.DrawHighlight(rect7);
				string str3 = string.Concat(new string[]
				{
					("Name".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					CS$<>8__locals1.ideo.name,
					"\n",
					("Adjective".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					CS$<>8__locals1.ideo.adjective.CapitalizeFirst(),
					"\n",
					("IdeoMembers".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					CS$<>8__locals1.ideo.memberName.CapitalizeFirst(),
					"\n",
					("LeaderTitle".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					CS$<>8__locals1.ideo.leaderTitleMale.CapitalizeFirst(),
					"\n",
					("WorshipRoom".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					CS$<>8__locals1.ideo.WorshipRoomLabel.CapitalizeFirst(),
					"\n",
					(CS$<>8__locals1.ideo.leaderTitleFemale != CS$<>8__locals1.ideo.leaderTitleMale) ? (" (" + CS$<>8__locals1.ideo.leaderTitleFemale.CapitalizeFirst() + ")") : "",
					(editMode != IdeoEditMode.None) ? ("\n" + IdeoUIUtility.ClickToEdit) : string.Empty
				});
				TooltipHandler.TipRegion(rect7, str3);
			}
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect7, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				Find.WindowStack.Add(new Dialog_ChooseIdeoSymbols(CS$<>8__locals1.ideo));
			}
			if (editMode != IdeoEditMode.None)
			{
				float x2 = Mathf.Max(num3 - IdeoUIUtility.AddPreceptButtonSize.x, num4 + num2 + 10f);
				Rect rect8 = new Rect(x2, curY, IdeoUIUtility.AddPreceptButtonSize.x, IdeoUIUtility.AddPreceptButtonSize.y);
				TooltipHandler.TipRegion(rect8, "RandomizeSymbolsTooltip".Translate());
				if (Widgets.ButtonText(rect8, "RandomizeSymbols".Translate(), true, true, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					CS$<>8__locals1.ideo.foundation.RandomizePlace();
					CS$<>8__locals1.ideo.foundation.GenerateTextSymbols();
					CS$<>8__locals1.ideo.foundation.GenerateLeaderTitle();
					CS$<>8__locals1.ideo.foundation.RandomizeIcon();
					CS$<>8__locals1.ideo.RegenerateAllPreceptNames();
					CS$<>8__locals1.ideo.RegenerateDescription(true);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				GUI.color = ColoredText.SubtleGrayColor;
				Text.Anchor = TextAnchor.UpperCenter;
				string text3 = "ClickAnyElementToEditIt".Translate();
				float height = Text.CalcHeight(text3, IdeoUIUtility.AddPreceptButtonSize.x);
				Widgets.Label(new Rect(x2, curY + IdeoUIUtility.AddPreceptButtonSize.y + 4f, IdeoUIUtility.AddPreceptButtonSize.x, height), text3);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			curY += 70f;
			curY += 34f;
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x001F0314 File Offset: 0x001EE514
		private static void DoFactions(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			if (Find.World == null)
			{
				return;
			}
			if (editMode == IdeoEditMode.GameStart)
			{
				return;
			}
			float startX = width - 150f;
			IdeoUIUtility.DoFactionIcons(ideo, startX, ref curY, 9999f, 30f, 20f, "IdeoligionOf".Translate() + ":");
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x001F0368 File Offset: 0x001EE568
		private static float DoFactionIcons(Ideo ideo, float startX, ref float curY, float width, float iconSize, float iconSizeMinor, string label = null)
		{
			IdeoUIUtility.tmpFactions.Clear();
			foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
			{
				if ((!faction.Hidden || IdeoUIUtility.showAll) && faction.ideos != null && (faction.ideos.IsPrimary(ideo) || faction.ideos.IsMinor(ideo)))
				{
					IdeoUIUtility.tmpFactions.Add(faction);
				}
			}
			if (IdeoUIUtility.tmpFactions.Any<Faction>())
			{
				IdeoUIUtility.tmpFactions.SortByDescending((Faction x) => x.ideos.IsPrimary(ideo));
				if (!label.NullOrEmpty())
				{
					Widgets.Label(startX, ref curY, width, label, default(TipSignal));
				}
				float num = startX;
				for (int i = 0; i < IdeoUIUtility.tmpFactions.Count; i++)
				{
					float num2 = IdeoUIUtility.tmpFactions[i].ideos.IsPrimary(ideo) ? iconSize : iconSizeMinor;
					if (num + num2 > width - startX)
					{
						num = startX;
						curY += num2 + 4f;
					}
					FactionUIUtility.DrawFactionIconWithTooltip(new Rect(num, curY, num2, num2), IdeoUIUtility.tmpFactions[i]);
					num += num2 + 4f;
				}
				curY += iconSize + 17f;
				return num;
			}
			return startX;
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x001F04E4 File Offset: 0x001EE6E4
		private static void DoPawnIcons(List<Pawn> pawns, float startX, ref float curY, float width, float iconSize)
		{
			float num = startX;
			for (int i = 0; i < pawns.Count; i++)
			{
				Rect rect = new Rect(num, curY, iconSize, iconSize);
				Widgets.DrawHighlightIfMouseover(rect);
				Widgets.ThingIcon(rect, pawns[i], 1f, null);
				TooltipHandler.TipRegion(rect, pawns[i].LabelCap);
				num += iconSize + 4f;
			}
			curY += iconSize + 17f;
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x001F0560 File Offset: 0x001EE760
		private static void DoDescription(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			Rect rect = new Rect(num, curY, Text.LineHeight, Text.LineHeight);
			if (Widgets.ButtonImage(rect, ideo.descriptionLocked ? IdeoUIUtility.LockedTex : IdeoUIUtility.UnlockedTex, true))
			{
				ideo.descriptionLocked = !ideo.descriptionLocked;
				if (ideo.descriptionLocked)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				string str = "LockButtonDesc".Translate() + "\n\n" + (ideo.descriptionLocked ? "LockInOn" : "LockInOff").Translate();
				TooltipHandler.TipRegion(rect, str);
			}
			float yMin = curY;
			Widgets.Label(num + Text.LineHeight + 4f, ref curY, width, "CoreNarrative".Translate(), default(TipSignal));
			float width2 = width - num - 80f;
			int num2 = (int)Mathf.Max(70f, Text.CalcHeight(ideo.description, width2));
			Rect rect2 = new Rect(num + 40f, curY, width2, (float)num2);
			Widgets.Label(rect2, ideo.description);
			if (editMode != IdeoEditMode.None)
			{
				Rect rect3 = rect2;
				rect3.yMin = yMin;
				rect3.xMin = num + Text.LineHeight + 4f;
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					string str2 = "CoreNarrativeDesc".Translate() + "\n\n" + IdeoUIUtility.ClickToEdit;
					TooltipHandler.TipRegion(rect2, str2);
				}
				if (Widgets.ButtonInvisible(rect3, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					Find.WindowStack.Add(new Dialog_EditIdeoDescription(ideo));
				}
			}
			curY += (float)num2;
			curY += 17f;
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x001F0755 File Offset: 0x001EE955
		private static void DoFoundationInfo(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			ideo.foundation.DoInfo(ref curY, width, editMode);
			curY += 17f;
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x001F0770 File Offset: 0x001EE970
		private static void DoMemes(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num2 = curY;
			Widgets.Label(num, ref num2, width, "Memes".Translate(), default(TipSignal));
			IdeoUIUtility.tmpMemesToShow.Clear();
			for (int i = 0; i < ideo.memes.Count; i++)
			{
				if (ideo.memes[i].category != MemeCategory.Structure)
				{
					IdeoUIUtility.tmpMemesToShow.Add(ideo.memes[i]);
				}
			}
			float num3 = (float)IdeoUIUtility.tmpMemesToShow.Count * IdeoUIUtility.MemeBoxSize.x + (float)((IdeoUIUtility.tmpMemesToShow.Count - 1) * 8);
			float num4 = (width - num3) / 2f;
			if (editMode == IdeoEditMode.GameStart && IdeoUIUtility.tmpMemesToShow.Any<MemeDef>())
			{
				IdeoUIUtility.DrawKnowledgeTip(ConceptDefOf.EditingMemes, curY + Text.LineHeight, num);
			}
			IdeoUIUtility.tmpMouseOverMeme = null;
			for (int j = 0; j < IdeoUIUtility.tmpMemesToShow.Count; j++)
			{
				Rect rect = new Rect(num4 + (float)j * IdeoUIUtility.MemeBoxSize.x + (float)(j * 8), curY, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y);
				if (editMode == IdeoEditMode.GameStart)
				{
					UIHighlighter.HighlightOpportunity(rect, "MemeBox");
				}
				IdeoUIUtility.DoMeme(rect, IdeoUIUtility.tmpMemesToShow[j], ideo, editMode, true);
			}
			curY += IdeoUIUtility.MemeBoxSize.y;
			curY += 17f;
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x001F08F8 File Offset: 0x001EEAF8
		private static void DrawKnowledgeTip(ConceptDef conceptDef, float curY, float labelAlignOffset)
		{
			if (Find.World == null && TutorSystem.AdaptiveTrainingEnabled && !PlayerKnowledgeDatabase.IsComplete(conceptDef))
			{
				Rect rect = new Rect(new Rect(0f, curY + Text.LineHeight, labelAlignOffset / 2f + 26f, Text.LineHeight * 2f));
				GUI.color = IdeoUIUtility.TutorArrowColor;
				GUI.DrawTexture(new Rect(rect.xMax - 1f, rect.y, rect.height, rect.height), IdeoUIUtility.ArrowTex);
				GUI.color = Color.white;
				Widgets.DrawWindowBackgroundTutor(rect);
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = Color.white;
				Widgets.Label(rect.ContractedBy(2f), IdeoUIUtility.ClickToEdit);
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x001F09CC File Offset: 0x001EEBCC
		public static void DoMeme(Rect memeBox, MemeDef meme, Ideo ideo = null, IdeoEditMode editMode = IdeoEditMode.None, bool drawHighlight = true)
		{
			if (drawHighlight)
			{
				Widgets.DrawLightHighlight(memeBox);
			}
			if (Mouse.IsOver(memeBox))
			{
				Widgets.DrawHighlight(memeBox);
				TooltipHandler.TipRegion(memeBox, IdeoUIUtility.GetMemeTip(meme, ideo) + ((editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
				IdeoUIUtility.tmpMouseOverMeme = meme;
			}
			else if (IdeoUIUtility.tmpMouseOverPrecept != null && IdeoUIUtility.IsPreceptRelatedForUI(meme, IdeoUIUtility.tmpMouseOverPrecept))
			{
				Widgets.DrawHighlight(memeBox);
			}
			GUI.DrawTexture(new Rect(memeBox.x + (memeBox.width - 80f) / 2f, memeBox.y + 8f, 80f, 80f), meme.Icon);
			if (meme.impact > 0)
			{
				Rect rect = memeBox.RightPartPixels(18f).TopPartPixels(18f);
				rect.x -= 4f;
				rect.y += 4f;
				IdeoImpactUtility.DrawImpactIcon(rect, meme.impact);
			}
			Rect rect2 = new Rect(memeBox.x, memeBox.yMax - Text.LineHeight * 2f + 4f, memeBox.width, Text.LineHeight * 2f - 4f).ContractedBy(10f, 0f);
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, meme.LabelCap);
			GenUI.ResetLabelAlign();
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(memeBox, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EditingMemes, KnowledgeAmount.Total);
				Find.WindowStack.Add(new Dialog_ChooseMemes(ideo, MemeCategory.Normal, false, null));
			}
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x001F0B6C File Offset: 0x001EED6C
		private static string GetMemeTip(MemeDef meme, Ideo ideo = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(meme.LabelCap.Colorize(ColoredText.TipSectionTitleColor));
			if (meme.impact > 0)
			{
				stringBuilder.AppendLine(("IdeoImpact".Translate() + ": " + IdeoImpactUtility.MemeImpactLabel(meme.impact).CapitalizeFirst()).Colorize(ColoredText.ImpactColor));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(meme.description);
			List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
			if (!meme.requireOne.NullOrEmpty<List<PreceptDef>>())
			{
				IdeoUIUtility.tmpRequiredPrecepts.Clear();
				for (int i = 0; i < meme.requireOne.Count; i++)
				{
					List<PreceptDef> list = meme.requireOne[i];
					if (list.Count == 1)
					{
						IdeoUIUtility.tmpRequiredPrecepts.Add(list[0]);
					}
				}
				if (IdeoUIUtility.tmpRequiredPrecepts.Count > 0)
				{
					stringBuilder.AppendInNewLine(("RequiredPrecepts".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
					stringBuilder.AppendLine();
					for (int j = 0; j < IdeoUIUtility.tmpRequiredPrecepts.Count; j++)
					{
						stringBuilder.AppendLine("- " + IdeoUIUtility.tmpRequiredPrecepts[j].issue.LabelCap + ": " + IdeoUIUtility.tmpRequiredPrecepts[j].LabelCap);
					}
					IdeoUIUtility.tmpRequiredPrecepts.Clear();
				}
				for (int k = 0; k < meme.requireOne.Count; k++)
				{
					List<PreceptDef> list2 = meme.requireOne[k];
					if (list2.Count > 1)
					{
						stringBuilder.AppendInNewLine(("RequiresOnePrecept".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
						stringBuilder.AppendLine();
						for (int l = 0; l < list2.Count; l++)
						{
							stringBuilder.AppendLine("- " + list2[l].issue.LabelCap + ": " + list2[l].LabelCap);
						}
					}
				}
			}
			if (meme.selectOneOrNone != null && meme.selectOneOrNone.preceptThingPairs.Count > 0)
			{
				stringBuilder.AppendInNewLine(("ChanceToHavePrecept".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				List<PreceptThingPair> preceptThingPairs = meme.selectOneOrNone.preceptThingPairs;
				for (int m = 0; m < preceptThingPairs.Count; m++)
				{
					stringBuilder.AppendLine("- " + preceptThingPairs[m].thing.LabelCap + ": " + preceptThingPairs[m].precept.LabelCap);
				}
			}
			int num = 0;
			for (int n = 0; n < allDefsListForReading.Count; n++)
			{
				if (allDefsListForReading[n].conflictingMemes.Contains(meme))
				{
					num++;
				}
			}
			if (num > 3)
			{
				stringBuilder.AppendInNewLine("DisablesPreceptsCount".Translate(num).Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
			}
			else if (num > 0)
			{
				stringBuilder.AppendInNewLine(("DisablesPrecepts".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				for (int num2 = 0; num2 < allDefsListForReading.Count; num2++)
				{
					if (allDefsListForReading[num2].conflictingMemes.Contains(meme))
					{
						stringBuilder.AppendLine("- " + allDefsListForReading[num2].issue.LabelCap + ": " + allDefsListForReading[num2].LabelCap);
					}
				}
			}
			if (!meme.requiredRituals.NullOrEmpty<RequiredRitualAndBuilding>())
			{
				stringBuilder.AppendInNewLine(("RequiredRituals".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				for (int num3 = 0; num3 < meme.requiredRituals.Count; num3++)
				{
					stringBuilder.AppendLine("- " + (meme.requiredRituals[num3].pattern.shortDescOverride.CapitalizeFirst() ?? meme.requiredRituals[num3].precept.LabelCap));
				}
			}
			List<string> list3 = meme.UnlockedRoles(ideo);
			if (!list3.NullOrEmpty<string>())
			{
				stringBuilder.AppendInNewLine(("MemeUnlocksRoles".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendInNewLine(list3.ToLineList("  - "));
				stringBuilder.AppendLine();
			}
			List<string> list4 = meme.UnlockedRituals();
			if (!list4.NullOrEmpty<string>())
			{
				stringBuilder.AppendInNewLine(("MemeUnlocksRituals".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendInNewLine(list4.ToLineList("  - "));
				stringBuilder.AppendLine();
			}
			if (!meme.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				stringBuilder.AppendInNewLine(("Styles".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				for (int num4 = 0; num4 < meme.thingStyleCategories.Count; num4++)
				{
					stringBuilder.AppendLine("- " + meme.thingStyleCategories[num4].category.LabelCap);
				}
			}
			IEnumerable<RecipeDef> source = from r in DefDatabase<RecipeDef>.AllDefsListForReading
			where r.memePrerequisitesAny != null && r.memePrerequisitesAny.Contains(meme)
			select r;
			if (source.Any<RecipeDef>())
			{
				stringBuilder.AppendInNewLine(("UnlockedRecipes".Translate() + ":").CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor));
				List<ThingDef> defs = (from r in source
				select r.ProducedThingDef).Distinct<ThingDef>().ToList<ThingDef>();
				stringBuilder.AppendLine(IdeoUIUtility.SortedLabelCaps<ThingDef>(defs, "\n  - "));
			}
			if (!meme.addDesignators.NullOrEmpty<BuildableDef>() || !meme.addDesignatorGroups.NullOrEmpty<DesignatorDropdownGroupDef>())
			{
				stringBuilder.AppendInNewLine(("IdeoMakesBuildingBuildable".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine(IdeoUIUtility.SortedLabelCaps<BuildableDef, DesignatorDropdownGroupDef>(meme.addDesignators, meme.addDesignatorGroups, "\n  - "));
			}
			if (meme.startingResearchProjects.Any<ResearchProjectDef>())
			{
				stringBuilder.AppendInNewLine(("IdeoStartWithResearch".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine(IdeoUIUtility.SortedLabelCaps<ResearchProjectDef>(meme.startingResearchProjects, "\n  - "));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x001F1316 File Offset: 0x001EF516
		private static string SortedLabelCaps<T>(List<T> defs, string prefix) where T : Def
		{
			return IdeoUIUtility.SortedLabelCaps<T, Def>(defs, null, prefix);
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x001F1320 File Offset: 0x001EF520
		private static string SortedLabelCaps<T1, T2>(List<T1> defs1, List<T2> defs2, string prefix) where T1 : Def where T2 : Def
		{
			IdeoUIUtility.tmpSortedLabelCaps.Clear();
			if (!defs1.NullOrEmpty<T1>())
			{
				foreach (T1 t in defs1)
				{
					IdeoUIUtility.tmpSortedLabelCaps.Add(t.LabelCap);
				}
			}
			if (!defs2.NullOrEmpty<T2>())
			{
				foreach (T2 t2 in defs2)
				{
					IdeoUIUtility.tmpSortedLabelCaps.Add(t2.LabelCap);
				}
			}
			if (IdeoUIUtility.tmpSortedLabelCaps.NullOrEmpty<string>())
			{
				return "";
			}
			IdeoUIUtility.tmpSortedLabelCaps.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in IdeoUIUtility.tmpSortedLabelCaps)
			{
				stringBuilder.Append(prefix).Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x001F1460 File Offset: 0x001EF660
		private static void DoPrecepts(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			IdeoUIUtility.DoPreceptsInt("Precepts".Translate(), "Precept".Translate(), true, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept), false);
			IdeoUIUtility.DoPreceptsInt("IdeoRoles".Translate(), "Role".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => typeof(Precept_Role).IsAssignableFrom(p.preceptClass), false);
			IdeoUIUtility.DoPreceptsInt("Rituals".Translate(), "Ritual".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Ritual), false);
			IdeoUIUtility.DoPreceptsInt("IdeoBuildings".Translate(), "IdeoBuilding".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Building) || p.preceptClass == typeof(Precept_RitualSeat), false);
			IdeoUIUtility.DoPreceptsInt("IdeoRelics".Translate(), "IdeoRelic".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Relic), false);
			IdeoUIUtility.DoPreceptsInt("IdeoWeapons".Translate(), "IdeoWeapon".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Weapon), false);
			IdeoUIUtility.DoPreceptsInt("VeneratedAnimals".Translate(), "Animal".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Animal), true);
			IdeoUIUtility.DoPreceptsInt("IdeoApparel".Translate(), "IdeoApparelDesire".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Apparel), true);
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x001F16B0 File Offset: 0x001EF8B0
		private static void DoPreceptsInt(string categoryLabel, string addPreceptLabel, bool mainPrecepts, Ideo ideo, IdeoEditMode editMode, ref float curY, float width, Func<PreceptDef, bool> filter, bool sortFloatMenuOptionsByLabel = false)
		{
			IdeoUIUtility.<>c__DisplayClass81_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass81_0();
			CS$<>8__locals1.filter = filter;
			CS$<>8__locals1.ideo = ideo;
			CS$<>8__locals1.editMode = editMode;
			IdeoUIUtility.tmpPrecepts.Clear();
			List<Precept> preceptsListForReading = CS$<>8__locals1.ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				if (preceptsListForReading[i].def.visible && CS$<>8__locals1.filter(preceptsListForReading[i].def))
				{
					IdeoUIUtility.tmpPrecepts.Add(preceptsListForReading[i]);
				}
			}
			if (!mainPrecepts && preceptsListForReading.Count == 0)
			{
				return;
			}
			IdeoUIUtility.tmpUsedThingDefs.Clear();
			using (List<Precept>.Enumerator enumerator = CS$<>8__locals1.ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_ThingDef precept_ThingDef;
					if ((precept_ThingDef = (enumerator.Current as Precept_ThingDef)) != null)
					{
						IdeoUIUtility.tmpUsedThingDefs.Add(precept_ThingDef.ThingDef);
					}
				}
			}
			curY += 4f;
			float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			Widgets.Label(num, ref curY, width, categoryLabel, default(TipSignal));
			if (CS$<>8__locals1.editMode == IdeoEditMode.GameStart && mainPrecepts && IdeoUIUtility.tmpPrecepts.Any<Precept>())
			{
				IdeoUIUtility.DrawKnowledgeTip(ConceptDefOf.EditingPrecepts, curY, num);
			}
			if (CS$<>8__locals1.editMode != IdeoEditMode.None)
			{
				float num2 = width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
				Rect rect = new Rect(num2 - IdeoUIUtility.AddPreceptButtonSize.x, curY - IdeoUIUtility.AddPreceptButtonSize.y, IdeoUIUtility.AddPreceptButtonSize.x, IdeoUIUtility.AddPreceptButtonSize.y);
				if (Widgets.ButtonText(rect, "AddPrecept".Translate(addPreceptLabel).CapitalizeFirst() + "...", true, true, true) && IdeoUIUtility.TutorAllowsInteraction(CS$<>8__locals1.editMode))
				{
					IdeoUIUtility.<>c__DisplayClass81_1 CS$<>8__locals2;
					CS$<>8__locals2.opts = new List<FloatMenuOption>();
					List<PreceptDef> list = (from x in DefDatabase<PreceptDef>.AllDefs
					where CS$<>8__locals1.filter(x)
					select x).ToList<PreceptDef>();
					bool flag = list.Any((PreceptDef p) => p.preceptClass == typeof(Precept_Ritual) && p.visible && IdeoUIUtility.CanListPrecept(CS$<>8__locals1.ideo, p, CS$<>8__locals1.editMode));
					int num3 = CS$<>8__locals1.ideo.PreceptsListForReading.Count((Precept p) => p is Precept_Ritual && p.def.visible);
					IdeoUIUtility.addedPatternDefsTmp.Clear();
					using (List<PreceptDef>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IdeoUIUtility.<>c__DisplayClass81_2 CS$<>8__locals3 = new IdeoUIUtility.<>c__DisplayClass81_2();
							CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
							CS$<>8__locals3.p = enumerator2.Current;
							IdeoUIUtility.<>c__DisplayClass81_3 CS$<>8__locals4;
							CS$<>8__locals4.acceptance = IdeoUIUtility.CanListPrecept(CS$<>8__locals3.CS$<>8__locals1.ideo, CS$<>8__locals3.p, CS$<>8__locals3.CS$<>8__locals1.editMode);
							if (CS$<>8__locals4.acceptance || !string.IsNullOrWhiteSpace(CS$<>8__locals4.acceptance.Reason))
							{
								int preceptCountOfDef = CS$<>8__locals3.CS$<>8__locals1.ideo.GetPreceptCountOfDef(CS$<>8__locals3.p);
								int num4 = CS$<>8__locals3.p.maxCount;
								if (CS$<>8__locals3.p.preceptInstanceCountCurve != null)
								{
									num4 = Mathf.Max(num4, Mathf.RoundToInt(CS$<>8__locals3.p.preceptInstanceCountCurve.Last<CurvePoint>().y));
								}
								if (preceptCountOfDef < num4 || CS$<>8__locals3.p.ignoreLimitsInEditMode)
								{
									IdeoUIUtility.<>c__DisplayClass81_6 CS$<>8__locals5 = new IdeoUIUtility.<>c__DisplayClass81_6();
									CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals3;
									if (!CS$<>8__locals5.CS$<>8__locals4.p.useChoicesFromBuildingDefs || CS$<>8__locals5.CS$<>8__locals4.p.Worker.ThingDefsForIdeo(CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo).EnumerableNullOrEmpty<PreceptThingChance>())
									{
										if (CS$<>8__locals5.CS$<>8__locals4.p.preceptClass == typeof(Precept_Weapon))
										{
											CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.<DoPreceptsInt>g__AddWeaponPreceptOption|5(CS$<>8__locals5.CS$<>8__locals4.p, ref CS$<>8__locals2, ref CS$<>8__locals4);
										}
										else
										{
											IdeoUIUtility.AddPreceptOption(CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo, CS$<>8__locals5.CS$<>8__locals4.p, CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.editMode, CS$<>8__locals2.opts, null);
										}
									}
									else
									{
										IdeoUIUtility.tmpAllowedThingDefs.Clear();
										IdeoUIUtility.tmpAllThingDefs.Clear();
										IdeoUIUtility.tmpAllowedThingDefs.AddRange(from td in CS$<>8__locals5.CS$<>8__locals4.p.Worker.ThingDefsForIdeo(CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo)
										select td.def);
										IdeoUIUtility.tmpAllThingDefs.AddRange(from bDef in CS$<>8__locals5.CS$<>8__locals4.p.Worker.ThingDefs
										orderby CS$<>8__locals5.CS$<>8__locals4.p.Worker.GetThingOrder(bDef), bDef.chance descending, bDef.def.LabelCap
										select bDef into bd
										select bd.def);
										if (CS$<>8__locals5.CS$<>8__locals4.p.preceptClass == typeof(Precept_Building))
										{
											foreach (MemeDef memeDef in CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo.memes)
											{
												if (!memeDef.consumableBuildings.NullOrEmpty<ThingDef>())
												{
													foreach (ThingDef item in memeDef.consumableBuildings)
													{
														if (!IdeoUIUtility.tmpAllowedThingDefs.Contains(item))
														{
															IdeoUIUtility.tmpAllowedThingDefs.Add(item);
														}
														if (!IdeoUIUtility.tmpAllThingDefs.Contains(item))
														{
															IdeoUIUtility.tmpAllThingDefs.Add(item);
														}
													}
												}
											}
										}
										using (List<ThingDef>.Enumerator enumerator4 = IdeoUIUtility.tmpAllThingDefs.GetEnumerator())
										{
											while (enumerator4.MoveNext())
											{
												ThingDef b = enumerator4.Current;
												TaggedString taggedString = b.LabelCap;
												if (CS$<>8__locals5.CS$<>8__locals4.p.preceptClass == typeof(Precept_Apparel))
												{
													taggedString += ": " + CS$<>8__locals5.CS$<>8__locals4.p.LabelCap;
												}
												FloatMenuOption floatMenuOption = null;
												if ((!CS$<>8__locals5.CS$<>8__locals4.p.canUseAlreadyUsedThingDef && IdeoUIUtility.tmpUsedThingDefs.Contains(b)) || CS$<>8__locals5.CS$<>8__locals4.p.Worker.ShouldSkipThing(CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo, b) || !IdeoUIUtility.tmpAllowedThingDefs.Contains(b))
												{
													if (!CS$<>8__locals5.CS$<>8__locals4.p.canUseAlreadyUsedThingDef && IdeoUIUtility.tmpUsedThingDefs.Contains(b))
													{
														floatMenuOption = null;
													}
													else
													{
														AcceptanceReport report = CS$<>8__locals5.CS$<>8__locals4.p.Worker.CanUse(b, CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo);
														if (!report)
														{
															floatMenuOption = new FloatMenuOption(string.IsNullOrWhiteSpace(report.Reason) ? taggedString : (taggedString + " (" + report.Reason + ")"), null, b, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
															floatMenuOption.thingStyle = CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo.GetStyleFor(b);
														}
													}
												}
												else
												{
													floatMenuOption = new FloatMenuOption(taggedString, delegate()
													{
														Precept precept = PreceptMaker.MakePrecept(CS$<>8__locals5.CS$<>8__locals4.p);
														Precept_Apparel precept_Apparel;
														if ((precept_Apparel = (precept as Precept_Apparel)) != null)
														{
															precept_Apparel.apparelDef = b;
														}
														CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo.AddPrecept(precept, true, null, null);
														Precept_ThingDef precept_ThingDef2;
														if ((precept_ThingDef2 = (precept as Precept_ThingDef)) != null)
														{
															precept_ThingDef2.ThingDef = b;
															precept_ThingDef2.RegenerateName();
														}
													}, b, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
													floatMenuOption.thingStyle = CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo.GetStyleFor(b);
													if (CS$<>8__locals5.CS$<>8__locals4.p.preceptClass == typeof(Precept_Apparel))
													{
														floatMenuOption.forceThingColor = new Color?(CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo.ApparelColor);
													}
												}
												if (floatMenuOption != null)
												{
													CS$<>8__locals2.opts.Add(IdeoUIUtility.<DoPreceptsInt>g__PostProcessOption|81_4(floatMenuOption, ref CS$<>8__locals4));
												}
											}
										}
									}
									IdeoUIUtility.<>c__DisplayClass81_6 CS$<>8__locals7 = CS$<>8__locals5;
									RitualPatternDef ritualPatternBase = CS$<>8__locals5.CS$<>8__locals4.p.ritualPatternBase;
									CS$<>8__locals7.groupTag = ((ritualPatternBase != null) ? ritualPatternBase.patternGroupTag : null);
									if (CS$<>8__locals5.groupTag.NullOrEmpty())
									{
										continue;
									}
									IEnumerable<RitualPatternDef> allDefs = DefDatabase<RitualPatternDef>.AllDefs;
									Func<RitualPatternDef, bool> predicate;
									if ((predicate = CS$<>8__locals5.<>9__14) == null)
									{
										predicate = (CS$<>8__locals5.<>9__14 = ((RitualPatternDef d) => d.patternGroupTag == CS$<>8__locals5.groupTag && d != CS$<>8__locals5.CS$<>8__locals4.p.ritualPatternBase));
									}
									using (IEnumerator<RitualPatternDef> enumerator5 = allDefs.Where(predicate).GetEnumerator())
									{
										while (enumerator5.MoveNext())
										{
											RitualPatternDef patternDef = enumerator5.Current;
											IdeoUIUtility.AddPreceptOption(CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.ideo, CS$<>8__locals5.CS$<>8__locals4.p, CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.editMode, CS$<>8__locals2.opts, patternDef);
										}
										continue;
									}
								}
								if (CS$<>8__locals3.p.preceptClass == typeof(Precept_Relic))
								{
									CS$<>8__locals2.opts.Add(new FloatMenuOption("MaxRelicCount".Translate(num4), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								else if (preceptCountOfDef >= num4 && CS$<>8__locals3.p.issue.allowMultiplePrecepts)
								{
									CS$<>8__locals2.opts.Add(new FloatMenuOption(CS$<>8__locals3.p.LabelCap + " (" + "MaxPreceptCount".Translate(num4) + ")", null, CS$<>8__locals3.p.Icon ?? CS$<>8__locals3.CS$<>8__locals1.ideo.Icon, CS$<>8__locals3.CS$<>8__locals1.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
						}
					}
					if (sortFloatMenuOptionsByLabel)
					{
						CS$<>8__locals2.opts.SortBy((FloatMenuOption x) => x.Label);
					}
					if (num3 < 6)
					{
						using (List<MemeDef>.Enumerator enumerator3 = CS$<>8__locals1.ideo.memes.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								MemeDef memeDef2 = enumerator3.Current;
								if (!memeDef2.replacementPatterns.NullOrEmpty<RitualPatternDef>())
								{
									foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefs.Where(CS$<>8__locals1.filter))
									{
										if (preceptDef.ritualPatternBase != null && !preceptDef.ritualPatternBase.tags.NullOrEmpty<string>() && memeDef2.replaceRitualsWithTags.Any(new Predicate<string>(preceptDef.ritualPatternBase.tags.Contains)))
										{
											foreach (RitualPatternDef ritualPatternDef in memeDef2.replacementPatterns)
											{
												if (IdeoUIUtility.CanAddRitualPattern(CS$<>8__locals1.ideo, ritualPatternDef, CS$<>8__locals1.editMode))
												{
													IdeoUIUtility.AddPreceptOption(CS$<>8__locals1.ideo, preceptDef, CS$<>8__locals1.editMode, CS$<>8__locals2.opts, ritualPatternDef);
												}
											}
										}
									}
								}
							}
							goto IL_CA1;
						}
					}
					if (flag)
					{
						CS$<>8__locals2.opts.Clear();
						CS$<>8__locals2.opts.Add(new FloatMenuOption("MaxRitualCount".Translate(6), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					IL_CA1:
					if (!CS$<>8__locals2.opts.Any<FloatMenuOption>())
					{
						CS$<>8__locals2.opts.Add(new FloatMenuOption("NoChoicesAvailable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(CS$<>8__locals2.opts));
				}
				if (mainPrecepts)
				{
					Rect rect2 = rect;
					rect2.x = rect.xMin - rect.width - 10f;
					if (Widgets.ButtonText(rect2, "RandomizePrecepts".Translate(), true, true, true) && IdeoUIUtility.TutorAllowsInteraction(CS$<>8__locals1.editMode))
					{
						CS$<>8__locals1.ideo.foundation.RandomizePrecepts(true, new IdeoGenerationParms(IdeoUIUtility.FactionForRandomization(CS$<>8__locals1.ideo), false, null, null));
						CS$<>8__locals1.ideo.RegenerateDescription(false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
				}
			}
			curY += 4f;
			PreceptImpact preceptImpact = IdeoUIUtility.tmpPrecepts.Any<Precept>() ? IdeoUIUtility.tmpPrecepts[0].def.impact : PreceptImpact.Low;
			float num5 = (width - 3f * IdeoUIUtility.PreceptBoxSize.x - 16f) / 2f;
			int num6 = 0;
			int num7 = 0;
			if (!IdeoUIUtility.tmpPrecepts.Any<Precept>())
			{
				GUI.color = Color.grey;
				Widgets.Label(new Rect(num + 36f, curY + 10f, 999f, Text.LineHeight), "(" + "NoneLower".Translate() + ")");
				GUI.color = Color.white;
			}
			for (int j = 0; j < IdeoUIUtility.tmpPrecepts.Count; j++)
			{
				if (preceptImpact != IdeoUIUtility.tmpPrecepts[j].def.impact)
				{
					preceptImpact = IdeoUIUtility.tmpPrecepts[j].def.impact;
					num7 = 0;
					num6++;
				}
				else if (num7 >= 2)
				{
					num7 = 0;
					num6++;
				}
				else if (j > 0)
				{
					num7++;
				}
				Rect rect3 = new Rect(num5 + (float)num7 * IdeoUIUtility.PreceptBoxSize.x + (float)(num7 * 8), curY + (float)num6 * IdeoUIUtility.PreceptBoxSize.y + (float)(num6 * 8), IdeoUIUtility.PreceptBoxSize.x, IdeoUIUtility.PreceptBoxSize.y);
				if (mainPrecepts && CS$<>8__locals1.editMode == IdeoEditMode.GameStart)
				{
					UIHighlighter.HighlightOpportunity(rect3, "PreceptBox");
				}
				IdeoUIUtility.tmpPrecepts[j].DrawPreceptBox(rect3, CS$<>8__locals1.editMode, IdeoUIUtility.tmpMouseOverMeme != null && IdeoUIUtility.IsPreceptRelatedForUI(IdeoUIUtility.tmpMouseOverMeme, IdeoUIUtility.tmpPrecepts[j].def));
				if (Mouse.IsOver(rect3))
				{
					IdeoUIUtility.tmpMouseOverPrecept = IdeoUIUtility.tmpPrecepts[j].def;
				}
				GUI.color = Color.white;
			}
			curY += (float)(num6 + 1) * IdeoUIUtility.PreceptBoxSize.y + (float)(num6 * 8);
			curY += 17f;
			IdeoUIUtility.tmpPrecepts.Clear();
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x001F2740 File Offset: 0x001F0940
		private static void AddPreceptOption(Ideo ideo, PreceptDef def, IdeoEditMode editMode, List<FloatMenuOption> options, RitualPatternDef patternDef = null)
		{
			IdeoUIUtility.tempRequiredMemes.Clear();
			RitualPatternDef pat = patternDef ?? def.ritualPatternBase;
			if (pat != null && (!IdeoUIUtility.CanAddRitualPattern(ideo, pat, editMode) || IdeoUIUtility.addedPatternDefsTmp.Contains(pat)))
			{
				return;
			}
			string text = ((patternDef != null) ? patternDef.shortDescOverride.CapitalizeFirst() : null) ?? def.LabelCap;
			bool flag = def.preceptClass == typeof(Precept);
			if (flag)
			{
				text = def.issue.LabelCap + ": " + text;
			}
			Action action = delegate()
			{
				Precept precept = PreceptMaker.MakePrecept(def);
				ideo.AddPrecept(precept, true, null, pat);
			};
			if (editMode != IdeoEditMode.Dev)
			{
				AcceptanceReport acceptanceReport = IdeoUIUtility.CanAddPrecept(def, pat, ideo);
				if (!acceptanceReport.Accepted)
				{
					action = null;
					text = text + " (" + acceptanceReport.Reason + ")";
				}
				else if (ideo.HasMaxPreceptsForIssue(def.issue))
				{
					return;
				}
			}
			string label = text;
			Action action2 = action;
			Texture2D itemIcon;
			if ((itemIcon = ((patternDef != null) ? patternDef.Icon : null)) == null)
			{
				itemIcon = (def.Icon ?? ideo.Icon);
			}
			options.Add(new FloatMenuOption(label, action2, itemIcon, flag ? IdeoUIUtility.GetIconAndLabelColor(def.impact) : ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			if (pat != null)
			{
				IdeoUIUtility.addedPatternDefsTmp.Add(pat);
			}
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x001F2904 File Offset: 0x001F0B04
		private static AcceptanceReport CanAddPrecept(PreceptDef def, RitualPatternDef pat, Ideo ideo)
		{
			AcceptanceReport acceptanceReport = ideo.CanAddPreceptAllFactions(def);
			if (!acceptanceReport)
			{
				return acceptanceReport;
			}
			if (pat != null && !pat.ritualObligationTargetFilter.thingDefs.NullOrEmpty<ThingDef>() && !pat.ignoreConsumableBuildingRequirement)
			{
				List<ThingDef> things = pat.ritualObligationTargetFilter.thingDefs;
				bool flag = false;
				Predicate<ThingDef> <>9__0;
				foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefs)
				{
					if (!memeDef.consumableBuildings.NullOrEmpty<ThingDef>())
					{
						List<ThingDef> consumableBuildings = memeDef.consumableBuildings;
						Predicate<ThingDef> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingDef x) => things.Contains(x)));
						}
						if (consumableBuildings.Any(predicate))
						{
							if (ideo.HasMeme(memeDef))
							{
								flag = true;
								break;
							}
							IdeoUIUtility.tempRequiredMemes.Add(memeDef.label);
						}
					}
				}
				if (IdeoUIUtility.tempRequiredMemes.Any<string>() && !flag)
				{
					if (IdeoUIUtility.tempRequiredMemes.Count == 1)
					{
						return "RequiresMeme".Translate() + ": " + IdeoUIUtility.tempRequiredMemes[0].CapitalizeFirst();
					}
					return "RequiresOneOfMemes".Translate() + ": " + IdeoUIUtility.tempRequiredMemes.ToCommaList(false, false).CapitalizeFirst();
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x001F2A78 File Offset: 0x001F0C78
		public static AcceptanceReport CanListPrecept(Ideo ideo, PreceptDef precept, IdeoEditMode editMode)
		{
			if (!precept.visible)
			{
				return false;
			}
			if (editMode == IdeoEditMode.Dev)
			{
				return true;
			}
			return ideo.CanAddPreceptAllFactions(precept);
		}

		// Token: 0x06005A3F RID: 23103 RVA: 0x001F2A9B File Offset: 0x001F0C9B
		private static bool CanAddRitualPattern(Ideo ideo, RitualPatternDef pattern, IdeoEditMode editMode)
		{
			return editMode == IdeoEditMode.Dev || pattern.CanFactionUse(IdeoUIUtility.FactionForRandomization(ideo));
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x001F2AB4 File Offset: 0x001F0CB4
		private static void DoAppearanceItems(Ideo ideo, IdeoEditMode editMode, ref float curY, float width)
		{
			IdeoUIUtility.<>c__DisplayClass86_0 CS$<>8__locals1;
			CS$<>8__locals1.editMode = editMode;
			CS$<>8__locals1.ideo = ideo;
			Widgets.Label((width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f, ref curY, width, "Appearance".Translate(), default(TipSignal));
			IdeoUIUtility.<DoAppearanceItems>g__DrawAppearanceItem|86_0(4f, curY, StyleItemTab.HairAndBeard, CS$<>8__locals1.ideo.style.DisplayedHairDef, ref CS$<>8__locals1);
			IdeoUIUtility.<DoAppearanceItems>g__DrawAppearanceItem|86_0(4f + IdeoUIUtility.PreceptBoxSize.x + 8f, curY, StyleItemTab.Tattoo, CS$<>8__locals1.ideo.style.DisplayedTattooDef, ref CS$<>8__locals1);
			curY += IdeoUIUtility.PreceptBoxSize.y + 17f;
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x001F2B74 File Offset: 0x001F0D74
		public static FactionDef FactionForRandomization(Ideo ideo)
		{
			FactionDef result = Find.Scenario.playerFaction.factionDef;
			if (Find.World != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction.ideos.IsPrimary(ideo))
					{
						result = faction.def;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x001F2BF0 File Offset: 0x001F0DF0
		private static void DoDebugButtons(ref float curY, float width, Ideo ideo)
		{
			curY += 17f;
			if (Widgets.ButtonText(new Rect(0f, curY, 200f, 40f), "Dev: Single precept", true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					PreceptDef defLocal = allDefsListForReading[i];
					list.Add(new FloatMenuOption(defLocal.issue.LabelCap + ": " + (defLocal.LabelCap.NullOrEmpty() ? defLocal.defName : defLocal.LabelCap), delegate()
					{
						ideo.ClearPrecepts();
						ideo.AddPrecept(PreceptMaker.MakePrecept(defLocal), true, null, null);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (Widgets.ButtonText(new Rect(210f, curY, 200f, 40f), "DEV: test descriptions...", true, true, true))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < 100; j++)
				{
					ideo.RegenerateDescription(true);
					stringBuilder.Append("template: " + ideo.descriptionTemplate).AppendLine();
					stringBuilder.Append("text: " + ideo.description).AppendLine().AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
			if (Widgets.ButtonText(new Rect(420f, curY, 200f, 40f), "Dev: Test names...", true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				list2.Add(new FloatMenuOption("Ideo names (replace)", delegate()
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					for (int k = 0; k < 200; k++)
					{
						ideo.foundation.GenerateTextSymbols();
						ideo.foundation.GenerateLeaderTitle();
						stringBuilder2.AppendLine(string.Concat(new string[]
						{
							ideo.name,
							"\n- ",
							ideo.adjective,
							"\n- ",
							ideo.memberName,
							"\n- ",
							ideo.leaderTitleMale
						}));
						stringBuilder2.AppendLine();
					}
					Log.Message(stringBuilder2.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept precept = enumerator.Current;
						PreceptDef def = precept.def;
						if (def.preceptClass == typeof(Precept_Ritual) || typeof(Precept_Role).IsAssignableFrom(def.preceptClass) || def.preceptClass == typeof(Precept_Building))
						{
							list2.Add(new FloatMenuOption(def.issue.LabelCap + ": " + precept.LabelCap, delegate()
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								for (int k = 0; k < 200; k++)
								{
									stringBuilder2.AppendLine(precept.GenerateNameRaw());
								}
								Log.Message(stringBuilder2.ToString());
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			curY += 50f;
		}

		// Token: 0x06005A43 RID: 23107 RVA: 0x001F2F1C File Offset: 0x001F111C
		public static void DrawExtraThoughtInfoFromIdeo(Pawn pawn, ref Rect rect)
		{
			if (!ModLister.IdeologyInstalled || pawn.Ideo == null)
			{
				return;
			}
			if (pawn.Ideo.IdeoCausesHumanMeatCravings())
			{
				string value = (Find.TickManager.TicksGame - pawn.mindState.lastHumanMeatIngestedTick).ToStringTicksToPeriod(true, false, true, true);
				TaggedString taggedString = "LastHumanMeat".Translate() + ": " + "TimeAgo".Translate(value);
				float num = Text.CalcHeight(taggedString, rect.width);
				Rect rect2 = new Rect(rect.x, rect.yMax - num, rect.width, num);
				Widgets.Label(rect2, taggedString);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, "LastHumanMeatDesc".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"), 8.Named("DURATION")).Resolve());
				}
				rect.yMax -= num;
			}
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x001F3030 File Offset: 0x001F1230
		public static Color GetBackgroundColor(PreceptImpact impact)
		{
			switch (impact)
			{
			case PreceptImpact.Low:
				return new Color(0.13f, 0.13f, 0.13f);
			case PreceptImpact.Medium:
				return new Color(0.18f, 0.18f, 0.18f);
			case PreceptImpact.High:
				return new Color(0.24f, 0.24f, 0.24f);
			default:
				return Color.white;
			}
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x001F3098 File Offset: 0x001F1298
		public static Color GetIconAndLabelColor(PreceptImpact impact)
		{
			switch (impact)
			{
			case PreceptImpact.Low:
				return new Color(0.7f, 0.7f, 0.7f);
			case PreceptImpact.Medium:
				return new Color(1f, 1f, 1f);
			case PreceptImpact.High:
				return new Color(1f, 1f, 0.5f);
			default:
				return Color.white;
			}
		}

		// Token: 0x06005A46 RID: 23110 RVA: 0x001F3100 File Offset: 0x001F1300
		public static bool IsPreceptRelatedForUI(MemeDef meme, PreceptDef precept)
		{
			return precept.requiredMemes.Contains(meme) || precept.associatedMemes.Contains(meme) || (meme.requireOne != null && meme.requireOne.Any((List<PreceptDef> pl) => pl.Contains(precept)));
		}

		// Token: 0x06005A47 RID: 23111 RVA: 0x001F3164 File Offset: 0x001F1364
		public static void DrawImpactInfo(Rect rect, List<MemeDef> memes)
		{
			int num = IdeoUIUtility.ImpactOf(memes);
			if (num == 0)
			{
				return;
			}
			string text = num.ToStringCached();
			Text.Font = GameFont.Medium;
			float num2 = Mathf.Max(16f, Text.CalcSize(text).x + 2f);
			Rect rect2 = rect;
			Rect rect3 = rect.TopPartPixels(rect.height - 12f);
			Rect rect4;
			Rect rect5;
			rect3.SplitVertically(rect3.width - num2, out rect4, out rect5, 0f);
			rect5.y += 1f;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("IdeoImpactOverallDesc".Translate());
				foreach (MemeDef memeDef in memes)
				{
					if (memeDef.impact != 0)
					{
						stringBuilder.Append("\n  - ").Append(memeDef.LabelCap).Append(": ").Append(memeDef.impact.ToStringCached());
					}
				}
				TooltipHandler.TipRegion(rect, stringBuilder.ToString());
			}
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.LowerRight;
			Widgets.Label(rect5, text);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerRight;
			Widgets.Label(rect2, IdeoImpactUtility.OverallImpactLabel(num).CapitalizeFirst());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.LowerRight;
			Widgets.Label(rect4, "IdeoImpactOverall".Translate() + ":");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005A48 RID: 23112 RVA: 0x001F330C File Offset: 0x001F150C
		private static int ImpactOf(List<MemeDef> memes)
		{
			int num = 0;
			foreach (MemeDef memeDef in memes)
			{
				num += memeDef.impact;
			}
			return num;
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x001F34C4 File Offset: 0x001F16C4
		[CompilerGenerated]
		internal static void <DoName>g__AppendDetails|56_5(ref IdeoUIUtility.<>c__DisplayClass56_3 A_0)
		{
			if (!A_0.desc.NullOrEmpty())
			{
				A_0.desc += "\n\n";
			}
			A_0.desc += ("StyleCategoryDetails".Translate(A_0.cat.label).CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor);
			A_0.showedDetails = true;
		}

		// Token: 0x06005A4B RID: 23115 RVA: 0x001F3542 File Offset: 0x001F1742
		[CompilerGenerated]
		internal static FloatMenuOption <DoPreceptsInt>g__PostProcessOption|81_4(FloatMenuOption option, ref IdeoUIUtility.<>c__DisplayClass81_3 A_1)
		{
			if (!A_1.acceptance)
			{
				option.action = null;
				option.Label = option.Label + " (" + A_1.acceptance.Reason + ")";
			}
			return option;
		}

		// Token: 0x06005A4C RID: 23116 RVA: 0x001F3580 File Offset: 0x001F1780
		[CompilerGenerated]
		internal static void <DoAppearanceItems>g__DrawAppearanceItem|86_0(float xOffset, float y, StyleItemTab tab, StyleItemDef defToDisplay, ref IdeoUIUtility.<>c__DisplayClass86_0 A_4)
		{
			Rect rect = new Rect(xOffset, y, IdeoUIUtility.PreceptBoxSize.x, IdeoUIUtility.PreceptBoxSize.y);
			Widgets.DrawRectFast(rect, IdeoUIUtility.GetBackgroundColor(PreceptImpact.Medium), null);
			string text = ((tab == StyleItemTab.HairAndBeard) ? "HairAndBeards" : "Tattoos").Translate();
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, text.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + ((tab == StyleItemTab.HairAndBeard) ? "HairAndBeardsDesc" : "TattoosDesc").Translate() + ((A_4.editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
			}
			rect = rect.ContractedBy(4f);
			Rect rect2 = new Rect(rect.x, rect.y, 50f, 50f);
			if (defToDisplay != null)
			{
				GUI.color = PawnHairColors.ReddishBrown;
				Widgets.DefIcon(rect2, defToDisplay, null, 1.25f, null, false, null);
				GUI.color = Color.white;
				rect.xMin = rect2.xMax + 10f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = new Color(0.8f, 0.8f, 0.8f);
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, rect.height / 2f), text);
			GUI.color = Color.white;
			Widgets.Label(new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f), "NumAvailable".Translate(((tab == StyleItemTab.HairAndBeard) ? A_4.ideo.style.NumHairAndBeardStylesAvailable : A_4.ideo.style.NumTattooStylesAvailable).ToString()));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(rect, true) && IdeoUIUtility.TutorAllowsInteraction(A_4.editMode))
			{
				Find.WindowStack.Add(new Dialog_EditIdeoStyleItems(A_4.ideo, tab, A_4.editMode));
			}
		}

		// Token: 0x040034C4 RID: 13508
		private static bool showAll;

		// Token: 0x040034C5 RID: 13509
		public static bool devEditMode;

		// Token: 0x040034C6 RID: 13510
		public static Ideo selected;

		// Token: 0x040034C7 RID: 13511
		private static List<Color> ideoIconColors;

		// Token: 0x040034C8 RID: 13512
		private static Ideo currentRitualAmbiencePreviewIdeo;

		// Token: 0x040034C9 RID: 13513
		private static Sustainer currentRitualAmbiencePreview;

		// Token: 0x040034CA RID: 13514
		private const float IdeoIconRectSize = 30f;

		// Token: 0x040034CB RID: 13515
		private const float IdeoIconRectGapX = 7f;

		// Token: 0x040034CC RID: 13516
		private const float IdeoIconRectGapY = 7f;

		// Token: 0x040034CD RID: 13517
		private const float RowMinHeight = 46f;

		// Token: 0x040034CE RID: 13518
		private const int IconSize = 70;

		// Token: 0x040034CF RID: 13519
		private const int CultureIconSize = 35;

		// Token: 0x040034D0 RID: 13520
		private const int StyleIconSize = 28;

		// Token: 0x040034D1 RID: 13521
		public const int ApparelRequirementIconSize = 24;

		// Token: 0x040034D2 RID: 13522
		public const int ApparelRequirementIconPad = 2;

		// Token: 0x040034D3 RID: 13523
		private const float IdeoListWidthPct = 0.25f;

		// Token: 0x040034D4 RID: 13524
		private const float IdeoDetailsPad = 17f;

		// Token: 0x040034D5 RID: 13525
		public const float CellPad = 6f;

		// Token: 0x040034D6 RID: 13526
		public const float ExtraSpaceBetweenRows = 4f;

		// Token: 0x040034D7 RID: 13527
		public const float TextTopPad = 2f;

		// Token: 0x040034D8 RID: 13528
		public static readonly Vector2 MemeBoxSize = new Vector2(122f, 120f);

		// Token: 0x040034D9 RID: 13529
		private const float MemeIconSize = 80f;

		// Token: 0x040034DA RID: 13530
		private const int MemeIconMargin = 8;

		// Token: 0x040034DB RID: 13531
		public static readonly Vector2 PreceptBoxSize = new Vector2(220f, 60f);

		// Token: 0x040034DC RID: 13532
		public const float PreceptIconSize = 50f;

		// Token: 0x040034DD RID: 13533
		public const int MaxPreceptsPerRow = 3;

		// Token: 0x040034DE RID: 13534
		public const int GapBetweenBoxes = 8;

		// Token: 0x040034DF RID: 13535
		private const int DescriptionTextBoxHeight = 70;

		// Token: 0x040034E0 RID: 13536
		private const int DescriptionTextBoxPadding = 40;

		// Token: 0x040034E1 RID: 13537
		public static readonly Vector2 ButtonSize = new Vector2(145f, 30f);

		// Token: 0x040034E2 RID: 13538
		private static readonly Vector2 AddPreceptButtonSize = new Vector2(175f, 30f);

		// Token: 0x040034E3 RID: 13539
		private static readonly Color TutorArrowColor = new Color(0.937f, 0.847f, 0f);

		// Token: 0x040034E4 RID: 13540
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/TutorArrowRight", true);

		// Token: 0x040034E5 RID: 13541
		private static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x040034E6 RID: 13542
		private static readonly Texture2D UnlockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/LockedMonochrome", true);

		// Token: 0x040034E7 RID: 13543
		public static readonly Texture2D LockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/Locked", true);

		// Token: 0x040034E8 RID: 13544
		public static readonly Texture2D PreviewRitualAmbience = ContentFinder<Texture2D>.Get("PlaceholderImage_Gear", true);

		// Token: 0x040034E9 RID: 13545
		public static readonly Texture2D PreviewRitualAmbienceOn = ContentFinder<Texture2D>.Get("PlaceholderImage", true);

		// Token: 0x040034EA RID: 13546
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040034EB RID: 13547
		private static List<ThingStyleCategoryWithPriority> tmpStyleCategories = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x040034EC RID: 13548
		private static List<IdeoUIUtility.StyleCatOverride> tmpOverrides = new List<IdeoUIUtility.StyleCatOverride>();

		// Token: 0x040034ED RID: 13549
		private static List<Faction> tmpFactions = new List<Faction>();

		// Token: 0x040034EE RID: 13550
		private static List<MemeDef> tmpMemesToShow = new List<MemeDef>();

		// Token: 0x040034EF RID: 13551
		private static MemeDef tmpMouseOverMeme = null;

		// Token: 0x040034F0 RID: 13552
		private static List<PreceptDef> tmpRequiredPrecepts = new List<PreceptDef>();

		// Token: 0x040034F1 RID: 13553
		private static List<string> tmpSortedLabelCaps = new List<string>();

		// Token: 0x040034F2 RID: 13554
		private static List<Precept> tmpPrecepts = new List<Precept>();

		// Token: 0x040034F3 RID: 13555
		private static List<ThingDef> tmpUsedThingDefs = new List<ThingDef>();

		// Token: 0x040034F4 RID: 13556
		private static List<ThingDef> tmpAllowedThingDefs = new List<ThingDef>();

		// Token: 0x040034F5 RID: 13557
		private static List<ThingDef> tmpAllThingDefs = new List<ThingDef>();

		// Token: 0x040034F6 RID: 13558
		private static List<string> tempRequiredMemes = new List<string>();

		// Token: 0x040034F7 RID: 13559
		private static PreceptDef tmpMouseOverPrecept = null;

		// Token: 0x040034F8 RID: 13560
		private static List<RitualPatternDef> addedPatternDefsTmp = new List<RitualPatternDef>();

		// Token: 0x0200234D RID: 9037
		private struct StyleCatOverride
		{
			// Token: 0x0600C682 RID: 50818 RVA: 0x003DFD14 File Offset: 0x003DDF14
			public StyleCatOverride(string label, StyleCategoryDef style)
			{
				this.label = label;
				this.style = style;
			}

			// Token: 0x04008683 RID: 34435
			public string label;

			// Token: 0x04008684 RID: 34436
			public StyleCategoryDef style;
		}
	}
}
