using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002173 RID: 8563
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		// Token: 0x0600B673 RID: 46707 RVA: 0x0004B7F4 File Offset: 0x000499F4
		public TransportPodsArrivalAction_GiveGift()
		{
		}

		// Token: 0x0600B674 RID: 46708 RVA: 0x000765DC File Offset: 0x000747DC
		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600B675 RID: 46709 RVA: 0x000765EB File Offset: 0x000747EB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600B676 RID: 46710 RVA: 0x0034BD08 File Offset: 0x00349F08
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

		// Token: 0x0600B677 RID: 46711 RVA: 0x0034BD54 File Offset: 0x00349F54
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
							if (pawn.FactionOrExtraMiniOrHomeFaction == this.settlement.Faction)
							{
								GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
							}
							else
							{
								GenGuest.AddPrisonerSoldThoughts(pawn);
							}
						}
						else if (pawn.RaceProps.Animal && pawn.relations != null)
						{
							Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null);
							if (firstDirectRelationPawn != null && firstDirectRelationPawn.needs.mood != null)
							{
								pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Bond, firstDirectRelationPawn);
								firstDirectRelationPawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoldMyBondedAnimalMood, null);
							}
						}
					}
				}
			}
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		// Token: 0x0600B678 RID: 46712 RVA: 0x0034BE5C File Offset: 0x0034A05C
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

		// Token: 0x0600B679 RID: 46713 RVA: 0x0034BF1C File Offset: 0x0034A11C
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

		// Token: 0x04007CF2 RID: 31986
		private Settlement settlement;
	}
}
