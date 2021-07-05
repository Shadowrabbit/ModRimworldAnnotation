using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020015A7 RID: 5543
	public static class GenHostility
	{
		// Token: 0x06007859 RID: 30809 RVA: 0x002499C4 File Offset: 0x00247BC4
		public static bool HostileTo(this Thing a, Thing b)
		{
			if (a.Destroyed || b.Destroyed || a == b)
			{
				return false;
			}
			if ((a.Faction == null && a.TryGetComp<CompCauseGameCondition>() != null) || (b.Faction == null && b.TryGetComp<CompCauseGameCondition>() != null))
			{
				return true;
			}
			Pawn pawn = a as Pawn;
			Pawn pawn2 = b as Pawn;
			return (pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b)) || (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a)) || (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn))) || ((a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2)) || (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn))) || ((a.Faction == null || pawn2 == null || pawn2.HostFaction != a.Faction) && (b.Faction == null || pawn == null || pawn.HostFaction != b.Faction) && (pawn == null || !pawn.IsPrisoner || pawn2 == null || !pawn2.IsPrisoner) && (pawn == null || pawn2 == null || ((!pawn.IsPrisoner || pawn.HostFaction != pawn2.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (!pawn2.IsPrisoner || pawn2.HostFaction != pawn.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (pawn == null || pawn2 == null || ((pawn.HostFaction == null || pawn2.Faction == null || pawn.HostFaction.HostileTo(pawn2.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (pawn2.HostFaction == null || pawn.Faction == null || pawn2.HostFaction.HostileTo(pawn.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (a.Faction == null || !a.Faction.IsPlayer || pawn2 == null || !pawn2.mindState.WillJoinColonyIfRescued) && (b.Faction == null || !b.Faction.IsPlayer || pawn == null || !pawn.mindState.WillJoinColonyIfRescued) && ((pawn != null && pawn.Faction == null && pawn.RaceProps.Humanlike && b.Faction != null && b.Faction.def.hostileToFactionlessHumanlikes) || (pawn2 != null && pawn2.Faction == null && pawn2.RaceProps.Humanlike && a.Faction != null && a.Faction.def.hostileToFactionlessHumanlikes) || (a.Faction != null && b.Faction != null && a.Faction.HostileTo(b.Faction))));
		}

		// Token: 0x0600785A RID: 30810 RVA: 0x00249C84 File Offset: 0x00247E84
		public static bool HostileTo(this Thing t, Faction fac)
		{
			if (t.Destroyed)
			{
				return false;
			}
			if (fac == null)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				MentalState mentalState = pawn.MentalState;
				if (mentalState != null && mentalState.ForceHostileTo(fac))
				{
					return true;
				}
				if (GenHostility.IsPredatorHostileTo(pawn, fac))
				{
					return true;
				}
				if (pawn.HostFaction == fac && PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return true;
				}
				if (pawn.HostFaction == fac)
				{
					return false;
				}
				if (pawn.HostFaction != null && !pawn.HostFaction.HostileTo(fac) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return false;
				}
				if (fac.IsPlayer && pawn.mindState.WillJoinColonyIfRescued)
				{
					return false;
				}
				if (fac.def.hostileToFactionlessHumanlikes && pawn.Faction == null && pawn.RaceProps.Humanlike)
				{
					return true;
				}
			}
			else if (t.Faction == null && t.TryGetComp<CompCauseGameCondition>() != null)
			{
				return true;
			}
			return t.Faction != null && t.Faction.HostileTo(fac);
		}

		// Token: 0x0600785B RID: 30811 RVA: 0x00249D70 File Offset: 0x00247F70
		private static bool IsPredatorHostileTo(Pawn predator, Pawn toPawn)
		{
			if (toPawn.Faction == null)
			{
				return false;
			}
			if (toPawn.Faction.HasPredatorRecentlyAttackedAnyone(predator))
			{
				return true;
			}
			Pawn preyOfMyFaction = GenHostility.GetPreyOfMyFaction(predator, toPawn.Faction);
			return preyOfMyFaction != null && predator.Position.InHorDistOf(preyOfMyFaction.Position, 12f);
		}

		// Token: 0x0600785C RID: 30812 RVA: 0x000510BE File Offset: 0x0004F2BE
		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			return toFaction.HasPredatorRecentlyAttackedAnyone(predator) || GenHostility.GetPreyOfMyFaction(predator, toFaction) != null;
		}

		// Token: 0x0600785D RID: 30813 RVA: 0x00249DC8 File Offset: 0x00247FC8
		private static Pawn GetPreyOfMyFaction(Pawn predator, Faction myFaction)
		{
			Job curJob = predator.CurJob;
			if (curJob != null && curJob.def == JobDefOf.PredatorHunt && !predator.jobs.curDriver.ended)
			{
				Pawn pawn = curJob.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn != null && !pawn.Dead && pawn.Faction == myFaction)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x0600785E RID: 30814 RVA: 0x000510D7 File Offset: 0x0004F2D7
		public static bool AnyHostileActiveThreatToPlayer(Map map, bool countDormantPawnsAsHostile = false)
		{
			return GenHostility.AnyHostileActiveThreatTo(map, Faction.OfPlayer, countDormantPawnsAsHostile);
		}

		// Token: 0x0600785F RID: 30815 RVA: 0x00249E2C File Offset: 0x0024802C
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, bool countDormantPawnsAsHostile = false)
		{
			IAttackTarget attackTarget;
			return GenHostility.AnyHostileActiveThreatTo(map, faction, out attackTarget, countDormantPawnsAsHostile);
		}

		// Token: 0x06007860 RID: 30816 RVA: 0x00249E44 File Offset: 0x00248044
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, out IAttackTarget threat, bool countDormantPawnsAsHostile = false)
		{
			foreach (IAttackTarget attackTarget in map.attackTargetsCache.TargetsHostileToFaction(faction))
			{
				if (GenHostility.IsActiveThreatTo(attackTarget, faction))
				{
					threat = attackTarget;
					return true;
				}
				Pawn pawn;
				if (countDormantPawnsAsHostile && attackTarget.Thing.HostileTo(faction) && !attackTarget.Thing.Fogged() && !attackTarget.ThreatDisabled(null) && (pawn = (attackTarget.Thing as Pawn)) != null)
				{
					CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
					if (comp != null && !comp.Awake)
					{
						threat = attackTarget;
						return true;
					}
				}
			}
			threat = null;
			return false;
		}

		// Token: 0x06007861 RID: 30817 RVA: 0x000510E5 File Offset: 0x0004F2E5
		public static bool IsActiveThreatToPlayer(IAttackTarget target)
		{
			return GenHostility.IsActiveThreatTo(target, Faction.OfPlayer);
		}

		// Token: 0x06007862 RID: 30818 RVA: 0x00249F00 File Offset: 0x00248100
		public static bool IsActiveThreatTo(IAttackTarget target, Faction faction)
		{
			if (!target.Thing.HostileTo(faction))
			{
				return false;
			}
			if (!(target.Thing is IAttackTargetSearcher))
			{
				return false;
			}
			if (target.ThreatDisabled(null))
			{
				return false;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.LordJob is LordJob_DefendAndExpandHive && (pawn.mindState.duty == null || pawn.mindState.duty.def != DutyDefOf.AssaultColony))
				{
					return false;
				}
			}
			Pawn pawn2 = target.Thing as Pawn;
			if (pawn2 != null && (pawn2.MentalStateDef == MentalStateDefOf.PanicFlee || pawn2.IsPrisoner))
			{
				return false;
			}
			CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null && !compCanBeDormant.Awake)
			{
				return false;
			}
			CompInitiatable compInitiatable = target.Thing.TryGetComp<CompInitiatable>();
			if (compInitiatable != null && !compInitiatable.Initiated)
			{
				return false;
			}
			if (target.Thing.Spawned)
			{
				TraverseParms traverseParms = (pawn2 != null) ? TraverseParms.For(pawn2, Danger.Deadly, TraverseMode.ByPawn, false) : TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
				if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007863 RID: 30819 RVA: 0x000510F2 File Offset: 0x0004F2F2
		public static bool IsDefMechClusterThreat(ThingDef def)
		{
			return (def.building != null && (def.building.IsTurret || def.building.IsMortar)) || def.isMechClusterThreat;
		}

		// Token: 0x06007864 RID: 30820 RVA: 0x0005111E File Offset: 0x0004F31E
		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreatToPlayer(map, false))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
