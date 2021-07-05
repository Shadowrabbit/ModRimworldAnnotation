using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010D0 RID: 4304
	public class FlyShipLeaving : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x060066F5 RID: 26357 RVA: 0x0022C0AD File Offset: 0x0022A2AD
		// (set) Token: 0x060066F6 RID: 26358 RVA: 0x0022C0C5 File Offset: 0x0022A2C5
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x0022C7BC File Offset: 0x0022A9BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alreadyLeft, "alreadyLeft", false, false);
			Scribe_Values.Look<bool>(ref this.createWorldObject, "createWorldObject", true, false);
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjectDef");
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x0022C83C File Offset: 0x0022AA3C
		protected override void LeaveMap()
		{
			if (this.alreadyLeft || !this.createWorldObject)
			{
				base.LeaveMap();
				return;
			}
			if (this.groupID < 0)
			{
				Log.Error("Drop pod left the map, but its group ID is " + this.groupID);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.destinationTile < 0)
			{
				Log.Error("Drop pod left the map, but its destination tile is " + this.destinationTile);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, base.Map);
			if (lord != null)
			{
				base.Map.lordManager.RemoveLord(lord);
			}
			TravelingTransportPods travelingTransportPods = (TravelingTransportPods)WorldObjectMaker.MakeWorldObject(this.worldObjectDef ?? WorldObjectDefOf.TravelingTransportPods);
			travelingTransportPods.Tile = base.Map.Tile;
			travelingTransportPods.SetFaction(Faction.OfPlayer);
			travelingTransportPods.destinationTile = this.destinationTile;
			travelingTransportPods.arrivalAction = this.arrivalAction;
			Find.WorldObjects.Add(travelingTransportPods);
			FlyShipLeaving.tmpActiveDropPods.Clear();
			FlyShipLeaving.tmpActiveDropPods.AddRange(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveDropPod));
			for (int i = 0; i < FlyShipLeaving.tmpActiveDropPods.Count; i++)
			{
				FlyShipLeaving flyShipLeaving = FlyShipLeaving.tmpActiveDropPods[i] as FlyShipLeaving;
				if (flyShipLeaving != null && flyShipLeaving.groupID == this.groupID)
				{
					flyShipLeaving.alreadyLeft = true;
					travelingTransportPods.AddPod(flyShipLeaving.Contents, true);
					flyShipLeaving.Contents = null;
					flyShipLeaving.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x04003A23 RID: 14883
		public int groupID = -1;

		// Token: 0x04003A24 RID: 14884
		public int destinationTile = -1;

		// Token: 0x04003A25 RID: 14885
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x04003A26 RID: 14886
		public bool createWorldObject = true;

		// Token: 0x04003A27 RID: 14887
		public WorldObjectDef worldObjectDef;

		// Token: 0x04003A28 RID: 14888
		private bool alreadyLeft;

		// Token: 0x04003A29 RID: 14889
		private static List<Thing> tmpActiveDropPods = new List<Thing>();
	}
}
