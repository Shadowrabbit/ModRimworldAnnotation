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
	// Token: 0x02001448 RID: 5192
	public static class PawnUtility
	{
		// Token: 0x06007005 RID: 28677 RVA: 0x00225228 File Offset: 0x00223428
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

		// Token: 0x06007006 RID: 28678 RVA: 0x0004B8A8 File Offset: 0x00049AA8
		public static bool IsFactionLeader(Pawn pawn)
		{
			return PawnUtility.GetFactionLeaderFaction(pawn) != null;
		}

		// Token: 0x06007007 RID: 28679 RVA: 0x0022526C File Offset: 0x0022346C
		public static bool IsInteractionBlocked(this Pawn pawn, InteractionDef interaction, bool isInitiator, bool isRandom)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
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

		// Token: 0x06007008 RID: 28680 RVA: 0x002252B8 File Offset: 0x002234B8
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

		// Token: 0x06007009 RID: 28681 RVA: 0x0004B8B3 File Offset: 0x00049AB3
		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return (pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveDropPodInfo>(pawn)) || ThingOwnerUtility.AnyParentIs<TravelingTransportPods>(pawn);
		}

		// Token: 0x0600700A RID: 28682 RVA: 0x0004B8CD File Offset: 0x00049ACD
		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is Settlement_TraderTracker;
		}

		// Token: 0x0600700B RID: 28683 RVA: 0x00225300 File Offset: 0x00223500
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

		// Token: 0x0600700C RID: 28684 RVA: 0x0004B8DD File Offset: 0x00049ADD
		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Pawn x) => Find.GameInitData.startingAndOptionalPawns.Contains(x)))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x00225340 File Offset: 0x00223540
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

		// Token: 0x0600700E RID: 28686 RVA: 0x002253D4 File Offset: 0x002235D4
		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false)
		{
			TraverseParms tp = passDoors ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false) : TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
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

		// Token: 0x0600700F RID: 28687 RVA: 0x00225454 File Offset: 0x00223654
		public static bool WillSoonHaveBasicNeed(Pawn p)
		{
			return p.needs != null && ((p.needs.rest != null && p.needs.rest.CurLevel < 0.33f) || (p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + 0.05f));
		}

		// Token: 0x06007010 RID: 28688 RVA: 0x0004B916 File Offset: 0x00049B16
		public static float AnimalFilthChancePerCell(ThingDef def, float bodySize)
		{
			return bodySize * 0.00125f * (1f - def.race.petness);
		}

		// Token: 0x06007011 RID: 28689 RVA: 0x0004B931 File Offset: 0x00049B31
		public static float HumanFilthChancePerCell(ThingDef def, float bodySize)
		{
			return bodySize * 0.00125f * 4f;
		}

		// Token: 0x06007012 RID: 28690 RVA: 0x002254CC File Offset: 0x002236CC
		public static bool CanCasuallyInteractNow(this Pawn p, bool twoWayInteraction = false)
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
			if (!p.Awake())
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

		// Token: 0x06007013 RID: 28691 RVA: 0x0004B940 File Offset: 0x00049B40
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

		// Token: 0x06007014 RID: 28692 RVA: 0x0004B950 File Offset: 0x00049B50
		public static bool InValidState(Pawn p)
		{
			return p.health != null && (p.Dead || (p.stances != null && p.mindState != null && p.needs != null && p.ageTracker != null));
		}

		// Token: 0x06007015 RID: 28693 RVA: 0x0022553C File Offset: 0x0022373C
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
				if (p.jobs == null)
				{
					return PawnPosture.Standing;
				}
				return p.jobs.posture;
			}
		}

		// Token: 0x06007016 RID: 28694 RVA: 0x00225598 File Offset: 0x00223798
		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639, false);
			}
			Job job = JobMaker.MakeJob(maintainPosture ? JobDefOf.Wait_MaintainPosture : JobDefOf.Wait, faceTarget);
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false);
		}

		// Token: 0x06007017 RID: 28695 RVA: 0x002255F8 File Offset: 0x002237F8
		public static void GiveNameBecauseOfNuzzle(Pawn namer, Pawn namee)
		{
			string value = (namee.Name == null) ? namee.LabelIndefinite() : namee.Name.ToStringFull;
			namee.Name = PawnBioAndNameGenerator.GeneratePawnName(namee, NameStyle.Full, null);
			if (namer.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageNuzzledPawnGaveNameTo".Translate(namer.Named("NAMER"), value, namee.Name.ToStringFull, namee.Named("NAMEE")), namee, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06007018 RID: 28696 RVA: 0x00225688 File Offset: 0x00223888
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

		// Token: 0x06007019 RID: 28697 RVA: 0x002256E4 File Offset: 0x002238E4
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

		// Token: 0x0600701A RID: 28698 RVA: 0x0022573C File Offset: 0x0022393C
		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			if (pawn.needs != null && pawn.needs.food != null)
			{
				switch (pawn.needs.food.CurCategory)
				{
				case HungerCategory.Fed:
					return 1f;
				case HungerCategory.Hungry:
					return 0.666f;
				case HungerCategory.UrgentlyHungry:
					return 0.333f;
				case HungerCategory.Starving:
					return 0f;
				}
			}
			return 1f;
		}

		// Token: 0x0600701B RID: 28699 RVA: 0x002257A4 File Offset: 0x002239A4
		public static bool FertileMateTarget(Pawn male, Pawn female)
		{
			if (female.gender != Gender.Female || !female.ageTracker.CurLifeStage.reproductive)
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

		// Token: 0x0600701C RID: 28700 RVA: 0x002257FC File Offset: 0x002239FC
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

		// Token: 0x0600701D RID: 28701 RVA: 0x00225884 File Offset: 0x00223A84
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

		// Token: 0x0600701E RID: 28702 RVA: 0x002258DC File Offset: 0x00223ADC
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

		// Token: 0x0600701F RID: 28703 RVA: 0x002259AC File Offset: 0x00223BAC
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

		// Token: 0x06007020 RID: 28704 RVA: 0x0004B987 File Offset: 0x00049B87
		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return !p.Downed && !p.Dead && p.mindState.anyCloseHostilesRecently;
		}

		// Token: 0x06007021 RID: 28705 RVA: 0x0004B9AB File Offset: 0x00049BAB
		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns, forPathFinder) != null;
		}

		// Token: 0x06007022 RID: 28706 RVA: 0x00225A40 File Offset: 0x00223C40
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

		// Token: 0x06007023 RID: 28707 RVA: 0x00225B98 File Offset: 0x00223D98
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

		// Token: 0x06007024 RID: 28708 RVA: 0x00225BE8 File Offset: 0x00223DE8
		public static bool KnownDangerAt(IntVec3 c, Map map, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.IsDangerousFor(forPawn);
		}

		// Token: 0x06007025 RID: 28709 RVA: 0x00225C0C File Offset: 0x00223E0C
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

		// Token: 0x06007026 RID: 28710 RVA: 0x0004B9BB File Offset: 0x00049BBB
		public static bool ShouldGetThoughtAbout(Pawn pawn, Pawn subject)
		{
			return pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn());
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x0004B9E0 File Offset: 0x00049BE0
		public static bool IsTeetotaler(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) < 0;
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x0004BA04 File Offset: 0x00049C04
		public static bool IsProsthophobe(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.BodyPurist);
		}

		// Token: 0x06007029 RID: 28713 RVA: 0x0004BA25 File Offset: 0x00049C25
		public static bool IsPrisonerInPrisonCell(this Pawn pawn)
		{
			return pawn.IsPrisoner && pawn.Spawned && pawn.Position.IsInPrisonCell(pawn.Map);
		}

		// Token: 0x0600702A RID: 28714 RVA: 0x00225CD4 File Offset: 0x00223ED4
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
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd);
		}

		// Token: 0x0600702B RID: 28715 RVA: 0x00225E5C File Offset: 0x0022405C
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

		// Token: 0x0600702C RID: 28716 RVA: 0x0004BA4A File Offset: 0x00049C4A
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x00225FC4 File Offset: 0x002241C4
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix, Color color)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			for (int i = 0; i < PawnUtility.tmpPawnKindsStr.Count; i++)
			{
				PawnUtility.tmpPawnKindsStr[i] = PawnUtility.tmpPawnKindsStr[i].Colorize(color);
			}
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x0004BA5E File Offset: 0x00049C5E
		public static string PawnKindsToCommaList(IEnumerable<PawnKindDef> pawnKinds, bool useAnd = false)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd);
		}

		// Token: 0x0600702F RID: 28719 RVA: 0x0004BA72 File Offset: 0x00049C72
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
			{
				return pawn.mindState.duty.locomotion;
			}
			return secondPriority;
		}

		// Token: 0x06007030 RID: 28720 RVA: 0x00226014 File Offset: 0x00224214
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			if (locomotionUrgency != LocomotionUrgency.None)
			{
				return locomotionUrgency;
			}
			return thirdPriority;
		}

		// Token: 0x06007031 RID: 28721 RVA: 0x0004BAAD File Offset: 0x00049CAD
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.maxDanger != Danger.Unspecified)
			{
				return pawn.mindState.duty.maxDanger;
			}
			return secondPriority;
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x00226030 File Offset: 0x00224230
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			if (danger != Danger.Unspecified)
			{
				return danger;
			}
			return thirdPriority;
		}

		// Token: 0x06007033 RID: 28723 RVA: 0x0022604C File Offset: 0x0022424C
		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.Wait_Combat || pawn.CurJob.def == JobDefOf.PredatorHunt);
		}

		// Token: 0x06007034 RID: 28724 RVA: 0x0004BAE8 File Offset: 0x00049CE8
		public static Hediff_Psylink GetMainPsylinkSource(this Pawn pawn)
		{
			return (Hediff_Psylink)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
		}

		// Token: 0x06007035 RID: 28725 RVA: 0x002260B0 File Offset: 0x002242B0
		public static int GetPsylinkLevel(this Pawn pawn)
		{
			Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
			if (mainPsylinkSource == null)
			{
				return 0;
			}
			return mainPsylinkSource.level;
		}

		// Token: 0x06007036 RID: 28726 RVA: 0x0004BB05 File Offset: 0x00049D05
		public static int GetMaxPsylinkLevel(this Pawn pawn)
		{
			return (int)HediffDefOf.PsychicAmplifier.maxSeverity;
		}

		// Token: 0x06007037 RID: 28727 RVA: 0x002260D0 File Offset: 0x002242D0
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

		// Token: 0x06007038 RID: 28728 RVA: 0x0022614C File Offset: 0x0022434C
		public static int GetMaxPsylinkLevelByTitle(this Pawn pawn)
		{
			RoyalTitle maxPsylinkLevelTitle = pawn.GetMaxPsylinkLevelTitle();
			if (maxPsylinkLevelTitle == null)
			{
				return 0;
			}
			return maxPsylinkLevelTitle.def.maxPsylinkLevel;
		}

		// Token: 0x06007039 RID: 28729 RVA: 0x00226170 File Offset: 0x00224370
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

		// Token: 0x0600703A RID: 28730 RVA: 0x002261EC File Offset: 0x002243EC
		public static float RecruitDifficulty(this Pawn pawn, Faction recruiterFaction)
		{
			float num = pawn.kindDef.baseRecruitDifficulty;
			Rand.PushState();
			Rand.Seed = pawn.HashOffset();
			num += Rand.Gaussian(0f, 0.15f);
			Rand.PopState();
			if (pawn.Faction != null)
			{
				int num2 = Mathf.Min((int)pawn.Faction.def.techLevel, 4);
				int num3 = Mathf.Min((int)recruiterFaction.def.techLevel, 4);
				int num4 = Mathf.Abs(num2 - num3);
				num += (float)num4 * 0.16f;
			}
			if (pawn.royalty != null)
			{
				RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
				if (mostSeniorTitle != null)
				{
					num += mostSeniorTitle.def.recruitmentDifficultyOffset;
				}
			}
			return Mathf.Clamp(num, 0.1f, 0.99f);
		}

		// Token: 0x0600703B RID: 28731 RVA: 0x002262A8 File Offset: 0x002244A8
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
									pawn.needs.mood.thoughts.memories.TryGainMemory(thought, pawn2);
								}
							}
							continue;
						}
					}
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought, null);
				}
			}
		}

		// Token: 0x0600703C RID: 28732 RVA: 0x00226390 File Offset: 0x00224590
		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid)
			{
				return pawn.mindState.duty.focus.Cell;
			}
			return pawn.Position;
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x0004BB12 File Offset: 0x00049D12
		public static bool EverBeenColonistOrTameAnimal(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsColonistOrColonyAnimal) > 0;
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x0004BB27 File Offset: 0x00049D27
		public static bool EverBeenPrisoner(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsPrisoner) > 0;
		}

		// Token: 0x0600703F RID: 28735 RVA: 0x0004BB3C File Offset: 0x00049D3C
		public static bool EverBeenQuestLodger(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsQuestLodger) > 0;
		}

		// Token: 0x06007040 RID: 28736 RVA: 0x002263E0 File Offset: 0x002245E0
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
						DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, pawn.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.Collapse, null);
						pawn.TakeDamage(damageInfo);
						if (!pawn.Dead)
						{
							pawn.Kill(new DamageInfo?(damageInfo), null);
						}
					}
				}
			}
		}

		// Token: 0x06007041 RID: 28737 RVA: 0x002264B4 File Offset: 0x002246B4
		public static float GetManhunterOnDamageChance(Pawn pawn, float distance, Thing instigator)
		{
			float num = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			num *= GenMath.LerpDoubleClamped(1f, 30f, 3f, 1f, distance);
			if (instigator != null)
			{
				num *= 1f - instigator.GetStatValue(StatDefOf.HuntingStealth, true);
			}
			return num;
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x0004BB51 File Offset: 0x00049D51
		public static float GetManhunterOnDamageChance(Pawn pawn, Thing instigator = null)
		{
			if (instigator != null)
			{
				return PawnUtility.GetManhunterOnDamageChance(pawn, pawn.Position.DistanceTo(instigator.Position), instigator);
			}
			return PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x0004BB7A File Offset: 0x00049D7A
		public static float GetManhunterOnDamageChance(PawnKindDef kind)
		{
			return kind.RaceProps.manhunterOnDamageChance * Find.Storyteller.difficultyValues.manhunterChanceOnDamageFactor;
		}

		// Token: 0x06007044 RID: 28740 RVA: 0x0004BB97 File Offset: 0x00049D97
		public static float GetManhunterOnDamageChance(RaceProperties race)
		{
			return race.manhunterOnDamageChance * Find.Storyteller.difficultyValues.manhunterChanceOnDamageFactor;
		}

		// Token: 0x04004A04 RID: 18948
		private const float HumanFilthFactor = 4f;

		// Token: 0x04004A05 RID: 18949
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04004A06 RID: 18950
		private static List<string> tmpPawnKindsStr = new List<string>();

		// Token: 0x04004A07 RID: 18951
		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		// Token: 0x04004A08 RID: 18952
		private static List<PawnKindDef> tmpPawnKinds = new List<PawnKindDef>();

		// Token: 0x04004A09 RID: 18953
		private const float RecruitDifficultyMin = 0.1f;

		// Token: 0x04004A0A RID: 18954
		private const float RecruitDifficultyMax = 0.99f;

		// Token: 0x04004A0B RID: 18955
		private const float RecruitDifficultyGaussianWidthFactor = 0.15f;

		// Token: 0x04004A0C RID: 18956
		private const float RecruitDifficultyOffsetPerTechDiff = 0.16f;

		// Token: 0x04004A0D RID: 18957
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
