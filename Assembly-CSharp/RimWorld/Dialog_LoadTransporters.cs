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
	// Token: 0x020019EA RID: 6634
	public class Dialog_LoadTransporters : Window
	{
		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x060092BA RID: 37562 RVA: 0x0006244E File Offset: 0x0006064E
		public bool CanChangeAssignedThingsAfterStarting
		{
			get
			{
				return this.transporters[0].Props.canChangeAssignedThingsAfterStarting;
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x060092BB RID: 37563 RVA: 0x00062466 File Offset: 0x00060666
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.transporters[0].LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x060092BC RID: 37564 RVA: 0x0006209B File Offset: 0x0006029B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x060092BD RID: 37565 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x060092BE RID: 37566 RVA: 0x002A3ABC File Offset: 0x002A1CBC
		private float MassCapacity
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.transporters.Count; i++)
				{
					num += this.transporters[i].Props.massCapacity;
				}
				return num;
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x060092BF RID: 37567 RVA: 0x002A3B00 File Offset: 0x002A1D00
		private float CaravanMassCapacity
		{
			get
			{
				if (this.caravanMassCapacityDirty)
				{
					this.caravanMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedCaravanMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedCaravanMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedCaravanMassCapacity;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x060092C0 RID: 37568 RVA: 0x002A3B48 File Offset: 0x002A1D48
		private string TransportersLabel
		{
			get
			{
				if (this.transporters[0].Props.max1PerGroup)
				{
					return this.transporters[0].parent.Label;
				}
				return Find.ActiveLanguageWorker.Pluralize(this.transporters[0].parent.Label, -1);
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x060092C1 RID: 37569 RVA: 0x00062479 File Offset: 0x00060679
		private string TransportersLabelCap
		{
			get
			{
				return this.TransportersLabel.CapitalizeFirst();
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x060092C2 RID: 37570 RVA: 0x00062486 File Offset: 0x00060686
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x060092C3 RID: 37571 RVA: 0x002A3BA8 File Offset: 0x002A1DA8
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					CompShuttle shuttle = this.transporters[0].Shuttle;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, shuttle == null || (shuttle.requiredColonistCount == 0 && !shuttle.IsMissionShuttle), false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x060092C4 RID: 37572 RVA: 0x00062493 File Offset: 0x00060693
		public float CaravanMassUsage
		{
			get
			{
				if (this.caravanMassUsageDirty)
				{
					this.caravanMassUsageDirty = false;
					this.cachedCaravanMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
				}
				return this.cachedCaravanMassUsage;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x060092C5 RID: 37573 RVA: 0x002A3C0C File Offset: 0x002A1E0C
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.map.Tile, -1, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x060092C6 RID: 37574 RVA: 0x002A3C6C File Offset: 0x002A1E6C
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, Faction.OfPlayer, null, 0f, 3300);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, null, 0f, 3300));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x060092C7 RID: 37575 RVA: 0x002A3CE4 File Offset: 0x002A1EE4
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

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x060092C8 RID: 37576 RVA: 0x002A3D38 File Offset: 0x002A1F38
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

		// Token: 0x060092C9 RID: 37577 RVA: 0x002A3D80 File Offset: 0x002A1F80
		public Dialog_LoadTransporters(Map map, List<CompTransporter> transporters)
		{
			this.map = map;
			this.transporters = new List<CompTransporter>();
			this.transporters.AddRange(transporters);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060092CA RID: 37578 RVA: 0x000624BE File Offset: 0x000606BE
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				this.SetLoadedItemsToLoad();
			}
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x002A3E10 File Offset: 0x002A2010
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "LoadTransporters".Translate(this.TransportersLabel));
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.transporters[0].Props.showOverallStats)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, "", this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, this.CaravanMassUsage, this.CaravanMassCapacity, this.cachedCaravanMassCapacityExplanation), null, this.map.Tile, null, this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), false, null, false);
				inRect.yMin += 52f;
			}
			Dialog_LoadTransporters.tabsList.Clear();
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_LoadTransporters.Tab.Pawns;
			}, this.tab == Dialog_LoadTransporters.Tab.Pawns));
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_LoadTransporters.Tab.Items;
			}, this.tab == Dialog_LoadTransporters.Tab.Items));
			inRect.yMin += 67f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_LoadTransporters.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			Dialog_LoadTransporters.Tab tab = this.tab;
			if (tab != Dialog_LoadTransporters.Tab.Pawns)
			{
				if (tab == Dialog_LoadTransporters.Tab.Items)
				{
					this.itemsTransfer.OnGUI(inRect2, out flag);
				}
			}
			else
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		// Token: 0x060092CC RID: 37580 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x060092CD RID: 37581 RVA: 0x002A404C File Offset: 0x002A224C
		private void AddToTransferables(Thing t)
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
		}

		// Token: 0x060092CE RID: 37582 RVA: 0x002A40A8 File Offset: 0x002A22A8
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, this.autoLoot ? "LoadSelected".Translate() : "AcceptButton".Translate(), true, true, true))
			{
				if (this.CaravanMassUsage > this.CaravanMassCapacity && this.CaravanMassCapacity != 0f)
				{
					if (this.CheckForErrors(TransferableUtility.GetPawnsFromTransferables(this.transferables)))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("TransportersCaravanWillBeImmobile".Translate(), delegate
						{
							if (this.TryAccept())
							{
								if (this.autoLoot)
								{
									this.LoadInstantly();
								}
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								this.Close(false);
							}
						}, false, null));
					}
				}
				else if (this.TryAccept())
				{
					if (this.autoLoot)
					{
						this.LoadInstantly();
					}
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
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
			if (Prefs.DevMode)
			{
				float width = 200f;
				float num = this.BottomButtonSize.y / 2f;
				if (!this.LoadingInProgressOrReadyToLaunch && Widgets.ButtonText(new Rect(0f, rect.height - 55f, width, num), "Dev: Load instantly", true, true, true) && this.DebugTryLoadInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(0f, rect.height - 55f + num, width, num), "Dev: Select everything", true, true, true))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToLoadEverything();
				}
			}
		}

		// Token: 0x060092CF RID: 37583 RVA: 0x002A4308 File Offset: 0x002A2508
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					for (int j = 0; j < this.transporters[i].innerContainer.Count; j++)
					{
						this.AddToTransferables(this.transporters[i].innerContainer[j]);
					}
				}
				foreach (Thing t in TransporterUtility.ThingsBeingHauledTo(this.transporters, this.map))
				{
					this.AddToTransferables(t);
				}
			}
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, this.map.Tile, true, true, true, false, true, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, this.map.Tile, true, false, false, true, false, true, false);
			this.CountToTransferChanged();
		}

		// Token: 0x060092D0 RID: 37584 RVA: 0x002A44AC File Offset: 0x002A26AC
		private bool DebugTryLoadInstantly()
		{
			TransporterUtility.InitiateLoading(this.transporters);
			int i;
			int j;
			for (i = 0; i < this.transferables.Count; i = j + 1)
			{
				TransferableUtility.Transfer(this.transferables[i].things, this.transferables[i].CountToTransfer, delegate(Thing splitPiece, IThingHolder originalThing)
				{
					this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece, true);
				});
				j = i;
			}
			return true;
		}

		// Token: 0x060092D1 RID: 37585 RVA: 0x002A453C File Offset: 0x002A273C
		private void LoadInstantly()
		{
			TransporterUtility.InitiateLoading(this.transporters);
			int i;
			int j;
			for (i = 0; i < this.transferables.Count; i = j + 1)
			{
				TransferableUtility.Transfer(this.transferables[i].things, this.transferables[i].CountToTransfer, delegate(Thing splitPiece, IThingHolder originalThing)
				{
					this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece, true);
				});
				j = i;
			}
		}

		// Token: 0x060092D2 RID: 37586 RVA: 0x002A45CC File Offset: 0x002A27CC
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				this.AssignTransferablesToRandomTransporters();
				TransporterUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.transporters, this.map);
				List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter && this.transporters.Contains(((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter))
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
			else
			{
				TransporterUtility.InitiateLoading(this.transporters);
				this.AssignTransferablesToRandomTransporters();
				TransporterUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.transporters, this.map);
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("MessageTransporterSingleLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
				else
				{
					Messages.Message("MessageTransportersLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
			}
			return true;
		}

		// Token: 0x060092D3 RID: 37587 RVA: 0x002A4728 File Offset: 0x002A2928
		private void SetLoadedItemsToLoad()
		{
			int i;
			int num;
			for (i = 0; i < this.transporters.Count; i = num + 1)
			{
				int j;
				for (j = 0; j < this.transporters[i].innerContainer.Count; j = num + 1)
				{
					TransferableOneWay transferableOneWay = this.transferables.Find((TransferableOneWay x) => x.things.Contains(this.transporters[i].innerContainer[j]));
					if (transferableOneWay != null && transferableOneWay.CanAdjustBy(this.transporters[i].innerContainer[j].stackCount).Accepted)
					{
						transferableOneWay.AdjustBy(this.transporters[i].innerContainer[j].stackCount);
					}
					num = j;
				}
				if (this.transporters[i].leftToLoad != null)
				{
					for (int k = 0; k < this.transporters[i].leftToLoad.Count; k++)
					{
						TransferableOneWay transferableOneWay2 = this.transporters[i].leftToLoad[k];
						if (transferableOneWay2.CountToTransfer != 0 && transferableOneWay2.HasAnyThing)
						{
							TransferableOneWay transferableOneWay3 = TransferableUtility.TransferableMatchingDesperate(transferableOneWay2.AnyThing, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay3 != null && transferableOneWay3.CanAdjustBy(transferableOneWay2.CountToTransferToDestination).Accepted)
							{
								transferableOneWay3.AdjustBy(transferableOneWay2.CountToTransferToDestination);
							}
						}
					}
				}
				num = i;
			}
		}

		// Token: 0x060092D4 RID: 37588 RVA: 0x002A490C File Offset: 0x002A2B0C
		private void AssignTransferablesToRandomTransporters()
		{
			Dialog_LoadTransporters.tmpLeftToLoadCopy.Clear();
			for (int i3 = 0; i3 < this.transporters.Count; i3++)
			{
				Dialog_LoadTransporters.tmpLeftToLoadCopy.Add((this.transporters[i3].leftToLoad != null) ? this.transporters[i3].leftToLoad.ToList<TransferableOneWay>() : new List<TransferableOneWay>());
				if (this.transporters[i3].leftToLoad != null)
				{
					this.transporters[i3].leftToLoad.Clear();
				}
			}
			Dialog_LoadTransporters.tmpLeftCountToTransfer.Clear();
			for (int j = 0; j < this.transferables.Count; j++)
			{
				Dialog_LoadTransporters.tmpLeftCountToTransfer.Add(this.transferables[j], this.transferables[j].CountToTransfer);
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				int i2;
				int i;
				Func<Thing, bool> <>9__0;
				for (i = 0; i < this.transferables.Count; i = i2 + 1)
				{
					if (this.transferables[i].HasAnyThing && Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[i]] > 0)
					{
						for (int k = 0; k < Dialog_LoadTransporters.tmpLeftToLoadCopy.Count; k++)
						{
							TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(this.transferables[i].AnyThing, Dialog_LoadTransporters.tmpLeftToLoadCopy[k], TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay != null)
							{
								int num = Mathf.Min(Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[i]], transferableOneWay.CountToTransfer);
								if (num > 0)
								{
									this.transporters[k].AddToTheToLoadList(this.transferables[i], num);
									Dictionary<TransferableOneWay, int> dictionary = Dialog_LoadTransporters.tmpLeftCountToTransfer;
									TransferableOneWay key = this.transferables[i];
									dictionary[key] -= num;
								}
							}
							IEnumerable<Thing> innerContainer = this.transporters[k].innerContainer;
							Func<Thing, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((Thing x) => TransferableUtility.TransferAsOne(this.transferables[i].AnyThing, x, TransferAsOneMode.PodsOrCaravanPacking)));
							}
							Thing thing = innerContainer.FirstOrDefault(predicate);
							if (thing != null)
							{
								int num2 = Mathf.Min(Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[i]], thing.stackCount);
								if (num2 > 0)
								{
									this.transporters[k].AddToTheToLoadList(this.transferables[i], num2);
									Dictionary<TransferableOneWay, int> dictionary = Dialog_LoadTransporters.tmpLeftCountToTransfer;
									TransferableOneWay key = this.transferables[i];
									dictionary[key] -= num2;
								}
							}
						}
					}
					i2 = i;
				}
			}
			Dialog_LoadTransporters.tmpLeftToLoadCopy.Clear();
			if (this.transferables.Any<TransferableOneWay>())
			{
				TransferableOneWay transferableOneWay2 = this.transferables.MaxBy((TransferableOneWay x) => Dialog_LoadTransporters.tmpLeftCountToTransfer[x]);
				int num3 = 0;
				for (int l = 0; l < this.transferables.Count; l++)
				{
					if (this.transferables[l] != transferableOneWay2 && Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[l]] > 0)
					{
						this.transporters[num3 % this.transporters.Count].AddToTheToLoadList(this.transferables[l], Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[l]]);
						num3++;
					}
				}
				if (num3 < this.transporters.Count)
				{
					int num4 = Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay2];
					int num5 = num4 / (this.transporters.Count - num3);
					for (int m = num3; m < this.transporters.Count; m++)
					{
						int num6 = (m == this.transporters.Count - 1) ? num4 : num5;
						if (num6 > 0)
						{
							this.transporters[m].AddToTheToLoadList(transferableOneWay2, num6);
						}
						num4 -= num6;
					}
				}
				else
				{
					this.transporters[num3 % this.transporters.Count].AddToTheToLoadList(transferableOneWay2, Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay2]);
				}
			}
			Dialog_LoadTransporters.tmpLeftCountToTransfer.Clear();
			for (int n = 0; n < this.transporters.Count; n++)
			{
				for (int num7 = 0; num7 < this.transporters[n].innerContainer.Count; num7++)
				{
					Thing thing2 = this.transporters[n].innerContainer[num7];
					int num8 = this.transporters[n].SubtractFromToLoadList(thing2, thing2.stackCount, false);
					if (num8 < thing2.stackCount)
					{
						Thing thing3;
						this.transporters[n].innerContainer.TryDrop(thing2, ThingPlaceMode.Near, thing2.stackCount - num8, out thing3, null, null);
					}
				}
			}
		}

		// Token: 0x060092D5 RID: 37589 RVA: 0x002A4E48 File Offset: 0x002A3048
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!this.CanChangeAssignedThingsAfterStarting)
			{
				if (!this.transferables.Any((TransferableOneWay x) => x.CountToTransfer != 0))
				{
					if (this.transporters[0].Props.max1PerGroup)
					{
						Messages.Message("CantSendEmptyTransporterSingle".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message("CantSendEmptyTransportPods".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			if (this.transporters[0].Props.max1PerGroup)
			{
				CompShuttle shuttle = this.transporters[0].Shuttle;
				if (shuttle != null && shuttle.requiredColonistCount > 0 && pawns.Count > shuttle.requiredColonistCount)
				{
					Messages.Message("TransporterSingleTooManyColonists".Translate(shuttle.requiredColonistCount), MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			if (this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("TooBigTransporterSingleMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("TooBigTransportersMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			Pawn pawn = pawns.Find((Pawn x) => !x.MapHeld.reachability.CanReach(x.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)) && !this.transporters.Any((CompTransporter y) => y.innerContainer.Contains(x)));
			if (pawn != null)
			{
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("PawnCantReachTransporterSingle".Translate(pawn.LabelShort, pawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("PawnCantReachTransporters".Translate(pawn.LabelShort, pawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			Map map = this.transporters[0].parent.Map;
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
							Pawn_CarryTracker pawn_CarryTracker = t.ParentHolder as Pawn_CarryTracker;
							if (map.reachability.CanReach(t.Position, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)) || this.transporters.Any((CompTransporter x) => x.innerContainer.Contains(t)) || (pawn_CarryTracker != null && pawn_CarryTracker.pawn.MapHeld.reachability.CanReach(pawn_CarryTracker.pawn.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false))))
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
								if (this.transporters[0].Props.max1PerGroup)
								{
									Messages.Message("TransporterSingleItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
								}
								else
								{
									Messages.Message("TransporterItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
								}
							}
							else if (this.transporters[0].Props.max1PerGroup)
							{
								Messages.Message("TransporterSingleItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("TransporterItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060092D6 RID: 37590 RVA: 0x002A5304 File Offset: 0x002A3504
		private void AddPawnsToTransferables()
		{
			foreach (Pawn t in TransporterUtility.AllSendablePawns_NewTmp(this.transporters, this.map, this.autoLoot))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x060092D7 RID: 37591 RVA: 0x002A5364 File Offset: 0x002A3564
		private void AddItemsToTransferables()
		{
			foreach (Thing t in TransporterUtility.AllSendableItems_NewTmp(this.transporters, this.map, this.autoLoot))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x060092D8 RID: 37592 RVA: 0x000624E2 File Offset: 0x000606E2
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x060092D9 RID: 37593 RVA: 0x002A53C4 File Offset: 0x002A35C4
		private void SetToLoadEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x060092DA RID: 37594 RVA: 0x000624EF File Offset: 0x000606EF
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.caravanMassUsageDirty = true;
			this.caravanMassCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x04005CE5 RID: 23781
		private Map map;

		// Token: 0x04005CE6 RID: 23782
		private List<CompTransporter> transporters;

		// Token: 0x04005CE7 RID: 23783
		private List<TransferableOneWay> transferables;

		// Token: 0x04005CE8 RID: 23784
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04005CE9 RID: 23785
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04005CEA RID: 23786
		private Dialog_LoadTransporters.Tab tab;

		// Token: 0x04005CEB RID: 23787
		private float lastMassFlashTime = -9999f;

		// Token: 0x04005CEC RID: 23788
		public bool autoLoot;

		// Token: 0x04005CED RID: 23789
		private bool massUsageDirty = true;

		// Token: 0x04005CEE RID: 23790
		private float cachedMassUsage;

		// Token: 0x04005CEF RID: 23791
		private bool caravanMassUsageDirty = true;

		// Token: 0x04005CF0 RID: 23792
		private float cachedCaravanMassUsage;

		// Token: 0x04005CF1 RID: 23793
		private bool caravanMassCapacityDirty = true;

		// Token: 0x04005CF2 RID: 23794
		private float cachedCaravanMassCapacity;

		// Token: 0x04005CF3 RID: 23795
		private string cachedCaravanMassCapacityExplanation;

		// Token: 0x04005CF4 RID: 23796
		private bool tilesPerDayDirty = true;

		// Token: 0x04005CF5 RID: 23797
		private float cachedTilesPerDay;

		// Token: 0x04005CF6 RID: 23798
		private string cachedTilesPerDayExplanation;

		// Token: 0x04005CF7 RID: 23799
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04005CF8 RID: 23800
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04005CF9 RID: 23801
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04005CFA RID: 23802
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04005CFB RID: 23803
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04005CFC RID: 23804
		private bool visibilityDirty = true;

		// Token: 0x04005CFD RID: 23805
		private float cachedVisibility;

		// Token: 0x04005CFE RID: 23806
		private string cachedVisibilityExplanation;

		// Token: 0x04005CFF RID: 23807
		private const float TitleRectHeight = 35f;

		// Token: 0x04005D00 RID: 23808
		private const float BottomAreaHeight = 55f;

		// Token: 0x04005D01 RID: 23809
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04005D02 RID: 23810
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04005D03 RID: 23811
		private static List<List<TransferableOneWay>> tmpLeftToLoadCopy = new List<List<TransferableOneWay>>();

		// Token: 0x04005D04 RID: 23812
		private static Dictionary<TransferableOneWay, int> tmpLeftCountToTransfer = new Dictionary<TransferableOneWay, int>();

		// Token: 0x020019EB RID: 6635
		private enum Tab
		{
			// Token: 0x04005D06 RID: 23814
			Pawns,
			// Token: 0x04005D07 RID: 23815
			Items
		}
	}
}
