using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021D5 RID: 8661
	[StaticConstructorOnStartup]
	public class WITab_Caravan_Health : WITab
	{
		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x0600B96D RID: 47469 RVA: 0x00077FF1 File Offset: 0x000761F1
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x0600B96E RID: 47470 RVA: 0x0035593C File Offset: 0x00353B3C
		private List<PawnCapacityDef> CapacitiesToDisplay
		{
			get
			{
				WITab_Caravan_Health.capacitiesToDisplay.Clear();
				List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].showOnCaravanHealthTab)
					{
						WITab_Caravan_Health.capacitiesToDisplay.Add(allDefsListForReading[i]);
					}
				}
				WITab_Caravan_Health.capacitiesToDisplay.SortBy((PawnCapacityDef x) => x.listOrder);
				return WITab_Caravan_Health.capacitiesToDisplay;
			}
		}

		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x0600B96F RID: 47471 RVA: 0x000780F9 File Offset: 0x000762F9
		private float SpecificHealthTabWidth
		{
			get
			{
				this.EnsureSpecificHealthTabForPawnValid();
				if (this.specificHealthTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 630f;
			}
		}

		// Token: 0x0600B970 RID: 47472 RVA: 0x00078119 File Offset: 0x00076319
		public WITab_Caravan_Health()
		{
			this.labelKey = "TabCaravanHealth";
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x003559B8 File Offset: 0x00353BB8
		protected override void FillTab()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoColumnHeaders(ref num);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600B972 RID: 47474 RVA: 0x00355A6C File Offset: 0x00353C6C
		protected override void UpdateSize()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.UpdateSize();
			this.size = this.GetRawSize(false);
			if (this.size.x + this.SpecificHealthTabWidth > (float)UI.screenWidth)
			{
				this.compactMode = true;
				this.size = this.GetRawSize(true);
				return;
			}
			this.compactMode = false;
		}

		// Token: 0x0600B973 RID: 47475 RVA: 0x00355AC8 File Offset: 0x00353CC8
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificHealthTabForPawn = this.specificHealthTabForPawn;
			if (localSpecificHealthTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificHealthTabWidth = this.SpecificHealthTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificHealthTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificHealthTabForPawn.DestroyedOrNull())
					{
						return;
					}
					HealthCardUtility.DrawPawnHealthCard(new Rect(0f, 20f, rect.width, rect.height - 20f), localSpecificHealthTabForPawn, false, true, localSpecificHealthTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificHealthTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f);
			}
		}

		// Token: 0x0600B974 RID: 47476 RVA: 0x00355B70 File Offset: 0x00353D70
		private void DoColumnHeaders(ref float curY)
		{
			if (!this.compactMode)
			{
				float num = 135f;
				Text.Anchor = TextAnchor.UpperCenter;
				GUI.color = Widgets.SeparatorLabelColor;
				Widgets.Label(new Rect(num, 3f, 100f, 100f), "Pain".Translate());
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Widgets.Label(new Rect(num, 3f, 100f, 100f), list[i].LabelCap.Truncate(100f, null));
					num += 100f;
				}
				Rect rect = new Rect(num + 8f, 0f, 24f, 24f);
				GUI.DrawTexture(rect, WITab_Caravan_Health.BeCarriedIfSickIcon);
				TooltipHandler.TipRegionByKey(rect, "BeCarriedIfSickTip");
				num += 40f;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600B975 RID: 47477 RVA: 0x00355C64 File Offset: 0x00353E64
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			if (this.specificHealthTabForPawn != null && !pawns.Contains(this.specificHealthTabForPawn))
			{
				this.specificHealthTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (!pawn2.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x0600B976 RID: 47478 RVA: 0x00355D3C File Offset: 0x00353F3C
		private Vector2 GetRawSize(bool compactMode)
		{
			float num = 100f;
			if (!compactMode)
			{
				num += 100f;
				num += (float)this.CapacitiesToDisplay.Count * 100f;
				num += 40f;
			}
			Vector2 result;
			result.x = 127f + num + 16f;
			result.y = Mathf.Min(550f, this.PaneTopY - 30f);
			return result;
		}

		// Token: 0x0600B977 RID: 47479 RVA: 0x00355DAC File Offset: 0x00353FAC
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 50f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 50f), p);
			}
			curY += 50f;
		}

		// Token: 0x0600B978 RID: 47480 RVA: 0x00355E14 File Offset: 0x00354014
		private void DoRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificHealthTabForPawn);
			rect2.width -= 24f;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float num = bgRect.xMax;
			if (!this.compactMode)
			{
				if (p.RaceProps.IsFlesh)
				{
					Rect rect4 = new Rect(num, 0f, 100f, 50f);
					this.DoPain(rect4, p);
				}
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Rect rect5 = new Rect(num, 0f, 100f, 50f);
					if ((p.RaceProps.Humanlike && !list[i].showOnHumanlikes) || (p.RaceProps.Animal && !list[i].showOnAnimals) || (p.RaceProps.IsMechanoid && !list[i].showOnMechanoids) || !PawnCapacityUtility.BodyCanEverDoCapacity(p.RaceProps.body, list[i]))
					{
						num += 100f;
					}
					else
					{
						this.DoCapacity(rect5, p, list[i]);
						num += 100f;
					}
				}
			}
			if (!this.compactMode)
			{
				Vector2 vector = new Vector2(num + 8f, 13f);
				Widgets.Checkbox(vector, ref p.health.beCarriedByCaravanIfSick, 24f, false, true, null, null);
				TooltipHandler.TipRegionByKey(new Rect(vector, new Vector2(24f, 24f)), "BeCarriedIfSickTip");
				num += 40f;
			}
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600B979 RID: 47481 RVA: 0x003560F0 File Offset: 0x003542F0
		private void DoPain(Rect rect, Pawn pawn)
		{
			Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(pawn);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = painLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, painLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string painTip = HealthCardUtility.GetPainTip(pawn);
				TooltipHandler.TipRegion(rect, painTip);
			}
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x0035615C File Offset: 0x0035435C
		private void DoCapacity(Rect rect, Pawn pawn, PawnCapacityDef capacity)
		{
			Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, capacity);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = efficiencyLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, efficiencyLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string pawnCapacityTip = HealthCardUtility.GetPawnCapacityTip(pawn, capacity);
				TooltipHandler.TipRegion(rect, pawnCapacityTip);
			}
		}

		// Token: 0x0600B97B RID: 47483 RVA: 0x0007812C File Offset: 0x0007632C
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificHealthTabForPawn = null;
		}

		// Token: 0x0600B97C RID: 47484 RVA: 0x0007813B File Offset: 0x0007633B
		private void EnsureSpecificHealthTabForPawnValid()
		{
			if (this.specificHealthTabForPawn != null && (this.specificHealthTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificHealthTabForPawn)))
			{
				this.specificHealthTabForPawn = null;
			}
		}

		// Token: 0x04007EAD RID: 32429
		private Vector2 scrollPosition;

		// Token: 0x04007EAE RID: 32430
		private float scrollViewHeight;

		// Token: 0x04007EAF RID: 32431
		private Pawn specificHealthTabForPawn;

		// Token: 0x04007EB0 RID: 32432
		private bool compactMode;

		// Token: 0x04007EB1 RID: 32433
		private static List<PawnCapacityDef> capacitiesToDisplay = new List<PawnCapacityDef>();

		// Token: 0x04007EB2 RID: 32434
		private const float RowHeight = 50f;

		// Token: 0x04007EB3 RID: 32435
		private const float PawnLabelHeight = 18f;

		// Token: 0x04007EB4 RID: 32436
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04007EB5 RID: 32437
		private const float SpaceAroundIcon = 4f;

		// Token: 0x04007EB6 RID: 32438
		private const float PawnCapacityColumnWidth = 100f;

		// Token: 0x04007EB7 RID: 32439
		private const float BeCarriedIfSickColumnWidth = 40f;

		// Token: 0x04007EB8 RID: 32440
		private const float BeCarriedIfSickIconSize = 24f;

		// Token: 0x04007EB9 RID: 32441
		private static readonly Texture2D BeCarriedIfSickIcon = ContentFinder<Texture2D>.Get("UI/Icons/CarrySick", true);
	}
}
