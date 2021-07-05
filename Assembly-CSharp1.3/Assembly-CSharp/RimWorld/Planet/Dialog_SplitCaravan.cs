using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001807 RID: 6151
	public class Dialog_SplitCaravan : Window
	{
		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x06008FD1 RID: 36817 RVA: 0x00273565 File Offset: 0x00271765
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x06008FD2 RID: 36818 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x06008FD3 RID: 36819 RVA: 0x00337D1D File Offset: 0x00335F1D
		private BiomeDef Biome
		{
			get
			{
				return this.caravan.Biome;
			}
		}

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x06008FD4 RID: 36820 RVA: 0x00337D2A File Offset: 0x00335F2A
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

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x06008FD5 RID: 36821 RVA: 0x00337D58 File Offset: 0x00335F58
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

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x06008FD6 RID: 36822 RVA: 0x00337DA0 File Offset: 0x00335FA0
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

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x06008FD7 RID: 36823 RVA: 0x00337E24 File Offset: 0x00336024
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

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x06008FD8 RID: 36824 RVA: 0x00337F94 File Offset: 0x00336194
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

		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x06008FD9 RID: 36825 RVA: 0x00337FE8 File Offset: 0x003361E8
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

		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x06008FDA RID: 36826 RVA: 0x0033802E File Offset: 0x0033622E
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

		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x06008FDB RID: 36827 RVA: 0x0033805C File Offset: 0x0033625C
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

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x06008FDC RID: 36828 RVA: 0x003380A4 File Offset: 0x003362A4
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

		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x06008FDD RID: 36829 RVA: 0x00338128 File Offset: 0x00336328
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

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06008FDE RID: 36830 RVA: 0x00338254 File Offset: 0x00336454
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

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06008FDF RID: 36831 RVA: 0x003382A8 File Offset: 0x003364A8
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

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06008FE0 RID: 36832 RVA: 0x003382EE File Offset: 0x003364EE
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

		// Token: 0x06008FE1 RID: 36833 RVA: 0x0033832C File Offset: 0x0033652C
		public Dialog_SplitCaravan(Caravan caravan)
		{
			this.caravan = caravan;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06008FE2 RID: 36834 RVA: 0x003383C4 File Offset: 0x003365C4
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x06008FE3 RID: 36835 RVA: 0x003383D4 File Offset: 0x003365D4
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
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("TravelSupplies".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.FoodAndMedicine;
			}, this.tab == Dialog_SplitCaravan.Tab.FoodAndMedicine));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_SplitCaravan.tabsList, 200f);
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

		// Token: 0x06008FE4 RID: 36836 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06008FE5 RID: 36837 RVA: 0x00338680 File Offset: 0x00336880
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
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + t);
				return;
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x06008FE6 RID: 36838 RVA: 0x003386DC File Offset: 0x003368DC
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

		// Token: 0x06008FE7 RID: 36839 RVA: 0x0033881C File Offset: 0x00336A1C
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.foodAndMedicineTransfer, "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, () => this.DestMassCapacity - this.DestMassUsage, false, this.caravan.Tile, false);
			this.CountToTransferChanged();
		}

		// Token: 0x06008FE8 RID: 36840 RVA: 0x0033888C File Offset: 0x00336A8C
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
							Log.Error("Error while splitting a caravan: Thing " + thing + " has no owner. Where did it come from then?");
							return;
						}
						CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, newCaravan.PawnsListForReading, null, numToTake);
					});
				}
				TransferableUtility.TransferNoSplit(things, countToTransfer, transfer, true, true);
			}
			return true;
		}

		// Token: 0x06008FE9 RID: 36841 RVA: 0x003389C0 File Offset: 0x00336BC0
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("SourceCaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008FEA RID: 36842 RVA: 0x00338AA0 File Offset: 0x00336CA0
		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		// Token: 0x06008FEB RID: 36843 RVA: 0x00338AD8 File Offset: 0x00336CD8
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x06008FEC RID: 36844 RVA: 0x00338B10 File Offset: 0x00336D10
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

		// Token: 0x06008FED RID: 36845 RVA: 0x00338B44 File Offset: 0x00336D44
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

		// Token: 0x04005A58 RID: 23128
		private Caravan caravan;

		// Token: 0x04005A59 RID: 23129
		private List<TransferableOneWay> transferables;

		// Token: 0x04005A5A RID: 23130
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04005A5B RID: 23131
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04005A5C RID: 23132
		private TransferableOneWayWidget foodAndMedicineTransfer;

		// Token: 0x04005A5D RID: 23133
		private Dialog_SplitCaravan.Tab tab;

		// Token: 0x04005A5E RID: 23134
		private bool sourceMassUsageDirty = true;

		// Token: 0x04005A5F RID: 23135
		private float cachedSourceMassUsage;

		// Token: 0x04005A60 RID: 23136
		private bool sourceMassCapacityDirty = true;

		// Token: 0x04005A61 RID: 23137
		private float cachedSourceMassCapacity;

		// Token: 0x04005A62 RID: 23138
		private string cachedSourceMassCapacityExplanation;

		// Token: 0x04005A63 RID: 23139
		private bool sourceTilesPerDayDirty = true;

		// Token: 0x04005A64 RID: 23140
		private float cachedSourceTilesPerDay;

		// Token: 0x04005A65 RID: 23141
		private string cachedSourceTilesPerDayExplanation;

		// Token: 0x04005A66 RID: 23142
		private bool sourceDaysWorthOfFoodDirty = true;

		// Token: 0x04005A67 RID: 23143
		private Pair<float, float> cachedSourceDaysWorthOfFood;

		// Token: 0x04005A68 RID: 23144
		private bool sourceForagedFoodPerDayDirty = true;

		// Token: 0x04005A69 RID: 23145
		private Pair<ThingDef, float> cachedSourceForagedFoodPerDay;

		// Token: 0x04005A6A RID: 23146
		private string cachedSourceForagedFoodPerDayExplanation;

		// Token: 0x04005A6B RID: 23147
		private bool sourceVisibilityDirty = true;

		// Token: 0x04005A6C RID: 23148
		private float cachedSourceVisibility;

		// Token: 0x04005A6D RID: 23149
		private string cachedSourceVisibilityExplanation;

		// Token: 0x04005A6E RID: 23150
		private bool destMassUsageDirty = true;

		// Token: 0x04005A6F RID: 23151
		private float cachedDestMassUsage;

		// Token: 0x04005A70 RID: 23152
		private bool destMassCapacityDirty = true;

		// Token: 0x04005A71 RID: 23153
		private float cachedDestMassCapacity;

		// Token: 0x04005A72 RID: 23154
		private string cachedDestMassCapacityExplanation;

		// Token: 0x04005A73 RID: 23155
		private bool destTilesPerDayDirty = true;

		// Token: 0x04005A74 RID: 23156
		private float cachedDestTilesPerDay;

		// Token: 0x04005A75 RID: 23157
		private string cachedDestTilesPerDayExplanation;

		// Token: 0x04005A76 RID: 23158
		private bool destDaysWorthOfFoodDirty = true;

		// Token: 0x04005A77 RID: 23159
		private Pair<float, float> cachedDestDaysWorthOfFood;

		// Token: 0x04005A78 RID: 23160
		private bool destForagedFoodPerDayDirty = true;

		// Token: 0x04005A79 RID: 23161
		private Pair<ThingDef, float> cachedDestForagedFoodPerDay;

		// Token: 0x04005A7A RID: 23162
		private string cachedDestForagedFoodPerDayExplanation;

		// Token: 0x04005A7B RID: 23163
		private bool destVisibilityDirty = true;

		// Token: 0x04005A7C RID: 23164
		private float cachedDestVisibility;

		// Token: 0x04005A7D RID: 23165
		private string cachedDestVisibilityExplanation;

		// Token: 0x04005A7E RID: 23166
		private bool ticksToArriveDirty = true;

		// Token: 0x04005A7F RID: 23167
		private int cachedTicksToArrive;

		// Token: 0x04005A80 RID: 23168
		private const float TitleRectHeight = 35f;

		// Token: 0x04005A81 RID: 23169
		private const float BottomAreaHeight = 55f;

		// Token: 0x04005A82 RID: 23170
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04005A83 RID: 23171
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x02002A73 RID: 10867
		private enum Tab
		{
			// Token: 0x04009FDA RID: 40922
			Pawns,
			// Token: 0x04009FDB RID: 40923
			Items,
			// Token: 0x04009FDC RID: 40924
			FoodAndMedicine
		}
	}
}
