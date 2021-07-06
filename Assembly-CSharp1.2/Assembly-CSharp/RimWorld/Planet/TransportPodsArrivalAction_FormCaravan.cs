using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002172 RID: 8562
	public class TransportPodsArrivalAction_FormCaravan : TransportPodsArrivalAction
	{
		// Token: 0x0600B66C RID: 46700 RVA: 0x0007655C File Offset: 0x0007475C
		public TransportPodsArrivalAction_FormCaravan()
		{
		}

		// Token: 0x0600B66D RID: 46701 RVA: 0x0007656F File Offset: 0x0007476F
		public TransportPodsArrivalAction_FormCaravan(string arrivalMessageKey)
		{
			this.arrivalMessageKey = arrivalMessageKey;
		}

		// Token: 0x0600B66E RID: 46702 RVA: 0x0034BB94 File Offset: 0x00349D94
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(pods, destinationTile);
		}

		// Token: 0x0600B66F RID: 46703 RVA: 0x0034BBC0 File Offset: 0x00349DC0
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			TransportPodsArrivalAction_FormCaravan.tmpPawns.Clear();
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null)
					{
						TransportPodsArrivalAction_FormCaravan.tmpPawns.Add(pawn);
						innerContainer.Remove(pawn);
					}
				}
			}
			int startingTile;
			if (!GenWorldClosest.TryFindClosestPassableTile(tile, out startingTile))
			{
				startingTile = tile;
			}
			Caravan caravan = CaravanMaker.MakeCaravan(TransportPodsArrivalAction_FormCaravan.tmpPawns, Faction.OfPlayer, startingTile, true);
			for (int k = 0; k < pods.Count; k++)
			{
				TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Clear();
				TransportPodsArrivalAction_FormCaravan.tmpContainedThings.AddRange(pods[k].innerContainer);
				for (int l = 0; l < TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Count; l++)
				{
					pods[k].innerContainer.Remove(TransportPodsArrivalAction_FormCaravan.tmpContainedThings[l]);
					CaravanInventoryUtility.GiveThing(caravan, TransportPodsArrivalAction_FormCaravan.tmpContainedThings[l]);
				}
			}
			TransportPodsArrivalAction_FormCaravan.tmpPawns.Clear();
			TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Clear();
			Messages.Message(this.arrivalMessageKey.Translate(), caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600B670 RID: 46704 RVA: 0x00076589 File Offset: 0x00074789
		public static bool CanFormCaravanAt(IEnumerable<IThingHolder> pods, int tile)
		{
			return TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer) && !Find.World.Impassable(tile);
		}

		// Token: 0x0600B671 RID: 46705 RVA: 0x000765A8 File Offset: 0x000747A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.arrivalMessageKey, "arrivalMessageKey", "MessageTransportPodsArrived", false);
		}

		// Token: 0x04007CEF RID: 31983
		private string arrivalMessageKey = "MessageTransportPodsArrived";

		// Token: 0x04007CF0 RID: 31984
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04007CF1 RID: 31985
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
