using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E4 RID: 6116
	public class TransportPodsArrivalAction_FormCaravan : TransportPodsArrivalAction
	{
		// Token: 0x06008E5E RID: 36446 RVA: 0x003320BE File Offset: 0x003302BE
		public TransportPodsArrivalAction_FormCaravan()
		{
		}

		// Token: 0x06008E5F RID: 36447 RVA: 0x003320D1 File Offset: 0x003302D1
		public TransportPodsArrivalAction_FormCaravan(string arrivalMessageKey)
		{
			this.arrivalMessageKey = arrivalMessageKey;
		}

		// Token: 0x06008E60 RID: 36448 RVA: 0x003320EC File Offset: 0x003302EC
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(pods, destinationTile);
		}

		// Token: 0x06008E61 RID: 36449 RVA: 0x00332118 File Offset: 0x00330318
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

		// Token: 0x06008E62 RID: 36450 RVA: 0x0033225F File Offset: 0x0033045F
		public static bool CanFormCaravanAt(IEnumerable<IThingHolder> pods, int tile)
		{
			return TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer) && !Find.World.Impassable(tile);
		}

		// Token: 0x06008E63 RID: 36451 RVA: 0x0033227E File Offset: 0x0033047E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.arrivalMessageKey, "arrivalMessageKey", "MessageTransportPodsArrived", false);
		}

		// Token: 0x040059DE RID: 23006
		private string arrivalMessageKey = "MessageTransportPodsArrived";

		// Token: 0x040059DF RID: 23007
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040059E0 RID: 23008
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
