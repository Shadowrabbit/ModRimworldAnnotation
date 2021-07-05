using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E9 RID: 6121
	public class TransportPodsArrivalAction_Trade : TransportPodsArrivalAction_VisitSettlement
	{
		// Token: 0x06008E81 RID: 36481 RVA: 0x00332A86 File Offset: 0x00330C86
		public TransportPodsArrivalAction_Trade()
		{
		}

		// Token: 0x06008E82 RID: 36482 RVA: 0x00332A8E File Offset: 0x00330C8E
		public TransportPodsArrivalAction_Trade(Settlement settlement) : base(settlement)
		{
		}

		// Token: 0x06008E83 RID: 36483 RVA: 0x00332A98 File Offset: 0x00330C98
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportPodsArrivalAction_Trade.CanTradeWith(pods, this.settlement);
		}

		// Token: 0x06008E84 RID: 36484 RVA: 0x00332AC4 File Offset: 0x00330CC4
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Pawn pawn = null;
			int num = 0;
			while (num < pods.Count && pawn == null)
			{
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)pods[num].GetDirectlyHeldThings()).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn pawn2;
						if ((pawn2 = (enumerator.Current as Pawn)) != null)
						{
							pawn = pawn2;
							break;
						}
					}
				}
				num++;
			}
			base.Arrived(pods, tile);
			if (pawn != null)
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null && CaravanArrivalAction_Trade.HasNegotiator(caravan, this.settlement))
				{
					CameraJumper.TryJumpAndSelect(caravan);
					Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
					Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
				}
			}
		}

		// Token: 0x06008E85 RID: 36485 RVA: 0x00332B9C File Offset: 0x00330D9C
		public static FloatMenuAcceptanceReport CanTradeWith(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (!TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement))
			{
				return false;
			}
			if (settlement.Faction == null || settlement.Faction == Faction.OfPlayer)
			{
				return false;
			}
			bool flag = false;
			foreach (IThingHolder thingHolder in pods)
			{
				using (IEnumerator<Thing> enumerator2 = ((IEnumerable<Thing>)thingHolder.GetDirectlyHeldThings()).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn p;
						if ((p = (enumerator2.Current as Pawn)) != null && p.CanTradeWith(settlement.Faction, settlement.TraderKind).Accepted)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			return flag && !settlement.HasMap && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow;
		}
	}
}
