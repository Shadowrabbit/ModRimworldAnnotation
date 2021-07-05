using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E5 RID: 6117
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		// Token: 0x06008E65 RID: 36453 RVA: 0x00331E93 File Offset: 0x00330093
		public TransportPodsArrivalAction_GiveGift()
		{
		}

		// Token: 0x06008E66 RID: 36454 RVA: 0x003322B2 File Offset: 0x003304B2
		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008E67 RID: 36455 RVA: 0x003322C1 File Offset: 0x003304C1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06008E68 RID: 36456 RVA: 0x003322DC File Offset: 0x003304DC
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, this.settlement);
		}

		// Token: 0x06008E69 RID: 36457 RVA: 0x00332328 File Offset: 0x00330528
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				for (int j = 0; j < pods[i].innerContainer.Count; j++)
				{
					Pawn pawn = pods[i].innerContainer[j] as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.Humanlike)
						{
							Pawn arg;
							if (pawn.HomeFaction == this.settlement.Faction)
							{
								GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
							}
							else if (PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.TryRandomElement(out arg))
							{
								Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SoldSlave, arg.Named(HistoryEventArgsNames.Doer)), true);
							}
						}
						else if (pawn.RaceProps.Animal && pawn.relations != null)
						{
							Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null);
							if (firstDirectRelationPawn != null && firstDirectRelationPawn.needs.mood != null)
							{
								pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Bond, firstDirectRelationPawn);
								firstDirectRelationPawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoldMyBondedAnimalMood, null, null);
							}
						}
					}
				}
			}
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		// Token: 0x06008E6A RID: 36458 RVA: 0x00332464 File Offset: 0x00330664
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn p;
					if ((p = (directlyHeldThings[i] as Pawn)) != null && p.IsQuestLodger())
					{
						return false;
					}
				}
			}
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x06008E6B RID: 36459 RVA: 0x00332524 File Offset: 0x00330724
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement.Faction == Faction.OfPlayer)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>(() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportPodsArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(settlement.Faction.Name, FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()), representative, settlement.Tile, delegate(Action action)
			{
				TradeRequestComp tradeReqComp = settlement.GetComponent<TradeRequestComp>();
				if (tradeReqComp.ActiveRequest && pods.Any((IThingHolder p) => p.GetDirectlyHeldThings().Contains(tradeReqComp.requestThingDef)))
				{
					Find.WindowStack.Add(new Dialog_MessageBox("GiveGiftViaTransportPodsTradeRequestWarning".Translate(), "Yes".Translate(), delegate()
					{
						action();
					}, "No".Translate(), null, null, false, null, null));
					return;
				}
				action();
			});
		}

		// Token: 0x040059E1 RID: 23009
		private Settlement settlement;
	}
}
