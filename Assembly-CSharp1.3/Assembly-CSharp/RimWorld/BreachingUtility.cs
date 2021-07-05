using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000774 RID: 1908
	public static class BreachingUtility
	{
		// Token: 0x0600348E RID: 13454 RVA: 0x00129E7C File Offset: 0x0012807C
		public static bool ShouldBreachBuilding(Thing thing)
		{
			Building building = thing as Building;
			return building != null && TrashUtility.ShouldTrashBuilding(building) && PathFinder.IsDestroyable(building) && building.def.IsEdifice() && !building.def.IsFrame && (building.def.mineable || building.Faction == Faction.OfPlayer || (building.Faction == null && BreachingUtility.BlocksBreaching(building.Map, building.Position)));
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x00129F00 File Offset: 0x00128100
		public static bool IsWorthBreachingBuilding(BreachingGrid grid, Building b)
		{
			if (b.def.passability != Traversability.Impassable || b.def.IsDoor)
			{
				return true;
			}
			if (BreachingUtility.OnEdgeOfPath(b.Map, grid.BreachGrid, b.Position))
			{
				if (BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.North) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.East) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.South) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.West))
				{
					return false;
				}
			}
			else if (!BreachingUtility.AnyAdjacentImpassibleOnBreachPath(b.Map, grid.BreachGrid, b.Position))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00129FB0 File Offset: 0x001281B0
		public static int CountReachableAdjacentCells(BreachingGrid grid, Building b)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = b.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(grid.Map) && grid.ReachableGrid[c])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x0012A004 File Offset: 0x00128204
		private static bool OnEdgeOfPath(Map map, BoolGrid grid, IntVec3 p)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = p + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && !grid[c])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x0012A044 File Offset: 0x00128244
		private static bool OnEdgeOfPathAndSufficientlyClear(BreachingGrid breachingGrid, IntVec3 p, IntVec3 right)
		{
			Map map = breachingGrid.Map;
			BoolGrid breachGrid = breachingGrid.BreachGrid;
			int num = breachingGrid.BreachRadius * 2 + 1;
			IntVec3 c = p - right;
			if (!c.InBounds(map) || breachGrid[c])
			{
				return false;
			}
			IntVec3 intVec = p;
			int num2 = 0;
			while (intVec.InBounds(map) && breachGrid[intVec] && BreachingUtility.BlocksBreaching(map, intVec) && num2 <= num)
			{
				intVec += right;
				num2++;
			}
			int num3 = 0;
			while (intVec.InBounds(map) && breachGrid[intVec] && !BreachingUtility.BlocksBreaching(map, intVec) && num2 <= num)
			{
				intVec += right;
				num2++;
				num3++;
			}
			int num4 = Math.Max(2, num - 2);
			return num3 >= num4;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x0012A114 File Offset: 0x00128314
		public static bool BlocksBreaching(Map map, IntVec3 c)
		{
			Building edifice = c.GetEdifice(map);
			return (edifice == null || !edifice.def.building.ai_neverTrashThis) && (c.Impassable(map) || c.GetDoor(map) != null);
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x0012A158 File Offset: 0x00128358
		private static bool AnyAdjacentImpassibleOnBreachPath(Map map, BoolGrid grid, IntVec3 position)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = position + GenAdj.AdjacentCellsAround[i];
				if (c.InBounds(map) && grid[c] && BreachingUtility.BlocksBreaching(map, c))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x0012A1A1 File Offset: 0x001283A1
		public static bool CanSoloAttackTargetBuilding(Thing thing)
		{
			return thing != null && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x0012A1B8 File Offset: 0x001283B8
		public static bool IsSoloAttackVerb(Verb verb)
		{
			return verb.verbProps.ai_AvoidFriendlyFireRadius > 0f;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x0012A1CC File Offset: 0x001283CC
		public static float EscortRadius(Pawn pawn)
		{
			float randomInRange;
			if (pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
			{
				randomInRange = BreachingUtility.EscortRadiusRanged.RandomInRange;
			}
			else
			{
				randomInRange = BreachingUtility.EscortRadiusMelee.RandomInRange;
			}
			return randomInRange;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x0012A224 File Offset: 0x00128424
		private static bool UsableVerb(Verb verb)
		{
			return verb != null && verb.Available() && verb.HarmsHealth();
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x0012A240 File Offset: 0x00128440
		public static Verb FindVerbToUseForBreaching(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			CompEquippable compEquippable = (equipment != null) ? equipment.PrimaryEq : null;
			if (compEquippable == null)
			{
				return null;
			}
			Verb primaryVerb = compEquippable.PrimaryVerb;
			if (BreachingUtility.UsableVerb(primaryVerb) && primaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return primaryVerb;
			}
			List<Verb> allVerbs = compEquippable.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				if (BreachingUtility.UsableVerb(verb) && verb.verbProps.ai_IsBuildingDestroyer)
				{
					return verb;
				}
			}
			if (BreachingUtility.UsableVerb(primaryVerb))
			{
				return primaryVerb;
			}
			return null;
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x0012A2CC File Offset: 0x001284CC
		public static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = BreachingUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x0012A2FA File Offset: 0x001284FA
		public static BreachingGrid BreachingGridFor(Pawn pawn)
		{
			return BreachingUtility.BreachingGridFor((pawn != null) ? pawn.GetLord() : null);
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x0012A30D File Offset: 0x0012850D
		public static BreachingGrid BreachingGridFor(Lord lord)
		{
			LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(lord);
			if (lordToilData_AssaultColonyBreaching == null)
			{
				return null;
			}
			return lordToilData_AssaultColonyBreaching.breachingGrid;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x0012A320 File Offset: 0x00128520
		public static LordToilData_AssaultColonyBreaching LordDataFor(Lord lord)
		{
			LordToil_AssaultColonyBreaching lordToil_AssaultColonyBreaching = ((lord != null) ? lord.CurLordToil : null) as LordToil_AssaultColonyBreaching;
			if (lordToil_AssaultColonyBreaching == null)
			{
				return null;
			}
			return lordToil_AssaultColonyBreaching.Data;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x0012A33E File Offset: 0x0012853E
		public static LordToil_AssaultColonyBreaching LordToilOf(Pawn pawn)
		{
			return ((pawn != null) ? pawn.GetLord().CurLordToil : null) as LordToil_AssaultColonyBreaching;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x0012A356 File Offset: 0x00128556
		public static bool TryFindCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
		{
			if (verb.IsMeleeAttack)
			{
				return BreachingUtility.TryFindMeleeCastPosition(pawn, verb, target, out result);
			}
			return BreachingUtility.cachedRangedCastPositionFinder.TryFindRangedCastPosition(pawn, verb, target, out result);
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x0012A378 File Offset: 0x00128578
		private static bool TryFindMeleeCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
		{
			result = IntVec3.Invalid;
			BreachingGrid breachingGrid = BreachingUtility.BreachingGridFor(pawn);
			if (breachingGrid == null)
			{
				return false;
			}
			if (BreachingUtility.SafeUseableFiringPosition(breachingGrid, pawn.Position) && pawn.CanReachImmediate(target, PathEndMode.Touch))
			{
				result = pawn.Position;
				return true;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = GenAdj.AdjacentCells[i] + target.Position;
				if (intVec.InBounds(target.Map) && BreachingUtility.SafeUseableFiringPosition(breachingGrid, intVec) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					result = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x0012A41E File Offset: 0x0012861E
		private static bool SafeUseableFiringPosition(BreachingGrid grid, IntVec3 c)
		{
			return grid.ReachableGrid[c] && grid.MarkerGrid[c] != 180 && !c.ContainsStaticFire(grid.Map);
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x0012A458 File Offset: 0x00128658
		public static Pawn FindPawnToEscort(Pawn follower)
		{
			Lord lord = follower.GetLord();
			if (lord == null)
			{
				return null;
			}
			Pawn result = null;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn != follower)
				{
					PawnDuty duty = pawn.mindState.duty;
					if (((duty != null) ? duty.def : null) == DutyDefOf.Breaching)
					{
						BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
						if (breachingTarget != null && breachingTarget.firingPosition.IsValid)
						{
							return pawn;
						}
						result = pawn;
					}
				}
			}
			return result;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x0012A4DC File Offset: 0x001286DC
		public static bool CanDamageTarget(Verb verb, Thing target)
		{
			return verb.GetDamageDef() != DamageDefOf.Flame || target.FlammableNow;
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x0012A4F8 File Offset: 0x001286F8
		public static bool IsGoodBreacher(Pawn pawn)
		{
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			return verb != null && verb.verbProps.ai_IsBuildingDestroyer;
		}

		// Token: 0x04001E57 RID: 7767
		public static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(120, 300);

		// Token: 0x04001E58 RID: 7768
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(15f, 19f);

		// Token: 0x04001E59 RID: 7769
		private static readonly FloatRange EscortRadiusMelee = new FloatRange(23f, 26f);

		// Token: 0x04001E5A RID: 7770
		private static BreachingUtility.BreachRangedCastPositionFinder cachedRangedCastPositionFinder = new BreachingUtility.BreachRangedCastPositionFinder();

		// Token: 0x02001F00 RID: 7936
		private class BreachRangedCastPositionFinder
		{
			// Token: 0x0600B24D RID: 45645 RVA: 0x003A9990 File Offset: 0x003A7B90
			public BreachRangedCastPositionFinder()
			{
				this.visitDangerousCellFunc = new Action<IntVec3>(this.VisitDangerousCell);
				this.safeForRangedCastFunc = new Func<IntVec3, bool>(this.SafeForRangedCast);
			}

			// Token: 0x0600B24E RID: 45646 RVA: 0x003A99BC File Offset: 0x003A7BBC
			private void VisitDangerousCell(IntVec3 cell)
			{
				if (this.breachingGrid.MarkerGrid[cell] == 180)
				{
					this.wouldPutSomeoneElseInDanger = true;
				}
			}

			// Token: 0x0600B24F RID: 45647 RVA: 0x003A99E0 File Offset: 0x003A7BE0
			private bool SafeForRangedCast(IntVec3 c)
			{
				if (!BreachingUtility.SafeUseableFiringPosition(this.breachingGrid, c))
				{
					return false;
				}
				this.wouldPutSomeoneElseInDanger = false;
				this.breachingGrid.VisitDangerousCellsOfAttack(c, this.target.Position, this.verb, this.visitDangerousCellFunc);
				return !this.wouldPutSomeoneElseInDanger;
			}

			// Token: 0x0600B250 RID: 45648 RVA: 0x003A9A30 File Offset: 0x003A7C30
			public bool TryFindRangedCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
			{
				bool result2;
				try
				{
					result = IntVec3.Invalid;
					LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(pawn.GetLord());
					if (lordToilData_AssaultColonyBreaching == null)
					{
						result2 = false;
					}
					else
					{
						this.breachingGrid = lordToilData_AssaultColonyBreaching.breachingGrid;
						this.verb = verb;
						this.target = target;
						CastPositionRequest castPositionRequest = default(CastPositionRequest);
						castPositionRequest.caster = pawn;
						castPositionRequest.target = target;
						castPositionRequest.verb = verb;
						castPositionRequest.maxRangeFromTarget = verb.verbProps.range;
						if (lordToilData_AssaultColonyBreaching.soloAttacker == null)
						{
							castPositionRequest.maxRangeFromTarget = Mathf.Min(lordToilData_AssaultColonyBreaching.maxRange, castPositionRequest.maxRangeFromTarget);
						}
						castPositionRequest.validator = this.safeForRangedCastFunc;
						IntVec3 intVec;
						if (CastPositionFinder.TryFindCastPosition(castPositionRequest, out intVec))
						{
							result = intVec;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
					}
				}
				finally
				{
				}
				return result2;
			}

			// Token: 0x040076B9 RID: 30393
			private BreachingGrid breachingGrid;

			// Token: 0x040076BA RID: 30394
			private Verb verb;

			// Token: 0x040076BB RID: 30395
			private Thing target;

			// Token: 0x040076BC RID: 30396
			private bool wouldPutSomeoneElseInDanger;

			// Token: 0x040076BD RID: 30397
			private Action<IntVec3> visitDangerousCellFunc;

			// Token: 0x040076BE RID: 30398
			private Func<IntVec3, bool> safeForRangedCastFunc;
		}
	}
}
