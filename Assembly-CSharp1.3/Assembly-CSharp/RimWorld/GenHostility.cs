using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000EC1 RID: 3777
	public static class GenHostility
	{
		// Token: 0x0600591E RID: 22814 RVA: 0x001E64D4 File Offset: 0x001E46D4
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
			return (pawn == null || pawn2 == null || ((pawn.story == null || !pawn.story.traits.DisableHostilityFrom(pawn2)) && (pawn2.story == null || !pawn2.story.traits.DisableHostilityFrom(pawn)))) && ((pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b)) || (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a)) || (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn))) || ((a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2)) || (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn))) || ((a.Faction != null && pawn2 != null && pawn2.IsSlave && pawn2.Faction == a.Faction && (pawn == null || !pawn.IsSlave) && SlaveRebellionUtility.IsRebelling(pawn2)) || (b.Faction != null && pawn != null && pawn.IsSlave && pawn.Faction == b.Faction && (pawn2 == null || !pawn2.IsSlave) && SlaveRebellionUtility.IsRebelling(pawn))) || ((a.Faction == null || pawn2 == null || pawn2.HostFaction != a.Faction) && (b.Faction == null || pawn == null || pawn.HostFaction != b.Faction) && (pawn == null || !pawn.IsPrisoner || pawn2 == null || !pawn2.IsPrisoner) && (pawn == null || !pawn.IsSlave || pawn2 == null || !pawn2.IsSlave) && (pawn == null || pawn2 == null || ((!pawn.IsPrisoner || pawn.HostFaction != pawn2.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (!pawn2.IsPrisoner || pawn2.HostFaction != pawn.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (pawn == null || pawn2 == null || ((pawn.HostFaction == null || pawn2.Faction == null || pawn.HostFaction.HostileTo(pawn2.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (pawn2.HostFaction == null || pawn.Faction == null || pawn2.HostFaction.HostileTo(pawn.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (a.Faction == null || !a.Faction.IsPlayer || pawn2 == null || !pawn2.mindState.WillJoinColonyIfRescued) && (b.Faction == null || !b.Faction.IsPlayer || pawn == null || !pawn.mindState.WillJoinColonyIfRescued) && ((pawn != null && pawn.Faction == null && pawn.RaceProps.Humanlike && b.Faction != null && b.Faction.def.hostileToFactionlessHumanlikes) || (pawn2 != null && pawn2.Faction == null && pawn2.RaceProps.Humanlike && a.Faction != null && a.Faction.def.hostileToFactionlessHumanlikes) || (a.Faction != null && b.Faction != null && a.Faction.HostileTo(b.Faction)))));
		}

		// Token: 0x0600591F RID: 22815 RVA: 0x001E6854 File Offset: 0x001E4A54
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
				if (pawn.Faction == fac && SlaveRebellionUtility.IsRebelling(pawn))
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

		// Token: 0x06005920 RID: 22816 RVA: 0x001E6954 File Offset: 0x001E4B54
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

		// Token: 0x06005921 RID: 22817 RVA: 0x001E69A9 File Offset: 0x001E4BA9
		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			return toFaction.HasPredatorRecentlyAttackedAnyone(predator) || GenHostility.GetPreyOfMyFaction(predator, toFaction) != null;
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x001E69C4 File Offset: 0x001E4BC4
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

		// Token: 0x06005923 RID: 22819 RVA: 0x001E6A26 File Offset: 0x001E4C26
		public static bool AnyHostileActiveThreatToPlayer(Map map, bool countDormantPawnsAsHostile = false, bool countSolitaryInsectsAsHostile = true)
		{
			return GenHostility.AnyHostileActiveThreatTo(map, Faction.OfPlayer, countDormantPawnsAsHostile, countSolitaryInsectsAsHostile);
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x001E6A38 File Offset: 0x001E4C38
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, bool countDormantPawnsAsHostile = false, bool countSolitaryInsectsAsHostile = true)
		{
			IAttackTarget attackTarget;
			return GenHostility.AnyHostileActiveThreatTo(map, faction, out attackTarget, countDormantPawnsAsHostile, countSolitaryInsectsAsHostile);
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x001E6A50 File Offset: 0x001E4C50
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, out IAttackTarget threat, bool countDormantPawnsAsHostile = false, bool countSolitaryInsectsAsHostile = true)
		{
			foreach (IAttackTarget attackTarget in map.attackTargetsCache.TargetsHostileToFaction(faction))
			{
				Pawn pawn;
				if (countSolitaryInsectsAsHostile || (pawn = (attackTarget as Pawn)) == null || !pawn.RaceProps.Insect || pawn.GetLord() != null)
				{
					if (GenHostility.IsActiveThreatTo(attackTarget, faction))
					{
						threat = attackTarget;
						return true;
					}
					Pawn pawn2;
					if (countDormantPawnsAsHostile && attackTarget.Thing.HostileTo(faction) && !attackTarget.Thing.Fogged() && !attackTarget.ThreatDisabled(null) && (pawn2 = (attackTarget.Thing as Pawn)) != null)
					{
						CompCanBeDormant comp = pawn2.GetComp<CompCanBeDormant>();
						if (comp != null && !comp.Awake)
						{
							threat = attackTarget;
							return true;
						}
					}
				}
			}
			threat = null;
			return false;
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x001E6B38 File Offset: 0x001E4D38
		public static bool IsActiveThreatToPlayer(IAttackTarget target)
		{
			return GenHostility.IsActiveThreatTo(target, Faction.OfPlayer);
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x001E6B48 File Offset: 0x001E4D48
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
				TraverseParms traverseParms = (pawn2 != null) ? TraverseParms.For(pawn2, Danger.Deadly, TraverseMode.ByPawn, false, false, false) : TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
				if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x001E6C76 File Offset: 0x001E4E76
		public static bool IsDefMechClusterThreat(ThingDef def)
		{
			return (def.building != null && (def.building.IsTurret || def.building.IsMortar)) || def.isMechClusterThreat;
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x001E6CA2 File Offset: 0x001E4EA2
		public static bool IsOfFriendlyFactionAndNonThreatAnymore(Pawn attacker, Pawn victim)
		{
			return victim.Faction != null && attacker.Faction != null && !victim.HostileTo(attacker);
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x001E6CC0 File Offset: 0x001E4EC0
		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreatToPlayer(map, false, true))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
