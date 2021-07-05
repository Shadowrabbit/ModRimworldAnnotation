using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EE RID: 2286
	public class ShipJob_FlyAway : ShipJob
	{
		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x0014E076 File Offset: 0x0014C276
		protected override bool ShouldEnd
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003BEC RID: 15340 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Interruptible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x0014E07E File Offset: 0x0014C27E
		public override bool HasDestination
		{
			get
			{
				return this.destinationTile >= 0;
			}
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x0014E08C File Offset: 0x0014C28C
		public override bool TryStart()
		{
			if (!base.TryStart())
			{
				return false;
			}
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return false;
			}
			if (!this.transportShip.ShuttleComp.AllRequiredThingsLoaded && this.dropMode != TransportShipDropMode.None && this.transportShip.TransporterComp.innerContainer.Any)
			{
				ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
				shipJob_Unload.dropMode = this.dropMode;
				this.transportShip.SetNextJob(shipJob_Unload);
				return false;
			}
			IntVec3 position = this.transportShip.shipThing.Position;
			Map map = this.transportShip.shipThing.Map;
			if (!this.transportShip.TransporterComp.LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.transportShip.TransporterComp));
			}
			this.transportShip.TransporterComp.TryRemoveLord(map);
			this.transportShip.ShuttleComp.SendLaunchedSignals();
			QuestUtility.SendQuestTargetSignals(this.transportShip.questTags, "FlewAway", this.transportShip.Named("SUBJECT"));
			ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeDropPod.Contents = new ActiveDropPodInfo();
			activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(this.transportShip.TransporterComp.GetDirectlyHeldThings(), true, true);
			int groupID = this.transportShip.TransporterComp.groupID;
			if (!this.transportShip.shipThing.Destroyed)
			{
				this.transportShip.shipThing.DeSpawn(DestroyMode.Vanish);
			}
			FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(this.transportShip.def.leavingSkyfaller, activeDropPod);
			flyShipLeaving.groupID = groupID;
			using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)activeDropPod.Contents.innerContainer).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null)
					{
						pawn.ExitMap(false, Rot4.Invalid);
					}
				}
			}
			if (this.destinationTile < 0)
			{
				flyShipLeaving.createWorldObject = false;
				activeDropPod.Contents.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.QuestLogic);
				if (!this.transportShip.shipThing.Destroyed)
				{
					this.transportShip.shipThing.Destroy(DestroyMode.QuestLogic);
				}
			}
			else
			{
				flyShipLeaving.createWorldObject = true;
				flyShipLeaving.worldObjectDef = this.transportShip.def.worldObject;
				flyShipLeaving.destinationTile = this.destinationTile;
				flyShipLeaving.arrivalAction = this.arrivalAction;
			}
			GenSpawn.Spawn(flyShipLeaving, position, map, WipeMode.Vanish);
			this.initialized = true;
			return true;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0014E324 File Offset: 0x0014C524
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x04002091 RID: 8337
		public int destinationTile = -1;

		// Token: 0x04002092 RID: 8338
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x04002093 RID: 8339
		public TransportShipDropMode dropMode = TransportShipDropMode.All;

		// Token: 0x04002094 RID: 8340
		private bool initialized;
	}
}
