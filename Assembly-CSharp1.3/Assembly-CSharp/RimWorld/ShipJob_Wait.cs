using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F1 RID: 2289
	[StaticConstructorOnStartup]
	public abstract class ShipJob_Wait : ShipJob
	{
		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003BFB RID: 15355 RVA: 0x0014E669 File Offset: 0x0014C869
		public override bool ShowGizmos
		{
			get
			{
				return this.showGizmos;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003BFC RID: 15356 RVA: 0x0014E671 File Offset: 0x0014C871
		private int MaxLaunchDistance
		{
			get
			{
				return this.transportShip.def.maxLaunchDistance;
			}
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0014E683 File Offset: 0x0014C883
		public override IEnumerable<Gizmo> GetJobGizmos()
		{
			if (this.transportShip.ShuttleComp.permitShuttle)
			{
				if (this.transportShip.TransporterComp.LoadingInProgressOrReadyToLaunch && this.transportShip.TransporterComp.innerContainer.Any<Thing>())
				{
					yield return this.LaunchAction();
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "CommandShuttleDismiss".Translate(),
					defaultDesc = "CommandShuttleDismissDesc".Translate(),
					icon = ShipJob_Wait.DismissTex,
					alsoClickIfOtherInGroupClicked = false,
					action = delegate()
					{
						this.transportShip.ForceJob(ShipJobDefOf.Unload);
						this.transportShip.AddJob(ShipJobDefOf.FlyAway);
					}
				};
				yield return command_Action;
			}
			else if (!this.leaveImmediatelyWhenSatisfied)
			{
				Command_Action command_Action2 = new Command_Action();
				command_Action2.defaultLabel = "CommandSendShuttle".Translate();
				command_Action2.defaultDesc = "CommandSendShuttleDesc".Translate();
				command_Action2.icon = ShipJob_Wait.SendCommandTex;
				command_Action2.alsoClickIfOtherInGroupClicked = false;
				command_Action2.action = delegate()
				{
					this.transportShip.ForceJob(ShipJobDefOf.FlyAway);
				};
				if (!this.transportShip.ShuttleComp.AllRequiredThingsLoaded)
				{
					command_Action2.Disable("CommandSendShuttleFailMissingRequiredThing".Translate());
				}
				yield return command_Action2;
			}
			yield break;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0014E694 File Offset: 0x0014C894
		private Command_Action LaunchAction()
		{
			return new Command_Action
			{
				defaultLabel = "CommandLaunchGroup".Translate(),
				defaultDesc = "CommandLaunchGroupDesc".Translate(),
				icon = CompLaunchable.LaunchCommandTex,
				alsoClickIfOtherInGroupClicked = false,
				action = delegate()
				{
					CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.transportShip.shipThing));
					Find.WorldSelector.ClearSelection();
					int tile = this.transportShip.shipThing.Map.Tile;
					Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
					{
						GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance);
					}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistance, Gen.YieldSingle<IThingHolder>(this.transportShip.TransporterComp), new Action<int, TransportPodsArrivalAction>(this.Launch), null), null);
				}
			};
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x0014E6F4 File Offset: 0x0014C8F4
		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			int tile = this.transportShip.shipThing.Map.Tile;
			return CompLaunchable.ChoseWorldTarget(target, tile, Gen.YieldSingle<IThingHolder>(this.transportShip.TransporterComp), this.MaxLaunchDistance, new Action<int, TransportPodsArrivalAction>(this.Launch), null);
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x0014E744 File Offset: 0x0014C944
		private void Launch(int destinationTile, TransportPodsArrivalAction arrivalAction)
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.destinationTile = destinationTile;
			shipJob_FlyAway.arrivalAction = arrivalAction;
			this.transportShip.SetNextJob(shipJob_FlyAway);
			this.transportShip.TryGetNextJob();
			CameraJumper.TryHideWorld();
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x0014E78C File Offset: 0x0014C98C
		public override void Tick()
		{
			base.Tick();
			if (this.transportShip.ShipExistsAndIsSpawned)
			{
				if (this.leaveImmediatelyWhenSatisfied && this.transportShip.ShuttleComp.AllRequiredThingsLoaded)
				{
					this.SendAway();
					return;
				}
				if (this.transportShip.shipThing.IsHashIntervalTick(60))
				{
					bool flag = false;
					if (!this.sendAwayIfAnyDespawnedDownedOrDead.NullOrEmpty<Thing>())
					{
						foreach (Thing thing in this.sendAwayIfAnyDespawnedDownedOrDead)
						{
							Pawn pawn = thing as Pawn;
							if ((!thing.Spawned || (pawn != null && (pawn.Dead || pawn.Downed))) && !this.transportShip.TransporterComp.innerContainer.Contains(thing))
							{
								flag = true;
								this.SendAway();
								break;
							}
						}
					}
					if (!flag && !this.sendAwayIfAllDespawned.NullOrEmpty<Thing>())
					{
						bool flag2 = false;
						foreach (Thing thing2 in this.sendAwayIfAllDespawned)
						{
							if (thing2.Spawned || this.transportShip.TransporterComp.innerContainer.Contains(thing2))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.SendAway();
						}
					}
				}
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0014E8FC File Offset: 0x0014CAFC
		protected virtual void SendAway()
		{
			this.transportShip.SetNextJob(ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway));
			this.End();
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x0014E91C File Offset: 0x0014CB1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.showGizmos, "showGizmos", true, false);
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawned", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.sendAwayIfAllDespawned;
				bool flag;
				if (list == null)
				{
					flag = false;
				}
				else
				{
					flag = (list.RemoveAll((Thing x) => x == null) > 0);
				}
				if (flag)
				{
					Log.Error("Removed null pawns from sendAwayIfAllDespawned.");
				}
				List<Thing> list2 = this.sendAwayIfAnyDespawnedDownedOrDead;
				bool flag2;
				if (list2 == null)
				{
					flag2 = false;
				}
				else
				{
					flag2 = (list2.RemoveAll((Thing x) => x == null) > 0);
				}
				if (flag2)
				{
					Log.Error("Removed null pawns from sendAwayIfAnyDespawned.");
				}
			}
		}

		// Token: 0x0400209D RID: 8349
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x0400209E RID: 8350
		public bool showGizmos = true;

		// Token: 0x0400209F RID: 8351
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x040020A0 RID: 8352
		public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;

		// Token: 0x040020A1 RID: 8353
		private static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Commands/DismissShuttle", true);

		// Token: 0x040020A2 RID: 8354
		protected static readonly Texture2D SendCommandTex = CompLaunchable.LaunchCommandTex;

		// Token: 0x040020A3 RID: 8355
		private const int CheckAllDespawnedInterval = 60;
	}
}
