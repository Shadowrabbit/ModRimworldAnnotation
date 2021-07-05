using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017D0 RID: 6096
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x060086D6 RID: 34518 RVA: 0x0005A7E2 File Offset: 0x000589E2
		public CompProperties_Launchable Props
		{
			get
			{
				return (CompProperties_Launchable)this.props;
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x060086D7 RID: 34519 RVA: 0x0005A7EF File Offset: 0x000589EF
		public Building FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map);
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x060086D8 RID: 34520 RVA: 0x0005A80C File Offset: 0x00058A0C
		public bool ConnectedToFuelingPort
		{
			get
			{
				return !this.Props.requireFuel || this.FuelingPortSource != null;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x060086D9 RID: 34521 RVA: 0x0005A826 File Offset: 0x00058A26
		public bool FuelingPortSourceHasAnyFuel
		{
			get
			{
				return !this.Props.requireFuel || (this.ConnectedToFuelingPort && this.FuelingPortSource.GetComp<CompRefuelable>().HasFuel);
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x060086DA RID: 34522 RVA: 0x0005A851 File Offset: 0x00058A51
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x060086DB RID: 34523 RVA: 0x0005A85E File Offset: 0x00058A5E
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnythingLeftToLoad;
			}
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x060086DC RID: 34524 RVA: 0x0005A86B File Offset: 0x00058A6B
		public Thing FirstThingLeftToLoad
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoad;
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x060086DD RID: 34525 RVA: 0x0005A878 File Offset: 0x00058A78
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x060086DE RID: 34526 RVA: 0x0005A890 File Offset: 0x00058A90
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x060086DF RID: 34527 RVA: 0x0005A89D File Offset: 0x00058A9D
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x060086E0 RID: 34528 RVA: 0x00279EE4 File Offset: 0x002780E4
		public bool AnyInGroupIsUnderRoof
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (transportersInGroup[i].parent.Position.Roofed(this.parent.Map))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x060086E1 RID: 34529 RVA: 0x0005A8AA File Offset: 0x00058AAA
		public CompTransporter Transporter
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.parent.GetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x060086E2 RID: 34530 RVA: 0x0005A8CB File Offset: 0x00058ACB
		public float FuelingPortSourceFuel
		{
			get
			{
				if (!this.ConnectedToFuelingPort)
				{
					return 0f;
				}
				return this.FuelingPortSource.GetComp<CompRefuelable>().Fuel;
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x060086E3 RID: 34531 RVA: 0x00279F30 File Offset: 0x00278130
		public bool AllInGroupConnectedToFuelingPort
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (!transportersInGroup[i].Launchable.ConnectedToFuelingPort)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x060086E4 RID: 34532 RVA: 0x00279F6C File Offset: 0x0027816C
		public bool AllFuelingPortSourcesInGroupHaveAnyFuel
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (!transportersInGroup[i].Launchable.FuelingPortSourceHasAnyFuel)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x060086E5 RID: 34533 RVA: 0x00279FA8 File Offset: 0x002781A8
		private float FuelInLeastFueledFuelingPortSource
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				bool flag = false;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					float num2 = this.Props.requireFuel ? transportersInGroup[i].Launchable.FuelingPortSourceFuel : float.PositiveInfinity;
					if (!flag || num2 < num)
					{
						num = num2;
						flag = true;
					}
				}
				if (!flag)
				{
					return 0f;
				}
				return num;
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x060086E6 RID: 34534 RVA: 0x0005A8EB File Offset: 0x00058AEB
		private int MaxLaunchDistance
		{
			get
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					return 0;
				}
				if (this.Props.fixedLaunchDistanceMax >= 0)
				{
					return this.Props.fixedLaunchDistanceMax;
				}
				return CompLaunchable.MaxLaunchDistanceAtFuelLevel(this.FuelInLeastFueledFuelingPortSource);
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x060086E7 RID: 34535 RVA: 0x0027A014 File Offset: 0x00278214
		private int MaxLaunchDistanceEverPossible
		{
			get
			{
				if (!this.LoadingInProgressOrReadyToLaunch)
				{
					return 0;
				}
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					Building fuelingPortSource = transportersInGroup[i].Launchable.FuelingPortSource;
					if (fuelingPortSource != null)
					{
						num = Mathf.Max(num, fuelingPortSource.GetComp<CompRefuelable>().Props.fuelCapacity);
					}
				}
				if (this.Props.fixedLaunchDistanceMax >= 0)
				{
					return this.Props.fixedLaunchDistanceMax;
				}
				return CompLaunchable.MaxLaunchDistanceAtFuelLevel(num);
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x060086E8 RID: 34536 RVA: 0x0027A098 File Offset: 0x00278298
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					ThingOwner innerContainer = transportersInGroup[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x060086E9 RID: 34537 RVA: 0x0027A100 File Offset: 0x00278300
		public bool CanTryLaunch
		{
			get
			{
				CompShuttle compShuttle = this.parent.TryGetComp<CompShuttle>();
				return compShuttle == null || ((compShuttle.permitShuttle || compShuttle.IsMissionShuttle) && this.Transporter.innerContainer.Any<Thing>());
			}
		}

		// Token: 0x060086EA RID: 34538 RVA: 0x0005A91C File Offset: 0x00058B1C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			CompShuttle shuttleComp = this.parent.TryGetComp<CompShuttle>();
			if (this.LoadingInProgressOrReadyToLaunch && this.CanTryLaunch)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandLaunchGroup".Translate();
				command_Action.defaultDesc = "CommandLaunchGroupDesc".Translate();
				command_Action.icon = CompLaunchable.LaunchCommandTex;
				command_Action.alsoClickIfOtherInGroupClicked = false;
				if (shuttleComp != null && shuttleComp.IsMissionShuttle && !shuttleComp.AllRequiredThingsLoaded)
				{
					command_Action.Disable("ShuttleRequiredItemsNotSatisfied".Translate());
				}
				command_Action.action = delegate()
				{
					if (this.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(this.FirstThingLeftToLoadInGroup.LabelCapNoCount, this.FirstThingLeftToLoadInGroup), new Action(this.StartChoosingDestination), false, null));
						return;
					}
					if (shuttleComp != null && shuttleComp.IsMissionShuttle)
					{
						TransportPodsArrivalAction_Shuttle transportPodsArrivalAction_Shuttle = new TransportPodsArrivalAction_Shuttle((MapParent)shuttleComp.missionShuttleTarget);
						transportPodsArrivalAction_Shuttle.missionShuttleHome = shuttleComp.missionShuttleHome;
						transportPodsArrivalAction_Shuttle.missionShuttleTarget = shuttleComp.missionShuttleTarget;
						transportPodsArrivalAction_Shuttle.sendAwayIfQuestFinished = shuttleComp.sendAwayIfQuestFinished;
						transportPodsArrivalAction_Shuttle.questTags = this.parent.questTags;
						this.TryLaunch((this.parent.Tile == shuttleComp.missionShuttleTarget.Tile) ? shuttleComp.missionShuttleHome.Tile : shuttleComp.missionShuttleTarget.Tile, transportPodsArrivalAction_Shuttle);
						return;
					}
					this.StartChoosingDestination();
				};
				if (!this.AllInGroupConnectedToFuelingPort)
				{
					command_Action.Disable("CommandLaunchGroupFailNotConnectedToFuelingPort".Translate());
				}
				else if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					command_Action.Disable("CommandLaunchGroupFailNoFuel".Translate());
				}
				else if (this.AnyInGroupIsUnderRoof)
				{
					command_Action.Disable("CommandLaunchGroupFailUnderRoof".Translate());
				}
				yield return command_Action;
			}
			if (shuttleComp != null && shuttleComp.permitShuttle)
			{
				Command_Action command_Action2 = new Command_Action
				{
					defaultLabel = "CommandShuttleDismiss".Translate(),
					defaultDesc = "CommandShuttleDismissDesc".Translate(),
					icon = CompLaunchable.DismissTex,
					alsoClickIfOtherInGroupClicked = false,
					action = delegate()
					{
						this.Transporter.innerContainer.TryDropAll(this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null);
						if (!this.LoadingInProgressOrReadyToLaunch)
						{
							TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
						}
						shuttleComp.Send();
					}
				};
				yield return command_Action2;
			}
			yield break;
			yield break;
		}

		// Token: 0x060086EB RID: 34539 RVA: 0x0027A140 File Offset: 0x00278340
		public override string CompInspectStringExtra()
		{
			CompShuttle compShuttle = this.parent.TryGetComp<CompShuttle>();
			if (this.LoadingInProgressOrReadyToLaunch && this.CanTryLaunch)
			{
				if (!this.AllInGroupConnectedToFuelingPort)
				{
					return "NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate().CapitalizeFirst() + ".";
				}
				if (!this.AllFuelingPortSourcesInGroupHaveAnyFuel)
				{
					return "NotReadyForLaunch".Translate() + ": " + "NotAllFuelingPortSourcesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
				}
				if (this.AnyInGroupHasAnythingLeftToLoad)
				{
					return "NotReadyForLaunch".Translate() + ": " + "TransportPodInGroupHasSomethingLeftToLoad".Translate().CapitalizeFirst() + ".";
				}
				if (compShuttle == null || !compShuttle.IsMissionShuttle || compShuttle.AllRequiredThingsLoaded)
				{
					return "ReadyForLaunch".Translate();
				}
			}
			return null;
		}

		// Token: 0x060086EC RID: 34540 RVA: 0x0027A25C File Offset: 0x0027845C
		public void StartChoosingDestination()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent));
			Find.WorldSelector.ClearSelection();
			int tile = this.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting_NewTemp(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
			{
				GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistance, this.TransportersInGroup.Cast<IThingHolder>(), new Action<int, TransportPodsArrivalAction>(this.TryLaunch), this), null);
		}

		// Token: 0x060086ED RID: 34541 RVA: 0x0005A92C File Offset: 0x00058B2C
		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			return !this.LoadingInProgressOrReadyToLaunch || CompLaunchable.ChoseWorldTarget(target, this.parent.Map.Tile, this.TransportersInGroup.Cast<IThingHolder>(), this.MaxLaunchDistance, new Action<int, TransportPodsArrivalAction>(this.TryLaunch), this);
		}

		// Token: 0x060086EE RID: 34542 RVA: 0x0027A2E4 File Offset: 0x002784E4
		public void TryLaunch(int destinationTile, TransportPodsArrivalAction arrivalAction)
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's unspawned.", false);
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's not in any group.", false);
				return;
			}
			if (!this.LoadingInProgressOrReadyToLaunch || !this.AllInGroupConnectedToFuelingPort || !this.AllFuelingPortSourcesInGroupHaveAnyFuel)
			{
				return;
			}
			Map map = this.parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, true, int.MaxValue);
			CompShuttle compShuttle = this.parent.TryGetComp<CompShuttle>();
			if (num > this.MaxLaunchDistance && (compShuttle == null || !compShuttle.IsMissionShuttle))
			{
				return;
			}
			this.Transporter.TryRemoveLord(map);
			int groupID = this.Transporter.groupID;
			float amount = Mathf.Max(CompLaunchable.FuelNeededToLaunchAtDist((float)num), 1f);
			if (compShuttle != null)
			{
				compShuttle.SendLaunchedSignals(transportersInGroup);
			}
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				CompTransporter compTransporter = transportersInGroup[i];
				Building fuelingPortSource = compTransporter.Launchable.FuelingPortSource;
				if (fuelingPortSource != null)
				{
					fuelingPortSource.TryGetComp<CompRefuelable>().ConsumeFuel(amount);
				}
				ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
				ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
				activeDropPod.Contents = new ActiveDropPodInfo();
				activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);
				DropPodLeaving dropPodLeaving = (DropPodLeaving)SkyfallerMaker.MakeSkyfaller(this.Props.skyfallerLeaving ?? ThingDefOf.DropPodLeaving, activeDropPod);
				dropPodLeaving.groupID = groupID;
				dropPodLeaving.destinationTile = destinationTile;
				dropPodLeaving.arrivalAction = arrivalAction;
				dropPodLeaving.worldObjectDef = ((compShuttle != null) ? WorldObjectDefOf.TravelingShuttle : WorldObjectDefOf.TravelingTransportPods);
				compTransporter.CleanUpLoadingVars(map);
				compTransporter.parent.Destroy(DestroyMode.Vanish);
				GenSpawn.Spawn(dropPodLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
			}
			CameraJumper.TryHideWorld();
		}

		// Token: 0x060086EF RID: 34543 RVA: 0x0005A96C File Offset: 0x00058B6C
		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060086F0 RID: 34544 RVA: 0x0005A9A0 File Offset: 0x00058BA0
		public static int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return Mathf.FloorToInt(fuelLevel / 2.25f);
		}

		// Token: 0x060086F1 RID: 34545 RVA: 0x0005A9AE File Offset: 0x00058BAE
		public static float FuelNeededToLaunchAtDist(float dist)
		{
			return 2.25f * dist;
		}

		// Token: 0x060086F2 RID: 34546 RVA: 0x0005A9B7 File Offset: 0x00058BB7
		private IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptionsAt(int tile)
		{
			if (this.parent.TryGetComp<CompShuttle>() != null)
			{
				IEnumerable<FloatMenuOption> optionsForTile = CompLaunchable.GetOptionsForTile(tile, this.TransportersInGroup.Cast<IThingHolder>(), new Action<int, TransportPodsArrivalAction>(this.TryLaunch));
				foreach (FloatMenuOption floatMenuOption in optionsForTile)
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				yield break;
			}
			bool anything = false;
			if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.TransportersInGroup.Cast<IThingHolder>(), tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					this.TryLaunch(tile, new TransportPodsArrivalAction_FormCaravan());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < worldObjects.Count; i = num + 1)
			{
				if (worldObjects[i].Tile == tile)
				{
					foreach (FloatMenuOption floatMenuOption2 in worldObjects[i].GetTransportPodsFloatMenuOptions(this.TransportersInGroup.Cast<IThingHolder>(), this))
					{
						anything = true;
						yield return floatMenuOption2;
					}
					IEnumerator<FloatMenuOption> enumerator = null;
				}
				num = i;
			}
			if (!anything && !Find.World.Impassable(tile))
			{
				yield return new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate()
				{
					this.TryLaunch(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
			yield break;
		}

		// Token: 0x060086F3 RID: 34547 RVA: 0x0027A4D0 File Offset: 0x002786D0
		public static bool ChoseWorldTarget(GlobalTargetInfo target, int tile, IEnumerable<IThingHolder> pods, int maxLaunchDistance, Action<int, TransportPodsArrivalAction> launchAction, CompLaunchable launchable)
		{
			if (!target.IsValid)
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, 2147483647) > maxLaunchDistance)
			{
				Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			IEnumerable<FloatMenuOption> source = (launchable != null) ? launchable.GetTransportPodsFloatMenuOptionsAt(target.Tile) : CompLaunchable.GetOptionsForTile(target.Tile, pods, launchAction);
			if (!source.Any<FloatMenuOption>())
			{
				if (Find.World.Impassable(target.Tile))
				{
					Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				launchAction(target.Tile, null);
				return true;
			}
			else
			{
				if (source.Count<FloatMenuOption>() != 1)
				{
					Find.WindowStack.Add(new FloatMenu(source.ToList<FloatMenuOption>()));
					return false;
				}
				if (!source.First<FloatMenuOption>().Disabled)
				{
					source.First<FloatMenuOption>().action();
					return true;
				}
				return false;
			}
		}

		// Token: 0x060086F4 RID: 34548 RVA: 0x0005A9CE File Offset: 0x00058BCE
		public static IEnumerable<FloatMenuOption> GetOptionsForTile(int tile, IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			bool anything = false;
			if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(pods, tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					launchAction(tile, new TransportPodsArrivalAction_FormCaravan("MessageShuttleArrived"));
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < worldObjects.Count; i = num + 1)
			{
				if (worldObjects[i].Tile == tile)
				{
					foreach (FloatMenuOption floatMenuOption in worldObjects[i].GetShuttleFloatMenuOptions(pods, launchAction))
					{
						anything = true;
						yield return floatMenuOption;
					}
					IEnumerator<FloatMenuOption> enumerator = null;
				}
				num = i;
			}
			if (!anything && !Find.World.Impassable(tile))
			{
				yield return new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate()
				{
					launchAction(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
			yield break;
		}

		// Token: 0x060086F5 RID: 34549 RVA: 0x0027A5E8 File Offset: 0x002787E8
		public static string TargetingLabelGetter(GlobalTargetInfo target, int tile, int maxLaunchDistance, IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction, CompLaunchable launchable)
		{
			if (!target.IsValid)
			{
				return null;
			}
			if (Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, 2147483647) > maxLaunchDistance)
			{
				GUI.color = ColoredText.RedReadable;
				return "TransportPodDestinationBeyondMaximumRange".Translate();
			}
			IEnumerable<FloatMenuOption> source = (launchable != null) ? launchable.GetTransportPodsFloatMenuOptionsAt(target.Tile) : CompLaunchable.GetOptionsForTile(target.Tile, pods, launchAction);
			if (!source.Any<FloatMenuOption>())
			{
				return string.Empty;
			}
			if (source.Count<FloatMenuOption>() == 1)
			{
				if (source.First<FloatMenuOption>().Disabled)
				{
					GUI.color = ColoredText.RedReadable;
				}
				return source.First<FloatMenuOption>().Label;
			}
			MapParent mapParent;
			if ((mapParent = (target.WorldObject as MapParent)) != null)
			{
				return "ClickToSeeAvailableOrders_WorldObject".Translate(mapParent.LabelCap);
			}
			return "ClickToSeeAvailableOrders_Empty".Translate();
		}

		// Token: 0x040056AC RID: 22188
		private CompTransporter cachedCompTransporter;

		// Token: 0x040056AD RID: 22189
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x040056AE RID: 22190
		public static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		// Token: 0x040056AF RID: 22191
		private static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Commands/DismissShuttle", true);

		// Token: 0x040056B0 RID: 22192
		private const float FuelPerTile = 2.25f;
	}
}
