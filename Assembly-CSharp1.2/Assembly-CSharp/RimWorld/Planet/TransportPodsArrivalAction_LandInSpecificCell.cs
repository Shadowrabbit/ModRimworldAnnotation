using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002178 RID: 8568
	public class TransportPodsArrivalAction_LandInSpecificCell : TransportPodsArrivalAction
	{
		// Token: 0x0600B68C RID: 46732 RVA: 0x0004B7F4 File Offset: 0x000499F4
		public TransportPodsArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x0600B68D RID: 46733 RVA: 0x000766B8 File Offset: 0x000748B8
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x0600B68E RID: 46734 RVA: 0x000766CE File Offset: 0x000748CE
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell, bool landInShuttle)
		{
			this.mapParent = mapParent;
			this.cell = cell;
			this.landInShuttle = landInShuttle;
		}

		// Token: 0x0600B68F RID: 46735 RVA: 0x0034C1FC File Offset: 0x0034A3FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.landInShuttle, "landInShuttle", false, false);
		}

		// Token: 0x0600B690 RID: 46736 RVA: 0x0034C24C File Offset: 0x0034A44C
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

		// Token: 0x0600B691 RID: 46737 RVA: 0x0034C29C File Offset: 0x0034A49C
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			if (this.landInShuttle)
			{
				TransportPodsArrivalActionUtility.DropShuttle_NewTemp(pods, this.mapParent.Map, this.cell, null);
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(pods, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600B692 RID: 46738 RVA: 0x0034C32C File Offset: 0x0034A52C
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true))));
		}

		// Token: 0x04007CFB RID: 31995
		private MapParent mapParent;

		// Token: 0x04007CFC RID: 31996
		private IntVec3 cell;

		// Token: 0x04007CFD RID: 31997
		private bool landInShuttle;
	}
}
