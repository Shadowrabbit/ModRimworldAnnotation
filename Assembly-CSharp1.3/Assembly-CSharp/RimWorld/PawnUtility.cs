using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DCB RID: 3531
	public static class PawnUtility
	{
		// Token: 0x060051AE RID: 20910 RVA: 0x001B8CB4 File Offset: 0x001B6EB4
		public static Faction GetFactionLeaderFaction(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].leader == pawn)
				{
					return allFactionsListForReading[i];
				}
			}
			return null;
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001B8CF5 File Offset: 0x001B6EF5
		public static bool IsFactionLeader(Pawn pawn)
		{
			return PawnUtility.GetFactionLeaderFaction(pawn) != null;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001B8D00 File Offset: 0x001B6F00
		public static bool IsInteractionBlocked(this Pawn pawn, InteractionDef interaction, bool isInitiator, bool isRandom)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].def.blocksSocialInteraction)
					{
						return true;
					}
				}
				return false;
			}
			if (isRandom)
			{
				return mentalStateDef.blockRandomInteraction;
			}
			if (interaction == null)
			{
				return false;
			}
			List<InteractionDef> list = isInitiator ? mentalStateDef.blockInteractionInitiationExcept : mentalStateDef.blockInteractionRecipientExcept;
			return list != null && !list.Contains(interaction);
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001B8D80 File Offset: 0x001B6F80
		public static bool IsKidnappedPawn(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001B8DC5 File Offset: 0x001B6FC5
		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return (pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveDropPodInfo>(pawn)) || ThingOwnerUtility.AnyParentIs<TravelingTransportPods>(pawn);
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x001B8DDF File Offset: 0x001B6FDF
		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is Settlement_TraderTracker;
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001B8DF0 File Offset: 0x001B6FF0
		public static bool IsInvisible(this Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].TryGetComp<HediffComp_Invisibility>() != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x001B8E30 File Offset: 0x001B7030
		public static bool ShouldDropCarriedThingAfterJob(this Pawn pawn, Job job)
		{
			return !pawn.Destroyed && pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && !job.def.carryThingAfterJob;
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x001B8E5F File Offset: 0x001B705F
		public static bool ShouldDropCarriedThingBeforeJob(this Pawn pawn, Job job)
		{
			return !pawn.Destroyed && pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && job.def.dropThingBeforeJob;
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001B8E8B File Offset: 0x001B708B
		public static bool IsCarryingPawn(this Pawn pawn, Pawn carryPawn = null)
		{
			return pawn.carryTracker != null && pawn.carryTracker.CarriedThing is Pawn && (carryPawn == null || pawn.carryTracker.CarriedThing == carryPawn);
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x001B8EBC File Offset: 0x001B70BC
		public static bool IsCarryingThing(this Pawn pawn, Thing carriedThing)
		{
			return pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing == carriedThing;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x001B8EE3 File Offset: 0x001B70E3
		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Pawn x) => Find.GameInitData.startingAndOptionalPawns.Contains(x)))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001B8F1C File Offset: 0x001B711C
		public static void DestroyStartingColonistFamily(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns.ToList<Pawn>())
			{
				if (!Find.GameInitData.startingAndOptionalPawns.Contains(pawn2))
				{
					WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn2);
					if (situation == WorldPawnSituation.Free || situation == WorldPawnSituation.Dead)
					{
						Find.WorldPawns.RemovePawn(pawn2);
						Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					}
				}
			}
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001B8FB0 File Offset: 0x001B71B0
		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false)
		{
			TraverseParms tp = passDoors ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false) : TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			bool foundEnemy = false;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].HostileTo(pawn))
					{
						foundEnemy = true;
						return true;
					}
				}
				return foundEnemy;
			}, regionsToScan, RegionType.Set_Passable);
			return foundEnemy;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001B9034 File Offset: 0x001B7234
		public static bool WillSoonHaveBasicNeed(Pawn p)
		{
			return p.needs != null && ((p.needs.rest != null && p.needs.rest.CurLevel < 0.33f) || (p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + 0.05f));
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x001B90AC File Offset: 0x001B72AC
		public static bool CanCasuallyInteractNow(this Pawn p, bool twoWayInteraction = false, bool canInteractWhileSleeping = false)
		{
			if (p.Drafted)
			{
				return false;
			}
			if (p.IsInvisible())
			{
				return false;
			}
			if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p))
			{
				return false;
			}
			if (p.InAggroMentalState)
			{
				return false;
			}
			if (!p.Awake() && !canInteractWhileSleeping)
			{
				return false;
			}
			if (p.IsFormingCaravan())
			{
				return false;
			}
			Job curJob = p.CurJob;
			return curJob == null || !twoWayInteraction || (curJob.def.casualInterruptible && curJob.playerForced);
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x001B911D File Offset: 0x001B731D
		public static IEnumerable<Pawn> SpawnedMasteredPawns(Pawn master)
		{
			if (Current.ProgramState != ProgramState.Playing || master.Faction == null || !master.RaceProps.Humanlike)
			{
				yield break;
			}
			if (!master.Spawned)
			{
				yield break;
			}
			List<Pawn> pawns = master.Map.mapPawns.SpawnedPawnsInFaction(master.Faction);
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].playerSettings != null && pawns[i].playerSettings.Master == master)
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x001B912D File Offset: 0x001B732D
		public static bool InValidState(Pawn p)
		{
			return p.health != null && (p.Dead || (p.stances != null && p.mindState != null && p.needs != null && p.ageTracker != null));
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x001B9164 File Offset: 0x001B7364
		public static PawnPosture GetPosture(this Pawn p)
		{
			if (p.Dead)
			{
				return PawnPosture.LayingOnGroundNormal;
			}
			if (p.Downed)
			{
				if (p.jobs != null && p.jobs.posture.Laying())
				{
					return p.jobs.posture;
				}
				return PawnPosture.LayingOnGroundNormal;
			}
			else
			{
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn;
				if ((thingHolderWithDrawnPawn = (p.ParentHolder as IThingHolderWithDrawnPawn)) != null)
				{
					return thingHolderWithDrawnPawn.HeldPawnPosture;
				}
				if (p.jobs == null)
				{
					return PawnPosture.Standing;
				}
				return p.jobs.posture;
			}
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x001B91D8 File Offset: 0x001B73D8
		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639);
			}
			Job job = JobMaker.MakeJob(maintainPosture ? JobDefOf.Wait_MaintainPosture : JobDefOf.Wait, faceTarget);
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false);
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x001B9238 File Offset: 0x001B7438
		public static void GainComfortFromCellIfPossible(this Pawn p, bool chairsOnly = false)
		{
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				Building edifice = p.Position.GetEdifice(p.Map);
				if (edifice != null && (!chairsOnly || (edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable)))
				{
					PawnUtility.GainComfortFromThingIfPossible(p, edifice);
				}
			}
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x001B9294 File Offset: 0x001B7494
		public static void GainComfortFromThingIfPossible(Pawn p, Thing from)
		{
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				float statValue = from.GetStatValue(StatDefOf.Comfort, true);
				if (statValue >= 0f && p.needs != null && p.needs.comfort != null)
				{
					p.needs.comfort.ComfortUsed(statValue);
				}
			}
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x001B92EC File Offset: 0x001B74EC
		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			if (pawn.needs != null && pawn.needs.food != null)
			{
				switch (pawn.needs.food.CurCategory)
				{
				case HungerCategory.Fed:
					return 1f;
				case HungerCategory.Hungry:
					return 0.5f;
				case HungerCategory.UrgentlyHungry:
					return 0.25f;
				case HungerCategory.Starving:
					return 0f;
				}
			}
			return 1f;
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x001B9354 File Offset: 0x001B7554
		public static bool FertileMateTarget(Pawn male, Pawn female)
		{
			if (female.gender != Gender.Female || !female.ageTracker.CurLifeStage.reproductive || female.health.hediffSet.HasHediff(HediffDefOf.Sterilized, false))
			{
				return false;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			if (compEggLayer != null)
			{
				return !compEggLayer.FullyFertilized;
			}
			return !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false);
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x001B93C4 File Offset: 0x001B75C4
		public static void Mated(Pawn male, Pawn female)
		{
			if (!female.ageTracker.CurLifeStage.reproductive)
			{
				return;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			if (compEggLayer != null)
			{
				compEggLayer.Fertilize(male);
				return;
			}
			if (Rand.Value < 0.5f && !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false))
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, female, null);
				hediff_Pregnant.father = male;
				female.health.AddHediff(hediff_Pregnant, null, null, null);
			}
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x001B944C File Offset: 0x001B764C
		public static bool PlayerForcedJobNowOrSoon(Pawn pawn)
		{
			if (pawn.jobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob != null)
			{
				return curJob.playerForced;
			}
			return pawn.jobs.jobQueue.Any<QueuedJob>() && pawn.jobs.jobQueue.Peek().job.playerForced;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001B94A4 File Offset: 0x001B76A4
		public static bool TrySpawnHatchedOrBornPawn(Pawn pawn, Thing motherOrEgg)
		{
			if (motherOrEgg.SpawnedOrAnyParentSpawned)
			{
				return GenSpawn.Spawn(pawn, motherOrEgg.PositionHeld, motherOrEgg.MapHeld, WipeMode.Vanish) != null;
			}
			Pawn pawn2 = motherOrEgg as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.IsCaravanMember())
				{
					pawn2.GetCaravan().AddPawn(pawn, true);
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
				if (pawn2.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
			}
			else if (motherOrEgg.ParentHolder != null)
			{
				Pawn_InventoryTracker pawn_InventoryTracker = motherOrEgg.ParentHolder as Pawn_InventoryTracker;
				if (pawn_InventoryTracker != null)
				{
					if (pawn_InventoryTracker.pawn.IsCaravanMember())
					{
						pawn_InventoryTracker.pawn.GetCaravan().AddPawn(pawn, true);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
					if (pawn_InventoryTracker.pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001B9574 File Offset: 0x001B7774
		public static ByteGrid GetAvoidGrid(this Pawn p, bool onlyIfLordAllows = true)
		{
			if (!p.Spawned)
			{
				return null;
			}
			if (p.Faction == null)
			{
				return null;
			}
			if (!p.Faction.def.canUseAvoidGrid)
			{
				return null;
			}
			if (p.Faction == Faction.OfPlayer || !p.Faction.HostileTo(Faction.OfPlayer))
			{
				return null;
			}
			if (!onlyIfLordAllows)
			{
				return p.Map.avoidGrid.Grid;
			}
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil.useAvoidGrid)
			{
				return lord.Map.avoidGrid.Grid;
			}
			return null;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x001B9607 File Offset: 0x001B7807
		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return !p.Downed && !p.Dead && p.mindState.anyCloseHostilesRecently;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001B962B File Offset: 0x001B782B
		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns, forPathFinder) != null;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x001B963C File Offset: 0x001B783C
		public static Pawn PawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			List<Thing> thingList = c.GetThingList(forPawn.Map);
			if (thingList.Count == 0)
			{
				return null;
			}
			bool flag = false;
			if (actAsIfHadCollideWithPawnsJob)
			{
				flag = true;
			}
			else
			{
				Job curJob = forPawn.CurJob;
				if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || forPawn.jobs.curDriver.collideWithPawns))
				{
					flag = true;
				}
				else if (forPawn.Drafted)
				{
					bool moving = forPawn.pather.Moving;
				}
			}
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null && pawn != forPawn && !pawn.Downed && (!collideOnlyWithStandingPawns || (!pawn.pather.MovingNow && (!pawn.pather.Moving || !pawn.pather.MovedRecently(60)))) && !PawnUtility.PawnsCanShareCellBecauseOfBodySize(pawn, forPawn))
				{
					if (pawn.HostileTo(forPawn))
					{
						return pawn;
					}
					if (flag && (forPathFinder || !forPawn.Drafted || !pawn.RaceProps.Animal))
					{
						Job curJob2 = pawn.CurJob;
						if (curJob2 != null && (curJob2.collideWithPawns || curJob2.def.collideWithPawns || pawn.jobs.curDriver.collideWithPawns))
						{
							return pawn;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x001B9794 File Offset: 0x001B7994
		private static bool PawnsCanShareCellBecauseOfBodySize(Pawn p1, Pawn p2)
		{
			if (p1.BodySize >= 1.5f || p2.BodySize >= 1.5f)
			{
				return false;
			}
			float num = p1.BodySize / p2.BodySize;
			if (num < 1f)
			{
				num = 1f / num;
			}
			return num > 3.57f;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x001B97E4 File Offset: 0x001B79E4
		public static bool KnownDangerAt(IntVec3 c, Map map, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.IsDangerousFor(forPawn);
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x001B9808 File Offset: 0x001B7A08
		public static bool ShouldSendNotificationAbout(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (PawnGenerator.IsBeingGenerated(p))
			{
				return false;
			}
			if (p.IsWorldPawn() && (!p.IsCaravanMember() || !p.GetCaravan().IsPlayerControlled) && !PawnUtility.IsTravelingInTransportPodWorldObject(p) && !p.IsBorrowedByAnyFaction() && p.Corpse.DestroyedOrNull())
			{
				return false;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (p.RaceProps.Humanlike && p.guest.Released && !p.Downed && !p.InBed())
				{
					return false;
				}
				if (p.CurJob != null && p.CurJob.exitMapOnArrival && !PrisonBreakUtility.IsPrisonBreaking(p))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x001B98CD File Offset: 0x001B7ACD
		public static bool ShouldGetThoughtAbout(Pawn pawn, Pawn subject)
		{
			return pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn());
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x001B98F4 File Offset: 0x001B7AF4
		public static bool IsTeetotaler(this Pawn pawn)
		{
			return !new HistoryEvent(HistoryEventDefOf.IngestedDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo() || !new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo() || (pawn.story != null && pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) < 0);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x001B995F File Offset: 0x001B7B5F
		public static bool IsProsthophobe(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.BodyPurist);
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x001B9980 File Offset: 0x001B7B80
		public static bool IsPrisonerInPrisonCell(this Pawn pawn)
		{
			return pawn.IsPrisoner && pawn.Spawned && pawn.Position.IsInPrisonCell(pawn.Map);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001B99A8 File Offset: 0x001B7BA8
		public static bool IsBeingArrested(Pawn pawn)
		{
			if (pawn.Map == null)
			{
				return false;
			}
			foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (pawn2 != pawn && pawn2.CurJobDef == JobDefOf.Arrest && pawn2.CurJob.AnyTargetIs(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001B9A34 File Offset: 0x001B7C34
		public static string PawnKindsToCommaList(IEnumerable<Pawn> pawns, bool useAnd = false)
		{
			PawnUtility.tmpPawns.Clear();
			PawnUtility.tmpPawns.AddRange(pawns);
			if (PawnUtility.tmpPawns.Count >= 2)
			{
				PawnUtility.tmpPawns.SortBy((Pawn x) => !x.RaceProps.Humanlike, (Pawn x) => x.GetKindLabelPlural(-1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawns.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawns[i].kindDef))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawns[i].kindDef);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawns.Count; j++)
					{
						if (PawnUtility.tmpPawns[j].kindDef == PawnUtility.tmpPawns[i].kindDef)
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + PawnUtility.tmpPawns[i].KindLabel);
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num + " " + PawnUtility.tmpPawns[i].GetKindLabelPlural(num));
					}
				}
			}
			PawnUtility.tmpPawns.Clear();
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd, false);
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001B9BBC File Offset: 0x001B7DBC
		public static List<string> PawnKindsToList(IEnumerable<PawnKindDef> pawnKinds)
		{
			PawnUtility.tmpPawnKinds.Clear();
			PawnUtility.tmpPawnKinds.AddRange(pawnKinds);
			if (PawnUtility.tmpPawnKinds.Count >= 2)
			{
				PawnUtility.tmpPawnKinds.SortBy((PawnKindDef x) => !x.RaceProps.Humanlike, (PawnKindDef x) => GenLabel.BestKindLabel(x, Gender.None, true, -1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawnKinds.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawnKinds[i]))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawnKinds[i]);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawnKinds.Count; j++)
					{
						if (PawnUtility.tmpPawnKinds[j] == PawnUtility.tmpPawnKinds[i])
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, false, -1));
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num + " " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, true, num));
					}
				}
			}
			return PawnUtility.tmpPawnKindsStr;
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001B9D24 File Offset: 0x001B7F24
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x001B9D38 File Offset: 0x001B7F38
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix, Color color)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			for (int i = 0; i < PawnUtility.tmpPawnKindsStr.Count; i++)
			{
				PawnUtility.tmpPawnKindsStr[i] = PawnUtility.tmpPawnKindsStr[i].Colorize(color);
			}
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x001B9D88 File Offset: 0x001B7F88
		public static string PawnKindsToCommaList(IEnumerable<PawnKindDef> pawnKinds, bool useAnd = false)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd, false);
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x001B9D9D File Offset: 0x001B7F9D
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
			{
				return pawn.mindState.duty.locomotion;
			}
			return secondPriority;
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x001B9DD8 File Offset: 0x001B7FD8
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			if (locomotionUrgency != LocomotionUrgency.None)
			{
				return locomotionUrgency;
			}
			return thirdPriority;
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x001B9DF3 File Offset: 0x001B7FF3
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.maxDanger != Danger.Unspecified)
			{
				return pawn.mindState.duty.maxDanger;
			}
			return secondPriority;
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001B9E30 File Offset: 0x001B8030
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			if (danger != Danger.Unspecified)
			{
				return danger;
			}
			return thirdPriority;
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001B9E4C File Offset: 0x001B804C
		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.Wait_Combat || pawn.CurJob.def == JobDefOf.PredatorHunt);
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x001B9EAD File Offset: 0x001B80AD
		public static Hediff_Psylink GetMainPsylinkSource(this Pawn pawn)
		{
			return (Hediff_Psylink)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x001B9ECC File Offset: 0x001B80CC
		public static int GetPsylinkLevel(this Pawn pawn)
		{
			int num = 0;
			using (List<Hediff>.Enumerator enumerator = pawn.health.hediffSet.hediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hediff_Psylink hediff_Psylink;
					if ((hediff_Psylink = (enumerator.Current as Hediff_Psylink)) != null)
					{
						num += hediff_Psylink.level;
					}
				}
			}
			return num;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001B9F38 File Offset: 0x001B8138
		public static int GetMaxPsylinkLevel(this Pawn pawn)
		{
			return (int)HediffDefOf.PsychicAmplifier.maxSeverity;
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001B9F48 File Offset: 0x001B8148
		public static RoyalTitle GetMaxPsylinkLevelTitle(this Pawn pawn)
		{
			if (pawn.royalty == null)
			{
				return null;
			}
			int num = 0;
			RoyalTitle result = null;
			foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
			{
				if (num < royalTitle.def.maxPsylinkLevel)
				{
					num = royalTitle.def.maxPsylinkLevel;
					result = royalTitle;
				}
			}
			return result;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x001B9FC4 File Offset: 0x001B81C4
		public static int GetMaxPsylinkLevelByTitle(this Pawn pawn)
		{
			RoyalTitle maxPsylinkLevelTitle = pawn.GetMaxPsylinkLevelTitle();
			if (maxPsylinkLevelTitle == null)
			{
				return 0;
			}
			return maxPsylinkLevelTitle.def.maxPsylinkLevel;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x001B9FE8 File Offset: 0x001B81E8
		public static void ChangePsylinkLevel(this Pawn pawn, int levelOffset, bool sendLetter = true)
		{
			Hediff_Psylink hediff_Psylink = pawn.GetMainPsylinkSource();
			if (hediff_Psylink == null)
			{
				hediff_Psylink = (Hediff_Psylink)HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, null);
				try
				{
					hediff_Psylink.suppressPostAddLetter = !sendLetter;
					pawn.health.AddHediff(hediff_Psylink, pawn.health.hediffSet.GetBrain(), null, null);
					return;
				}
				finally
				{
					hediff_Psylink.suppressPostAddLetter = false;
				}
			}
			hediff_Psylink.ChangeLevel(levelOffset, sendLetter);
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x001BA064 File Offset: 0x001B8264
		public static void GiveAllStartingPlayerPawnsThought(ThoughtDef thought)
		{
			foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
			{
				if (pawn.needs.mood != null)
				{
					if (thought.IsSocial)
					{
						using (List<Pawn>.Enumerator enumerator2 = Find.GameInitData.startingAndOptionalPawns.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Pawn pawn2 = enumerator2.Current;
								if (pawn2 != pawn)
								{
									pawn.needs.mood.thoughts.memories.TryGainMemory(thought, pawn2, null);
								}
							}
							continue;
						}
					}
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought, null, null);
				}
			}
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x001BA14C File Offset: 0x001B834C
		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid)
			{
				return pawn.mindState.duty.focus.Cell;
			}
			return pawn.Position;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x001BA199 File Offset: 0x001B8399
		public static bool EverBeenColonistOrTameAnimal(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsColonistOrColonyAnimal) > 0;
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x001BA1AE File Offset: 0x001B83AE
		public static bool EverBeenPrisoner(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsPrisoner) > 0;
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x001BA1C3 File Offset: 0x001B83C3
		public static bool EverBeenQuestLodger(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsQuestLodger) > 0;
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x001BA1D8 File Offset: 0x001B83D8
		public static void RecoverFromUnwalkablePositionOrKill(IntVec3 c, Map map)
		{
			if (!c.InBounds(map) || c.Walkable(map))
			{
				return;
			}
			PawnUtility.tmpThings.Clear();
			PawnUtility.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < PawnUtility.tmpThings.Count; i++)
			{
				Pawn pawn = PawnUtility.tmpThings[i] as Pawn;
				if (pawn != null)
				{
					IntVec3 position;
					if (CellFinder.TryFindBestPawnStandCell(pawn, out position, false))
					{
						pawn.Position = position;
						pawn.Notify_Teleported(true, false);
					}
					else
					{
						DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, pawn.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.Collapse, null, true, true);
						pawn.TakeDamage(damageInfo);
						if (!pawn.Dead)
						{
							pawn.Kill(new DamageInfo?(damageInfo), null);
						}
					}
				}
			}
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x001BA2AC File Offset: 0x001B84AC
		public static float GetManhunterOnDamageChance(Pawn pawn, float distance, Thing instigator)
		{
			float num = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			num *= GenMath.LerpDoubleClamped(1f, 30f, 3f, 1f, distance);
			if (instigator != null)
			{
				num *= 1f - instigator.GetStatValue(StatDefOf.HuntingStealth, true);
				Pawn instigator2;
				if ((instigator2 = (instigator as Pawn)) != null)
				{
					num *= PawnUtility.GetManhunterChanceFactorForInstigator(instigator2);
				}
			}
			return num;
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001BA30E File Offset: 0x001B850E
		public static float GetManhunterOnDamageChance(Pawn pawn, Thing instigator = null)
		{
			if (instigator != null)
			{
				return PawnUtility.GetManhunterOnDamageChance(pawn, pawn.Position.DistanceTo(instigator.Position), instigator);
			}
			return PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x001BA338 File Offset: 0x001B8538
		public static float GetManhunterChanceFactorForInstigator(Pawn instigator)
		{
			float num = 1f;
			if (ModsConfig.IdeologyActive && instigator.Ideo != null)
			{
				Precept_Role role = instigator.Ideo.GetRole(instigator);
				if (role != null && role.def.roleEffects != null)
				{
					RoleEffect roleEffect = role.def.roleEffects.FirstOrDefault((RoleEffect eff) => eff is RoleEffect_HuntingRevengeChanceFactor);
					if (roleEffect != null)
					{
						num *= ((RoleEffect_HuntingRevengeChanceFactor)roleEffect).factor;
					}
				}
			}
			return num;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001BA3B9 File Offset: 0x001B85B9
		public static float GetManhunterOnDamageChance(PawnKindDef kind)
		{
			return kind.RaceProps.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x001BA3D6 File Offset: 0x001B85D6
		public static float GetManhunterOnDamageChance(RaceProperties race)
		{
			return race.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001BA3F0 File Offset: 0x001B85F0
		public static bool PlayerHasReproductivePair(PawnKindDef pawnKindDef)
		{
			if (!pawnKindDef.RaceProps.Animal)
			{
				return false;
			}
			List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction.Count; i++)
			{
				Pawn pawn = allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction[i];
				if (pawn.kindDef == pawnKindDef && pawn.ageTracker.CurLifeStage.reproductive)
				{
					if (pawn.gender == Gender.Male)
					{
						flag = true;
					}
					else if (pawn.gender == Gender.Female)
					{
						flag2 = true;
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x001BA470 File Offset: 0x001B8670
		public static float PlayerAnimalBodySizePerCapita()
		{
			float num = 0f;
			int num2 = 0;
			List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction.Count; i++)
			{
				Pawn pawn = allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction[i];
				if (pawn.IsFreeColonist && !pawn.IsQuestLodger())
				{
					num2++;
				}
				if (pawn.RaceProps.Animal)
				{
					num += pawn.BodySize;
				}
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x001BA4E4 File Offset: 0x001B86E4
		private static List<Pawn> PawnsOfFactionOnMapOrInCaravan(Pawn pawn)
		{
			List<Pawn> result;
			if (pawn.Spawned)
			{
				result = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			}
			else
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan == null)
				{
					return null;
				}
				result = caravan.PawnsListForReading;
			}
			return result;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001BA528 File Offset: 0x001B8728
		public static float PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(Pawn pawn)
		{
			if (pawn.Ideo == null || pawn.Faction == null)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 0;
			List<Pawn> list = PawnUtility.PawnsOfFactionOnMapOrInCaravan(pawn);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == pawn.Faction && !pawn.IsQuestLodger())
				{
					if (list[i].RaceProps.Animal && pawn.Ideo.IsVeneratedAnimal(list[i]))
					{
						num += list[i].BodySize;
					}
					else if (list[i].RaceProps.Humanlike)
					{
						num2++;
					}
				}
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x001BA5E4 File Offset: 0x001B87E4
		public static Pawn FirstVeneratedAnimalOnMapOrCaravan(Pawn pawn)
		{
			if (pawn.Ideo == null || pawn.Faction == null)
			{
				return null;
			}
			List<Pawn> list = PawnUtility.PawnsOfFactionOnMapOrInCaravan(pawn);
			for (int i = 0; i < list.Count; i++)
			{
				if (pawn.Faction == list[i].Faction && pawn.Ideo.IsVeneratedAnimal(list[i]))
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x001BA64C File Offset: 0x001B884C
		public static bool HasClothingNotRequiredByKind(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			if (wornApparel.Count > 0 && p.kindDef.apparelRequired.NullOrEmpty<ThingDef>())
			{
				return true;
			}
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (apparel.def.apparel.countsAsClothingForNudity && !p.kindDef.apparelRequired.Contains(apparel.def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x001BA6D2 File Offset: 0x001B88D2
		public static IEnumerable<PawnKindDef> GetCombatPawnKindsForPoints(Func<PawnKindDef, bool> selector, float points, Func<PawnKindDef, float> selectionWeight = null)
		{
			IEnumerable<PawnKindDef> allKinds = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(selector);
			Func<PawnKindDef, float> func;
			if (selectionWeight == null)
			{
				func = ((PawnKindDef pk) => 1f);
			}
			else
			{
				func = selectionWeight;
			}
			selectionWeight = func;
			Func<PawnKindDef, bool> <>9__1;
			while (points > 0f)
			{
				IEnumerable<PawnKindDef> source = allKinds;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PawnKindDef def) => def.combatPower > 0f && def.combatPower <= points));
				}
				PawnKindDef pawnKindDef;
				if (!source.Where(predicate).TryRandomElementByWeight(selectionWeight, out pawnKindDef))
				{
					break;
				}
				points -= pawnKindDef.combatPower;
				yield return pawnKindDef;
			}
			yield break;
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x001BA6F0 File Offset: 0x001B88F0
		public static int GetMaxAllowedToPickUp(Pawn pawn, ThingDef thingDef)
		{
			if (!PawnUtility.CanPickUp(pawn, thingDef))
			{
				return 0;
			}
			if (!pawn.Map.IsPlayerHome)
			{
				return int.MaxValue;
			}
			if (thingDef.orderedTakeGroup == null)
			{
				return 0;
			}
			int num = pawn.inventory.Count((Thing t) => t.def.orderedTakeGroup == thingDef.orderedTakeGroup);
			return Math.Max(thingDef.orderedTakeGroup.max - num, 0);
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x001BA76C File Offset: 0x001B896C
		public static bool CanPickUp(Pawn pawn, ThingDef thingDef)
		{
			return !pawn.Map.IsPlayerHome || (pawn.inventory != null && thingDef.orderedTakeGroup != null && thingDef.orderedTakeGroup.max > 0);
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x001BA7A0 File Offset: 0x001B89A0
		public static bool ShouldBeSlaughtered(this Pawn pawn)
		{
			return pawn.Spawned && pawn.RaceProps.Animal && (pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) != null || pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Contains(pawn)) && pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.ReleaseAnimalToWild) == null;
		}

		// Token: 0x04003070 RID: 12400
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04003071 RID: 12401
		private static List<string> tmpPawnKindsStr = new List<string>();

		// Token: 0x04003072 RID: 12402
		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		// Token: 0x04003073 RID: 12403
		private static List<PawnKindDef> tmpPawnKinds = new List<PawnKindDef>();

		// Token: 0x04003074 RID: 12404
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
