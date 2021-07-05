using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E6 RID: 4838
	[StaticConstructorOnStartup]
	public class Dialog_EditIdeoStyleItems : Window
	{
		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x060073D0 RID: 29648 RVA: 0x00271C02 File Offset: 0x0026FE02
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(Mathf.Min(1200f, (float)UI.screenWidth), 750f);
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x060073D1 RID: 29649 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060073D2 RID: 29650 RVA: 0x00271C20 File Offset: 0x0026FE20
		public Dialog_EditIdeoStyleItems(Ideo ideo, StyleItemTab category, IdeoEditMode editMode)
		{
			this.ideo = ideo;
			this.curTab = category;
			this.editMode = editMode;
			this.absorbInputAroundWindow = true;
			this.allFrequencies = (StyleItemFrequency[])Enum.GetValues(typeof(StyleItemFrequency));
			this.expandedInfos = new List<Dialog_EditIdeoStyleItems.ExpandedInfo>();
			foreach (Dialog_EditIdeoStyleItems.ItemType itemType in (Dialog_EditIdeoStyleItems.ItemType[])Enum.GetValues(typeof(Dialog_EditIdeoStyleItems.ItemType)))
			{
				foreach (StyleItemCategoryDef styleItemCategoryDef in DefDatabase<StyleItemCategoryDef>.AllDefs)
				{
					this.expandedInfos.Add(new Dialog_EditIdeoStyleItems.ExpandedInfo(styleItemCategoryDef, itemType, this.CanList(styleItemCategoryDef, itemType)));
				}
			}
			this.Reset();
		}

		// Token: 0x060073D3 RID: 29651 RVA: 0x00271D0C File Offset: 0x0026FF0C
		private void Reset()
		{
			this.hairFrequencies = new DefMap<HairDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				this.hairFrequencies[keyValuePair.Key].frequency = this.ideo.style.GetFrequency(keyValuePair.Key);
				this.hairFrequencies[keyValuePair.Key].gender = this.ideo.style.GetGender(keyValuePair.Key);
			}
			this.beardFrequencies = new DefMap<BeardDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				this.beardFrequencies[keyValuePair2.Key].frequency = this.ideo.style.GetFrequency(keyValuePair2.Key);
				this.beardFrequencies[keyValuePair2.Key].gender = this.ideo.style.GetGender(keyValuePair2.Key);
			}
			this.tattooFrequencies = new DefMap<TattooDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				this.tattooFrequencies[keyValuePair3.Key].frequency = this.ideo.style.GetFrequency(keyValuePair3.Key);
				this.tattooFrequencies[keyValuePair3.Key].gender = this.ideo.style.GetGender(keyValuePair3.Key);
			}
		}

		// Token: 0x060073D4 RID: 29652 RVA: 0x00271EF8 File Offset: 0x002700F8
		public override void PostOpen()
		{
			base.PostOpen();
			if (!ModLister.CheckIdeology("Appearance editing"))
			{
				this.Close(true);
			}
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x00271F14 File Offset: 0x00270114
		public override void DoWindowContents(Rect rect)
		{
			this.hover = null;
			Rect rect2 = new Rect(rect);
			rect2.xMin += 18f;
			rect2.yMin += 10f;
			rect2.height = 35f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "EditAppearanceItems".Translate());
			Text.Font = GameFont.Small;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax + 35f;
			rect3.yMax -= Dialog_EditIdeoStyleItems.ButSize.y + 10f;
			this.FillDialog(rect3);
			Rect rect4 = new Rect(rect.xMax - Dialog_EditIdeoStyleItems.ButSize.x - 10f, rect.y + 10f, Dialog_EditIdeoStyleItems.ButSize.x, 30f);
			if (Widgets.ButtonText(rect4, "ExpandAllCategories".Translate().CapitalizeFirst(), true, true, true))
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				foreach (Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo in this.expandedInfos)
				{
					expandedInfo.expanded = true;
				}
			}
			if (Widgets.ButtonText(new Rect(rect4.x, rect4.yMax + 4f, Dialog_EditIdeoStyleItems.ButSize.x, 30f), "CollapseAllCategories".Translate().CapitalizeFirst(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				foreach (Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo2 in this.expandedInfos)
				{
					expandedInfo2.expanded = false;
				}
			}
			rect = rect.ContractedBy(18f);
			float y = rect.height - Dialog_EditIdeoStyleItems.ButSize.y + 18f;
			Rect rect5 = new Rect(rect.x + rect.width / 2f - Dialog_EditIdeoStyleItems.ButSize.x / 2f, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y);
			if (this.editMode == IdeoEditMode.None)
			{
				if (Widgets.ButtonText(rect5, "Back".Translate(), true, true, true))
				{
					this.Close(true);
					return;
				}
			}
			else
			{
				if (Widgets.ButtonText(new Rect(rect.x, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y), "Cancel".Translate(), true, true, true))
				{
					this.Close(true);
				}
				TooltipHandler.TipRegion(rect5, "ResetButtonDesc".Translate());
				if (Widgets.ButtonText(rect5, "Reset".Translate(), true, true, true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.Reset();
				}
				if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditIdeoStyleItems.ButSize.x + 18f, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y), "DoneButton".Translate(), true, true, true))
				{
					this.Done();
				}
			}
		}

		// Token: 0x060073D6 RID: 29654 RVA: 0x00272268 File Offset: 0x00270468
		public override void OnAcceptKeyPressed()
		{
			this.Done();
			Event.current.Use();
		}

		// Token: 0x060073D7 RID: 29655 RVA: 0x0027227C File Offset: 0x0027047C
		private void FillDialog(Rect rect)
		{
			this.DrawTabs(rect);
			rect = rect.ContractedBy(18f);
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 2f - 5f, rect.height);
			this.DrawSection(rect2, ref this.scrollPositionLeft, ref this.scrollViewHeightLeft, (this.curTab == StyleItemTab.HairAndBeard) ? Dialog_EditIdeoStyleItems.ItemType.Hair : Dialog_EditIdeoStyleItems.ItemType.FaceTattoo);
			Rect rect3 = new Rect(rect2.xMax + 10f, rect.y, rect.width / 2f - 10f, rect.height);
			this.DrawSection(rect3, ref this.scrollPositionRight, ref this.scrollViewHeightRight, (this.curTab == StyleItemTab.HairAndBeard) ? Dialog_EditIdeoStyleItems.ItemType.Beard : Dialog_EditIdeoStyleItems.ItemType.BodyTattoo);
			if (!Input.GetMouseButton(0))
			{
				this.painting = false;
			}
		}

		// Token: 0x060073D8 RID: 29656 RVA: 0x00272350 File Offset: 0x00270550
		private void DrawTabs(Rect rect)
		{
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("HairAndBeards".Translate(), delegate()
			{
				this.curTab = StyleItemTab.HairAndBeard;
				this.scrollPositionLeft = (this.scrollPositionRight = Vector2.zero);
			}, this.curTab == StyleItemTab.HairAndBeard));
			this.tabs.Add(new TabRecord("Tattoos".Translate(), delegate()
			{
				this.curTab = StyleItemTab.Tattoo;
				this.scrollPositionLeft = (this.scrollPositionRight = Vector2.zero);
			}, this.curTab == StyleItemTab.Tattoo));
			TabDrawer.DrawTabs<TabRecord>(rect, this.tabs, 200f);
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x002723E4 File Offset: 0x002705E4
		private void DrawSection(Rect rect, ref Vector2 scrollPosition, ref float scrollViewHeight, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect.x + 10f, rect.y, rect.width, 30f), this.GetSectionLabel(itemType).CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			rect.yMin += 30f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			for (int i = 0; i < this.allFrequencies.Length; i++)
			{
				Widgets.Label(this.FrequencyPosition(new Rect(rect.x, rect.y, rect.width, Text.LineHeight), i), this.allFrequencies[i].GetLabel());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			rect.yMin += Text.LineHeight;
			GUI.BeginGroup(rect);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
			float num = 0f;
			float num2 = 28f;
			Widgets.BeginScrollView(rect.AtZero(), ref scrollPosition, viewRect, true);
			using (IEnumerator<StyleItemCategoryDef> enumerator = DefDatabase<StyleItemCategoryDef>.AllDefs.GetEnumerator())
			{
				Func<StyleItemDef, bool> <>9__1;
				while (enumerator.MoveNext())
				{
					StyleItemCategoryDef c = enumerator.Current;
					Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo = this.expandedInfos.FirstOrDefault((Dialog_EditIdeoStyleItems.ExpandedInfo x) => x.categoryDef == c && itemType == x.itemType);
					if (expandedInfo != null && expandedInfo.any)
					{
						this.ListStyleItemCategory(c, ref num, viewRect, expandedInfo, itemType);
						float num3 = num2;
						float num4 = 28f;
						float num5 = 28f;
						IEnumerable<StyleItemDef> itemsInCategory = c.ItemsInCategory;
						Func<StyleItemDef, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((StyleItemDef x) => this.CanList(x, itemType)));
						}
						num2 = num3 + (num4 + num5 * (float)itemsInCategory.Where(predicate).Count<StyleItemDef>());
					}
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = num2;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x060073DA RID: 29658 RVA: 0x0027263C File Offset: 0x0027083C
		private void ListStyleItemCategory(StyleItemCategoryDef category, ref float curY, Rect viewRect, Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Rect rect = new Rect(viewRect.x, viewRect.y + curY, viewRect.width, 28f);
			Widgets.DrawHighlightSelected(rect);
			Rect rect2 = new Rect(viewRect.x, curY, 28f, 28f);
			GUI.DrawTexture(rect2.ContractedBy(4f), expandedInfo.expanded ? Dialog_EditIdeoStyleItems.Minus : Dialog_EditIdeoStyleItems.Plus);
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect3 = new Rect(rect2.xMax + 4f, curY, 110f, 28f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, category.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(new Rect(rect2.x, rect2.y, rect2.width + rect3.width + 4f, 28f), true))
			{
				expandedInfo.expanded = !expandedInfo.expanded;
				if (expandedInfo.expanded)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			this.DrawConfigInfo(viewRect, curY, delegate(Gender g)
			{
				foreach (StyleItemDef styleItemDef2 in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef2, itemType))
					{
						this.ChangeGender(styleItemDef2, g);
					}
				}
			}, delegate(StyleItemFrequency f)
			{
				foreach (StyleItemDef styleItemDef2 in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef2, itemType))
					{
						this.ChangeFrequency(styleItemDef2, f);
					}
				}
			}, new Gender?(this.TryGetGender(category, itemType)), this.TryGetFrequency(category, itemType), (StyleItemFrequency f) => category.ItemsInCategory.Any((StyleItemDef x) => this.CanList(x, itemType) && this.GetFrequency(x) == f));
			curY += rect.height;
			if (expandedInfo.expanded)
			{
				int num = 0;
				foreach (StyleItemDef styleItemDef in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef, itemType))
					{
						this.ListStyleItem(styleItemDef, ref curY, num, viewRect);
						num++;
					}
				}
			}
			curY += 4f;
		}

		// Token: 0x060073DB RID: 29659 RVA: 0x00272858 File Offset: 0x00270A58
		private void ListStyleItem(StyleItemDef styleItem, ref float curY, int index, Rect viewRect)
		{
			Rect rect = new Rect(viewRect.x + 17f, viewRect.y + curY, viewRect.width - 17f, 28f);
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(new Rect(rect.x + 28f, rect.y, rect.width - 28f, rect.height));
			}
			if (Mouse.IsOver(rect))
			{
				this.hover = styleItem;
				Rect r = new Rect(UI.MousePositionOnUI.x + 10f, UI.MousePositionOnUIInverted.y, 100f, 100f + Text.LineHeight);
				Find.WindowStack.ImmediateWindow(12918217, r, WindowLayer.Super, delegate
				{
					Rect rect5 = r.AtZero();
					rect5.height -= Text.LineHeight;
					Widgets.DrawHighlight(rect5);
					if (this.hover != null)
					{
						Text.Anchor = TextAnchor.UpperCenter;
						Widgets.LabelFit(new Rect(0f, rect5.yMax, rect5.width, Text.LineHeight), this.hover.LabelCap);
						Text.Anchor = TextAnchor.UpperLeft;
						float scale = 1.1f;
						if (this.hover is HairDef)
						{
							GUI.color = Dialog_EditIdeoStyleItems.HairColor;
							rect5.y += 10f;
						}
						else if (this.hover is BeardDef)
						{
							GUI.color = Dialog_EditIdeoStyleItems.HairColor;
							rect5.y -= 10f;
						}
						else if (this.hover is TattooDef)
						{
							Widgets.DrawRectFast(rect5, Dialog_EditIdeoStyleItems.SemiSelectedColor, null);
							scale = 1.25f;
						}
						Widgets.DefIcon(rect5, this.hover, null, scale, null, false, null);
						GUI.color = Color.white;
					}
				}, true, false, 1f, null);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect2 = new Rect(rect.x, curY, 28f, 28f);
			Rect rect3 = rect2.ContractedBy(2f);
			Widgets.DrawHighlight(rect3);
			if (styleItem is HairDef || styleItem is BeardDef)
			{
				GUI.color = Dialog_EditIdeoStyleItems.HairColor;
			}
			else if (styleItem is TattooDef)
			{
				Widgets.DrawHighlight(rect3);
			}
			Widgets.DefIcon(rect2, styleItem, null, 1.25f, null, false, null);
			GUI.color = Color.white;
			Rect rect4 = new Rect(rect2.xMax + 4f, curY, 110f, 28f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, styleItem.LabelCap.Truncate(rect4.width, this.labelCache));
			Text.Anchor = TextAnchor.UpperLeft;
			this.DrawConfigInfo(viewRect, curY, delegate(Gender g)
			{
				this.ChangeGender(styleItem, g);
			}, delegate(StyleItemFrequency f)
			{
				this.ChangeFrequency(styleItem, f);
			}, this.GetGender(styleItem), new StyleItemFrequency?(this.GetFrequency(styleItem)), null);
			curY += 28f;
		}

		// Token: 0x060073DC RID: 29660 RVA: 0x00272AA4 File Offset: 0x00270CA4
		private void DrawConfigInfo(Rect viewRect, float curY, Action<Gender> changeGenderAction, Action<StyleItemFrequency> changeFrequencyAction, Gender? gender = null, StyleItemFrequency? curFrequency = null, Func<StyleItemFrequency, bool> semiSelectedValidator = null)
		{
			Rect inRect = new Rect(viewRect.x, curY, viewRect.width + 16f, 28f);
			Rect rect = this.FrequencyPosition(inRect, 0);
			Rect rect2 = new Rect(rect.xMin - rect.width / 2f, curY, 28f, 28f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				if (gender != null)
				{
					TooltipHandler.TipRegion(rect2, Dialog_EditIdeoStyleItems.<DrawConfigInfo>g__GenderLabel|42_0(gender.Value).CapitalizeFirst());
				}
			}
			if (gender != null)
			{
				GUI.DrawTexture(rect2.ContractedBy(4f), gender.Value.GetIcon());
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator enumerator = Enum.GetValues(typeof(Gender)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Gender g = (Gender)enumerator.Current;
						list.Add(new FloatMenuOption(Dialog_EditIdeoStyleItems.<DrawConfigInfo>g__GenderLabel|42_0(g).CapitalizeFirst(), delegate()
						{
							changeGenderAction(g);
						}, g.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			for (int i = 0; i < this.allFrequencies.Length; i++)
			{
				StyleItemFrequency styleItemFrequency = this.allFrequencies[i];
				StyleItemFrequency? styleItemFrequency2 = curFrequency;
				StyleItemFrequency styleItemFrequency3 = styleItemFrequency;
				bool flag = styleItemFrequency2.GetValueOrDefault() == styleItemFrequency3 & styleItemFrequency2 != null;
				Rect rect3 = this.FrequencyPosition(inRect, i);
				Widgets.DrawHighlightIfMouseover(rect3);
				Vector2 vector = rect3.center - new Vector2(12f, 12f);
				if (this.editMode != IdeoEditMode.None)
				{
					Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect3, false);
					if (draggableResult == Widgets.DraggableResult.Dragged)
					{
						this.painting = true;
					}
					Widgets.RadioButton(vector, flag);
					if (!flag && semiSelectedValidator != null && semiSelectedValidator(styleItemFrequency))
					{
						GUI.DrawTexture(new Rect(vector.x, vector.y, 24f, 24f), Dialog_EditIdeoStyleItems.RadioButSemiOn);
					}
					if (!flag && ((this.painting && Mouse.IsOver(rect3)) || draggableResult.AnyPressed()))
					{
						SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
						changeFrequencyAction(styleItemFrequency);
					}
				}
				else if (flag)
				{
					Widgets.RadioButton(vector, flag);
				}
			}
		}

		// Token: 0x060073DD RID: 29661 RVA: 0x00272D58 File Offset: 0x00270F58
		private StyleItemFrequency GetFrequency(StyleItemDef def)
		{
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				return this.hairFrequencies[def2].frequency;
			}
			BeardDef def3;
			if ((def3 = (def as BeardDef)) != null)
			{
				return this.beardFrequencies[def3].frequency;
			}
			TattooDef def4;
			if ((def4 = (def as TattooDef)) != null)
			{
				return this.tattooFrequencies[def4].frequency;
			}
			return StyleItemFrequency.Never;
		}

		// Token: 0x060073DE RID: 29662 RVA: 0x00272DBC File Offset: 0x00270FBC
		private Gender? GetGender(StyleItemDef def)
		{
			StyleGender styleGender = StyleGender.Any;
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				styleGender = this.hairFrequencies[def2].gender;
			}
			else
			{
				if (def is BeardDef)
				{
					return null;
				}
				TattooDef def3;
				if ((def3 = (def as TattooDef)) != null)
				{
					styleGender = this.tattooFrequencies[def3].gender;
				}
			}
			switch (styleGender)
			{
			case StyleGender.Male:
			case StyleGender.MaleUsually:
				return new Gender?(Gender.Male);
			case StyleGender.FemaleUsually:
			case StyleGender.Female:
				return new Gender?(Gender.Female);
			}
			return new Gender?(Gender.None);
		}

		// Token: 0x060073DF RID: 29663 RVA: 0x00272E48 File Offset: 0x00271048
		private void ChangeFrequency(StyleItemDef def, StyleItemFrequency freq)
		{
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				this.hairFrequencies[def2].frequency = freq;
				return;
			}
			BeardDef def3;
			if ((def3 = (def as BeardDef)) != null)
			{
				this.beardFrequencies[def3].frequency = freq;
				return;
			}
			TattooDef def4;
			if ((def4 = (def as TattooDef)) != null)
			{
				this.tattooFrequencies[def4].frequency = freq;
			}
		}

		// Token: 0x060073E0 RID: 29664 RVA: 0x00272EAC File Offset: 0x002710AC
		private void ChangeGender(StyleItemDef def, StyleGender gender)
		{
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				this.hairFrequencies[def2].gender = gender;
				return;
			}
			BeardDef def3;
			if ((def3 = (def as BeardDef)) != null)
			{
				this.beardFrequencies[def3].gender = gender;
				return;
			}
			TattooDef def4;
			if ((def4 = (def as TattooDef)) != null)
			{
				this.tattooFrequencies[def4].gender = gender;
			}
		}

		// Token: 0x060073E1 RID: 29665 RVA: 0x00272F10 File Offset: 0x00271110
		private void ChangeGender(StyleItemDef def, Gender gender)
		{
			StyleGender gender2 = StyleGender.Any;
			if (gender == Gender.Male)
			{
				gender2 = StyleGender.Male;
			}
			else if (gender == Gender.Female)
			{
				gender2 = StyleGender.Female;
			}
			this.ChangeGender(def, gender2);
		}

		// Token: 0x060073E2 RID: 29666 RVA: 0x00272F38 File Offset: 0x00271138
		private bool CanList(StyleItemDef s, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			switch (itemType)
			{
			case Dialog_EditIdeoStyleItems.ItemType.Hair:
				return s is HairDef;
			case Dialog_EditIdeoStyleItems.ItemType.Beard:
				return s is BeardDef;
			case Dialog_EditIdeoStyleItems.ItemType.FaceTattoo:
			{
				TattooDef tattooDef;
				return (tattooDef = (s as TattooDef)) != null && tattooDef.tattooType == TattooType.Face;
			}
			case Dialog_EditIdeoStyleItems.ItemType.BodyTattoo:
			{
				TattooDef tattooDef2;
				return (tattooDef2 = (s as TattooDef)) != null && tattooDef2.tattooType == TattooType.Body;
			}
			default:
				return false;
			}
		}

		// Token: 0x060073E3 RID: 29667 RVA: 0x00272FA0 File Offset: 0x002711A0
		private Dialog_EditIdeoStyleItems.ItemType GetItemType(StyleItemDef s)
		{
			if (s is HairDef)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Hair;
			}
			if (s is BeardDef)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Beard;
			}
			TattooDef tattooDef;
			if ((tattooDef = (s as TattooDef)) == null)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Hair;
			}
			if (tattooDef.tattooType == TattooType.Body)
			{
				return Dialog_EditIdeoStyleItems.ItemType.BodyTattoo;
			}
			return Dialog_EditIdeoStyleItems.ItemType.FaceTattoo;
		}

		// Token: 0x060073E4 RID: 29668 RVA: 0x00272FDC File Offset: 0x002711DC
		private bool CanList(StyleItemCategoryDef category, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			foreach (StyleItemDef s in category.ItemsInCategory)
			{
				if (this.GetItemType(s) == itemType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060073E5 RID: 29669 RVA: 0x0027303C File Offset: 0x0027123C
		private string GetSectionLabel(Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			switch (itemType)
			{
			case Dialog_EditIdeoStyleItems.ItemType.Hair:
				return "Hair".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.Beard:
				return "Beard".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.FaceTattoo:
				return "TattooFace".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.BodyTattoo:
				return "TattooBody".Translate();
			default:
				return string.Empty;
			}
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x002730A8 File Offset: 0x002712A8
		private Gender TryGetGender(StyleItemCategoryDef def, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Gender? gender = null;
			Gender? gender3;
			foreach (StyleItemDef styleItemDef in def.ItemsInCategory)
			{
				if (this.CanList(styleItemDef, itemType))
				{
					if (gender == null)
					{
						gender = this.GetGender(styleItemDef);
					}
					else
					{
						Gender? gender2 = this.GetGender(styleItemDef);
						gender3 = gender;
						if (!(gender2.GetValueOrDefault() == gender3.GetValueOrDefault() & gender2 != null == (gender3 != null)))
						{
							return Gender.None;
						}
					}
				}
			}
			gender3 = gender;
			if (gender3 == null)
			{
				return Gender.None;
			}
			return gender3.GetValueOrDefault();
		}

		// Token: 0x060073E7 RID: 29671 RVA: 0x00273164 File Offset: 0x00271364
		private StyleItemFrequency? TryGetFrequency(StyleItemCategoryDef def, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			StyleItemFrequency? styleItemFrequency = null;
			foreach (StyleItemDef styleItemDef in def.ItemsInCategory)
			{
				if (this.CanList(styleItemDef, itemType))
				{
					if (styleItemFrequency == null)
					{
						styleItemFrequency = new StyleItemFrequency?(this.GetFrequency(styleItemDef));
					}
					else
					{
						StyleItemFrequency frequency = this.GetFrequency(styleItemDef);
						StyleItemFrequency? result = styleItemFrequency;
						if (!(frequency == result.GetValueOrDefault() & result != null))
						{
							result = null;
							return result;
						}
					}
				}
			}
			return styleItemFrequency;
		}

		// Token: 0x060073E8 RID: 29672 RVA: 0x00273208 File Offset: 0x00271408
		private Rect FrequencyPosition(Rect inRect, int index)
		{
			Rect result = inRect;
			result.width = (inRect.width - 178f) / (float)this.allFrequencies.Length - 4f;
			result.x = inRect.x + 178f + result.width * (float)index;
			return result;
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x0027325C File Offset: 0x0027145C
		private void Done()
		{
			if (this.editMode != IdeoEditMode.None)
			{
				foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair.Key, keyValuePair.Value.frequency);
					this.ideo.style.SetGender(keyValuePair.Key, keyValuePair.Value.gender);
				}
				foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair2.Key, keyValuePair2.Value.frequency);
					this.ideo.style.SetGender(keyValuePair2.Key, keyValuePair2.Value.gender);
				}
				foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair3.Key, keyValuePair3.Value.frequency);
					this.ideo.style.SetGender(keyValuePair3.Key, keyValuePair3.Value.gender);
				}
				this.ideo.style.EnsureAtLeastOneStyleItemAvailable();
			}
			this.Close(true);
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x002734D4 File Offset: 0x002716D4
		[CompilerGenerated]
		internal static string <DrawConfigInfo>g__GenderLabel|42_0(Gender g)
		{
			if (g == Gender.None)
			{
				return "MaleAndFemale".Translate();
			}
			return g.GetLabel(false);
		}

		// Token: 0x04003FA0 RID: 16288
		private Ideo ideo;

		// Token: 0x04003FA1 RID: 16289
		private StyleItemDef hover;

		// Token: 0x04003FA2 RID: 16290
		private StyleItemTab curTab;

		// Token: 0x04003FA3 RID: 16291
		private DefMap<HairDef, StyleItemSpawningProperties> hairFrequencies;

		// Token: 0x04003FA4 RID: 16292
		private DefMap<BeardDef, StyleItemSpawningProperties> beardFrequencies;

		// Token: 0x04003FA5 RID: 16293
		private DefMap<TattooDef, StyleItemSpawningProperties> tattooFrequencies;

		// Token: 0x04003FA6 RID: 16294
		private List<Dialog_EditIdeoStyleItems.ExpandedInfo> expandedInfos;

		// Token: 0x04003FA7 RID: 16295
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04003FA8 RID: 16296
		private Vector2 scrollPositionLeft;

		// Token: 0x04003FA9 RID: 16297
		private Vector2 scrollPositionRight;

		// Token: 0x04003FAA RID: 16298
		private float scrollViewHeightLeft;

		// Token: 0x04003FAB RID: 16299
		private float scrollViewHeightRight;

		// Token: 0x04003FAC RID: 16300
		private bool painting;

		// Token: 0x04003FAD RID: 16301
		private StyleItemFrequency[] allFrequencies;

		// Token: 0x04003FAE RID: 16302
		private IdeoEditMode editMode;

		// Token: 0x04003FAF RID: 16303
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04003FB0 RID: 16304
		private static readonly Color HairColor = PawnHairColors.ReddishBrown;

		// Token: 0x04003FB1 RID: 16305
		private static readonly Color SemiSelectedColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04003FB2 RID: 16306
		private const float HeaderHeight = 35f;

		// Token: 0x04003FB3 RID: 16307
		private const float TabsSpacing = 45f;

		// Token: 0x04003FB4 RID: 16308
		private const float ItemHeight = 28f;

		// Token: 0x04003FB5 RID: 16309
		private const float ItemLabelWidth = 110f;

		// Token: 0x04003FB6 RID: 16310
		private static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x04003FB7 RID: 16311
		private static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x04003FB8 RID: 16312
		private static readonly Texture2D RadioButSemiOn = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButSemiOn", true);

		// Token: 0x04003FB9 RID: 16313
		private Dictionary<string, string> labelCache = new Dictionary<string, string>();

		// Token: 0x02002651 RID: 9809
		public enum ItemType
		{
			// Token: 0x040091E5 RID: 37349
			Hair,
			// Token: 0x040091E6 RID: 37350
			Beard,
			// Token: 0x040091E7 RID: 37351
			FaceTattoo,
			// Token: 0x040091E8 RID: 37352
			BodyTattoo
		}

		// Token: 0x02002652 RID: 9810
		public class ExpandedInfo
		{
			// Token: 0x0600D5EE RID: 54766 RVA: 0x00408A1A File Offset: 0x00406C1A
			public ExpandedInfo(StyleItemCategoryDef categoryDef, Dialog_EditIdeoStyleItems.ItemType itemType, bool any)
			{
				this.categoryDef = categoryDef;
				this.itemType = itemType;
				this.any = any;
				this.expanded = false;
			}

			// Token: 0x040091E9 RID: 37353
			public StyleItemCategoryDef categoryDef;

			// Token: 0x040091EA RID: 37354
			public Dialog_EditIdeoStyleItems.ItemType itemType;

			// Token: 0x040091EB RID: 37355
			public bool any;

			// Token: 0x040091EC RID: 37356
			public bool expanded;
		}
	}
}
