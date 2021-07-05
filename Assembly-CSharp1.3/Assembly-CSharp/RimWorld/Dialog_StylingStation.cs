using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012FE RID: 4862
	public class Dialog_StylingStation : Window
	{
		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x060074CC RID: 29900 RVA: 0x0027C040 File Offset: 0x0027A240
		private List<Color> AllColors
		{
			get
			{
				if (this.allColors == null)
				{
					this.allColors = (from ic in DefDatabase<ColorDef>.AllDefsListForReading
					select ic.color).ToList<Color>();
					this.allColors.SortByColor((Color x) => x);
				}
				return this.allColors;
			}
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x060074CD RID: 29901 RVA: 0x0027C0B9 File Offset: 0x0027A2B9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(850f, 750f);
			}
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x0027C0CC File Offset: 0x0027A2CC
		public Dialog_StylingStation(Pawn pawn, Thing stylingStation)
		{
			this.pawn = pawn;
			this.stylingStation = stylingStation;
			this.initialHairDef = pawn.story.hairDef;
			this.initialHairColor = pawn.story.hairColor;
			this.initialBeardDef = pawn.style.beardDef;
			this.initialFaceTattoo = pawn.style.FaceTattoo;
			this.initialBodyTattoo = pawn.style.BodyTattoo;
			this.forcePause = true;
			this.closeOnCancel = false;
			this.showClothes = true;
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				if (apparel.TryGetComp<CompColorable>() != null)
				{
					this.apparelColors.Add(apparel, apparel.DesiredColor ?? apparel.DrawColor);
				}
			}
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x0027C1E4 File Offset: 0x0027A3E4
		public override void PreOpen()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				this.Close(true);
			}
			base.PreOpen();
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x0027C200 File Offset: 0x0027A400
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(inRect)
			{
				height = Text.LineHeight * 2f
			};
			Widgets.Label(rect, "StylePawn".Translate().CapitalizeFirst() + ": " + Find.ActiveLanguageWorker.WithDefiniteArticle(this.pawn.Name.ToStringShort, this.pawn.gender, false, true).ApplyTag(TagType.Name, null));
			Text.Font = GameFont.Small;
			inRect.yMin = rect.yMax + 4f;
			Rect rect2 = inRect;
			rect2.width *= 0.3f;
			rect2.yMax -= Dialog_StylingStation.ButSize.y + 4f;
			this.DrawPawn(rect2);
			Rect rect3 = inRect;
			rect3.xMin = rect2.xMax + 10f;
			rect3.yMax -= Dialog_StylingStation.ButSize.y + 4f;
			this.DrawTabs(rect3);
			this.DrawBottomButtons(inRect);
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(inRect.xMax - 120f, 0f, 120f, 30f), "Dev: Show all", ref this.devEditMode, false, null, null, false);
			}
		}

		// Token: 0x060074D1 RID: 29905 RVA: 0x0027C354 File Offset: 0x0027A554
		private void DrawPawn(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin = rect.yMax - Text.LineHeight * 2f;
			bool flag = this.showHeadgear;
			bool flag2 = this.showClothes;
			Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y, rect2.width, rect2.height / 2f), "ShowHeadgear".Translate(), ref this.showHeadgear, false, null, null, false);
			Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y + rect2.height / 2f, rect2.width, rect2.height / 2f), "ShowApparel".Translate(), ref this.showClothes, false, null, null, false);
			rect.yMax = rect2.yMin - 4f;
			if (flag != this.showHeadgear || flag2 != this.showClothes)
			{
				this.pawn.style.SetGraphicsDirty();
			}
			GUI.BeginGroup(rect);
			for (int i = 0; i < 3; i++)
			{
				Rect position = new Rect(0f, rect.height / 3f * (float)i, rect.width, rect.height / 3f).ContractedBy(4f);
				GUI.DrawTexture(position, PortraitsCache.Get(this.pawn, new Vector2(position.width, position.height), new Rot4(i), new Vector3(0f, 0f, 0.15f), 1.1f, true, true, this.showHeadgear, this.showClothes, this.apparelColors, true));
			}
			GUI.EndGroup();
			if (this.pawn.style.HasAnyUnwantedStyleItem)
			{
				string t = "PawnUnhappyWithStyleItems".Translate(this.pawn.Named("PAWN")) + ": ";
				Dialog_StylingStation.tmpUnwantedStyleNames.Clear();
				if (this.pawn.style.HasUnwantedHairStyle)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("Hair".Translate());
				}
				if (this.pawn.style.HasUnwantedBeard)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("Beard".Translate());
				}
				if (this.pawn.style.HasUnwantedFaceTattoo)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("TattooFace".Translate());
				}
				if (this.pawn.style.HasUnwantedBodyTattoo)
				{
					Dialog_StylingStation.tmpUnwantedStyleNames.Add("TattooBody".Translate());
				}
				GUI.color = ColorLibrary.RedReadable;
				Widgets.Label(new Rect(rect.x, rect.yMin - 30f, rect.width, Text.LineHeight * 2f + 10f), "Warning".Translate() + ": " + t + Dialog_StylingStation.tmpUnwantedStyleNames.ToCommaList(false, false).CapitalizeFirst());
				GUI.color = Color.white;
			}
		}

		// Token: 0x060074D2 RID: 29906 RVA: 0x0027C678 File Offset: 0x0027A878
		private void DrawTabs(Rect rect)
		{
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("Hair".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.Hair;
			}, this.curTab == Dialog_StylingStation.StylingTab.Hair));
			if (this.pawn.gender == Gender.Male || this.devEditMode)
			{
				this.tabs.Add(new TabRecord("Beard".Translate().CapitalizeFirst(), delegate()
				{
					this.curTab = Dialog_StylingStation.StylingTab.Beard;
				}, this.curTab == Dialog_StylingStation.StylingTab.Beard));
			}
			this.tabs.Add(new TabRecord("TattooFace".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.TattooFace;
			}, this.curTab == Dialog_StylingStation.StylingTab.TattooFace));
			this.tabs.Add(new TabRecord("TattooBody".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.TattooBody;
			}, this.curTab == Dialog_StylingStation.StylingTab.TattooBody));
			this.tabs.Add(new TabRecord("ApparelColor".Translate().CapitalizeFirst(), delegate()
			{
				this.curTab = Dialog_StylingStation.StylingTab.ApparelColor;
			}, this.curTab == Dialog_StylingStation.StylingTab.ApparelColor));
			Widgets.DrawMenuSection(rect);
			TabDrawer.DrawTabs<TabRecord>(rect, this.tabs, 200f);
			rect = rect.ContractedBy(18f);
			switch (this.curTab)
			{
			case Dialog_StylingStation.StylingTab.Hair:
				this.DrawStylingItemType<HairDef>(rect, ref this.hairScrollPosition, delegate(Rect r, HairDef h)
				{
					GUI.color = this.pawn.story.hairColor;
					Widgets.DefIcon(r, h, null, 1.25f, null, false, null);
					GUI.color = Color.white;
				}, delegate(HairDef h)
				{
					this.pawn.story.hairDef = h;
				}, (StyleItemDef h) => this.pawn.story.hairDef == h, (StyleItemDef h) => this.initialHairDef == h, null);
				return;
			case Dialog_StylingStation.StylingTab.Beard:
				this.DrawStylingItemType<BeardDef>(rect, ref this.beardScrollPosition, delegate(Rect r, BeardDef b)
				{
					GUI.color = this.pawn.story.hairColor;
					Widgets.DefIcon(r, b, null, 1.25f, null, false, null);
					GUI.color = Color.white;
				}, delegate(BeardDef b)
				{
					this.pawn.style.beardDef = b;
				}, (StyleItemDef b) => this.pawn.style.beardDef == b, (StyleItemDef b) => this.initialBeardDef == b, null);
				return;
			case Dialog_StylingStation.StylingTab.TattooFace:
				this.DrawStylingItemType<TattooDef>(rect, ref this.faceTattooScrollPosition, delegate(Rect r, TattooDef t)
				{
					Widgets.DefIcon(r, t, null, 1f, null, false, null);
				}, delegate(TattooDef t)
				{
					this.pawn.style.FaceTattoo = t;
				}, (StyleItemDef t) => this.pawn.style.FaceTattoo == t, (StyleItemDef t) => this.initialFaceTattoo == t, (StyleItemDef t) => ((TattooDef)t).tattooType == TattooType.Face);
				return;
			case Dialog_StylingStation.StylingTab.TattooBody:
				this.DrawStylingItemType<TattooDef>(rect, ref this.bodyTattooScrollPosition, delegate(Rect r, TattooDef t)
				{
					Widgets.DefIcon(r, t, null, 1f, null, false, null);
				}, delegate(TattooDef t)
				{
					this.pawn.style.BodyTattoo = t;
				}, (StyleItemDef t) => this.pawn.style.BodyTattoo == t, (StyleItemDef t) => this.initialBodyTattoo == t, (StyleItemDef t) => ((TattooDef)t).tattooType == TattooType.Body);
				return;
			case Dialog_StylingStation.StylingTab.ApparelColor:
				this.DrawApparelColor(rect);
				return;
			default:
				return;
			}
		}

		// Token: 0x060074D3 RID: 29907 RVA: 0x0027C980 File Offset: 0x0027AB80
		private void DrawApparelColor(Rect rect)
		{
			bool flag = false;
			Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, this.viewRectHeight);
			Widgets.BeginScrollView(rect, ref this.apparelColorScrollPosition, viewRect, true);
			int num = 0;
			float num2 = rect.y;
			foreach (Apparel apparel in this.pawn.apparel.WornApparel)
			{
				Rect rect2 = new Rect(rect.x, num2, viewRect.width, 92f);
				Color color = this.apparelColors[apparel];
				flag |= Widgets.ColorSelector(rect2, ref color, this.AllColors, apparel.def.uiIcon, 22, 2);
				num2 += rect2.height + 10f;
				if (this.pawn.Ideo != null)
				{
					rect2 = new Rect(rect.x, num2, viewRect.width / 2f - 10f, 24f);
					if (Widgets.ButtonText(rect2, "SetIdeoColor".Translate(), true, true, true))
					{
						flag = true;
						color = this.pawn.Ideo.ApparelColor;
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
				}
				Pawn_StoryTracker story = this.pawn.story;
				if (story != null && story.favoriteColor != null)
				{
					rect2 = new Rect(rect2.xMax + 10f, num2, viewRect.width / 2f - 10f, 24f);
					if (Widgets.ButtonText(rect2, "SetFavoriteColor".Translate(), true, true, true))
					{
						flag = true;
						color = this.pawn.story.favoriteColor.Value;
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
				}
				if (color != apparel.DrawColor)
				{
					num++;
				}
				this.apparelColors[apparel] = color;
				num2 += 34f;
			}
			if (num > 0)
			{
				Widgets.Label(new Rect(rect.x, num2, rect.width, 25f), "Required".Translate() + ": " + num + " " + ThingDefOf.Dye.LabelCap);
				num2 += 25f;
			}
			if (this.pawn.Map.resourceCounter.GetCount(ThingDefOf.Dye) < num)
			{
				Rect rect3 = new Rect(rect.x, num2, rect.width - 16f - 10f, 60f);
				Color color2 = GUI.color;
				GUI.color = ColorLibrary.RedReadable;
				Widgets.Label(rect3, "NotEnoughDye".Translate());
				GUI.color = color2;
				num2 += rect3.height;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.viewRectHeight = num2 - rect.y;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x0027CCA0 File Offset: 0x0027AEA0
		private void DrawStylingItemType<T>(Rect rect, ref Vector2 scrollPosition, Action<Rect, T> drawAction, Action<T> selectAction, Func<StyleItemDef, bool> hasStyleItem, Func<StyleItemDef, bool> hadStyleItem, Func<StyleItemDef, bool> extraValidator = null) where T : StyleItemDef
		{
			Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, this.viewRectHeight);
			int num = Mathf.FloorToInt(viewRect.width / 60f) - 1;
			float num2 = (viewRect.width - (float)num * 60f - (float)(num - 1) * 10f) / 2f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			Dialog_StylingStation.tmpStyleItems.Clear();
			Dialog_StylingStation.tmpStyleItems.AddRange(from x in DefDatabase<T>.AllDefs
			where (this.devEditMode || PawnStyleItemChooser.WantsToUseStyle(this.pawn, x, null) || hadStyleItem(x)) && (extraValidator == null || extraValidator(x))
			select x);
			Dialog_StylingStation.tmpStyleItems.SortBy((StyleItemDef x) => -PawnStyleItemChooser.StyleItemChoiceLikelihoodFor(x, this.pawn));
			if (Dialog_StylingStation.tmpStyleItems.NullOrEmpty<StyleItemDef>())
			{
				Widgets.NoneLabelCenteredVertically(rect, "(" + "NoneUsableForPawn".Translate(this.pawn.Named("PAWN")) + ")");
				return;
			}
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			foreach (StyleItemDef styleItemDef in Dialog_StylingStation.tmpStyleItems)
			{
				if (num5 >= num - 1)
				{
					num5 = 0;
					num4++;
				}
				else if (num3 > 0)
				{
					num5++;
				}
				Rect rect2 = new Rect(rect.x + num2 + (float)num5 * 60f + (float)num5 * 10f, rect.y + (float)num4 * 60f + (float)num4 * 10f, 60f, 60f);
				Widgets.DrawHighlight(rect2);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, styleItemDef.LabelCap);
				}
				if (drawAction != null)
				{
					drawAction(rect2, styleItemDef as T);
				}
				if (hasStyleItem(styleItemDef))
				{
					Widgets.DrawBox(rect2, 2, null);
				}
				if (Widgets.ButtonInvisible(rect2, true))
				{
					if (selectAction != null)
					{
						selectAction(styleItemDef as T);
					}
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.pawn.style.SetGraphicsDirty();
				}
				num3++;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.viewRectHeight = (float)(num4 + 1) * 60f + (float)num4 * 10f + 10f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x0027CF44 File Offset: 0x0027B144
		private void DrawBottomButtons(Rect inRect)
		{
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - Dialog_StylingStation.ButSize.y, Dialog_StylingStation.ButSize.x, Dialog_StylingStation.ButSize.y), "Cancel".Translate(), true, true, true))
			{
				this.Reset(true);
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.xMin + inRect.width / 2f - Dialog_StylingStation.ButSize.x / 2f, inRect.yMax - Dialog_StylingStation.ButSize.y, Dialog_StylingStation.ButSize.x, Dialog_StylingStation.ButSize.y), "Reset".Translate(), true, true, true))
			{
				this.Reset(true);
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(inRect.xMax - Dialog_StylingStation.ButSize.x, inRect.yMax - Dialog_StylingStation.ButSize.y, Dialog_StylingStation.ButSize.x, Dialog_StylingStation.ButSize.y), "Accept".Translate(), true, true, true))
			{
				if (this.pawn.story.hairDef != this.initialHairDef || this.pawn.style.beardDef != this.initialBeardDef || this.pawn.story.hairColor != this.initialHairColor || this.pawn.style.FaceTattoo != this.initialFaceTattoo || this.pawn.style.BodyTattoo != this.initialBodyTattoo)
				{
					if (this.stylingStation != null)
					{
						this.pawn.style.SetupNextLookChangeData(this.pawn.story.hairDef, this.pawn.style.beardDef, this.pawn.style.FaceTattoo, this.pawn.style.BodyTattoo);
						this.Reset(false);
						this.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseStylingStation, this.stylingStation), new JobTag?(JobTag.Misc), false);
					}
					else
					{
						this.pawn.style.Notify_StyleItemChanged();
						this.MakeHairFilth();
					}
				}
				this.ApplyApparelColors();
				this.Close(true);
			}
		}

		// Token: 0x060074D6 RID: 29910 RVA: 0x0027D1B4 File Offset: 0x0027B3B4
		private void ApplyApparelColors()
		{
			foreach (KeyValuePair<Apparel, Color> keyValuePair in this.apparelColors)
			{
				if (keyValuePair.Key.DrawColor != keyValuePair.Value)
				{
					keyValuePair.Key.DesiredColor = new Color?(keyValuePair.Value);
				}
			}
			this.pawn.mindState.Notify_OutfitChanged();
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x0027D244 File Offset: 0x0027B444
		private void MakeHairFilth()
		{
			if (this.pawn.Spawned && (this.pawn.story.hairDef != this.initialHairDef || this.pawn.style.beardDef != this.initialBeardDef))
			{
				this.pawn.style.MakeHairFilth();
			}
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x0027D2A0 File Offset: 0x0027B4A0
		private void Reset(bool resetApparelColors = true)
		{
			if (resetApparelColors)
			{
				this.apparelColors.Clear();
				foreach (Apparel apparel in this.pawn.apparel.WornApparel)
				{
					if (apparel.TryGetComp<CompColorable>() != null)
					{
						this.apparelColors.Add(apparel, apparel.DesiredColor ?? apparel.DrawColor);
					}
				}
			}
			this.pawn.story.hairDef = this.initialHairDef;
			this.pawn.story.hairColor = this.initialHairColor;
			this.pawn.style.beardDef = this.initialBeardDef;
			this.pawn.style.FaceTattoo = this.initialFaceTattoo;
			this.pawn.style.BodyTattoo = this.initialBodyTattoo;
			this.pawn.style.SetGraphicsDirty();
		}

		// Token: 0x0400405C RID: 16476
		private Pawn pawn;

		// Token: 0x0400405D RID: 16477
		private Thing stylingStation;

		// Token: 0x0400405E RID: 16478
		private HairDef initialHairDef;

		// Token: 0x0400405F RID: 16479
		private BeardDef initialBeardDef;

		// Token: 0x04004060 RID: 16480
		private TattooDef initialFaceTattoo;

		// Token: 0x04004061 RID: 16481
		private TattooDef initialBodyTattoo;

		// Token: 0x04004062 RID: 16482
		private Color initialHairColor;

		// Token: 0x04004063 RID: 16483
		private Dialog_StylingStation.StylingTab curTab;

		// Token: 0x04004064 RID: 16484
		private Vector2 hairScrollPosition;

		// Token: 0x04004065 RID: 16485
		private Vector2 beardScrollPosition;

		// Token: 0x04004066 RID: 16486
		private Vector2 faceTattooScrollPosition;

		// Token: 0x04004067 RID: 16487
		private Vector2 bodyTattooScrollPosition;

		// Token: 0x04004068 RID: 16488
		private Vector2 apparelColorScrollPosition;

		// Token: 0x04004069 RID: 16489
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400406A RID: 16490
		private Dictionary<Apparel, Color> apparelColors = new Dictionary<Apparel, Color>();

		// Token: 0x0400406B RID: 16491
		private float viewRectHeight;

		// Token: 0x0400406C RID: 16492
		private bool showHeadgear;

		// Token: 0x0400406D RID: 16493
		private bool showClothes;

		// Token: 0x0400406E RID: 16494
		private bool devEditMode;

		// Token: 0x0400406F RID: 16495
		private List<Color> allColors;

		// Token: 0x04004070 RID: 16496
		private static readonly Vector2 ButSize = new Vector2(200f, 40f);

		// Token: 0x04004071 RID: 16497
		private const float TabMargin = 18f;

		// Token: 0x04004072 RID: 16498
		private const float IconSize = 60f;

		// Token: 0x04004073 RID: 16499
		private const float LeftRectPercent = 0.3f;

		// Token: 0x04004074 RID: 16500
		private const float ApparelRowHeight = 126f;

		// Token: 0x04004075 RID: 16501
		private const float ApparelRowButtonsHeight = 24f;

		// Token: 0x04004076 RID: 16502
		private static List<string> tmpUnwantedStyleNames = new List<string>();

		// Token: 0x04004077 RID: 16503
		private static Dictionary<Apparel, Color> originalApparelColor = new Dictionary<Apparel, Color>();

		// Token: 0x04004078 RID: 16504
		private static List<StyleItemDef> tmpStyleItems = new List<StyleItemDef>();

		// Token: 0x02002685 RID: 9861
		public enum StylingTab
		{
			// Token: 0x0400926F RID: 37487
			Hair,
			// Token: 0x04009270 RID: 37488
			Beard,
			// Token: 0x04009271 RID: 37489
			TattooFace,
			// Token: 0x04009272 RID: 37490
			TattooBody,
			// Token: 0x04009273 RID: 37491
			ApparelColor
		}
	}
}
