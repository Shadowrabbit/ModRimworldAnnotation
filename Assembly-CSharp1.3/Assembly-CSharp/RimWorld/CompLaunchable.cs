using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001149 RID: 4425
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06006A46 RID: 27206 RVA: 0x0023C37C File Offset: 0x0023A57C
		public CompProperties_Launchable Props
		{
			get
			{
				return (CompProperties_Launchable)this.props;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06006A47 RID: 27207 RVA: 0x0023C389 File Offset: 0x0023A589
		public Building FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map);
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06006A48 RID: 27208 RVA: 0x0023C3A6 File Offset: 0x0023A5A6
		public bool ConnectedToFuelingPort
		{
			get
			{
				return !this.Props.requireFuel || this.FuelingPortSource != null;
			}
		}

		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06006A49 RID: 27209 RVA: 0x0023C3C0 File Offset: 0x0023A5C0
		public bool FuelingPortSourceHasAnyFuel
		{
			get
			{
				return !this.Props.requireFuel || (this.ConnectedToFuelingPort && this.FuelingPortSource.GetComp<CompRefuelable>().HasFuel);
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06006A4A RID: 27210 RVA: 0x0023C3EB File Offset: 0x0023A5EB
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.Transporter.LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06006A4B RID: 27211 RVA: 0x0023C3F8 File Offset: 0x0023A5F8
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnythingLeftToLoad;
			}
		}

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06006A4C RID: 27212 RVA: 0x0023C405 File Offset: 0x0023A605
		public Thing FirstThingLeftToLoad
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoad;
			}
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06006A4D RID: 27213 RVA: 0x0023C412 File Offset: 0x0023A612
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06006A4E RID: 27214 RVA: 0x0023C42A File Offset: 0x0023A62A
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06006A4F RID: 27215 RVA: 0x0023C437 File Offset: 0x0023A637
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06006A50 RID: 27216 RVA: 0x0023C444 File Offset: 0x0023A644
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

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06006A51 RID: 27217 RVA: 0x0023C48F File Offset: 0x0023A68F
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

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06006A52 RID: 27218 RVA: 0x0023C4B0 File Offset: 0x0023A6B0
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

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06006A53 RID: 27219 RVA: 0x0023C4D0 File Offset: 0x0023A6D0
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

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06006A54 RID: 27220 RVA: 0x0023C50C File Offset: 0x0023A70C
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

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06006A55 RID: 27221 RVA: 0x0023C548 File Offset: 0x0023A748
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

		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06006A56 RID: 27222 RVA: 0x0023C5B3 File Offset: 0x0023A7B3
		public int MaxLaunchDistance
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

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06006A57 RID: 27223 RVA: 0x0023C5E4 File Offset: 0x0023A7E4
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

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06006A58 RID: 27224 RVA: 0x0023C668 File Offset: 0x0023A868
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

		// Token: 0x06006A59 RID: 27225 RVA: 0x0023C6CE File Offset: 0x0023A8CE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandLaunchGroup".Translate();
				command_Action.defaultDesc = "CommandLaunchGroupDesc".Translate();
				command_Action.icon = CompLaunchable.LaunchCommandTex;
				command_Action.alsoClickIfOtherInGroupClicked = false;
				command_Action.action = delegate()
				{
					if (this.AnyInGroupHasAnythingLeftToLoad)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSendNotCompletelyLoadedPods".Translate(this.FirstThingLeftToLoadInGroup.LabelCapNoCount, this.FirstThingLeftToLoadInGroup), new Action(this.StartChoosingDestination), false, null));
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
			yield break;
			yield break;
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x0023C6E0 File Offset: 0x0023A8E0
		public override string CompInspectStringExtra()
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
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
			return "ReadyForLaunch".Translate();
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x0023C7D4 File Offset: 0x0023A9D4
		public void StartChoosingDestination()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent));
			Find.WorldSelector.ClearSelection();
			int tile = this.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
			{
				GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistance, this.TransportersInGroup.Cast<IThingHolder>(), new Action<int, TransportPodsArrivalAction>(this.TryLaunch), this), null);
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x0023C859 File Offset: 0x0023AA59
		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			return !this.LoadingInProgressOrReadyToLaunch || CompLaunchable.ChoseWorldTarget(target, this.parent.Map.Tile, this.TransportersInGroup.Cast<IThingHolder>(), this.MaxLaunchDistance, new Action<int, TransportPodsArrivalAction>(this.TryLaunch), this);
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x0023C89C File Offset: 0x0023AA9C
		public void TryLaunch(int destinationTile, TransportPodsArrivalAction arrivalAction)
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's unspawned.");
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error("Tried to launch " + this.parent + ", but it's not in any group.");
				return;
			}
			if (!this.LoadingInProgressOrReadyToLaunch || !this.AllInGroupConnectedToFuelingPort || !this.AllFuelingPortSourcesInGroupHaveAnyFuel)
			{
				return;
			}
			Map map = this.parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, true, int.MaxValue);
			if (num > this.MaxLaunchDistance)
			{
				return;
			}
			this.Transporter.TryRemoveLord(map);
			int groupID = this.Transporter.groupID;
			float amount = Mathf.Max(CompLaunchable.FuelNeededToLaunchAtDist((float)num), 1f);
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
				FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(this.Props.skyfallerLeaving ?? ThingDefOf.DropPodLeaving, activeDropPod);
				flyShipLeaving.groupID = groupID;
				flyShipLeaving.destinationTile = destinationTile;
				flyShipLeaving.arrivalAction = arrivalAction;
				flyShipLeaving.worldObjectDef = WorldObjectDefOf.TravelingTransportPods;
				compTransporter.CleanUpLoadingVars(map);
				compTransporter.parent.Destroy(DestroyMode.Vanish);
				GenSpawn.Spawn(flyShipLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
			}
			CameraJumper.TryHideWorld();
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x0023CA58 File Offset: 0x0023AC58
		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x0023CA8C File Offset: 0x0023AC8C
		public static int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return Mathf.FloorToInt(fuelLevel / 2.25f);
		}

		// Token: 0x06006A60 RID: 27232 RVA: 0x0023CA9A File Offset: 0x0023AC9A
		public static float FuelNeededToLaunchAtDist(float dist)
		{
			return 2.25f * dist;
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x0023CAA3 File Offset: 0x0023ACA3
		private IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptionsAt(int tile)
		{
			bool anything = false;
			if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.TransportersInGroup.Cast<IThingHolder>(), tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					this.TryLaunch(tile, new TransportPodsArrivalAction_FormCaravan());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < worldObjects.Count; i = num + 1)
			{
				if (worldObjects[i].Tile == tile)
				{
					foreach (FloatMenuOption floatMenuOption in worldObjects[i].GetTransportPodsFloatMenuOptions(this.TransportersInGroup.Cast<IThingHolder>(), this))
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
					this.TryLaunch(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x0023CABC File Offset: 0x0023ACBC
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

		// Token: 0x06006A63 RID: 27235 RVA: 0x0023CBD1 File Offset: 0x0023ADD1
		public static IEnumerable<FloatMenuOption> GetOptionsForTile(int tile, IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			bool anything = false;
			if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(pods, tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					launchAction(tile, new TransportPodsArrivalAction_FormCaravan("MessageShuttleArrived"));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
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
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x0023CBF0 File Offset: 0x0023ADF0
		public static string TargetingLabelGetter(GlobalTargetInfo target, int tile, int maxLaunchDistance, IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction, CompLaunchable launchable)
		{
			if (!target.IsValid)
			{
				return null;
			}
			if (Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, 2147483647) > maxLaunchDistance)
			{
				GUI.color = ColorLibrary.RedReadable;
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
					GUI.color = ColorLibrary.RedReadable;
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

		// Token: 0x04003B48 RID: 15176
		private CompTransporter cachedCompTransporter;

		// Token: 0x04003B49 RID: 15177
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x04003B4A RID: 15178
		public static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		// Token: 0x04003B4B RID: 15179
		private const float FuelPerTile = 2.25f;
	}
}
