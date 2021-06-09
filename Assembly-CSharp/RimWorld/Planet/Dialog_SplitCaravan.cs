using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021C0 RID: 8640
	public class Dialog_SplitCaravan : Window
	{
		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x0600B8ED RID: 47341 RVA: 0x0006209B File Offset: 0x0006029B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600B8EE RID: 47342 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x0600B8EF RID: 47343 RVA: 0x00077D6F File Offset: 0x00075F6F
		private BiomeDef Biome
		{
			get
			{
				return this.caravan.Biome;
			}
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x0600B8F0 RID: 47344 RVA: 0x00077D7C File Offset: 0x00075F7C
		private float SourceMassUsage
		{
			get
			{
				if (this.sourceMassUsageDirty)
				{
					this.sourceMassUsageDirty = false;
					this.cachedSourceMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTransfer(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedSourceMassUsage;
			}
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x0600B8F1 RID: 47345 RVA: 0x003527E0 File Offset: 0x003509E0
		private float SourceMassCapacity
		{
			get
			{
				if (this.sourceMassCapacityDirty)
				{
					this.sourceMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceMassCapacity;
			}
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x0600B8F2 RID: 47346 RVA: 0x00352828 File Offset: 0x00350A28
		private float SourceTilesPerDay
		{
			get
			{
				if (this.sourceTilesPerDayDirty)
				{
					this.sourceTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTransfer(this.transferables, this.SourceMassUsage, this.SourceMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : -1, stringBuilder);
					this.cachedSourceTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceTilesPerDay;
			}
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x0600B8F3 RID: 47347 RVA: 0x003528AC File Offset: 0x00350AAC
		private Pair<float, float> SourceDaysWorthOfFood
		{
			get
			{
				if (this.sourceDaysWorthOfFoodDirty)
				{
					this.sourceDaysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.caravan.pather.Moving)
					{
						using (Find.WorldPathFinder.FindPath(this.caravan.Tile, this.caravan.pather.Destination, null, null))
						{
							first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							second = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							goto IL_13D;
						}
					}
					first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
					second = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					IL_13D:
					this.cachedSourceDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedSourceDaysWorthOfFood;
			}
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x0600B8F4 RID: 47348 RVA: 0x00352A1C File Offset: 0x00350C1C
		private Pair<ThingDef, float> SourceForagedFoodPerDay
		{
			get
			{
				if (this.sourceForagedFoodPerDayDirty)
				{
					this.sourceForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTransfer(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedSourceForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceForagedFoodPerDay;
			}
		}

		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x0600B8F5 RID: 47349 RVA: 0x00352A70 File Offset: 0x00350C70
		private float SourceVisibility
		{
			get
			{
				if (this.sourceVisibilityDirty)
				{
					this.sourceVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceVisibility;
			}
		}

		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x0600B8F6 RID: 47350 RVA: 0x00077DA7 File Offset: 0x00075FA7
		private float DestMassUsage
		{
			get
			{
				if (this.destMassUsageDirty)
				{
					this.destMassUsageDirty = false;
					this.cachedDestMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedDestMassUsage;
			}
		}

		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x0600B8F7 RID: 47351 RVA: 0x00352AB8 File Offset: 0x00350CB8
		private float DestMassCapacity
		{
			get
			{
				if (this.destMassCapacityDirty)
				{
					this.destMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedDestMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestMassCapacity;
			}
		}

		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x0600B8F8 RID: 47352 RVA: 0x00352B00 File Offset: 0x00350D00
		private float DestTilesPerDay
		{
			get
			{
				if (this.destTilesPerDayDirty)
				{
					this.destTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.DestMassUsage, this.DestMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : -1, stringBuilder);
					this.cachedDestTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestTilesPerDay;
			}
		}

		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x0600B8F9 RID: 47353 RVA: 0x00352B84 File Offset: 0x00350D84
		private Pair<float, float> DestDaysWorthOfFood
		{
			get
			{
				if (this.destDaysWorthOfFoodDirty)
				{
					this.destDaysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.caravan.pather.Moving)
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
					}
					else
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					}
					this.cachedDestDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedDestDaysWorthOfFood;
			}
		}

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x0600B8FA RID: 47354 RVA: 0x00352CB0 File Offset: 0x00350EB0
		private Pair<ThingDef, float> DestForagedFoodPerDay
		{
			get
			{
				if (this.destForagedFoodPerDayDirty)
				{
					this.destForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedDestForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestForagedFoodPerDay;
			}
		}

		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x0600B8FB RID: 47355 RVA: 0x00352D04 File Offset: 0x00350F04
		private float DestVisibility
		{
			get
			{
				if (this.destVisibilityDirty)
				{
					this.destVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedDestVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestVisibility;
			}
		}

		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x0600B8FC RID: 47356 RVA: 0x00077DD2 File Offset: 0x00075FD2
		private int TicksToArrive
		{
			get
			{
				if (!this.caravan.pather.Moving)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, false);
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x0600B8FD RID: 47357 RVA: 0x00352D4C File Offset: 0x00350F4C
		public Dialog_SplitCaravan(Caravan caravan)
		{
			this.caravan = caravan;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x00077E0F File Offset: 0x0007600F
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x0600B8FF RID: 47359 RVA: 0x00352DE4 File Offset: 0x00350FE4
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SplitCaravan".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.SourceMassUsage, this.SourceMassCapacity, this.cachedSourceMassCapacityExplanation, this.SourceTilesPerDay, this.cachedSourceTilesPerDayExplanation, this.SourceDaysWorthOfFood, this.SourceForagedFoodPerDay, this.cachedSourceForagedFoodPerDayExplanation, this.SourceVisibility, this.cachedSourceVisibilityExplanation, -1f, -1f, null), new CaravanUIUtility.CaravanInfo?(new CaravanUIUtility.CaravanInfo(this.DestMassUsage, this.DestMassCapacity, this.cachedDestMassCapacityExplanation, this.DestTilesPerDay, this.cachedDestTilesPerDayExplanation, this.DestDaysWorthOfFood, this.DestForagedFoodPerDay, this.cachedDestForagedFoodPerDayExplanation, this.DestVisibility, this.cachedDestVisibilityExplanation, -1f, -1f, null)), this.caravan.Tile, this.caravan.pather.Moving ? new int?(this.TicksToArrive) : null, -9999f, new Rect(12f, 35f, inRect.width - 24f, 40f), true, null, false);
			Dialog_SplitCaravan.tabsList.Clear();
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Pawns;
			}, this.tab == Dialog_SplitCaravan.Tab.Pawns));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Items;
			}, this.tab == Dialog_SplitCaravan.Tab.Items));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("FoodAndMedicineTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.FoodAndMedicine;
			}, this.tab == Dialog_SplitCaravan.Tab.FoodAndMedicine));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_SplitCaravan.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			switch (this.tab)
			{
			case Dialog_SplitCaravan.Tab.Pawns:
				this.pawnsTransfer.OnGUI(inRect2, out flag);
				break;
			case Dialog_SplitCaravan.Tab.Items:
				this.itemsTransfer.OnGUI(inRect2, out flag);
				break;
			case Dialog_SplitCaravan.Tab.FoodAndMedicine:
				this.foodAndMedicineTransfer.OnGUI(inRect2, out flag);
				break;
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		// Token: 0x0600B900 RID: 47360 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x0600B901 RID: 47361 RVA: 0x00353090 File Offset: 0x00351290
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.Normal);
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
		}

		// Token: 0x0600B902 RID: 47362 RVA: 0x003530EC File Offset: 0x003512EC
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true) && this.TrySplitCaravan())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
			if (Widgets.ButtonText(new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600B903 RID: 47363 RVA: 0x0035322C File Offset: 0x0035142C
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets_NewTmp(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.foodAndMedicineTransfer, "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, () => this.DestMassCapacity - this.DestMassUsage, false, this.caravan.Tile, false);
			this.CountToTransferChanged();
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x0035329C File Offset: 0x0035149C
		private bool TrySplitCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			for (int i = 0; i < pawnsFromTransferables.Count; i++)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawnsFromTransferables[i], this.caravan.PawnsListForReading, pawnsFromTransferables);
			}
			for (int j = 0; j < pawnsFromTransferables.Count; j++)
			{
				this.caravan.RemovePawn(pawnsFromTransferables[j]);
			}
			Caravan newCaravan = CaravanMaker.MakeCaravan(pawnsFromTransferables, this.caravan.Faction, this.caravan.Tile, true);
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			Action<Thing, int> <>9__1;
			for (int k = 0; k < this.transferables.Count; k++)
			{
				List<Thing> things = this.transferables[k].things;
				int countToTransfer = this.transferables[k].CountToTransfer;
				Action<Thing, int> transfer;
				if ((transfer = <>9__1) == null)
				{
					transfer = (<>9__1 = delegate(Thing thing, int numToTake)
					{
						Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
						if (ownerOf == null)
						{
							Log.Error("Error while splitting a caravan: Thing " + thing + " has no owner. Where did it come from then?", false);
							return;
						}
						CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, newCaravan.PawnsListForReading, null, numToTake);
					});
				}
				TransferableUtility.TransferNoSplit(things, countToTransfer, transfer, true, true);
			}
			return true;
		}

		// Token: 0x0600B905 RID: 47365 RVA: 0x003533D0 File Offset: 0x003515D0
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600B906 RID: 47366 RVA: 0x0035345C File Offset: 0x0035165C
		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		// Token: 0x0600B907 RID: 47367 RVA: 0x00353494 File Offset: 0x00351694
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x0600B908 RID: 47368 RVA: 0x003534CC File Offset: 0x003516CC
		private bool AnyNonDownedColonistLeftInSourceCaravan(List<Pawn> pawnsToTransfer)
		{
			Predicate<Thing> <>9__1;
			return this.transferables.Any(delegate(TransferableOneWay x)
			{
				List<Thing> things = x.things;
				Predicate<Thing> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(Thing y)
					{
						Pawn pawn = y as Pawn;
						return pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.Downed && !pawnsToTransfer.Contains(pawn);
					});
				}
				return things.Any(predicate);
			});
		}

		// Token: 0x0600B909 RID: 47369 RVA: 0x00353500 File Offset: 0x00351700
		private void CountToTransferChanged()
		{
			this.sourceMassUsageDirty = true;
			this.sourceMassCapacityDirty = true;
			this.sourceTilesPerDayDirty = true;
			this.sourceDaysWorthOfFoodDirty = true;
			this.sourceForagedFoodPerDayDirty = true;
			this.sourceVisibilityDirty = true;
			this.destMassUsageDirty = true;
			this.destMassCapacityDirty = true;
			this.destTilesPerDayDirty = true;
			this.destDaysWorthOfFoodDirty = true;
			this.destForagedFoodPerDayDirty = true;
			this.destVisibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x04007E3D RID: 32317
		private Caravan caravan;

		// Token: 0x04007E3E RID: 32318
		private List<TransferableOneWay> transferables;

		// Token: 0x04007E3F RID: 32319
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04007E40 RID: 32320
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04007E41 RID: 32321
		private TransferableOneWayWidget foodAndMedicineTransfer;

		// Token: 0x04007E42 RID: 32322
		private Dialog_SplitCaravan.Tab tab;

		// Token: 0x04007E43 RID: 32323
		private bool sourceMassUsageDirty = true;

		// Token: 0x04007E44 RID: 32324
		private float cachedSourceMassUsage;

		// Token: 0x04007E45 RID: 32325
		private bool sourceMassCapacityDirty = true;

		// Token: 0x04007E46 RID: 32326
		private float cachedSourceMassCapacity;

		// Token: 0x04007E47 RID: 32327
		private string cachedSourceMassCapacityExplanation;

		// Token: 0x04007E48 RID: 32328
		private bool sourceTilesPerDayDirty = true;

		// Token: 0x04007E49 RID: 32329
		private float cachedSourceTilesPerDay;

		// Token: 0x04007E4A RID: 32330
		private string cachedSourceTilesPerDayExplanation;

		// Token: 0x04007E4B RID: 32331
		private bool sourceDaysWorthOfFoodDirty = true;

		// Token: 0x04007E4C RID: 32332
		private Pair<float, float> cachedSourceDaysWorthOfFood;

		// Token: 0x04007E4D RID: 32333
		private bool sourceForagedFoodPerDayDirty = true;

		// Token: 0x04007E4E RID: 32334
		private Pair<ThingDef, float> cachedSourceForagedFoodPerDay;

		// Token: 0x04007E4F RID: 32335
		private string cachedSourceForagedFoodPerDayExplanation;

		// Token: 0x04007E50 RID: 32336
		private bool sourceVisibilityDirty = true;

		// Token: 0x04007E51 RID: 32337
		private float cachedSourceVisibility;

		// Token: 0x04007E52 RID: 32338
		private string cachedSourceVisibilityExplanation;

		// Token: 0x04007E53 RID: 32339
		private bool destMassUsageDirty = true;

		// Token: 0x04007E54 RID: 32340
		private float cachedDestMassUsage;

		// Token: 0x04007E55 RID: 32341
		private bool destMassCapacityDirty = true;

		// Token: 0x04007E56 RID: 32342
		private float cachedDestMassCapacity;

		// Token: 0x04007E57 RID: 32343
		private string cachedDestMassCapacityExplanation;

		// Token: 0x04007E58 RID: 32344
		private bool destTilesPerDayDirty = true;

		// Token: 0x04007E59 RID: 32345
		private float cachedDestTilesPerDay;

		// Token: 0x04007E5A RID: 32346
		private string cachedDestTilesPerDayExplanation;

		// Token: 0x04007E5B RID: 32347
		private bool destDaysWorthOfFoodDirty = true;

		// Token: 0x04007E5C RID: 32348
		private Pair<float, float> cachedDestDaysWorthOfFood;

		// Token: 0x04007E5D RID: 32349
		private bool destForagedFoodPerDayDirty = true;

		// Token: 0x04007E5E RID: 32350
		private Pair<ThingDef, float> cachedDestForagedFoodPerDay;

		// Token: 0x04007E5F RID: 32351
		private string cachedDestForagedFoodPerDayExplanation;

		// Token: 0x04007E60 RID: 32352
		private bool destVisibilityDirty = true;

		// Token: 0x04007E61 RID: 32353
		private float cachedDestVisibility;

		// Token: 0x04007E62 RID: 32354
		private string cachedDestVisibilityExplanation;

		// Token: 0x04007E63 RID: 32355
		private bool ticksToArriveDirty = true;

		// Token: 0x04007E64 RID: 32356
		private int cachedTicksToArrive;

		// Token: 0x04007E65 RID: 32357
		private const float TitleRectHeight = 35f;

		// Token: 0x04007E66 RID: 32358
		private const float BottomAreaHeight = 55f;

		// Token: 0x04007E67 RID: 32359
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04007E68 RID: 32360
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x020021C1 RID: 8641
		private enum Tab
		{
			// Token: 0x04007E6A RID: 32362
			Pawns,
			// Token: 0x04007E6B RID: 32363
			Items,
			// Token: 0x04007E6C RID: 32364
			FoodAndMedicine
		}
	}
}
