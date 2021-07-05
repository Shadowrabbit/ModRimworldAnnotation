using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200180F RID: 6159
	[StaticConstructorOnStartup]
	public class WITab_Caravan_Health : WITab
	{
		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06009032 RID: 36914 RVA: 0x00339EDE File Offset: 0x003380DE
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06009033 RID: 36915 RVA: 0x0033B0C0 File Offset: 0x003392C0
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

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06009034 RID: 36916 RVA: 0x0033B13B File Offset: 0x0033933B
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

		// Token: 0x06009035 RID: 36917 RVA: 0x0033B15B File Offset: 0x0033935B
		public WITab_Caravan_Health()
		{
			this.labelKey = "TabCaravanHealth";
		}

		// Token: 0x06009036 RID: 36918 RVA: 0x0033B170 File Offset: 0x00339370
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

		// Token: 0x06009037 RID: 36919 RVA: 0x0033B224 File Offset: 0x00339424
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

		// Token: 0x06009038 RID: 36920 RVA: 0x0033B280 File Offset: 0x00339480
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
				}, true, false, 1f, null);
			}
		}

		// Token: 0x06009039 RID: 36921 RVA: 0x0033B328 File Offset: 0x00339528
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

		// Token: 0x0600903A RID: 36922 RVA: 0x0033B41C File Offset: 0x0033961C
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

		// Token: 0x0600903B RID: 36923 RVA: 0x0033B4F4 File Offset: 0x003396F4
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

		// Token: 0x0600903C RID: 36924 RVA: 0x0033B564 File Offset: 0x00339764
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 40f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 40f), p);
			}
			curY += 40f;
		}

		// Token: 0x0600903D RID: 36925 RVA: 0x0033B5CC File Offset: 0x003397CC
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
			CaravanThingsTabUtility.DoOpenSpecificTabButtonInvisible(rect2, p, ref this.specificHealthTabForPawn);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null);
			Rect bgRect = new Rect(rect3.xMax + 4f, 11f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float num = bgRect.xMax;
			if (!this.compactMode)
			{
				if (p.RaceProps.IsFlesh)
				{
					Rect rect4 = new Rect(num, 0f, 100f, 40f);
					this.DoPain(rect4, p);
				}
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Rect rect5 = new Rect(num, 0f, 100f, 40f);
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
				Vector2 vector = new Vector2(num + 8f, 8f);
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

		// Token: 0x0600903E RID: 36926 RVA: 0x0033B8BC File Offset: 0x00339ABC
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

		// Token: 0x0600903F RID: 36927 RVA: 0x0033B928 File Offset: 0x00339B28
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

		// Token: 0x06009040 RID: 36928 RVA: 0x0033B996 File Offset: 0x00339B96
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificHealthTabForPawn = null;
		}

		// Token: 0x06009041 RID: 36929 RVA: 0x0033B9A5 File Offset: 0x00339BA5
		private void EnsureSpecificHealthTabForPawnValid()
		{
			if (this.specificHealthTabForPawn != null && (this.specificHealthTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificHealthTabForPawn)))
			{
				this.specificHealthTabForPawn = null;
			}
		}

		// Token: 0x04005AAF RID: 23215
		private Vector2 scrollPosition;

		// Token: 0x04005AB0 RID: 23216
		private float scrollViewHeight;

		// Token: 0x04005AB1 RID: 23217
		private Pawn specificHealthTabForPawn;

		// Token: 0x04005AB2 RID: 23218
		private bool compactMode;

		// Token: 0x04005AB3 RID: 23219
		private static List<PawnCapacityDef> capacitiesToDisplay = new List<PawnCapacityDef>();

		// Token: 0x04005AB4 RID: 23220
		private const float RowHeight = 40f;

		// Token: 0x04005AB5 RID: 23221
		private const float PawnLabelHeight = 18f;

		// Token: 0x04005AB6 RID: 23222
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04005AB7 RID: 23223
		private const float SpaceAroundIcon = 4f;

		// Token: 0x04005AB8 RID: 23224
		private const float PawnCapacityColumnWidth = 100f;

		// Token: 0x04005AB9 RID: 23225
		private const float BeCarriedIfSickColumnWidth = 40f;

		// Token: 0x04005ABA RID: 23226
		private const float BeCarriedIfSickIconSize = 24f;

		// Token: 0x04005ABB RID: 23227
		private static readonly Texture2D BeCarriedIfSickIcon = ContentFinder<Texture2D>.Get("UI/Icons/CarrySick", true);
	}
}
