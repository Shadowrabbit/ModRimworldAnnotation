using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019DC RID: 6620
	public class Dialog_FormCaravan : Window
	{
		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x06009248 RID: 37448 RVA: 0x0006208E File Offset: 0x0006028E
		public int CurrentTile
		{
			get
			{
				return this.map.Tile;
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x06009249 RID: 37449 RVA: 0x0006209B File Offset: 0x0006029B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x0600924A RID: 37450 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x0600924B RID: 37451 RVA: 0x000620AD File Offset: 0x000602AD
		private bool AutoStripSpawnedCorpses
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x0600924C RID: 37452 RVA: 0x000620AD File Offset: 0x000602AD
		private bool ListPlayerPawnsInventorySeparately
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x0600924D RID: 37453 RVA: 0x000620B5 File Offset: 0x000602B5
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x0600924E RID: 37454 RVA: 0x000620C2 File Offset: 0x000602C2
		private bool MustChooseRoute
		{
			get
			{
				return this.canChooseRoute && (!this.reform || this.map.Parent is Settlement);
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x0600924F RID: 37455 RVA: 0x002A07E0 File Offset: 0x0029E9E0
		private bool ShowCancelButton
		{
			get
			{
				if (!this.mapAboutToBeRemoved)
				{
					return true;
				}
				bool flag = false;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					Pawn pawn = this.transferables[i].AnyThing as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x06009250 RID: 37456 RVA: 0x000620EB File Offset: 0x000602EB
		private IgnorePawnsInventoryMode IgnoreInventoryMode
		{
			get
			{
				if (!this.ListPlayerPawnsInventorySeparately)
				{
					return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
				}
				return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn;
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x06009251 RID: 37457 RVA: 0x000620F8 File Offset: 0x000602F8
		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, this.IgnoreInventoryMode, false, this.AutoStripSpawnedCorpses);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x06009252 RID: 37458 RVA: 0x002A0844 File Offset: 0x0029EA44
		public float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x06009253 RID: 37459 RVA: 0x002A088C File Offset: 0x0029EA8C
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.CurrentTile, this.startingTile, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x06009254 RID: 37460 RVA: 0x002A08EC File Offset: 0x0029EAEC
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.destinationTile != -1)
					{
						using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
						{
							int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
							first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, worldPath, 0f, ticksPerMove);
							second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, worldPath, 0f, ticksPerMove);
							goto IL_DB;
						}
					}
					first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, null, 0f, 3300);
					second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, null, 0f, 3300);
					IL_DB:
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x06009255 RID: 37461 RVA: 0x002A09F8 File Offset: 0x0029EBF8
		private Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x06009256 RID: 37462 RVA: 0x002A0A4C File Offset: 0x0029EC4C
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x06009257 RID: 37463 RVA: 0x002A0A94 File Offset: 0x0029EC94
		private int TicksToArrive
		{
			get
			{
				if (this.destinationTile == -1)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
					{
						this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.CurrentTile, this.destinationTile, worldPath, 0f, CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null), Find.TickManager.TicksAbs);
					}
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x06009258 RID: 37464 RVA: 0x002A0B2C File Offset: 0x0029ED2C
		private bool MostFoodWillRotSoon
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = this.transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.CountToTransfer > 0 && transferableOneWay.ThingDef.IsNutritionGivingIngestible && !(transferableOneWay.AnyThing is Corpse))
					{
						float num3 = 600f;
						CompRottable compRottable = transferableOneWay.AnyThing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num3 = (float)DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.CurrentTile, null) / 60000f;
						}
						float num4 = transferableOneWay.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)transferableOneWay.CountToTransfer;
						if (num3 < 5f)
						{
							num += num4;
						}
						else
						{
							num2 += num4;
						}
					}
				}
				return (num != 0f || num2 != 0f) && num / (num + num2) >= 0.75f;
			}
		}

		// Token: 0x06009259 RID: 37465 RVA: 0x002A0C1C File Offset: 0x0029EE1C
		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool mapAboutToBeRemoved = false)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.mapAboutToBeRemoved = mapAboutToBeRemoved;
			this.canChooseRoute = (!reform || !map.retainedCaravanData.HasDestinationTile);
			this.closeOnAccept = !reform;
			this.closeOnCancel = !reform;
			this.autoSelectFoodAndMedicine = !reform;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600925A RID: 37466 RVA: 0x0006212D File Offset: 0x0006032D
		public override void PostOpen()
		{
			base.PostOpen();
			this.choosingRoute = false;
			if (!this.thisWindowInstanceEverOpened)
			{
				this.thisWindowInstanceEverOpened = true;
				this.CalculateAndRecacheTransferables();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.FormCaravan, KnowledgeAmount.Total);
				Find.WorldRoutePlanner.Start(this);
			}
		}

		// Token: 0x0600925B RID: 37467 RVA: 0x00062167 File Offset: 0x00060367
		public override void PostClose()
		{
			base.PostClose();
			if (this.onClosed != null && !this.choosingRoute)
			{
				this.onClosed();
			}
		}

		// Token: 0x0600925C RID: 37468 RVA: 0x0006218A File Offset: 0x0006038A
		public void Notify_NoLongerChoosingRoute()
		{
			this.choosingRoute = false;
			if (!Find.WindowStack.IsOpen(this) && this.onClosed != null)
			{
				this.onClosed();
			}
		}

		// Token: 0x0600925D RID: 37469 RVA: 0x002A0CF4 File Offset: 0x0029EEF4
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, (this.reform ? "ReformCaravan" : "FormCaravan").Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.CurrentTile, (this.destinationTile == -1) ? null : new int?(this.TicksToArrive), this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), true, (this.destinationTile == -1) ? null : ("\n" + "DaysWorthOfFoodTooltip_OnlyFirstWaypoint".Translate()), false);
			Dialog_FormCaravan.tabsList.Clear();
			Dialog_FormCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Pawns;
			}, this.tab == Dialog_FormCaravan.Tab.Pawns));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Items;
			}, this.tab == Dialog_FormCaravan.Tab.Items));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("FoodAndMedicineTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.FoodAndMedicine;
			}, this.tab == Dialog_FormCaravan.Tab.FoodAndMedicine));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_FormCaravan.tabsList, 200f);
			Dialog_FormCaravan.tabsList.Clear();
			inRect = inRect.ContractedBy(17f);
			inRect.height += 17f;
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect rect3 = rect2;
			rect3.yMax -= 76f;
			bool flag = false;
			switch (this.tab)
			{
			case Dialog_FormCaravan.Tab.Pawns:
				this.pawnsTransfer.OnGUI(rect3, out flag);
				break;
			case Dialog_FormCaravan.Tab.Items:
				this.itemsTransfer.OnGUI(rect3, out flag);
				break;
			case Dialog_FormCaravan.Tab.FoodAndMedicine:
				this.foodAndMedicineTransfer.extraHeaderSpace = 35f;
				this.foodAndMedicineTransfer.OnGUI(rect3, out flag);
				this.DrawAutoSelectCheckbox(rect3, ref flag);
				break;
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		// Token: 0x0600925E RID: 37470 RVA: 0x002A0FBC File Offset: 0x0029F1BC
		public void DrawAutoSelectCheckbox(Rect rect, ref bool anythingChanged)
		{
			rect.yMin += 37f;
			rect.height = 35f;
			bool flag = this.autoSelectFoodAndMedicine;
			Widgets.CheckboxLabeled(rect, "AutomaticallySelectFoodAndMedicine".Translate(), ref this.autoSelectFoodAndMedicine, false, null, null, true);
			this.foodAndMedicineTransfer.readOnly = this.autoSelectFoodAndMedicine;
			if (flag != this.autoSelectFoodAndMedicine)
			{
				anythingChanged = true;
			}
		}

		// Token: 0x0600925F RID: 37471 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06009260 RID: 37472 RVA: 0x002A102C File Offset: 0x0029F22C
		public void Notify_ChoseRoute(int destinationTile)
		{
			this.destinationTile = destinationTile;
			this.startingTile = CaravanExitMapUtility.BestExitTileToGoTo(destinationTile, this.map);
			this.ticksToArriveDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.soundAppear.PlayOneShotOnCamera(null);
			if (this.autoSelectFoodAndMedicine)
			{
				this.SelectApproximateBestFoodAndMedicine();
			}
		}

		// Token: 0x06009261 RID: 37473 RVA: 0x002A107C File Offset: 0x0029F27C
		private void AddToTransferables(Thing t, bool setToTransferMax = false)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			if (transferableOneWay.things.Contains(t))
			{
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + t, false);
				return;
			}
			transferableOneWay.things.Add(t);
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + t.stackCount);
			}
		}

		// Token: 0x06009262 RID: 37474 RVA: 0x002A10F0 File Offset: 0x0029F2F0
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width - this.BottomButtonSize.x, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "Send".Translate(), true, true, true))
			{
				if (this.reform)
				{
					if (this.TryReformCaravan())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
				else
				{
					List<string> list = new List<string>();
					Pair<float, float> daysWorthOfFood = this.DaysWorthOfFood;
					if (daysWorthOfFood.First < 5f)
					{
						list.Add((daysWorthOfFood.First < 0.1f) ? "DaysWorthOfFoodWarningDialog_NoFood".Translate() : "DaysWorthOfFoodWarningDialog".Translate(daysWorthOfFood.First.ToString("0.#")));
					}
					else if (this.MostFoodWillRotSoon)
					{
						list.Add("CaravanFoodWillRotSoonWarningDialog".Translate());
					}
					if (!TransferableUtility.GetPawnsFromTransferables(this.transferables).Any((Pawn pawn) => CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled))
					{
						list.Add("CaravanIncapableOfSocial".Translate());
					}
					if (list.Count > 0)
					{
						if (this.CheckForErrors(TransferableUtility.GetPawnsFromTransferables(this.transferables)))
						{
							string str2 = string.Concat((from str in list
							select str + "\n\n").ToArray<string>()) + "CaravanAreYouSure".Translate();
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str2, delegate
							{
								if (this.TryFormAndSendCaravan())
								{
									this.Close(false);
								}
							}, false, null));
						}
					}
					else if (this.TryFormAndSendCaravan())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
			}
			if (this.ShowCancelButton && Widgets.ButtonText(new Rect(0f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - this.BottomButtonSize.x - 8.5f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (this.canChooseRoute)
			{
				if (Widgets.ButtonText(new Rect(rect.width / 2f + 8.5f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ChangeRouteButton".Translate(), true, true, true))
				{
					this.soundClose.PlayOneShotOnCamera(null);
					Find.WorldRoutePlanner.Start(this);
				}
				if (this.destinationTile != -1)
				{
					Rect rect3 = rect2;
					rect3.y += rect2.height + 4f;
					rect3.height = 200f;
					rect3.xMin -= 200f;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect3, "CaravanEstimatedDaysToDestination".Translate(((float)this.TicksToArrive / 60000f).ToString("0.#")));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (Prefs.DevMode)
			{
				float width = 200f;
				float height = this.BottomButtonSize.y / 2f;
				if (Widgets.ButtonText(new Rect(0f, rect2.yMax + 4f, width, height), "Dev: Send instantly", true, true, true) && this.DebugTryFormCaravanInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(204f, rect2.yMax + 4f, width, height), "Dev: Select everything", true, true, true))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToSendEverything();
				}
			}
		}

		// Token: 0x06009263 RID: 37475 RVA: 0x002A1548 File Offset: 0x0029F748
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets_NewTmp(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.foodAndMedicineTransfer, "FormCaravanColonyThingCountTip".Translate(), this.IgnoreInventoryMode, () => this.MassCapacity - this.MassUsage, this.AutoStripSpawnedCorpses, this.CurrentTile, this.mapAboutToBeRemoved);
			this.CountToTransferChanged();
		}

		// Token: 0x06009264 RID: 37476 RVA: 0x002A15C4 File Offset: 0x0029F7C4
		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			int num = this.startingTile;
			if (num < 0)
			{
				num = CaravanExitMapUtility.RandomBestExitTileFrom(this.map);
			}
			if (num < 0)
			{
				num = this.CurrentTile;
			}
			CaravanFormingUtility.FormAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, num, this.destinationTile);
			return true;
		}

		// Token: 0x06009265 RID: 37477 RVA: 0x002A1664 File Offset: 0x0029F864
		private bool TryFormAndSendCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, this.startingTile);
			IntVec3 intVec;
			if (!this.TryFindExitSpot(pawnsFromTransferables, true, out intVec))
			{
				if (!this.TryFindExitSpot(pawnsFromTransferables, false, out intVec))
				{
					Messages.Message("CaravanCouldNotFindExitSpot".Translate(direction8WayFromTo.LabelShort()), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.CautionInput, false);
			}
			IntVec3 meetingPoint;
			if (!this.TryFindRandomPackingSpot(intVec, out meetingPoint))
			{
				Messages.Message("CaravanCouldNotFindPackingSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			CaravanFormingUtility.StartFormingCaravan((from x in pawnsFromTransferables
			where !x.Downed
			select x).ToList<Pawn>(), (from x in pawnsFromTransferables
			where x.Downed
			select x).ToList<Pawn>(), Faction.OfPlayer, this.transferables, meetingPoint, intVec, this.startingTile, this.destinationTile);
			Messages.Message("CaravanFormationProcessStarted".Translate(), pawnsFromTransferables[0], MessageTypeDefOf.PositiveEvent, false);
			return true;
		}

		// Token: 0x06009266 RID: 37478 RVA: 0x002A17F4 File Offset: 0x0029F9F4
		private bool TryReformCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, this.CurrentTile, this.destinationTile, false);
			this.map.Parent.CheckRemoveMapNow();
			TaggedString taggedString = "MessageReformedCaravan".Translate();
			if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
			{
				taggedString += " " + "MessageFormedCaravan_Orders".Translate() + ": " + caravan.pather.ArrivalAction.Label + ".";
			}
			Messages.Message(taggedString, caravan, MessageTypeDefOf.TaskCompletion, false);
			return true;
		}

		// Token: 0x06009267 RID: 37479 RVA: 0x002A18D0 File Offset: 0x0029FAD0
		private void AddItemsFromTransferablesToRandomInventories(List<Pawn> pawns)
		{
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			if (this.ListPlayerPawnsInventorySeparately)
			{
				for (int i = 0; i < pawns.Count; i++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(pawns[i]))
					{
						ThingOwner<Thing> innerContainer = pawns[i].inventory.innerContainer;
						for (int j = innerContainer.Count - 1; j >= 0; j--)
						{
							this.RemoveCarriedItemFromTransferablesOrDrop(innerContainer[j], pawns[i], this.transferables);
						}
					}
				}
				for (int k = 0; k < this.transferables.Count; k++)
				{
					if (this.transferables[k].things.Any((Thing x) => !x.Spawned))
					{
						this.transferables[k].things.SortBy((Thing x) => x.Spawned);
					}
				}
			}
			Action<Thing, IThingHolder> <>9__3;
			for (int l = 0; l < this.transferables.Count; l++)
			{
				if (!(this.transferables[l].AnyThing is Corpse))
				{
					List<Thing> things = this.transferables[l].things;
					int countToTransfer = this.transferables[l].CountToTransfer;
					Action<Thing, IThingHolder> transferred;
					if ((transferred = <>9__3) == null)
					{
						transferred = (<>9__3 = delegate(Thing splitPiece, IThingHolder originalHolder)
						{
							Thing item = splitPiece.TryMakeMinified();
							CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.innerContainer.TryAdd(item, true);
						});
					}
					TransferableUtility.Transfer(things, countToTransfer, transferred);
				}
			}
			Action<Thing, int> <>9__4;
			for (int m = 0; m < this.transferables.Count; m++)
			{
				if (this.transferables[m].AnyThing is Corpse)
				{
					List<Thing> things2 = this.transferables[m].things;
					int countToTransfer2 = this.transferables[m].CountToTransfer;
					Action<Thing, int> transfer;
					if ((transfer = <>9__4) == null)
					{
						transfer = (<>9__4 = delegate(Thing originalThing, int numToTake)
						{
							if (this.AutoStripSpawnedCorpses)
							{
								Corpse corpse = originalThing as Corpse;
								if (corpse != null && corpse.Spawned)
								{
									corpse.Strip();
								}
							}
							Thing item = originalThing.SplitOff(numToTake);
							CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.innerContainer.TryAdd(item, true);
						});
					}
					TransferableUtility.TransferNoSplit(things2, countToTransfer2, transfer, true, true);
				}
			}
		}

		// Token: 0x06009268 RID: 37480 RVA: 0x002A1B20 File Offset: 0x0029FD20
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (this.MustChooseRoute && this.destinationTile < 0)
			{
				Messages.Message("MessageMustChooseRouteFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.reform && this.startingTile < 0)
			{
				Messages.Message("MessageNoValidExitTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pawn pawn = pawns.Find((Pawn x) => !x.IsColonist && !pawns.Any((Pawn y) => y.IsColonist && y.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)));
			if (pawn != null)
			{
				Messages.Message("CaravanPawnIsUnreachable".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			for (int i = 0; i < this.transferables.Count; i++)
			{
				if (this.transferables[i].ThingDef.category == ThingCategory.Item)
				{
					int countToTransfer = this.transferables[i].CountToTransfer;
					int num = 0;
					if (countToTransfer > 0)
					{
						for (int j = 0; j < this.transferables[i].things.Count; j++)
						{
							Thing t = this.transferables[i].things[j];
							if (!t.Spawned || pawns.Any((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)))
							{
								num += t.stackCount;
								if (num >= countToTransfer)
								{
									break;
								}
							}
						}
						if (num < countToTransfer)
						{
							if (countToTransfer == 1)
							{
								Messages.Message("CaravanItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("CaravanItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06009269 RID: 37481 RVA: 0x002A1DC0 File Offset: 0x0029FFC0
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			Rot4 rot;
			Rot4 rot2;
			CaravanExitMapUtility.GetExitMapEdges(Find.WorldGrid, this.CurrentTile, this.startingTile, out rot, out rot2);
			return (rot != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot, out spot)) || (rot2 != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot2, out spot)) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Clockwise), out spot) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Counterclockwise), out spot);
		}

		// Token: 0x0600926A RID: 37482 RVA: 0x002A1E40 File Offset: 0x002A0040
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, Rot4 exitDirection, out IntVec3 spot)
		{
			if (this.startingTile < 0)
			{
				Log.Error("Can't find exit spot because startingTile is not set.", false);
				spot = IntVec3.Invalid;
				return false;
			}
			Predicate<IntVec3> validator = (IntVec3 x) => !x.Fogged(this.map) && x.Standable(this.map);
			if (reachableForEveryColonist)
			{
				return CellFinder.TryFindRandomEdgeCellWith(delegate(IntVec3 x)
				{
					if (!validator(x))
					{
						return false;
					}
					for (int j = 0; j < pawns.Count; j++)
					{
						if (pawns[j].IsColonist && !pawns[j].Downed && !pawns[j].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return false;
						}
					}
					return true;
				}, this.map, exitDirection, CellFinder.EdgeRoadChance_Always, out spot);
			}
			IntVec3 intVec = IntVec3.Invalid;
			int num = -1;
			foreach (IntVec3 intVec2 in CellRect.WholeMap(this.map).GetEdgeCells(exitDirection).InRandomOrder(null))
			{
				if (validator(intVec2))
				{
					int num2 = 0;
					for (int i = 0; i < pawns.Count; i++)
					{
						if (pawns[i].IsColonist && !pawns[i].Downed && pawns[i].CanReach(intVec2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							num2++;
						}
					}
					if (num2 > num)
					{
						num = num2;
						intVec = intVec2;
					}
				}
			}
			spot = intVec;
			return intVec.IsValid;
		}

		// Token: 0x0600926B RID: 37483 RVA: 0x002A1FA8 File Offset: 0x002A01A8
		private bool TryFindRandomPackingSpot(IntVec3 exitSpot, out IntVec3 packingSpot)
		{
			Dialog_FormCaravan.tmpPackingSpots.Clear();
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.map.reachability.CanReach(exitSpot, list[i], PathEndMode.OnCell, traverseParams))
				{
					Dialog_FormCaravan.tmpPackingSpots.Add(list[i]);
				}
			}
			if (Dialog_FormCaravan.tmpPackingSpots.Any<Thing>())
			{
				Thing thing = Dialog_FormCaravan.tmpPackingSpots.RandomElement<Thing>();
				Dialog_FormCaravan.tmpPackingSpots.Clear();
				packingSpot = thing.Position;
				return true;
			}
			return RCellFinder.TryFindRandomSpotJustOutsideColony(exitSpot, this.map, out packingSpot);
		}

		// Token: 0x0600926C RID: 37484 RVA: 0x002A2060 File Offset: 0x002A0260
		private void AddPawnsToTransferables()
		{
			List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				bool setToTransferMax = (this.reform || this.mapAboutToBeRemoved) && !CaravanUtility.ShouldAutoCapture(list[i], Faction.OfPlayer);
				this.AddToTransferables(list[i], setToTransferMax);
			}
		}

		// Token: 0x0600926D RID: 37485 RVA: 0x002A20C8 File Offset: 0x002A02C8
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, this.reform, this.reform, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i], false);
			}
			if (this.AutoStripSpawnedCorpses)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Spawned)
					{
						this.TryAddCorpseInventoryAndGearToTransferables(list[j]);
					}
				}
			}
			if (this.ListPlayerPawnsInventorySeparately)
			{
				List<Pawn> list2 = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
				for (int k = 0; k < list2.Count; k++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(list2[k]))
					{
						ThingOwner<Thing> innerContainer = list2[k].inventory.innerContainer;
						for (int l = 0; l < innerContainer.Count; l++)
						{
							this.AddToTransferables(innerContainer[l], true);
							if (this.AutoStripSpawnedCorpses && innerContainer[l].Spawned)
							{
								this.TryAddCorpseInventoryAndGearToTransferables(innerContainer[l]);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600926E RID: 37486 RVA: 0x002A21E8 File Offset: 0x002A03E8
		private void TryAddCorpseInventoryAndGearToTransferables(Thing potentiallyCorpse)
		{
			Corpse corpse = potentiallyCorpse as Corpse;
			if (corpse != null)
			{
				this.AddCorpseInventoryAndGearToTransferables(corpse);
			}
		}

		// Token: 0x0600926F RID: 37487 RVA: 0x002A2208 File Offset: 0x002A0408
		private void AddCorpseInventoryAndGearToTransferables(Corpse corpse)
		{
			Pawn_InventoryTracker inventory = corpse.InnerPawn.inventory;
			Pawn_ApparelTracker apparel = corpse.InnerPawn.apparel;
			Pawn_EquipmentTracker equipment = corpse.InnerPawn.equipment;
			for (int i = 0; i < inventory.innerContainer.Count; i++)
			{
				this.AddToTransferables(inventory.innerContainer[i], false);
			}
			if (apparel != null)
			{
				List<Apparel> wornApparel = apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					this.AddToTransferables(wornApparel[j], false);
				}
			}
			if (equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = equipment.AllEquipmentListForReading;
				for (int k = 0; k < allEquipmentListForReading.Count; k++)
				{
					this.AddToTransferables(allEquipmentListForReading[k], false);
				}
			}
		}

		// Token: 0x06009270 RID: 37488 RVA: 0x002A22C4 File Offset: 0x002A04C4
		private void RemoveCarriedItemFromTransferablesOrDrop(Thing carried, Pawn carrier, List<TransferableOneWay> transferables)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(carried, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			int num;
			if (transferableOneWay == null)
			{
				num = carried.stackCount;
			}
			else if (transferableOneWay.CountToTransfer >= carried.stackCount)
			{
				transferableOneWay.AdjustBy(-carried.stackCount);
				transferableOneWay.things.Remove(carried);
				num = 0;
			}
			else
			{
				num = carried.stackCount - transferableOneWay.CountToTransfer;
				transferableOneWay.AdjustTo(0);
			}
			if (num > 0)
			{
				Thing thing = carried.SplitOff(num);
				if (carrier.SpawnedOrAnyParentSpawned)
				{
					GenPlace.TryPlaceThing(thing, carrier.PositionHeld, carrier.MapHeld, ThingPlaceMode.Near, null, null, default(Rot4));
					return;
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06009271 RID: 37489 RVA: 0x000621B3 File Offset: 0x000603B3
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x06009272 RID: 37490 RVA: 0x000621C0 File Offset: 0x000603C0
		public static bool CanListInventorySeparately(Pawn p)
		{
			return p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
		}

		// Token: 0x06009273 RID: 37491 RVA: 0x002A2364 File Offset: 0x002A0564
		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x06009274 RID: 37492 RVA: 0x002A23B0 File Offset: 0x002A05B0
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
			this.ticksToArriveDirty = true;
			if (this.autoSelectFoodAndMedicine)
			{
				this.SelectApproximateBestFoodAndMedicine();
			}
		}

		// Token: 0x06009275 RID: 37493 RVA: 0x002A23FC File Offset: 0x002A05FC
		private void SelectApproximateBestFoodAndMedicine()
		{
			IEnumerable<TransferableOneWay> enumerable = from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn && !x.ThingDef.thingCategories.NullOrEmpty<ThingCategoryDef>() && x.ThingDef.thingCategories.Contains(ThingCategoryDefOf.Medicine)
			select x;
			IEnumerable<TransferableOneWay> enumerable2 = from x in this.transferables
			where x.ThingDef.IsIngestible && !x.ThingDef.IsDrug && !x.ThingDef.IsCorpse
			select x;
			Dialog_FormCaravan.tmpBeds.Clear();
			for (int i = 0; i < this.transferables.Count; i++)
			{
				for (int j = 0; j < this.transferables[i].things.Count; j++)
				{
					Thing thing = this.transferables[i].things[j];
					for (int k = 0; k < thing.stackCount; k++)
					{
						Building_Bed building_Bed;
						if ((building_Bed = (thing.GetInnerIfMinified() as Building_Bed)) != null && building_Bed.def.building.bed_caravansCanUse)
						{
							for (int l = 0; l < building_Bed.SleepingSlotsCount; l++)
							{
								Dialog_FormCaravan.tmpBeds.Add(this.transferables[i]);
							}
						}
					}
				}
			}
			Dialog_FormCaravan.tmpBeds.SortByDescending((TransferableOneWay x) => x.AnyThing.GetStatValue(StatDefOf.BedRestEffectiveness, true));
			foreach (TransferableOneWay transferableOneWay in enumerable)
			{
				transferableOneWay.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay2 in enumerable2)
			{
				transferableOneWay2.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay3 in Dialog_FormCaravan.tmpBeds)
			{
				transferableOneWay3.AdjustTo(0);
			}
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any<Pawn>())
			{
				return;
			}
			foreach (Pawn pawn in pawnsFromTransferables)
			{
				TransferableOneWay transferableOneWay4 = this.BestBedFor(pawn);
				if (transferableOneWay4 != null)
				{
					Dialog_FormCaravan.tmpBeds.Remove(transferableOneWay4);
					if (transferableOneWay4.CanAdjustBy(1).Accepted)
					{
						this.AddOneIfMassAllows(transferableOneWay4);
					}
				}
				if (!pawn.AnimalOrWildMan() && !pawn.guest.IsPrisoner)
				{
					for (int m = 0; m < 2; m++)
					{
						Transferable transferable = this.BestMedicineItemFor(pawn, enumerable);
						if (transferable != null)
						{
							this.AddOneIfMassAllows(transferable);
						}
					}
				}
			}
			if (this.destinationTile == -1 || !DaysWorthOfFoodCalculator.AnyFoodEatingPawn(pawnsFromTransferables) || !enumerable2.Any<TransferableOneWay>())
			{
				return;
			}
			try
			{
				using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
				{
					int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
					CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(this.CurrentTile, this.destinationTile, worldPath, 0f, ticksPerMove, Find.TickManager.TicksAbs, Dialog_FormCaravan.tmpTicksToArrive);
					float num = (float)Dialog_FormCaravan.tmpTicksToArrive.Last<Pair<int, int>>().Second / 60000f + 4f;
					float num2;
					do
					{
						num2 = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, worldPath, 0f, ticksPerMove);
						if (num2 >= num)
						{
							break;
						}
						bool flag = false;
						foreach (Pawn pawn2 in pawnsFromTransferables)
						{
							Transferable transferable2 = this.BestFoodItemFor(pawn2, enumerable2, Dialog_FormCaravan.tmpTicksToArrive);
							if (transferable2 != null && this.AddOneIfMassAllows(transferable2))
							{
								flag = true;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					while (num2 < num && this.MassUsage < this.MassCapacity);
				}
			}
			finally
			{
				Dialog_FormCaravan.tmpTicksToArrive.Clear();
				this.daysWorthOfFoodDirty = true;
				this.massUsageDirty = true;
			}
		}

		// Token: 0x06009276 RID: 37494 RVA: 0x002A28A4 File Offset: 0x002A0AA4
		private bool AddOneIfMassAllows(Transferable transferable)
		{
			if (transferable.CanAdjustBy(1).Accepted && this.MassUsage + transferable.ThingDef.BaseMass < this.MassCapacity)
			{
				transferable.AdjustBy(1);
				this.massUsageDirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06009277 RID: 37495 RVA: 0x002A28F0 File Offset: 0x002A0AF0
		private TransferableOneWay BestBedFor(Pawn pawn)
		{
			for (int i = 0; i < Dialog_FormCaravan.tmpBeds.Count; i++)
			{
				Thing innerIfMinified = Dialog_FormCaravan.tmpBeds[i].AnyThing.GetInnerIfMinified();
				if (RestUtility.CanUseBedEver(pawn, innerIfMinified.def))
				{
					return Dialog_FormCaravan.tmpBeds[i];
				}
			}
			return null;
		}

		// Token: 0x06009278 RID: 37496 RVA: 0x002A2944 File Offset: 0x002A0B44
		private Transferable BestFoodItemFor(Pawn pawn, IEnumerable<TransferableOneWay> food, List<Pair<int, int>> ticksToArrive)
		{
			Transferable result = null;
			float num = 0f;
			if (!pawn.RaceProps.EatsFood)
			{
				return result;
			}
			foreach (TransferableOneWay transferableOneWay in food)
			{
				if (transferableOneWay.CanAdjustBy(1).Accepted)
				{
					float foodScore = this.GetFoodScore(pawn, transferableOneWay.AnyThing, ticksToArrive);
					if (foodScore > num)
					{
						result = transferableOneWay;
						num = foodScore;
					}
				}
			}
			return result;
		}

		// Token: 0x06009279 RID: 37497 RVA: 0x002A29CC File Offset: 0x002A0BCC
		private float GetFoodScore(Pawn pawn, Thing food, List<Pair<int, int>> ticksToArrive)
		{
			if (!CaravanPawnsNeedsUtility.CanEatForNutritionEver(food.def, pawn) || !food.def.ingestible.canAutoSelectAsFoodForCaravan)
			{
				return 0f;
			}
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true));
			CompRottable compRottable = food.TryGetComp<CompRottable>();
			if (compRottable != null && compRottable.Active && DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.CurrentTile, ticksToArrive) < ticksToArrive.Last<Pair<int, int>>().Second)
			{
				num *= 0.1f;
			}
			return num;
		}

		// Token: 0x0600927A RID: 37498 RVA: 0x002A2A50 File Offset: 0x002A0C50
		private Transferable BestMedicineItemFor(Pawn pawn, IEnumerable<TransferableOneWay> medicine)
		{
			Transferable transferable = null;
			float num = 0f;
			foreach (TransferableOneWay transferableOneWay in medicine)
			{
				Thing anyThing = transferableOneWay.AnyThing;
				if (transferableOneWay.CanAdjustBy(1).Accepted && pawn.playerSettings.medCare.AllowsMedicine(anyThing.def))
				{
					float statValue = anyThing.GetStatValue(StatDefOf.MedicalPotency, true);
					if (transferable == null || statValue > num)
					{
						transferable = transferableOneWay;
						num = statValue;
					}
				}
			}
			return transferable;
		}

		// Token: 0x0600927B RID: 37499 RVA: 0x000621DE File Offset: 0x000603DE
		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, true, reform, reform, reform, false, -1);
		}

		// Token: 0x04005C7F RID: 23679
		private Map map;

		// Token: 0x04005C80 RID: 23680
		private bool reform;

		// Token: 0x04005C81 RID: 23681
		private Action onClosed;

		// Token: 0x04005C82 RID: 23682
		private bool canChooseRoute;

		// Token: 0x04005C83 RID: 23683
		private bool mapAboutToBeRemoved;

		// Token: 0x04005C84 RID: 23684
		public bool choosingRoute;

		// Token: 0x04005C85 RID: 23685
		private bool thisWindowInstanceEverOpened;

		// Token: 0x04005C86 RID: 23686
		public List<TransferableOneWay> transferables;

		// Token: 0x04005C87 RID: 23687
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04005C88 RID: 23688
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04005C89 RID: 23689
		private TransferableOneWayWidget foodAndMedicineTransfer;

		// Token: 0x04005C8A RID: 23690
		private Dialog_FormCaravan.Tab tab;

		// Token: 0x04005C8B RID: 23691
		private float lastMassFlashTime = -9999f;

		// Token: 0x04005C8C RID: 23692
		private int startingTile = -1;

		// Token: 0x04005C8D RID: 23693
		private int destinationTile = -1;

		// Token: 0x04005C8E RID: 23694
		private bool massUsageDirty = true;

		// Token: 0x04005C8F RID: 23695
		private float cachedMassUsage;

		// Token: 0x04005C90 RID: 23696
		private bool massCapacityDirty = true;

		// Token: 0x04005C91 RID: 23697
		private float cachedMassCapacity;

		// Token: 0x04005C92 RID: 23698
		private string cachedMassCapacityExplanation;

		// Token: 0x04005C93 RID: 23699
		private bool tilesPerDayDirty = true;

		// Token: 0x04005C94 RID: 23700
		private float cachedTilesPerDay;

		// Token: 0x04005C95 RID: 23701
		private string cachedTilesPerDayExplanation;

		// Token: 0x04005C96 RID: 23702
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04005C97 RID: 23703
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04005C98 RID: 23704
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04005C99 RID: 23705
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04005C9A RID: 23706
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04005C9B RID: 23707
		private bool visibilityDirty = true;

		// Token: 0x04005C9C RID: 23708
		private float cachedVisibility;

		// Token: 0x04005C9D RID: 23709
		private string cachedVisibilityExplanation;

		// Token: 0x04005C9E RID: 23710
		private bool ticksToArriveDirty = true;

		// Token: 0x04005C9F RID: 23711
		private int cachedTicksToArrive;

		// Token: 0x04005CA0 RID: 23712
		private bool autoSelectFoodAndMedicine;

		// Token: 0x04005CA1 RID: 23713
		private const float TitleRectHeight = 35f;

		// Token: 0x04005CA2 RID: 23714
		private const float BottomAreaHeight = 55f;

		// Token: 0x04005CA3 RID: 23715
		private const float AutoSelectCheckBoxHeight = 35f;

		// Token: 0x04005CA4 RID: 23716
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04005CA5 RID: 23717
		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		// Token: 0x04005CA6 RID: 23718
		private const float AutoSelectFoodThresholdDays = 4f;

		// Token: 0x04005CA7 RID: 23719
		private const int AutoMedicinePerColonist = 2;

		// Token: 0x04005CA8 RID: 23720
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04005CA9 RID: 23721
		private static List<Thing> tmpPackingSpots = new List<Thing>();

		// Token: 0x04005CAA RID: 23722
		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();

		// Token: 0x04005CAB RID: 23723
		private static List<TransferableOneWay> tmpBeds = new List<TransferableOneWay>();

		// Token: 0x020019DD RID: 6621
		private enum Tab
		{
			// Token: 0x04005CAD RID: 23725
			Pawns,
			// Token: 0x04005CAE RID: 23726
			Items,
			// Token: 0x04005CAF RID: 23727
			FoodAndMedicine
		}
	}
}
