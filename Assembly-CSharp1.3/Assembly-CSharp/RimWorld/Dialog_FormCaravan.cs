using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E8 RID: 4840
	public class Dialog_FormCaravan : Window
	{
		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x060073F1 RID: 29681 RVA: 0x00273558 File Offset: 0x00271758
		public int CurrentTile
		{
			get
			{
				return this.map.Tile;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x060073F2 RID: 29682 RVA: 0x00273565 File Offset: 0x00271765
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x060073F3 RID: 29683 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x060073F4 RID: 29684 RVA: 0x00273577 File Offset: 0x00271777
		private bool AutoStripSpawnedCorpses
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x060073F5 RID: 29685 RVA: 0x00273577 File Offset: 0x00271777
		private bool ListPlayerPawnsInventorySeparately
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x060073F6 RID: 29686 RVA: 0x0027357F File Offset: 0x0027177F
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x060073F7 RID: 29687 RVA: 0x0027358C File Offset: 0x0027178C
		private bool MustChooseRoute
		{
			get
			{
				return this.canChooseRoute && (!this.reform || this.map.Parent is Settlement);
			}
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x060073F8 RID: 29688 RVA: 0x002735B8 File Offset: 0x002717B8
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

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x060073F9 RID: 29689 RVA: 0x0027361B File Offset: 0x0027181B
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

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x060073FA RID: 29690 RVA: 0x00273628 File Offset: 0x00271828
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

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x060073FB RID: 29691 RVA: 0x00273660 File Offset: 0x00271860
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

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x060073FC RID: 29692 RVA: 0x002736A8 File Offset: 0x002718A8
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

		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x060073FD RID: 29693 RVA: 0x00273708 File Offset: 0x00271908
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

		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x060073FE RID: 29694 RVA: 0x00273814 File Offset: 0x00271A14
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

		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x060073FF RID: 29695 RVA: 0x00273868 File Offset: 0x00271A68
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

		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x06007400 RID: 29696 RVA: 0x002738B0 File Offset: 0x00271AB0
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

		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x06007401 RID: 29697 RVA: 0x00273948 File Offset: 0x00271B48
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

		// Token: 0x06007402 RID: 29698 RVA: 0x00273A38 File Offset: 0x00271C38
		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool mapAboutToBeRemoved = false)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.mapAboutToBeRemoved = mapAboutToBeRemoved;
			this.canChooseRoute = (!reform || !map.retainedCaravanData.HasDestinationTile);
			this.closeOnAccept = !reform;
			this.closeOnCancel = !reform;
			this.autoSelectTravelSupplies = !reform;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06007403 RID: 29699 RVA: 0x00273B0D File Offset: 0x00271D0D
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

		// Token: 0x06007404 RID: 29700 RVA: 0x00273B47 File Offset: 0x00271D47
		public override void PostClose()
		{
			base.PostClose();
			if (this.onClosed != null && !this.choosingRoute)
			{
				this.onClosed();
			}
		}

		// Token: 0x06007405 RID: 29701 RVA: 0x00273B6A File Offset: 0x00271D6A
		public void Notify_NoLongerChoosingRoute()
		{
			this.choosingRoute = false;
			if (!Find.WindowStack.IsOpen(this) && this.onClosed != null)
			{
				this.onClosed();
			}
		}

		// Token: 0x06007406 RID: 29702 RVA: 0x00273B94 File Offset: 0x00271D94
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
			Dialog_FormCaravan.tabsList.Add(new TabRecord("TravelSupplies".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.TravelSupplies;
			}, this.tab == Dialog_FormCaravan.Tab.TravelSupplies));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_FormCaravan.tabsList, 200f);
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
			case Dialog_FormCaravan.Tab.TravelSupplies:
				this.travelSuppliesTransfer.extraHeaderSpace = 35f;
				this.travelSuppliesTransfer.OnGUI(rect3, out flag);
				this.DrawAutoSelectCheckbox(rect3, ref flag);
				break;
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		// Token: 0x06007407 RID: 29703 RVA: 0x00273E5C File Offset: 0x0027205C
		public void DrawAutoSelectCheckbox(Rect rect, ref bool anythingChanged)
		{
			rect.yMin += 37f;
			rect.height = 35f;
			bool flag = this.autoSelectTravelSupplies;
			Widgets.CheckboxLabeled(rect, "AutomaticallySelectTravelSupplies".Translate(), ref this.autoSelectTravelSupplies, false, null, null, true);
			this.travelSuppliesTransfer.readOnly = this.autoSelectTravelSupplies;
			if (flag != this.autoSelectTravelSupplies)
			{
				anythingChanged = true;
			}
		}

		// Token: 0x06007408 RID: 29704 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06007409 RID: 29705 RVA: 0x00273ECC File Offset: 0x002720CC
		public void Notify_ChoseRoute(int destinationTile)
		{
			this.destinationTile = destinationTile;
			this.startingTile = CaravanExitMapUtility.BestExitTileToGoTo(destinationTile, this.map);
			this.ticksToArriveDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.soundAppear.PlayOneShotOnCamera(null);
			if (this.autoSelectTravelSupplies)
			{
				this.SelectApproximateBestTravelSupplies();
			}
		}

		// Token: 0x0600740A RID: 29706 RVA: 0x00273F1C File Offset: 0x0027211C
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
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + t);
				return;
			}
			transferableOneWay.things.Add(t);
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + t.stackCount);
			}
		}

		// Token: 0x0600740B RID: 29707 RVA: 0x00273F90 File Offset: 0x00272190
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

		// Token: 0x0600740C RID: 29708 RVA: 0x002743E8 File Offset: 0x002725E8
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.travelSuppliesTransfer, "FormCaravanColonyThingCountTip".Translate(), this.IgnoreInventoryMode, () => this.MassCapacity - this.MassUsage, this.AutoStripSpawnedCorpses, this.CurrentTile, this.mapAboutToBeRemoved);
			this.CountToTransferChanged();
		}

		// Token: 0x0600740D RID: 29709 RVA: 0x00274464 File Offset: 0x00272664
		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
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

		// Token: 0x0600740E RID: 29710 RVA: 0x00274528 File Offset: 0x00272728
		private bool TryFormAndSendCaravan()
		{
			List<Pawn> pawns = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawns))
			{
				return false;
			}
			Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, this.startingTile);
			IntVec3 exitSpot;
			if (!this.TryFindExitSpot(pawns, true, out exitSpot))
			{
				if (!this.TryFindExitSpot(pawns, false, out exitSpot))
				{
					Messages.Message("CaravanCouldNotFindExitSpot".Translate(direction8WayFromTo.LabelShort()), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(exitSpot, this.map, false), MessageTypeDefOf.CautionInput, false);
			}
			IntVec3 meetingPoint;
			if (!this.TryFindRandomPackingSpot(exitSpot, out meetingPoint))
			{
				Messages.Message("CaravanCouldNotFindPackingSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(exitSpot, this.map, false), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pawn pawn = pawns.Find((Pawn ropee) => AnimalPenUtility.NeedsToBeManagedByRope(ropee) && !pawns.Any((Pawn roper) => roper.IsColonist && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(roper, ropee, exitSpot) && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(roper, ropee, meetingPoint)));
			if (pawn != null)
			{
				Messages.Message("CaravanRoamerCannotReachSpots".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.CautionInput, false);
				return false;
			}
			CaravanFormingUtility.StartFormingCaravan((from x in pawns
			where !x.Downed
			select x).ToList<Pawn>(), (from x in pawns
			where x.Downed
			select x).ToList<Pawn>(), Faction.OfPlayer, this.transferables, meetingPoint, exitSpot, this.startingTile, this.destinationTile);
			Messages.Message("CaravanFormationProcessStarted".Translate(), pawns[0], MessageTypeDefOf.PositiveEvent, false);
			return true;
		}

		// Token: 0x0600740F RID: 29711 RVA: 0x00274754 File Offset: 0x00272954
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

		// Token: 0x06007410 RID: 29712 RVA: 0x00274830 File Offset: 0x00272A30
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

		// Token: 0x06007411 RID: 29713 RVA: 0x00274A80 File Offset: 0x00272C80
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
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
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
							if (!t.Spawned || pawns.Any((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
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

		// Token: 0x06007412 RID: 29714 RVA: 0x00274CD4 File Offset: 0x00272ED4
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			Rot4 rot;
			Rot4 rot2;
			CaravanExitMapUtility.GetExitMapEdges(Find.WorldGrid, this.CurrentTile, this.startingTile, out rot, out rot2);
			return (rot != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot, out spot)) || (rot2 != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot2, out spot)) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Clockwise), out spot) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Counterclockwise), out spot);
		}

		// Token: 0x06007413 RID: 29715 RVA: 0x00274D54 File Offset: 0x00272F54
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, Rot4 exitDirection, out IntVec3 spot)
		{
			if (this.startingTile < 0)
			{
				Log.Error("Can't find exit spot because startingTile is not set.");
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
						if (pawns[j].IsColonist && !pawns[j].Downed && !pawns[j].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
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
						if (pawns[i].IsColonist && !pawns[i].Downed && pawns[i].CanReach(intVec2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
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

		// Token: 0x06007414 RID: 29716 RVA: 0x00274EBC File Offset: 0x002730BC
		private bool TryFindRandomPackingSpot(IntVec3 exitSpot, out IntVec3 packingSpot)
		{
			Dialog_FormCaravan.tmpPackingSpots.Clear();
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
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

		// Token: 0x06007415 RID: 29717 RVA: 0x00274F74 File Offset: 0x00273174
		private void AddPawnsToTransferables()
		{
			List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				bool setToTransferMax = (this.reform || this.mapAboutToBeRemoved) && !CaravanUtility.ShouldAutoCapture(list[i], Faction.OfPlayer);
				this.AddToTransferables(list[i], setToTransferMax);
			}
		}

		// Token: 0x06007416 RID: 29718 RVA: 0x00274FDC File Offset: 0x002731DC
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

		// Token: 0x06007417 RID: 29719 RVA: 0x002750FC File Offset: 0x002732FC
		private void TryAddCorpseInventoryAndGearToTransferables(Thing potentiallyCorpse)
		{
			Corpse corpse = potentiallyCorpse as Corpse;
			if (corpse != null)
			{
				this.AddCorpseInventoryAndGearToTransferables(corpse);
			}
		}

		// Token: 0x06007418 RID: 29720 RVA: 0x0027511C File Offset: 0x0027331C
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

		// Token: 0x06007419 RID: 29721 RVA: 0x002751D8 File Offset: 0x002733D8
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

		// Token: 0x0600741A RID: 29722 RVA: 0x00275276 File Offset: 0x00273476
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x0600741B RID: 29723 RVA: 0x00275283 File Offset: 0x00273483
		public static bool CanListInventorySeparately(Pawn p)
		{
			return p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
		}

		// Token: 0x0600741C RID: 29724 RVA: 0x002752A4 File Offset: 0x002734A4
		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x0600741D RID: 29725 RVA: 0x002752F0 File Offset: 0x002734F0
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
			this.ticksToArriveDirty = true;
			if (this.autoSelectTravelSupplies)
			{
				this.SelectApproximateBestTravelSupplies();
			}
		}

		// Token: 0x0600741E RID: 29726 RVA: 0x0027533C File Offset: 0x0027353C
		private void SelectApproximateBestTravelSupplies()
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
			Dialog_FormCaravan.tmpBeds.SortByDescending((TransferableOneWay x) => x.AnyThing.GetInnerIfMinified().GetStatValue(StatDefOf.BedRestEffectiveness, true));
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
				Dialog_FormCaravan.<>c__DisplayClass110_0 CS$<>8__locals1;
				CS$<>8__locals1.path = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null);
				try
				{
					Dialog_FormCaravan.<>c__DisplayClass110_1 CS$<>8__locals2;
					CS$<>8__locals2.ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
					CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(this.CurrentTile, this.destinationTile, CS$<>8__locals1.path, 0f, CS$<>8__locals2.ticksPerMove, Find.TickManager.TicksAbs, Dialog_FormCaravan.tmpTicksToArrive);
					float num = (float)Dialog_FormCaravan.tmpTicksToArrive.Last<Pair<int, int>>().Second / 60000f;
					float num2 = num + Dialog_FormCaravan.ExtraFoodDaysRange.ClampToRange(num * 0.35f);
					foreach (Pawn pawn2 in pawnsFromTransferables)
					{
						if (VirtualPlantsUtility.CanEverEatVirtualPlants(pawn2))
						{
							for (float num3 = 0f; num3 < num; num3 += 0.25f)
							{
								int ticksAbs = Find.TickManager.TicksGame + (int)(60000f * num3);
								if (VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(CaravanArrivalTimeEstimator.TileIllBeInAt(ticksAbs, Dialog_FormCaravan.tmpTicksToArrive, Find.TickManager.TicksGame), ticksAbs))
								{
									Dialog_FormCaravan.tmpPawnNutritionDays.SetOrAdd(pawn2, 0.25f);
								}
							}
						}
					}
					float num4 = this.<SelectApproximateBestTravelSupplies>g__DaysOfFood|110_3(ref CS$<>8__locals1, ref CS$<>8__locals2);
					while (num4 < num2 && this.MassUsage < this.MassCapacity)
					{
						bool flag = false;
						foreach (Pawn pawn3 in pawnsFromTransferables)
						{
							if (pawn3.needs.food != null && pawn3.needs.food.MaxLevel > 0f && pawn3.needs.food.FoodFallPerTick != 0f && Dialog_FormCaravan.tmpPawnNutritionDays.TryGetValue(pawn3, 0f) < num2)
							{
								Transferable transferable2 = this.BestFoodItemFor(pawn3, enumerable2, Dialog_FormCaravan.tmpTicksToArrive);
								if (transferable2 != null && this.AddFoodItem(pawn3, transferable2, num2))
								{
									flag = true;
								}
							}
						}
						if (!flag)
						{
							break;
						}
						num4 = this.<SelectApproximateBestTravelSupplies>g__DaysOfFood|110_3(ref CS$<>8__locals1, ref CS$<>8__locals2);
					}
				}
				finally
				{
					if (CS$<>8__locals1.path != null)
					{
						((IDisposable)CS$<>8__locals1.path).Dispose();
					}
				}
			}
			finally
			{
				Dialog_FormCaravan.tmpTicksToArrive.Clear();
				Dialog_FormCaravan.tmpPawnNutritionDays.Clear();
				this.daysWorthOfFoodDirty = true;
				this.massUsageDirty = true;
			}
		}

		// Token: 0x0600741F RID: 29727 RVA: 0x00275914 File Offset: 0x00273B14
		private bool AddFoodItem(Pawn pawn, Transferable transferable, float tripDays)
		{
			int num = transferable.GetMaximumToTransfer() - transferable.CountToTransfer;
			if (num <= 0)
			{
				return false;
			}
			float num2 = Dialog_FormCaravan.tmpPawnNutritionDays.TryGetValue(pawn, 0f);
			float num3 = pawn.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true) * 60000f;
			float statValueAbstract = transferable.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
			int num4 = Mathf.Min(num, Mathf.CeilToInt((tripDays - num2) * num3 / statValueAbstract));
			if (num4 <= 0 || !transferable.CanAdjustBy(num4).Accepted)
			{
				return false;
			}
			transferable.AdjustBy(num4);
			Dialog_FormCaravan.tmpPawnNutritionDays.SetOrAdd(pawn, num2 + num3 / (statValueAbstract * (float)num4));
			this.daysWorthOfFoodDirty = true;
			this.massUsageDirty = true;
			return true;
		}

		// Token: 0x06007420 RID: 29728 RVA: 0x002759CC File Offset: 0x00273BCC
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

		// Token: 0x06007421 RID: 29729 RVA: 0x00275A18 File Offset: 0x00273C18
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

		// Token: 0x06007422 RID: 29730 RVA: 0x00275A6C File Offset: 0x00273C6C
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

		// Token: 0x06007423 RID: 29731 RVA: 0x00275AF4 File Offset: 0x00273CF4
		private float GetFoodScore(Pawn pawn, Thing food, List<Pair<int, int>> ticksToArrive)
		{
			if (!CaravanPawnsNeedsUtility.CanEatForNutritionEver(food.def, pawn))
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

		// Token: 0x06007424 RID: 29732 RVA: 0x00275B68 File Offset: 0x00273D68
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

		// Token: 0x06007425 RID: 29733 RVA: 0x00275C04 File Offset: 0x00273E04
		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, true, reform, reform, reform, false, -1);
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x00275CA2 File Offset: 0x00273EA2
		[CompilerGenerated]
		private float <SelectApproximateBestTravelSupplies>g__DaysOfFood|110_3(ref Dialog_FormCaravan.<>c__DisplayClass110_0 A_1, ref Dialog_FormCaravan.<>c__DisplayClass110_1 A_2)
		{
			return DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, A_1.path, 0f, A_2.ticksPerMove);
		}

		// Token: 0x04003FBC RID: 16316
		private Map map;

		// Token: 0x04003FBD RID: 16317
		private bool reform;

		// Token: 0x04003FBE RID: 16318
		private Action onClosed;

		// Token: 0x04003FBF RID: 16319
		private bool canChooseRoute;

		// Token: 0x04003FC0 RID: 16320
		private bool mapAboutToBeRemoved;

		// Token: 0x04003FC1 RID: 16321
		public bool choosingRoute;

		// Token: 0x04003FC2 RID: 16322
		private bool thisWindowInstanceEverOpened;

		// Token: 0x04003FC3 RID: 16323
		public List<TransferableOneWay> transferables;

		// Token: 0x04003FC4 RID: 16324
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04003FC5 RID: 16325
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04003FC6 RID: 16326
		private TransferableOneWayWidget travelSuppliesTransfer;

		// Token: 0x04003FC7 RID: 16327
		private Dialog_FormCaravan.Tab tab;

		// Token: 0x04003FC8 RID: 16328
		private float lastMassFlashTime = -9999f;

		// Token: 0x04003FC9 RID: 16329
		private int startingTile = -1;

		// Token: 0x04003FCA RID: 16330
		private int destinationTile = -1;

		// Token: 0x04003FCB RID: 16331
		private bool massUsageDirty = true;

		// Token: 0x04003FCC RID: 16332
		private float cachedMassUsage;

		// Token: 0x04003FCD RID: 16333
		private bool massCapacityDirty = true;

		// Token: 0x04003FCE RID: 16334
		private float cachedMassCapacity;

		// Token: 0x04003FCF RID: 16335
		private string cachedMassCapacityExplanation;

		// Token: 0x04003FD0 RID: 16336
		private bool tilesPerDayDirty = true;

		// Token: 0x04003FD1 RID: 16337
		private float cachedTilesPerDay;

		// Token: 0x04003FD2 RID: 16338
		private string cachedTilesPerDayExplanation;

		// Token: 0x04003FD3 RID: 16339
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04003FD4 RID: 16340
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04003FD5 RID: 16341
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04003FD6 RID: 16342
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04003FD7 RID: 16343
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04003FD8 RID: 16344
		private bool visibilityDirty = true;

		// Token: 0x04003FD9 RID: 16345
		private float cachedVisibility;

		// Token: 0x04003FDA RID: 16346
		private string cachedVisibilityExplanation;

		// Token: 0x04003FDB RID: 16347
		private bool ticksToArriveDirty = true;

		// Token: 0x04003FDC RID: 16348
		private int cachedTicksToArrive;

		// Token: 0x04003FDD RID: 16349
		private bool autoSelectTravelSupplies;

		// Token: 0x04003FDE RID: 16350
		private const float TitleRectHeight = 35f;

		// Token: 0x04003FDF RID: 16351
		private const float BottomAreaHeight = 55f;

		// Token: 0x04003FE0 RID: 16352
		private const float AutoSelectCheckBoxHeight = 35f;

		// Token: 0x04003FE1 RID: 16353
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04003FE2 RID: 16354
		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		// Token: 0x04003FE3 RID: 16355
		private static readonly FloatRange ExtraFoodDaysRange = new FloatRange(1f, 5f);

		// Token: 0x04003FE4 RID: 16356
		private const int AutoMedicinePerColonist = 2;

		// Token: 0x04003FE5 RID: 16357
		private const float AdjustedTravelTimeFactor = 0.35f;

		// Token: 0x04003FE6 RID: 16358
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04003FE7 RID: 16359
		private static List<Thing> tmpPackingSpots = new List<Thing>();

		// Token: 0x04003FE8 RID: 16360
		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();

		// Token: 0x04003FE9 RID: 16361
		private static Dictionary<Pawn, float> tmpPawnNutritionDays = new Dictionary<Pawn, float>();

		// Token: 0x04003FEA RID: 16362
		private static List<TransferableOneWay> tmpBeds = new List<TransferableOneWay>();

		// Token: 0x0200265B RID: 9819
		private enum Tab
		{
			// Token: 0x040091FF RID: 37375
			Pawns,
			// Token: 0x04009200 RID: 37376
			Items,
			// Token: 0x04009201 RID: 37377
			TravelSupplies
		}
	}
}
