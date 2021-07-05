using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B3 RID: 2227
	public class LordToil_ExitMapAndEscortCarriers : LordToil
	{
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003AD3 RID: 15059 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x00148D4C File Offset: 0x00146F4C
		public override void UpdateAllDuties()
		{
			Pawn trader;
			this.UpdateTraderDuty(out trader);
			this.UpdateCarriersDuties(trader);
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn p = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = p.GetTraderCaravanRole();
				if (traderCaravanRole != TraderCaravanRole.Carrier && traderCaravanRole != TraderCaravanRole.Trader)
				{
					this.UpdateDutyForChattelOrGuard(p, trader);
				}
			}
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x00148DAC File Offset: 0x00146FAC
		private void UpdateTraderDuty(out Pawn trader)
		{
			trader = TraderCaravanUtility.FindTrader(this.lord);
			if (trader != null)
			{
				trader.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				trader.mindState.duty.radius = 18f;
				trader.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x00148E08 File Offset: 0x00147008
		private void UpdateCarriersDuties(Pawn trader)
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier)
				{
					if (trader != null)
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.Follow, trader, 5f);
					}
					else
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
						pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
					}
				}
			}
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x00148E98 File Offset: 0x00147098
		private void UpdateDutyForChattelOrGuard(Pawn p, Pawn trader)
		{
			if (p.GetTraderCaravanRole() == TraderCaravanRole.Chattel)
			{
				if (trader != null)
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.Escort, trader, 14f);
					return;
				}
				if (!this.TryToDefendClosestCarrier(p, 14f))
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
					p.mindState.duty.radius = 10f;
					p.mindState.duty.locomotion = LocomotionUrgency.Jog;
					return;
				}
			}
			else if (!this.TryToDefendClosestCarrier(p, 26f))
			{
				if (trader != null)
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.Escort, trader, 26f);
					return;
				}
				p.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				p.mindState.duty.radius = 18f;
				p.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x00148F8C File Offset: 0x0014718C
		private bool TryToDefendClosestCarrier(Pawn p, float escortRadius)
		{
			Pawn closestCarrier = this.GetClosestCarrier(p);
			Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.Corpse), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 20f, delegate(Thing x)
			{
				Pawn innerPawn = ((Corpse)x).InnerPawn;
				return innerPawn.Faction == p.Faction && innerPawn.RaceProps.packAnimal;
			}, null, 0, 15, false, RegionType.Set_Passable, false);
			Thing thing2 = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 20f, delegate(Thing x)
			{
				Pawn pawn = (Pawn)x;
				return pawn.Downed && pawn.Faction == p.Faction && pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier;
			}, null, 0, 15, false, RegionType.Set_Passable, false);
			Thing thing3 = null;
			if (closestCarrier != null)
			{
				thing3 = closestCarrier;
			}
			if (thing != null && (thing3 == null || thing.Position.DistanceToSquared(p.Position) < thing3.Position.DistanceToSquared(p.Position)))
			{
				thing3 = thing;
			}
			if (thing2 != null && (thing3 == null || thing2.Position.DistanceToSquared(p.Position) < thing3.Position.DistanceToSquared(p.Position)))
			{
				thing3 = thing2;
			}
			if (thing3 == null)
			{
				return false;
			}
			if (thing3 is Pawn && !((Pawn)thing3).Downed)
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.Escort, thing3, escortRadius);
				return true;
			}
			if (!GenHostility.AnyHostileActiveThreatTo(base.Map, this.lord.faction, true, true))
			{
				return false;
			}
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, thing3.Position, 16f);
			return true;
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x00149150 File Offset: 0x00147350
		public static bool IsDefendingPosition(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Defend;
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x00149178 File Offset: 0x00147378
		public static bool IsAnyDefendingPosition(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (LordToil_ExitMapAndEscortCarriers.IsDefendingPosition(pawns[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x001491A8 File Offset: 0x001473A8
		private Pawn GetClosestCarrier(Pawn closestTo)
		{
			Pawn pawn = null;
			float num = 0f;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = this.lord.ownedPawns[i];
				if (pawn2.GetTraderCaravanRole() == TraderCaravanRole.Carrier)
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(closestTo.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}
	}
}
