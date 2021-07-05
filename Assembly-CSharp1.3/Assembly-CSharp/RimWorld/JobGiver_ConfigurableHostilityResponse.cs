using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000768 RID: 1896
	public class JobGiver_ConfigurableHostilityResponse : ThinkNode_JobGiver
	{
		// Token: 0x0600345B RID: 13403 RVA: 0x00128C84 File Offset: 0x00126E84
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

		// Token: 0x0600345C RID: 13404 RVA: 0x00128CE8 File Offset: 0x00126EE8
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
			Thing thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, null, 0f, maxDist, default(IntVec3), float.MaxValue, false, true, false);
			if (thing == null)
			{
				return null;
			}
			if (isMeleeAttack || pawn.CanReachImmediate(thing, PathEndMode.Touch))
			{
				return JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
			}
			Verb verb = pawn.TryGetAttackVerb(thing, !pawn.IsColonist);
			if (verb == null || verb.ApparelPreventsShooting())
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, thing);
			job.maxNumStaticAttacks = 2;
			job.expiryInterval = 2000;
			job.endIfCantShootTargetFromCurPos = true;
			return job;
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x00128DD8 File Offset: 0x00126FD8
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
					Log.Error(pawn.LabelShort + " decided to flee but there is not any threat around.");
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
								}));
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

		// Token: 0x04001E45 RID: 7749
		private static List<Thing> tmpThreats = new List<Thing>();
	}
}
