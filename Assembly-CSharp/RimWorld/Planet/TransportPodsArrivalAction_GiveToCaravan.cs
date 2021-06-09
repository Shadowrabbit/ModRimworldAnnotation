using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002176 RID: 8566
	public class TransportPodsArrivalAction_GiveToCaravan : TransportPodsArrivalAction
	{
		// Token: 0x0600B681 RID: 46721 RVA: 0x0004B7F4 File Offset: 0x000499F4
		public TransportPodsArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x0600B682 RID: 46722 RVA: 0x00076649 File Offset: 0x00074849
		public TransportPodsArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B683 RID: 46723 RVA: 0x00076658 File Offset: 0x00074858
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x0600B684 RID: 46724 RVA: 0x0034C074 File Offset: 0x0034A274
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

		// Token: 0x0600B685 RID: 46725 RVA: 0x0034C0C8 File Offset: 0x0034A2C8
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

		// Token: 0x0600B686 RID: 46726 RVA: 0x00076671 File Offset: 0x00074871
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x0600B687 RID: 46727 RVA: 0x0034C190 File Offset: 0x0034A390
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveToCaravan>(() => TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportPodsArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(caravan.Label), representative, caravan.Tile, null);
		}

		// Token: 0x04007CF7 RID: 31991
		private Caravan caravan;

		// Token: 0x04007CF8 RID: 31992
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
