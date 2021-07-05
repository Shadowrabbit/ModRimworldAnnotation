using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E7 RID: 6119
	public class TransportPodsArrivalAction_LandInSpecificCell : TransportPodsArrivalAction
	{
		// Token: 0x06008E74 RID: 36468 RVA: 0x00331E93 File Offset: 0x00330093
		public TransportPodsArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x06008E75 RID: 36469 RVA: 0x003327AA File Offset: 0x003309AA
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x06008E76 RID: 36470 RVA: 0x003327C0 File Offset: 0x003309C0
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell, bool landInShuttle)
		{
			this.mapParent = mapParent;
			this.cell = cell;
			this.landInShuttle = landInShuttle;
		}

		// Token: 0x06008E77 RID: 36471 RVA: 0x003327E0 File Offset: 0x003309E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.landInShuttle, "landInShuttle", false, false);
		}

		// Token: 0x06008E78 RID: 36472 RVA: 0x00332830 File Offset: 0x00330A30
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this.mapParent);
		}

		// Token: 0x06008E79 RID: 36473 RVA: 0x00332880 File Offset: 0x00330A80
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			if (this.landInShuttle)
			{
				TransportPodsArrivalActionUtility.DropShuttle(pods, this.mapParent.Map, this.cell, null);
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(pods, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06008E7A RID: 36474 RVA: 0x00332910 File Offset: 0x00330B10
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true))));
		}

		// Token: 0x040059E4 RID: 23012
		private MapParent mapParent;

		// Token: 0x040059E5 RID: 23013
		private IntVec3 cell;

		// Token: 0x040059E6 RID: 23014
		private bool landInShuttle;
	}
}
