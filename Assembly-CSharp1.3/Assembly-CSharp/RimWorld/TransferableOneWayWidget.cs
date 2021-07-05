using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001317 RID: 4887
	[StaticConstructorOnStartup]
	public class TransferableOneWayWidget
	{
		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x060075DD RID: 30173 RVA: 0x002891CC File Offset: 0x002873CC
		public float TotalNumbersColumnsWidths
		{
			get
			{
				float num = 315f;
				if (this.drawMass)
				{
					num += 100f;
				}
				if (this.drawMarketValue)
				{
					num += 100f;
				}
				if (this.drawDaysUntilRot)
				{
					num += 75f;
				}
				if (this.drawItemNutrition)
				{
					num += 75f;
				}
				if (this.drawNutritionEatenPerDay)
				{
					num += 75f;
				}
				if (this.drawForagedFoodPerDay)
				{
					num += 75f;
				}
				return num;
			}
		}

		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x060075DE RID: 30174 RVA: 0x00289240 File Offset: 0x00287440
		private bool AnyTransferable
		{
			get
			{
				if (!this.transferablesCached)
				{
					this.CacheTransferables();
				}
				for (int i = 0; i < this.sections.Count; i++)
				{
					if (this.sections[i].cachedTransferables.Any<TransferableOneWay>())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x0028928C File Offset: 0x0028748C
		public TransferableOneWayWidget(IEnumerable<TransferableOneWay> transferables, string sourceLabel, string destinationLabel, string sourceCountDesc, bool drawMass = false, IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore, bool includePawnsMassInMassUsage = false, Func<float> availableMassGetter = null, float extraHeaderSpace = 0f, bool ignoreSpawnedCorpseGearAndInventoryMass = false, int tile = -1, bool drawMarketValue = false, bool drawEquippedWeapon = false, bool drawNutritionEatenPerDay = false, bool drawItemNutrition = false, bool drawForagedFoodPerDay = false, bool drawDaysUntilRot = false, bool playerPawnsReadOnly = false, bool drawIdeo = false)
		{
			if (transferables != null)
			{
				this.AddSection(null, transferables);
			}
			this.sourceLabel = sourceLabel;
			this.destinationLabel = destinationLabel;
			this.sourceCountDesc = sourceCountDesc;
			this.drawMass = drawMass;
			this.ignorePawnInventoryMass = ignorePawnInventoryMass;
			this.includePawnsMassInMassUsage = includePawnsMassInMassUsage;
			this.availableMassGetter = availableMassGetter;
			this.extraHeaderSpace = extraHeaderSpace;
			this.ignoreSpawnedCorpseGearAndInventoryMass = ignoreSpawnedCorpseGearAndInventoryMass;
			this.tile = tile;
			this.drawMarketValue = drawMarketValue;
			this.drawEquippedWeapon = drawEquippedWeapon;
			this.drawNutritionEatenPerDay = drawNutritionEatenPerDay;
			this.drawItemNutrition = drawItemNutrition;
			this.drawForagedFoodPerDay = drawForagedFoodPerDay;
			this.drawDaysUntilRot = drawDaysUntilRot;
			this.playerPawnsReadOnly = playerPawnsReadOnly;
			this.drawIdeo = drawIdeo;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x002893A0 File Offset: 0x002875A0
		public void AddSection(string title, IEnumerable<TransferableOneWay> transferables)
		{
			TransferableOneWayWidget.Section item = default(TransferableOneWayWidget.Section);
			item.title = title;
			item.transferables = transferables;
			item.cachedTransferables = new List<TransferableOneWay>();
			this.sections.Add(item);
			this.transferablesCached = false;
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x002893E4 File Offset: 0x002875E4
		private void CacheTransferables()
		{
			this.transferablesCached = true;
			for (int i = 0; i < this.sections.Count; i++)
			{
				List<TransferableOneWay> cachedTransferables = this.sections[i].cachedTransferables;
				cachedTransferables.Clear();
				cachedTransferables.AddRange((from tr in this.sections[i].transferables
				where this.quickSearchWidget.filter.Matches(tr.Label)
				select tr).OrderBy((TransferableOneWay tr) => tr, this.sorter1.Comparer).ThenBy((TransferableOneWay tr) => tr, this.sorter2.Comparer).ThenBy((TransferableOneWay tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableOneWay>());
			}
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x002894DC File Offset: 0x002876DC
		public void OnGUI(Rect inRect)
		{
			bool flag;
			this.OnGUI(inRect, out flag);
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x002894F4 File Offset: 0x002876F4
		public void OnGUI(Rect inRect, out bool anythingChanged)
		{
			if (!this.transferablesCached)
			{
				this.CacheTransferables();
			}
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTransferables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTransferables();
			});
			this.quickSearchWidget.noResultsMatched = !this.AnyTransferable;
			TransferableUIUtility.DoTransferableSearcher(this.quickSearchWidget, new Action(this.CacheTransferables));
			if (!this.sourceLabel.NullOrEmpty() || !this.destinationLabel.NullOrEmpty())
			{
				float num = inRect.width - 515f;
				Rect position = new Rect(inRect.x + num, inRect.y, inRect.width - num, 37f);
				GUI.BeginGroup(position);
				Text.Font = GameFont.Medium;
				if (!this.sourceLabel.NullOrEmpty())
				{
					Rect rect = new Rect(0f, 0f, position.width / 2f, position.height);
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, this.sourceLabel);
				}
				if (!this.destinationLabel.NullOrEmpty())
				{
					Rect rect2 = new Rect(position.width / 2f, 0f, position.width / 2f, position.height);
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect2, this.destinationLabel);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.EndGroup();
			}
			Rect mainRect = new Rect(inRect.x, inRect.y + 37f + this.extraHeaderSpace, inRect.width, inRect.height - 37f - this.extraHeaderSpace);
			this.FillMainRect(mainRect, out anythingChanged);
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x002896A0 File Offset: 0x002878A0
		private void FillMainRect(Rect mainRect, out bool anythingChanged)
		{
			anythingChanged = false;
			Text.Font = GameFont.Small;
			if (this.AnyTransferable)
			{
				float num = 6f;
				for (int i = 0; i < this.sections.Count; i++)
				{
					num += (float)this.sections[i].cachedTransferables.Count * 30f;
					if (this.sections[i].title != null)
					{
						num += 30f;
					}
				}
				float num2 = 6f;
				float availableMass = (this.availableMassGetter != null) ? this.availableMassGetter() : float.MaxValue;
				Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, num);
				Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
				float num3 = this.scrollPosition.y - 30f;
				float num4 = this.scrollPosition.y + mainRect.height;
				for (int j = 0; j < this.sections.Count; j++)
				{
					List<TransferableOneWay> cachedTransferables = this.sections[j].cachedTransferables;
					if (cachedTransferables.Any<TransferableOneWay>())
					{
						if (this.sections[j].title != null)
						{
							Widgets.ListSeparator(ref num2, viewRect.width, this.sections[j].title);
							num2 += 5f;
						}
						for (int k = 0; k < cachedTransferables.Count; k++)
						{
							if (num2 > num3 && num2 < num4)
							{
								Rect rect = new Rect(0f, num2, viewRect.width, 30f);
								int countToTransfer = cachedTransferables[k].CountToTransfer;
								this.DoRow(rect, cachedTransferables[k], k, availableMass);
								if (countToTransfer != cachedTransferables[k].CountToTransfer)
								{
									anythingChanged = true;
								}
							}
							num2 += 30f;
						}
					}
				}
				Widgets.EndScrollView();
				return;
			}
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(mainRect, "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x002898BC File Offset: 0x00287ABC
		private void DoRow(Rect rect, TransferableOneWay trad, int index, float availableMass)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float num = rect.width;
			int maxCount = trad.MaxCount;
			Rect rect2 = new Rect(num - 240f, 0f, 240f, rect.height);
			TransferableOneWayWidget.stoppingPoints.Clear();
			if (this.availableMassGetter != null && (!(trad.AnyThing is Pawn) || this.includePawnsMassInMassUsage))
			{
				float num2 = availableMass + this.GetMass(trad.AnyThing) * (float)trad.CountToTransfer;
				int threshold = (num2 <= 0f) ? 0 : Mathf.FloorToInt(num2 / this.GetMass(trad.AnyThing));
				TransferableOneWayWidget.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(threshold, "M<", ">M"));
			}
			Pawn pawn = trad.AnyThing as Pawn;
			bool flag = pawn != null && (pawn.IsColonist || pawn.IsPrisonerOfColony);
			TransferableUIUtility.DoCountAdjustInterface(rect2, trad, index, 0, maxCount, false, TransferableOneWayWidget.stoppingPoints, (this.playerPawnsReadOnly && flag) || this.readOnly);
			num -= 240f;
			if (this.drawMarketValue)
			{
				Rect rect3 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMarketValue(rect3, trad);
				num -= 100f;
			}
			if (this.drawMass)
			{
				Rect rect4 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMass(rect4, trad, availableMass);
				num -= 100f;
			}
			if (this.drawDaysUntilRot)
			{
				Rect rect5 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawDaysUntilRot(rect5, trad);
				num -= 75f;
			}
			if (this.drawItemNutrition)
			{
				Rect rect6 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawItemNutrition(rect6, trad);
				num -= 75f;
			}
			if (this.drawForagedFoodPerDay)
			{
				Rect rect7 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				if (!this.DrawGrazeability(rect7, trad))
				{
					this.DrawForagedFoodPerDay(rect7, trad);
				}
				num -= 75f;
			}
			if (this.drawNutritionEatenPerDay)
			{
				Rect rect8 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawNutritionEatenPerDay(rect8, trad);
				num -= 75f;
			}
			if (this.ShouldShowCount(trad))
			{
				Rect rect9 = new Rect(num - 75f, 0f, 75f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect9);
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect10 = rect9;
				rect10.xMin += 5f;
				rect10.xMax -= 5f;
				Widgets.Label(rect10, maxCount.ToStringCached());
				TooltipHandler.TipRegion(rect9, this.sourceCountDesc);
			}
			num -= 75f;
			if (this.drawIdeo)
			{
				if (pawn != null && pawn.Ideo != null)
				{
					Rect rect11 = new Rect(num - 30f, 0f, 30f, rect.height);
					Widgets.DrawHighlightIfMouseover(rect11);
					GUI.DrawTexture(rect11, pawn.Ideo.Icon);
					TooltipHandler.TipRegion(rect11, pawn.Ideo.name);
				}
				num -= 30f;
			}
			if (pawn != null && pawn.IsSlave)
			{
				Rect rect12 = new Rect(num - 30f, 0f, 30f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect12);
				GUI.DrawTexture(rect12, pawn.guest.GetIcon());
				TooltipHandler.TipRegion(rect12, pawn.guest.GetLabel());
				num -= 30f;
			}
			if (this.drawEquippedWeapon)
			{
				Rect rect13 = new Rect(num - 30f, 0f, 30f, rect.height);
				Rect iconRect = new Rect(num - 30f, (rect.height - 30f) / 2f, 30f, 30f);
				this.DrawEquippedWeapon(rect13, iconRect, trad);
				num -= 30f;
			}
			TransferableUIUtility.DoExtraAnimalIcons(trad, rect, ref num);
			Rect idRect = new Rect(0f, 0f, num, rect.height);
			Color color;
			if (pawn != null)
			{
				GuestStatus? guestStatus = pawn.GuestStatus;
				GuestStatus guestStatus2 = GuestStatus.Slave;
				if (guestStatus.GetValueOrDefault() == guestStatus2 & guestStatus != null)
				{
					color = PawnNameColorUtility.PawnNameColorOf(pawn);
					goto IL_487;
				}
			}
			color = Color.white;
			IL_487:
			Color labelColor = color;
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, labelColor);
			GenUI.ResetLabelAlign();
			GUI.EndGroup();
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x00289D68 File Offset: 0x00287F68
		private bool ShouldShowCount(TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return true;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			return pawn == null || !pawn.RaceProps.Humanlike || trad.MaxCount != 1;
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x00289DAC File Offset: 0x00287FAC
		private void DrawDaysUntilRot(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			if (!trad.ThingDef.IsNutritionGivingIngestible)
			{
				return;
			}
			int num;
			if (!this.cachedTicksUntilRot.TryGetValue(trad, out num))
			{
				num = int.MaxValue;
				for (int i = 0; i < trad.things.Count; i++)
				{
					CompRottable compRottable = trad.things[i].TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						num = Mathf.Min(num, DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.tile, null));
					}
				}
				this.cachedTicksUntilRot.Add(trad, num);
			}
			if (num >= 36000000 || (float)num >= 36000000f)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			float num2 = (float)num / 60000f;
			GUI.color = Color.yellow;
			Widgets.Label(rect, num2.ToString("0.#"));
			GUI.color = Color.white;
			TooltipHandler.TipRegionByKey(rect, "DaysUntilRotTip");
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x00289E84 File Offset: 0x00288084
		private void DrawItemNutrition(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			if (!trad.ThingDef.IsNutritionGivingIngestible)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.color = Color.green;
			Widgets.Label(rect, trad.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##"));
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "ItemNutritionTip".Translate((1.6f * ThingDefOf.Human.race.baseHungerRate).ToString("0.##")));
			}
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x00289F2C File Offset: 0x0028812C
		private bool DrawGrazeability(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return false;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || !VirtualPlantsUtility.CanEverEatVirtualPlants(pawn))
			{
				return false;
			}
			rect.width = 40f;
			Rect position = new Rect(rect.x + (float)((int)((rect.width - 28f) / 2f)), rect.y + (float)((int)((rect.height - 28f) / 2f)), 28f, 28f);
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.DrawTexture(position, TransferableOneWayWidget.CanGrazeIcon);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, delegate()
				{
					TaggedString taggedString = "AnimalCanGrazeTip".Translate();
					if (this.tile != -1)
					{
						taggedString += "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(this.tile, Find.TickManager.TicksAbs);
					}
					return taggedString;
				}, trad.GetHashCode() ^ 1948571634);
			}
			return true;
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x00289FE8 File Offset: 0x002881E8
		private void DrawForagedFoodPerDay(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn p = trad.AnyThing as Pawn;
			if (p == null)
			{
				return;
			}
			bool flag;
			float foragedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(p, out flag);
			if (flag)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.color = ((foragedNutritionPerDay == 0f) ? Color.gray : Color.green);
			Widgets.Label(rect, "+" + foragedNutritionPerDay.ToString("0.##"));
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "NutritionForagedPerDayTip".Translate(StatDefOf.ForagedNutritionPerDay.Worker.GetExplanationFull(StatRequest.For(p), StatDefOf.ForagedNutritionPerDay.toStringNumberSense, foragedNutritionPerDay)), trad.GetHashCode() ^ 1958671422);
			}
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x0028A0AC File Offset: 0x002882AC
		private void DrawNutritionEatenPerDay(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn p = trad.AnyThing as Pawn;
			if (p == null || !p.RaceProps.EatsFood || p.Dead || p.needs.food == null)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			string text = (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true) * 60000f).ToString("0.##");
			DietCategory resolvedDietCategory = p.RaceProps.ResolvedDietCategory;
			if (resolvedDietCategory != DietCategory.Omnivorous)
			{
				text = text + " (" + resolvedDietCategory.ToStringHumanShort() + ")";
			}
			GUI.color = new Color(1f, 0.5f, 0f);
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => RaceProperties.NutritionEatenPerDayExplanation(p, true, true, false), trad.GetHashCode() ^ 385968958);
			}
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x0028A1C0 File Offset: 0x002883C0
		private void DrawMarketValue(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Widgets.Label(rect, trad.AnyThing.MarketValue.ToStringMoney(null));
			TooltipHandler.TipRegionByKey(rect, "MarketValueTip");
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x0028A1F4 File Offset: 0x002883F4
		private void DrawMass(Rect rect, TransferableOneWay trad, float availableMass)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Thing anyThing = trad.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null && !this.includePawnsMassInMassUsage && !MassUtility.CanEverCarryAnything(pawn))
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (pawn == null || this.includePawnsMassInMassUsage)
			{
				float mass = this.GetMass(anyThing);
				if (Mouse.IsOver(rect))
				{
					if (pawn != null)
					{
						float gearMass = 0f;
						float invMass = 0f;
						gearMass = MassUtility.GearMass(pawn);
						if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
						{
							invMass = MassUtility.InventoryMass(pawn);
						}
						TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, 0f, mass - gearMass - invMass, gearMass, invMass), trad.GetHashCode() * 59);
					}
					else
					{
						TooltipHandler.TipRegion(rect, "ItemWeightTip".Translate());
					}
				}
				if (mass > availableMass)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				else
				{
					GUI.color = TransferableOneWayWidget.ItemMassColor;
				}
				Widgets.Label(rect, mass.ToStringMass());
			}
			else
			{
				float cap = MassUtility.Capacity(pawn, null);
				float gearMass = MassUtility.GearMass(pawn);
				float invMass = InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass) ? 0f : MassUtility.InventoryMass(pawn);
				float num = cap - gearMass - invMass;
				if (num > 0f)
				{
					GUI.color = Color.green;
				}
				else if (num < 0f)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				else
				{
					GUI.color = Color.gray;
				}
				Widgets.Label(rect, num.ToStringMassOffset());
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, cap, 0f, gearMass, invMass), trad.GetHashCode() * 59);
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x0028A42C File Offset: 0x0028862C
		private void DrawEquippedWeapon(Rect rect, Rect iconRect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || pawn.equipment == null || pawn.equipment.Primary == null)
			{
				return;
			}
			ThingWithComps primary = pawn.equipment.Primary;
			Widgets.DrawHighlightIfMouseover(rect);
			Widgets.ThingIcon(iconRect, primary, 1f, null);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, primary.LabelCap);
			}
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x0028A4A8 File Offset: 0x002886A8
		private string GetPawnMassTip(TransferableOneWay trad, float capacity, float pawnMass, float gearMass, float invMass)
		{
			if (!trad.HasAnyThing)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (capacity != 0f)
			{
				stringBuilder.Append("MassCapacity".Translate() + ": " + capacity.ToStringMass());
			}
			else
			{
				stringBuilder.Append("Mass".Translate() + ": " + pawnMass.ToStringMass());
			}
			if (gearMass != 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("EquipmentAndApparelMass".Translate() + ": " + gearMass.ToStringMass());
			}
			if (invMass != 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("InventoryMass".Translate() + ": " + invMass.ToStringMass());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060075F0 RID: 30192 RVA: 0x0028A5A8 File Offset: 0x002887A8
		private float GetMass(Thing thing)
		{
			if (thing == null)
			{
				return 0f;
			}
			float num = thing.GetStatValue(StatDefOf.Mass, true);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
				{
					num -= MassUtility.InventoryMass(pawn);
				}
			}
			else if (this.ignoreSpawnedCorpseGearAndInventoryMass)
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.Spawned)
				{
					num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn);
				}
			}
			return num;
		}

		// Token: 0x04004126 RID: 16678
		private List<TransferableOneWayWidget.Section> sections = new List<TransferableOneWayWidget.Section>();

		// Token: 0x04004127 RID: 16679
		private string sourceLabel;

		// Token: 0x04004128 RID: 16680
		private string destinationLabel;

		// Token: 0x04004129 RID: 16681
		private string sourceCountDesc;

		// Token: 0x0400412A RID: 16682
		private bool drawMass;

		// Token: 0x0400412B RID: 16683
		private IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore;

		// Token: 0x0400412C RID: 16684
		private bool includePawnsMassInMassUsage;

		// Token: 0x0400412D RID: 16685
		private Func<float> availableMassGetter;

		// Token: 0x0400412E RID: 16686
		public float extraHeaderSpace;

		// Token: 0x0400412F RID: 16687
		private bool ignoreSpawnedCorpseGearAndInventoryMass;

		// Token: 0x04004130 RID: 16688
		private int tile;

		// Token: 0x04004131 RID: 16689
		private bool drawMarketValue;

		// Token: 0x04004132 RID: 16690
		private bool drawEquippedWeapon;

		// Token: 0x04004133 RID: 16691
		private bool drawNutritionEatenPerDay;

		// Token: 0x04004134 RID: 16692
		private bool drawItemNutrition;

		// Token: 0x04004135 RID: 16693
		private bool drawForagedFoodPerDay;

		// Token: 0x04004136 RID: 16694
		private bool drawDaysUntilRot;

		// Token: 0x04004137 RID: 16695
		private bool playerPawnsReadOnly;

		// Token: 0x04004138 RID: 16696
		public bool readOnly;

		// Token: 0x04004139 RID: 16697
		public bool drawIdeo;

		// Token: 0x0400413A RID: 16698
		private bool transferablesCached;

		// Token: 0x0400413B RID: 16699
		private Vector2 scrollPosition;

		// Token: 0x0400413C RID: 16700
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400413D RID: 16701
		private TransferableSorterDef sorter1;

		// Token: 0x0400413E RID: 16702
		private TransferableSorterDef sorter2;

		// Token: 0x0400413F RID: 16703
		private Dictionary<TransferableOneWay, int> cachedTicksUntilRot = new Dictionary<TransferableOneWay, int>();

		// Token: 0x04004140 RID: 16704
		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		// Token: 0x04004141 RID: 16705
		public const float TopAreaHeight = 37f;

		// Token: 0x04004142 RID: 16706
		protected readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x04004143 RID: 16707
		protected readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04004144 RID: 16708
		private const float ColumnWidth = 120f;

		// Token: 0x04004145 RID: 16709
		private const float FirstTransferableY = 6f;

		// Token: 0x04004146 RID: 16710
		private const float RowInterval = 30f;

		// Token: 0x04004147 RID: 16711
		public const float CountColumnWidth = 75f;

		// Token: 0x04004148 RID: 16712
		public const float AdjustColumnWidth = 240f;

		// Token: 0x04004149 RID: 16713
		public const float MassColumnWidth = 100f;

		// Token: 0x0400414A RID: 16714
		public static readonly Color ItemMassColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x0400414B RID: 16715
		private const float MarketValueColumnWidth = 100f;

		// Token: 0x0400414C RID: 16716
		private const float ExtraSpaceAfterSectionTitle = 5f;

		// Token: 0x0400414D RID: 16717
		private const float DaysUntilRotColumnWidth = 75f;

		// Token: 0x0400414E RID: 16718
		private const float NutritionEatenPerDayColumnWidth = 75f;

		// Token: 0x0400414F RID: 16719
		private const float ItemNutritionColumnWidth = 75f;

		// Token: 0x04004150 RID: 16720
		private const float ForagedFoodPerDayColumnWidth = 75f;

		// Token: 0x04004151 RID: 16721
		private const float GrazeabilityInnerColumnWidth = 40f;

		// Token: 0x04004152 RID: 16722
		private const float MiscIconSize = 30f;

		// Token: 0x04004153 RID: 16723
		public const float TopAreaWidth = 515f;

		// Token: 0x04004154 RID: 16724
		private static readonly Texture2D CanGrazeIcon = ContentFinder<Texture2D>.Get("UI/Icons/CanGraze", true);

		// Token: 0x020026DC RID: 9948
		private struct Section
		{
			// Token: 0x040093A8 RID: 37800
			public string title;

			// Token: 0x040093A9 RID: 37801
			public IEnumerable<TransferableOneWay> transferables;

			// Token: 0x040093AA RID: 37802
			public List<TransferableOneWay> cachedTransferables;
		}
	}
}
