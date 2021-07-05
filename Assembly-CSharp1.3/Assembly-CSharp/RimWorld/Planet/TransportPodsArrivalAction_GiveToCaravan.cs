using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E6 RID: 6118
	public class TransportPodsArrivalAction_GiveToCaravan : TransportPodsArrivalAction
	{
		// Token: 0x06008E6C RID: 36460 RVA: 0x00331E93 File Offset: 0x00330093
		public TransportPodsArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x06008E6D RID: 36461 RVA: 0x003325D1 File Offset: 0x003307D1
		public TransportPodsArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008E6E RID: 36462 RVA: 0x003325E0 File Offset: 0x003307E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x06008E6F RID: 36463 RVA: 0x003325FC File Offset: 0x003307FC
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.caravan != null && !Find.WorldGrid.IsNeighborOrSame(this.caravan.Tile, destinationTile))
			{
				return false;
			}
			return TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, this.caravan);
		}

		// Token: 0x06008E70 RID: 36464 RVA: 0x00332650 File Offset: 0x00330850
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
				TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.AddRange(pods[i].innerContainer);
				for (int j = 0; j < TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Count; j++)
				{
					pods[i].innerContainer.Remove(TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings[j]);
					this.caravan.AddPawnOrItem(TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings[j], true);
				}
			}
			TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrivedAndAddedToCaravan".Translate(this.caravan.Name), this.caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06008E71 RID: 36465 RVA: 0x00332716 File Offset: 0x00330916
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x06008E72 RID: 36466 RVA: 0x00332734 File Offset: 0x00330934
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveToCaravan>(() => TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportPodsArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(caravan.Label), representative, caravan.Tile, null);
		}

		// Token: 0x040059E2 RID: 23010
		private Caravan caravan;

		// Token: 0x040059E3 RID: 23011
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
