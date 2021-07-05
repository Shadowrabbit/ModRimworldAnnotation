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
	// Token: 0x020012EE RID: 4846
	public class Dialog_LoadTransporters : Window
	{
		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x06007448 RID: 29768 RVA: 0x00276C5F File Offset: 0x00274E5F
		public bool CanChangeAssignedThingsAfterStarting
		{
			get
			{
				return this.transporters[0].Props.canChangeAssignedThingsAfterStarting;
			}
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x06007449 RID: 29769 RVA: 0x00276C77 File Offset: 0x00274E77
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.transporters[0].LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x0600744A RID: 29770 RVA: 0x00273565 File Offset: 0x00271765
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x0600744B RID: 29771 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x0600744C RID: 29772 RVA: 0x00276C8C File Offset: 0x00274E8C
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

		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x0600744D RID: 29773 RVA: 0x00276CD0 File Offset: 0x00274ED0
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

		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x0600744E RID: 29774 RVA: 0x00276D18 File Offset: 0x00274F18
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

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x0600744F RID: 29775 RVA: 0x00276D75 File Offset: 0x00274F75
		private string TransportersLabelCap
		{
			get
			{
				return this.TransportersLabel.CapitalizeFirst();
			}
		}

		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x06007450 RID: 29776 RVA: 0x00276D82 File Offset: 0x00274F82
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x06007451 RID: 29777 RVA: 0x00276D90 File Offset: 0x00274F90
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					CompShuttle shuttle = this.transporters[0].Shuttle;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, shuttle == null || shuttle.requiredColonistCount == 0, false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x06007452 RID: 29778 RVA: 0x00276DE6 File Offset: 0x00274FE6
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

		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x06007453 RID: 29779 RVA: 0x00276E14 File Offset: 0x00275014
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

		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x06007454 RID: 29780 RVA: 0x00276E74 File Offset: 0x00275074
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

		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06007455 RID: 29781 RVA: 0x00276EEC File Offset: 0x002750EC
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

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x06007456 RID: 29782 RVA: 0x00276F40 File Offset: 0x00275140
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

		// Token: 0x06007457 RID: 29783 RVA: 0x00276F88 File Offset: 0x00275188
		public Dialog_LoadTransporters(Map map, List<CompTransporter> transporters)
		{
			this.map = map;
			this.transporters = new List<CompTransporter>();
			this.transporters.AddRange(transporters);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06007458 RID: 29784 RVA: 0x00277018 File Offset: 0x00275218
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				this.SetLoadedItemsToLoad();
			}
		}

		// Token: 0x06007459 RID: 29785 RVA: 0x0027703C File Offset: 0x0027523C
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
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_LoadTransporters.tabsList, 200f);
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

		// Token: 0x0600745A RID: 29786 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x0600745B RID: 29787 RVA: 0x00277278 File Offset: 0x00275478
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
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + t);
				return;
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x0600745C RID: 29788 RVA: 0x002772D4 File Offset: 0x002754D4
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, this.autoLoot ? "LoadSelected".Translate() : "AcceptButton".Translate(), true, true, true))
			{
				if (this.CaravanMassUsage > this.CaravanMassCapacity && this.CaravanMassCapacity != 0f && (this.transporters[0].Shuttle == null || this.transporters[0].Shuttle.shipParent == null || !this.transporters[0].Shuttle.shipParent.HasPredeterminedDestination))
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

		// Token: 0x0600745D RID: 29789 RVA: 0x00277584 File Offset: 0x00275784
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
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, this.map.Tile, true, true, true, false, true, false, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, this.map.Tile, true, false, false, true, false, true, false, false);
			this.CountToTransferChanged();
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x00277728 File Offset: 0x00275928
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

		// Token: 0x0600745F RID: 29791 RVA: 0x002777B8 File Offset: 0x002759B8
		private void LoadInstantly()
		{
			TransporterUtility.InitiateLoading(this.transporters);
			int i;
			int j;
			for (i = 0; i < this.transferables.Count; i = j + 1)
			{
				TransferableUtility.Transfer(this.transferables[i].things, this.transferables[i].CountToTransfer, delegate(Thing splitPiece, IThingHolder originalThing)
				{
					this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece.TryMakeMinified(), true);
				});
				j = i;
			}
		}

		// Token: 0x06007460 RID: 29792 RVA: 0x00277848 File Offset: 0x00275A48
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

		// Token: 0x06007461 RID: 29793 RVA: 0x002779A4 File Offset: 0x00275BA4
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

		// Token: 0x06007462 RID: 29794 RVA: 0x00277B88 File Offset: 0x00275D88
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

		// Token: 0x06007463 RID: 29795 RVA: 0x002780C4 File Offset: 0x002762C4
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
			Pawn pawn = pawns.Find((Pawn x) => !x.MapHeld.reachability.CanReach(x.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)) && !this.transporters.Any((CompTransporter y) => y.innerContainer.Contains(x)));
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
							if (map.reachability.CanReach(t.Position, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)) || this.transporters.Any((CompTransporter x) => x.innerContainer.Contains(t)) || (pawn_CarryTracker != null && pawn_CarryTracker.pawn.MapHeld.reachability.CanReach(pawn_CarryTracker.pawn.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false))))
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

		// Token: 0x06007464 RID: 29796 RVA: 0x00278584 File Offset: 0x00276784
		private void AddPawnsToTransferables()
		{
			foreach (Pawn t in TransporterUtility.AllSendablePawns(this.transporters, this.map, this.autoLoot))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x002785E4 File Offset: 0x002767E4
		private void AddItemsToTransferables()
		{
			foreach (Thing t in TransporterUtility.AllSendableItems(this.transporters, this.map, this.autoLoot))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x00278644 File Offset: 0x00276844
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x06007467 RID: 29799 RVA: 0x00278654 File Offset: 0x00276854
		private void SetToLoadEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x06007468 RID: 29800 RVA: 0x0027869F File Offset: 0x0027689F
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

		// Token: 0x04004008 RID: 16392
		private Map map;

		// Token: 0x04004009 RID: 16393
		private List<CompTransporter> transporters;

		// Token: 0x0400400A RID: 16394
		private List<TransferableOneWay> transferables;

		// Token: 0x0400400B RID: 16395
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400400C RID: 16396
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400400D RID: 16397
		private Dialog_LoadTransporters.Tab tab;

		// Token: 0x0400400E RID: 16398
		private float lastMassFlashTime = -9999f;

		// Token: 0x0400400F RID: 16399
		public bool autoLoot;

		// Token: 0x04004010 RID: 16400
		private bool massUsageDirty = true;

		// Token: 0x04004011 RID: 16401
		private float cachedMassUsage;

		// Token: 0x04004012 RID: 16402
		private bool caravanMassUsageDirty = true;

		// Token: 0x04004013 RID: 16403
		private float cachedCaravanMassUsage;

		// Token: 0x04004014 RID: 16404
		private bool caravanMassCapacityDirty = true;

		// Token: 0x04004015 RID: 16405
		private float cachedCaravanMassCapacity;

		// Token: 0x04004016 RID: 16406
		private string cachedCaravanMassCapacityExplanation;

		// Token: 0x04004017 RID: 16407
		private bool tilesPerDayDirty = true;

		// Token: 0x04004018 RID: 16408
		private float cachedTilesPerDay;

		// Token: 0x04004019 RID: 16409
		private string cachedTilesPerDayExplanation;

		// Token: 0x0400401A RID: 16410
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x0400401B RID: 16411
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x0400401C RID: 16412
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x0400401D RID: 16413
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x0400401E RID: 16414
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x0400401F RID: 16415
		private bool visibilityDirty = true;

		// Token: 0x04004020 RID: 16416
		private float cachedVisibility;

		// Token: 0x04004021 RID: 16417
		private string cachedVisibilityExplanation;

		// Token: 0x04004022 RID: 16418
		private const float TitleRectHeight = 35f;

		// Token: 0x04004023 RID: 16419
		private const float BottomAreaHeight = 55f;

		// Token: 0x04004024 RID: 16420
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04004025 RID: 16421
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04004026 RID: 16422
		private static List<List<TransferableOneWay>> tmpLeftToLoadCopy = new List<List<TransferableOneWay>>();

		// Token: 0x04004027 RID: 16423
		private static Dictionary<TransferableOneWay, int> tmpLeftCountToTransfer = new Dictionary<TransferableOneWay, int>();

		// Token: 0x02002667 RID: 9831
		private enum Tab
		{
			// Token: 0x04009227 RID: 37415
			Pawns,
			// Token: 0x04009228 RID: 37416
			Items
		}
	}
}
