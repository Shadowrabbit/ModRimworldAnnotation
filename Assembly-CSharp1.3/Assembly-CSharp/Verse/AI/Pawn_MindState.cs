using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200060E RID: 1550
	public class Pawn_MindState : IExposable
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x0010C0CC File Offset: 0x0010A2CC
		public bool InRoamingCooldown
		{
			get
			{
				return this.lastStartRoamCooldownTick != null && this.lastStartRoamCooldownTick.Value + 30000 > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x0010C0FA File Offset: 0x0010A2FA
		public bool AvailableForGoodwillReward
		{
			get
			{
				return Find.TickManager.TicksGame >= this.noAidRelationsGainUntilTick;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x0010C111 File Offset: 0x0010A311
		// (set) Token: 0x06002CB4 RID: 11444 RVA: 0x0010C119 File Offset: 0x0010A319
		public bool Active
		{
			get
			{
				return this.activeInt;
			}
			set
			{
				if (value != this.activeInt)
				{
					this.activeInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
					}
				}
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x0010C153 File Offset: 0x0010A353
		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x0010C17C File Offset: 0x0010A37C
		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.Downed && this.meleeThreat.Awake() && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map, false, null, 0, 0);
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x0010C23D File Offset: 0x0010A43D
		// (set) Token: 0x06002CB8 RID: 11448 RVA: 0x0010C245 File Offset: 0x0010A445
		public bool WildManEverReachedOutside
		{
			get
			{
				return this.wildManEverReachedOutsideInt;
			}
			set
			{
				if (this.wildManEverReachedOutsideInt == value)
				{
					return;
				}
				this.wildManEverReachedOutsideInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002CB9 RID: 11449 RVA: 0x0010C263 File Offset: 0x0010A463
		// (set) Token: 0x06002CBA RID: 11450 RVA: 0x0010C26B File Offset: 0x0010A46B
		public bool WillJoinColonyIfRescued
		{
			get
			{
				return this.willJoinColonyIfRescuedInt;
			}
			set
			{
				if (this.willJoinColonyIfRescuedInt == value)
				{
					return;
				}
				this.willJoinColonyIfRescuedInt = value;
				if (this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002CBB RID: 11451 RVA: 0x0010C2A8 File Offset: 0x0010A4A8
		public bool AnythingPreventsJoiningColonyIfRescued
		{
			get
			{
				return this.pawn.Faction == Faction.OfPlayer || (this.pawn.IsPrisoner && !this.pawn.HostFaction.HostileTo(Faction.OfPlayer)) || (!this.pawn.IsPrisoner && this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer) && !this.pawn.Downed);
			}
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x0010C330 File Offset: 0x0010A530
		public Pawn_MindState()
		{
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x0010C444 File Offset: 0x0010A644
		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x0010C590 File Offset: 0x0010A790
		public void Reset(bool clearInspiration = false, bool clearMentalState = true)
		{
			if (clearMentalState)
			{
				this.mentalStateHandler.Reset();
				this.mentalBreaker.Reset();
			}
			if (clearInspiration)
			{
				this.inspirationHandler.Reset();
			}
			this.activeInt = true;
			this.lastJobTag = JobTag.Misc;
			this.lastIngestTick = -99999;
			this.nextApparelOptimizeTick = -99999;
			this.canFleeIndividual = true;
			this.exitMapAfterTick = -99999;
			this.lastDisturbanceTick = -99999;
			this.forcedGotoPosition = IntVec3.Invalid;
			this.knownExploder = null;
			this.wantsToTradeWithColony = false;
			this.lastMannedThing = null;
			this.canLovinTick = -99999;
			this.canSleepTick = -99999;
			this.meleeThreat = null;
			this.lastMeleeThreatHarmTick = -99999;
			this.lastEngageTargetTick = -99999;
			this.lastAttackTargetTick = -99999;
			this.lastAttackedTarget = LocalTargetInfo.Invalid;
			this.enemyTarget = null;
			this.breachingTarget = null;
			this.duty = null;
			this.thinkData.Clear();
			this.lastAssignedInteractTime = -99999;
			this.interactionsToday = 0;
			this.lastInventoryRawFoodUseTick = 0;
			this.priorityWork.Clear();
			this.nextMoveOrderIsWait = true;
			this.lastTakeCombatEnhancingDrugTick = -99999;
			this.lastHarmTick = -99999;
			this.anyCloseHostilesRecently = false;
			this.WillJoinColonyIfRescued = false;
			this.WildManEverReachedOutside = false;
			this.timesGuestTendedToByPlayer = 0;
			this.lastSelfTendTick = -99999;
			this.spawnedByInfestationThingComp = false;
			this.lastPredatorHuntingPlayerNotificationTick = -99999;
			this.lastSlaveSuppressedTick = -99999;
			this.lastTakeRecreationalDrugTick = -60000;
			this.lastStartRoamCooldownTick = null;
			this.nextInventoryStockTick = -99999;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x0010C738 File Offset: 0x0010A938
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.meleeThreat, "meleeThreat", false);
			Scribe_References.Look<Thing>(ref this.enemyTarget, "enemyTarget", false);
			Scribe_References.Look<Thing>(ref this.knownExploder, "knownExploder", false);
			Scribe_References.Look<Thing>(ref this.lastMannedThing, "lastMannedThing", false);
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Collections.Look<int, int>(ref this.thinkData, "thinkData", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<bool>(ref this.activeInt, "active", true, false);
			Scribe_Values.Look<JobTag>(ref this.lastJobTag, "lastJobTag", JobTag.Misc, false);
			Scribe_Values.Look<int>(ref this.lastIngestTick, "lastIngestTick", -99999, false);
			Scribe_Values.Look<int>(ref this.nextApparelOptimizeTick, "nextApparelOptimizeTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastEngageTargetTick, "lastEngageTargetTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canFleeIndividual, "canFleeIndividual", false, false);
			Scribe_Values.Look<int>(ref this.exitMapAfterTick, "exitMapAfterTick", -99999, false);
			Scribe_Values.Look<IntVec3>(ref this.forcedGotoPosition, "forcedGotoPosition", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.lastMeleeThreatHarmTick, "lastMeleeThreatHarmTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAssignedInteractTime, "lastAssignedInteractTime", -99999, false);
			Scribe_Values.Look<int>(ref this.interactionsToday, "interactionsToday", 0, false);
			Scribe_Values.Look<int>(ref this.lastInventoryRawFoodUseTick, "lastInventoryRawFoodUseTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastDisturbanceTick, "lastDisturbanceTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.wantsToTradeWithColony, "wantsToTradeWithColony", false, false);
			Scribe_Values.Look<int>(ref this.canLovinTick, "canLovinTick", -99999, false);
			Scribe_Values.Look<int>(ref this.canSleepTick, "canSleepTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.nextMoveOrderIsWait, "nextMoveOrderIsWait", true, false);
			Scribe_Values.Look<int>(ref this.lastTakeCombatEnhancingDrugTick, "lastTakeCombatEnhancingDrugTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastTakeRecreationalDrugTick, "lastTakeRecreationalDrugTick", -60000, false);
			Scribe_Values.Look<int>(ref this.lastHarmTick, "lastHarmTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.anyCloseHostilesRecently, "anyCloseHostilesRecently", false, false);
			Scribe_Deep.Look<PawnDuty>(ref this.duty, "duty", Array.Empty<object>());
			Scribe_Deep.Look<MentalStateHandler>(ref this.mentalStateHandler, "mentalStateHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalBreaker>(ref this.mentalBreaker, "mentalBreaker", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<InspirationHandler>(ref this.inspirationHandler, "inspirationHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PriorityWork>(ref this.priorityWork, "priorityWork", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.applyBedThoughtsTick, "applyBedThoughtsTick", 0, false);
			Scribe_Values.Look<int>(ref this.applyThroneThoughtsTick, "applyThroneThoughtsTick", 0, false);
			Scribe_Values.Look<bool>(ref this.applyBedThoughtsOnLeave, "applyBedThoughtsOnLeave", false, false);
			Scribe_Values.Look<bool>(ref this.willJoinColonyIfRescuedInt, "willJoinColonyIfRescued", false, false);
			Scribe_Values.Look<bool>(ref this.wildManEverReachedOutsideInt, "wildManEverReachedOutside", false, false);
			Scribe_Values.Look<int>(ref this.timesGuestTendedToByPlayer, "timesGuestTendedToByPlayer", 0, false);
			Scribe_Values.Look<int>(ref this.noAidRelationsGainUntilTick, "noAidRelationsGainUntilTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastSelfTendTick, "lastSelfTendTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
			Scribe_Values.Look<int>(ref this.lastPredatorHuntingPlayerNotificationTick, "lastPredatorHuntingPlayerNotificationTick", -99999, false);
			Scribe_Deep.Look<BreachingTargetData>(ref this.breachingTarget, "breachingTarget", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastSlaveSuppressedTick, "lastSlaveSuppressedTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastHumanMeatIngestedTick, "lastHumanMeatIngestedTick", -99999, false);
			Scribe_Values.Look<int?>(ref this.lastStartRoamCooldownTick, "lastStartRoamCooldownTick", null, false);
			Scribe_Values.Look<int>(ref this.nextInventoryStockTick, "nextInventoryStockTick", -99999, false);
			Scribe_Defs.Look<ThingDef>(ref this.lastBedDefSleptIn, "lastBedDefSleptIn");
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x0010CB38 File Offset: 0x0010AD38
		public void MindStateTick()
		{
			if (this.wantsToTradeWithColony)
			{
				TradeUtility.CheckInteractWithTradersTeachOpportunity(this.pawn);
			}
			if (this.meleeThreat != null && !this.MeleeThreatStillThreat)
			{
				this.meleeThreat = null;
			}
			this.mentalStateHandler.MentalStateHandlerTick();
			this.mentalBreaker.MentalBreakerTick();
			this.inspirationHandler.InspirationHandlerTick();
			if (!this.pawn.GetPosture().Laying())
			{
				this.applyBedThoughtsTick = 0;
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				if (this.pawn.Spawned)
				{
					int regionsToScan = this.anyCloseHostilesRecently ? 24 : 18;
					this.anyCloseHostilesRecently = PawnUtility.EnemiesAreNearby(this.pawn, regionsToScan, true);
				}
				else
				{
					this.anyCloseHostilesRecently = false;
				}
			}
			if (this.WillJoinColonyIfRescued && this.AnythingPreventsJoiningColonyIfRescued)
			{
				this.WillJoinColonyIfRescued = false;
			}
			if (this.pawn.Spawned && this.pawn.IsWildMan() && !this.WildManEverReachedOutside && this.pawn.GetDistrict(RegionType.Set_Passable) != null && this.pawn.GetDistrict(RegionType.Set_Passable).TouchesMapEdge)
			{
				this.WildManEverReachedOutside = true;
			}
			if (Find.TickManager.TicksGame % 123 == 0 && this.pawn.Spawned && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain.traversedThought != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(terrain.traversedThought, null);
				}
				WeatherDef curWeatherLerped = this.pawn.Map.weatherManager.CurWeatherLerped;
				if (curWeatherLerped.exposedThought != null && !this.pawn.Position.Roofed(this.pawn.Map))
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(curWeatherLerped.exposedThought, null);
				}
			}
			if (GenLocalDate.DayTick(this.pawn) == 0)
			{
				this.interactionsToday = 0;
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x0010CD60 File Offset: 0x0010AF60
		public void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.WillJoinColonyIfRescued = false;
			if (this.AnythingPreventsJoiningColonyIfRescued)
			{
				return;
			}
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, false);
			if (this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMeByOfferingHelp, by, null);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), "LetterRescueQuestFinished".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x0010CE53 File Offset: 0x0010B053
		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x0010CE60 File Offset: 0x0010B060
		public void SetupLastHumanMeatTick()
		{
			if (this.pawn.Ideo != null && this.pawn.Ideo.IdeoCausesHumanMeatCravings())
			{
				this.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				this.lastHumanMeatIngestedTick -= Pawn_MindState.LastHumanMeatEatenTicksRange.RandomInRange;
			}
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x0010CEB6 File Offset: 0x0010B0B6
		public IEnumerable<Gizmo> GetGizmos()
		{
			IEnumerator<Gizmo> enumerator;
			if (this.pawn.IsColonistPlayerControlled)
			{
				foreach (Gizmo gizmo in this.priorityWork.GetGizmos())
				{
					yield return gizmo;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo2 in CaravanFormingUtility.GetGizmos(this.pawn))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x0010CEC6 File Offset: 0x0010B0C6
		public void SetNoAidRelationsGainUntilTick(int tick)
		{
			if (tick > this.noAidRelationsGainUntilTick)
			{
				this.noAidRelationsGainUntilTick = tick;
			}
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x0010CED8 File Offset: 0x0010B0D8
		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x0010CEEC File Offset: 0x0010B0EC
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			this.mentalStateHandler.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				this.lastHarmTick = Find.TickManager.TicksGame;
				if (this.pawn.Spawned)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (!this.mentalStateHandler.InMentalState && dinfo.Instigator != null && (pawn != null || dinfo.Instigator is Building_Turret) && dinfo.Instigator.Faction != null && (dinfo.Instigator.Faction.def.humanlikeFaction || (pawn != null && pawn.def.race.intelligence >= Intelligence.ToolUser)) && this.pawn.Faction == null && (this.pawn.RaceProps.Animal || this.pawn.IsWildMan()) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.PredatorHunt || dinfo.Instigator != ((JobDriver_PredatorHunt)this.pawn.jobs.curDriver).Prey) && Rand.Chance(PawnUtility.GetManhunterOnDamageChance(this.pawn, dinfo.Instigator)))
					{
						this.StartManhunterBecauseOfPawnAction(pawn, "AnimalManhunterFromDamage", true);
					}
					else if (dinfo.Instigator != null && dinfo.Def.makesAnimalsFlee && Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
					{
						this.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
					}
				}
				if (this.pawn.GetPosture() != PawnPosture.Standing)
				{
					this.lastDisturbanceTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x0010D0AC File Offset: 0x0010B2AC
		public void Notify_ClamorImpact(Thing instigator)
		{
			this.canSleepTick = Find.TickManager.TicksGame + 1000;
			if (this.pawn.RaceProps.Animal && instigator is Projectile && ((Projectile)instigator).AnimalsFleeImpact && (this.pawn.playerSettings == null || this.pawn.playerSettings.Master == null) && Rand.Chance(0.4f) && Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(((Projectile)instigator).Launcher);
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x0010D13F File Offset: 0x0010B33F
		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x0010D151 File Offset: 0x0010B351
		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x0010D16C File Offset: 0x0010B36C
		internal bool CheckStartMentalStateBecauseRecruitAttempted(Pawn tamer)
		{
			if (!this.pawn.RaceProps.Animal && (!this.pawn.IsWildMan() || this.pawn.IsPrisoner))
			{
				return false;
			}
			if (!this.mentalStateHandler.InMentalState && this.pawn.Faction == null && Rand.Chance(this.pawn.RaceProps.manhunterOnTameFailChance))
			{
				this.StartManhunterBecauseOfPawnAction(tamer, "AnimalManhunterFromTaming", false);
				return true;
			}
			return false;
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x0010D1E8 File Offset: 0x0010B3E8
		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x0010D214 File Offset: 0x0010B414
		public void Notify_Explosion(Explosion explosion)
		{
			if (this.pawn.Faction != null)
			{
				return;
			}
			if (explosion.radius < 3.5f || !this.pawn.Position.InHorDistOf(explosion.Position, explosion.radius + 7f))
			{
				return;
			}
			if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(explosion);
			}
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x0010D278 File Offset: 0x0010B478
		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.WildManEverReachedOutside = false;
			}
			this.ResetLastDisturbanceTick();
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x0010D294 File Offset: 0x0010B494
		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x0010D2A6 File Offset: 0x0010B4A6
		public void Notify_PredatorHuntingPlayerNotification()
		{
			this.lastPredatorHuntingPlayerNotificationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x0010D2B8 File Offset: 0x0010B4B8
		private IEnumerable<Pawn> GetPackmates(Pawn pawn, float radius)
		{
			District pawnRoom = pawn.GetDistrict(RegionType.Set_Passable);
			List<Pawn> raceMates = pawn.Map.mapPawns.AllPawnsSpawned;
			int num;
			for (int i = 0; i < raceMates.Count; i = num + 1)
			{
				if (pawn != raceMates[i] && raceMates[i].def == pawn.def && raceMates[i].Faction == pawn.Faction && raceMates[i].Position.InHorDistOf(pawn.Position, radius) && raceMates[i].GetDistrict(RegionType.Set_Passable) == pawnRoom)
				{
					yield return raceMates[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x0010D2D0 File Offset: 0x0010B4D0
		private void StartManhunterBecauseOfPawnAction(Pawn instigator, string letterTextKey, bool causedByDamage = false)
		{
			if (!this.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false))
			{
				return;
			}
			string text = letterTextKey.Translate(this.pawn.Label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			GlobalTargetInfo target = this.pawn;
			float num = 0.5f;
			if (causedByDamage)
			{
				num *= PawnUtility.GetManhunterChanceFactorForInstigator(instigator);
			}
			int num2 = 1;
			if (Find.Storyteller.difficulty.allowBigThreats && Rand.Value < num)
			{
				using (IEnumerator<Pawn> enumerator = this.GetPackmates(this.pawn, 24f).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, causedByDamage, false))
						{
							num2++;
						}
					}
				}
				if (num2 > 1)
				{
					target = new TargetInfo(this.pawn.Position, this.pawn.Map, false);
					text += "\n\n";
					text += "AnimalManhunterOthers".Translate(this.pawn.kindDef.GetLabelPlural(-1), this.pawn);
				}
			}
			string value = this.pawn.RaceProps.Animal ? this.pawn.Label : this.pawn.def.label;
			string str = "LetterLabelAnimalManhunterRevenge".Translate(value).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(str, text, (num2 == 1) ? LetterDefOf.ThreatSmall : LetterDefOf.ThreatBig, target, null, null, null, null);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x0010D4CC File Offset: 0x0010B6CC
		private static bool CanStartFleeingBecauseOfPawnAction(Pawn p)
		{
			return p.RaceProps.Animal && !p.InMentalState && !p.IsFighting() && !p.Downed && !p.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p) && (p.jobs.curJob == null || p.jobs.curJob.def != JobDefOf.Flee || p.jobs.curJob.startTick != Find.TickManager.TicksGame);
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x0010D558 File Offset: 0x0010B758
		public void StartFleeingBecauseOfPawnAction(Thing instigator)
		{
			List<Thing> threats = new List<Thing>
			{
				instigator
			};
			IntVec3 fleeDest = CellFinderLoose.GetFleeDest(this.pawn, threats, this.pawn.Position.DistanceTo(instigator.Position) + 28f);
			if (fleeDest != this.pawn.Position)
			{
				Vector3 lhs = (fleeDest - this.pawn.Position).ToVector3();
				Vector3 rhs = (this.pawn.Map.Center - this.pawn.Position).ToVector3();
				bool flag = Vector3.Dot(lhs, rhs) < 0f;
				IntVec3 c = default(IntVec3);
				if (this.pawn.RaceProps.Animal && this.pawn.Faction == null && flag && Rand.Chance(0.5f) && CellFinderLoose.GetFleeExitPosition(this.pawn, 10f, out c))
				{
					Job job = JobMaker.MakeJob(JobDefOf.Flee, c, instigator);
					job.exitMapOnArrival = true;
					this.pawn.jobs.StartJob(job, JobCondition.InterruptOptional, null, false, true, null, null, false, false);
				}
				else
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false);
				}
			}
			if (this.pawn.RaceProps.herdAnimal && Rand.Chance(0.1f))
			{
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(pawn))
					{
						IntVec3 fleeDest2 = CellFinderLoose.GetFleeDest(pawn, threats, pawn.Position.DistanceTo(instigator.Position) + 28f);
						if (fleeDest2 != pawn.Position)
						{
							pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest2, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false);
						}
					}
				}
			}
		}

		// Token: 0x04001B66 RID: 7014
		public Pawn pawn;

		// Token: 0x04001B67 RID: 7015
		public MentalStateHandler mentalStateHandler;

		// Token: 0x04001B68 RID: 7016
		public MentalBreaker mentalBreaker;

		// Token: 0x04001B69 RID: 7017
		public InspirationHandler inspirationHandler;

		// Token: 0x04001B6A RID: 7018
		public PriorityWork priorityWork;

		// Token: 0x04001B6B RID: 7019
		private bool activeInt = true;

		// Token: 0x04001B6C RID: 7020
		public JobTag lastJobTag;

		// Token: 0x04001B6D RID: 7021
		public int lastIngestTick = -99999;

		// Token: 0x04001B6E RID: 7022
		public int nextApparelOptimizeTick = -99999;

		// Token: 0x04001B6F RID: 7023
		public bool canFleeIndividual = true;

		// Token: 0x04001B70 RID: 7024
		public int exitMapAfterTick = -99999;

		// Token: 0x04001B71 RID: 7025
		public int lastDisturbanceTick = -99999;

		// Token: 0x04001B72 RID: 7026
		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		// Token: 0x04001B73 RID: 7027
		public Thing knownExploder;

		// Token: 0x04001B74 RID: 7028
		public bool wantsToTradeWithColony;

		// Token: 0x04001B75 RID: 7029
		public Thing lastMannedThing;

		// Token: 0x04001B76 RID: 7030
		public int canLovinTick = -99999;

		// Token: 0x04001B77 RID: 7031
		public int canSleepTick = -99999;

		// Token: 0x04001B78 RID: 7032
		public Pawn meleeThreat;

		// Token: 0x04001B79 RID: 7033
		public int lastMeleeThreatHarmTick = -99999;

		// Token: 0x04001B7A RID: 7034
		public int lastEngageTargetTick = -99999;

		// Token: 0x04001B7B RID: 7035
		public int lastAttackTargetTick = -99999;

		// Token: 0x04001B7C RID: 7036
		public LocalTargetInfo lastAttackedTarget;

		// Token: 0x04001B7D RID: 7037
		public Thing enemyTarget;

		// Token: 0x04001B7E RID: 7038
		public BreachingTargetData breachingTarget;

		// Token: 0x04001B7F RID: 7039
		public PawnDuty duty;

		// Token: 0x04001B80 RID: 7040
		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		// Token: 0x04001B81 RID: 7041
		public int lastAssignedInteractTime = -99999;

		// Token: 0x04001B82 RID: 7042
		public int interactionsToday;

		// Token: 0x04001B83 RID: 7043
		public int lastInventoryRawFoodUseTick;

		// Token: 0x04001B84 RID: 7044
		public bool nextMoveOrderIsWait;

		// Token: 0x04001B85 RID: 7045
		public int lastTakeCombatEnhancingDrugTick = -99999;

		// Token: 0x04001B86 RID: 7046
		public int lastTakeRecreationalDrugTick = -60000;

		// Token: 0x04001B87 RID: 7047
		public int lastHarmTick = -99999;

		// Token: 0x04001B88 RID: 7048
		public int noAidRelationsGainUntilTick = -99999;

		// Token: 0x04001B89 RID: 7049
		public bool anyCloseHostilesRecently;

		// Token: 0x04001B8A RID: 7050
		public int applyBedThoughtsTick;

		// Token: 0x04001B8B RID: 7051
		public int applyThroneThoughtsTick;

		// Token: 0x04001B8C RID: 7052
		public bool applyBedThoughtsOnLeave;

		// Token: 0x04001B8D RID: 7053
		public bool willJoinColonyIfRescuedInt;

		// Token: 0x04001B8E RID: 7054
		private bool wildManEverReachedOutsideInt;

		// Token: 0x04001B8F RID: 7055
		public int timesGuestTendedToByPlayer;

		// Token: 0x04001B90 RID: 7056
		public int lastSelfTendTick = -99999;

		// Token: 0x04001B91 RID: 7057
		public bool spawnedByInfestationThingComp;

		// Token: 0x04001B92 RID: 7058
		public int lastPredatorHuntingPlayerNotificationTick = -99999;

		// Token: 0x04001B93 RID: 7059
		public int lastSlaveSuppressedTick = -99999;

		// Token: 0x04001B94 RID: 7060
		public ThingDef lastBedDefSleptIn;

		// Token: 0x04001B95 RID: 7061
		public int lastHumanMeatIngestedTick = -99999;

		// Token: 0x04001B96 RID: 7062
		public int? lastStartRoamCooldownTick;

		// Token: 0x04001B97 RID: 7063
		public int nextInventoryStockTick = -99999;

		// Token: 0x04001B98 RID: 7064
		public float maxDistToSquadFlag = -1f;

		// Token: 0x04001B99 RID: 7065
		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		// Token: 0x04001B9A RID: 7066
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		// Token: 0x04001B9B RID: 7067
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		// Token: 0x04001B9C RID: 7068
		private const float HarmForgetDistance = 3f;

		// Token: 0x04001B9D RID: 7069
		private const int MeleeHarmForgetDelay = 400;

		// Token: 0x04001B9E RID: 7070
		private const float ClamorImpactFleeChance = 0.4f;

		// Token: 0x04001B9F RID: 7071
		private const float PawnActionFleeOffMapChance = 0.5f;

		// Token: 0x04001BA0 RID: 7072
		private const int RoamingCooldownTicks = 30000;

		// Token: 0x04001BA1 RID: 7073
		private static readonly IntRange LastHumanMeatEatenTicksRange = new IntRange(0, 60000);
	}
}
