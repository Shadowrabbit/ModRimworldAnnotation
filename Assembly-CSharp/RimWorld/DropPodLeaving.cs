using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001732 RID: 5938
	public class DropPodLeaving : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x060082F4 RID: 33524 RVA: 0x00057E59 File Offset: 0x00056059
		// (set) Token: 0x060082F5 RID: 33525 RVA: 0x00057E71 File Offset: 0x00056071
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

		// Token: 0x060082F6 RID: 33526 RVA: 0x0026D168 File Offset: 0x0026B368
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

		// Token: 0x060082F7 RID: 33527 RVA: 0x0026D1E8 File Offset: 0x0026B3E8
		protected override void LeaveMap()
		{
			if (this.alreadyLeft || !this.createWorldObject)
			{
				base.LeaveMap();
				return;
			}
			if (this.groupID < 0)
			{
				Log.Error("Drop pod left the map, but its group ID is " + this.groupID, false);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.destinationTile < 0)
			{
				Log.Error("Drop pod left the map, but its destination tile is " + this.destinationTile, false);
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
			DropPodLeaving.tmpActiveDropPods.Clear();
			DropPodLeaving.tmpActiveDropPods.AddRange(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveDropPod));
			for (int i = 0; i < DropPodLeaving.tmpActiveDropPods.Count; i++)
			{
				DropPodLeaving dropPodLeaving = DropPodLeaving.tmpActiveDropPods[i] as DropPodLeaving;
				if (dropPodLeaving != null && dropPodLeaving.groupID == this.groupID)
				{
					dropPodLeaving.alreadyLeft = true;
					travelingTransportPods.AddPod(dropPodLeaving.Contents, true);
					dropPodLeaving.Contents = null;
					dropPodLeaving.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x040054DD RID: 21725
		public int groupID = -1;

		// Token: 0x040054DE RID: 21726
		public int destinationTile = -1;

		// Token: 0x040054DF RID: 21727
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x040054E0 RID: 21728
		public bool createWorldObject = true;

		// Token: 0x040054E1 RID: 21729
		public WorldObjectDef worldObjectDef;

		// Token: 0x040054E2 RID: 21730
		private bool alreadyLeft;

		// Token: 0x040054E3 RID: 21731
		private static List<Thing> tmpActiveDropPods = new List<Thing>();
	}
}
