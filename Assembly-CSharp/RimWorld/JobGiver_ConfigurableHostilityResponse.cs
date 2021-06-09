using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C7B RID: 3195
	public class JobGiver_ConfigurableHostilityResponse : ThinkNode_JobGiver
	{
		// Token: 0x06004AB8 RID: 19128 RVA: 0x001A29F4 File Offset: 0x001A0BF4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.playerSettings == null || !pawn.playerSettings.UsesConfigurableHostilityResponse)
			{
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				return null;
			case HostilityResponseMode.Attack:
				return this.TryGetAttackNearbyEnemyJob(pawn);
			case HostilityResponseMode.Flee:
				return this.TryGetFleeJob(pawn);
			default:
				return null;
			}
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x001A2A58 File Offset: 0x001A0C58
		private Job TryGetAttackNearbyEnemyJob(Pawn pawn)
		{
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			bool isMeleeAttack = pawn.CurrentEffectiveVerb.IsMeleeAttack;
			float maxDist = 8f;
			if (!isMeleeAttack)
			{
				maxDist = Mathf.Clamp(pawn.CurrentEffectiveVerb.verbProps.range * 0.66f, 2f, 20f);
			}
			Thing thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, null, 0f, maxDist, default(IntVec3), float.MaxValue, false, true);
			if (thing == null)
			{
				return null;
			}
			if (isMeleeAttack || pawn.CanReachImmediate(thing, PathEndMode.Touch))
			{
				return JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
			}
			Verb verb = pawn.TryGetAttackVerb(thing, !pawn.IsColonist);
			if (verb == null || verb.ApparelPreventsShooting(pawn.Position, thing))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, thing);
			job.maxNumStaticAttacks = 2;
			job.expiryInterval = 2000;
			job.endIfCantShootTargetFromCurPos = true;
			return job;
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x001A2B54 File Offset: 0x001A0D54
		private Job TryGetFleeJob(Pawn pawn)
		{
			if (!SelfDefenseUtility.ShouldStartFleeing(pawn))
			{
				return null;
			}
			IntVec3 c;
			if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.FleeAndCower)
			{
				c = pawn.CurJob.targetA.Cell;
			}
			else
			{
				JobGiver_ConfigurableHostilityResponse.tmpThreats.Clear();
				List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
				for (int i = 0; i < potentialTargetsFor.Count; i++)
				{
					Thing thing = potentialTargetsFor[i].Thing;
					if (SelfDefenseUtility.ShouldFleeFrom(thing, pawn, false, false))
					{
						JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing);
					}
				}
				List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing2 = list[j];
					if (SelfDefenseUtility.ShouldFleeFrom(thing2, pawn, false, false))
					{
						JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing2);
					}
				}
				if (!JobGiver_ConfigurableHostilityResponse.tmpThreats.Any<Thing>())
				{
					Log.Error(pawn.LabelShort + " decided to flee but there is not any threat around.", false);
					Region region = pawn.GetRegion(RegionType.Set_Passable);
					if (region == null)
					{
						return null;
					}
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.door == null || reg.door.Open, delegate(Region reg)
					{
						List<Thing> list2 = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
						for (int k = 0; k < list2.Count; k++)
						{
							Thing thing3 = list2[k];
							if (SelfDefenseUtility.ShouldFleeFrom(thing3, pawn, false, false))
							{
								JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing3);
								Log.Warning(string.Format("  Found a viable threat {0}; tests are {1}, {2}, {3}", new object[]
								{
									thing3.LabelShort,
									thing3.Map.attackTargetsCache.Debug_CheckIfInAllTargets(thing3 as IAttackTarget),
									thing3.Map.attackTargetsCache.Debug_CheckIfHostileToFaction(pawn.Faction, thing3 as IAttackTarget),
									thing3 is IAttackTarget
								}), false);
							}
						}
						return false;
					}, 9, RegionType.Set_Passable);
					if (!JobGiver_ConfigurableHostilityResponse.tmpThreats.Any<Thing>())
					{
						return null;
					}
				}
				c = CellFinderLoose.GetFleeDest(pawn, JobGiver_ConfigurableHostilityResponse.tmpThreats, 23f);
				JobGiver_ConfigurableHostilityResponse.tmpThreats.Clear();
			}
			return JobMaker.MakeJob(JobDefOf.FleeAndCower, c);
		}

		// Token: 0x04003187 RID: 12679
		private static List<Thing> tmpThreats = new List<Thing>();
	}
}
