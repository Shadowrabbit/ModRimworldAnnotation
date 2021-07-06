using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A65 RID: 2661
	public class Pawn_MindState : IExposable
	{
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x0002F85E File Offset: 0x0002DA5E
		public bool AvailableForGoodwillReward
		{
			get
			{
				return Find.TickManager.TicksGame >= this.noAidRelationsGainUntilTick;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x0002F875 File Offset: 0x0002DA75
		// (set) Token: 0x06003F6C RID: 16236 RVA: 0x0002F87D File Offset: 0x0002DA7D
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

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x0002F8B7 File Offset: 0x0002DAB7
		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003F6E RID: 16238 RVA: 0x0017EDA0 File Offset: 0x0017CFA0
		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.Downed && this.meleeThreat.Awake() && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map, false, null, 0, 0);
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003F6F RID: 16239 RVA: 0x0002F8E0 File Offset: 0x0002DAE0
		// (set) Token: 0x06003F70 RID: 16240 RVA: 0x0002F8E8 File Offset: 0x0002DAE8
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

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003F71 RID: 16241 RVA: 0x0002F906 File Offset: 0x0002DB06
		// (set) Token: 0x06003F72 RID: 16242 RVA: 0x0002F90E File Offset: 0x0002DB0E
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

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003F73 RID: 16243 RVA: 0x0017EE64 File Offset: 0x0017D064
		public bool AnythingPreventsJoiningColonyIfRescued
		{
			get
			{
				return this.pawn.Faction == Faction.OfPlayer || (this.pawn.IsPrisoner && !this.pawn.HostFaction.HostileTo(Faction.OfPlayer)) || (!this.pawn.IsPrisoner && this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer) && !this.pawn.Downed);
			}
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x0017EEEC File Offset: 0x0017D0EC
		public Pawn_MindState()
		{
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x0017EFD4 File Offset: 0x0017D1D4
		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x0017F0F4 File Offset: 0x0017D2F4
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
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x0017F268 File Offset: 0x0017D468
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
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0017F5D0 File Offset: 0x0017D7D0
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
			if (this.pawn.Spawned && this.pawn.IsWildMan() && !this.WildManEverReachedOutside && this.pawn.GetRoom(RegionType.Set_Passable) != null && this.pawn.GetRoom(RegionType.Set_Passable).TouchesMapEdge)
			{
				this.WildManEverReachedOutside = true;
			}
			if (Find.TickManager.TicksGame % 123 == 0 && this.pawn.Spawned && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain.traversedThought != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(terrain.traversedThought);
				}
				WeatherDef curWeatherLerped = this.pawn.Map.weatherManager.CurWeatherLerped;
				if (curWeatherLerped.exposedThought != null && !this.pawn.Position.Roofed(this.pawn.Map))
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(curWeatherLerped.exposedThought);
				}
			}
			if (GenLocalDate.DayTick(this.pawn) == 0)
			{
				this.interactionsToday = 0;
			}
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0017F7F4 File Offset: 0x0017D9F4
		public void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.WillJoinColonyIfRescued = false;
			if (this.AnythingPreventsJoiningColonyIfRescued)
			{
				return;
			}
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, 1f, false);
			if (this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMeByOfferingHelp, by);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), "LetterRescueQuestFinished".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0002F949 File Offset: 0x0002DB49
		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0002F956 File Offset: 0x0002DB56
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

		// Token: 0x06003F7C RID: 16252 RVA: 0x0002F966 File Offset: 0x0002DB66
		public void SetNoAidRelationsGainUntilTick(int tick)
		{
			if (tick > this.noAidRelationsGainUntilTick)
			{
				this.noAidRelationsGainUntilTick = tick;
			}
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0002F978 File Offset: 0x0002DB78
		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x0017F8EC File Offset: 0x0017DAEC
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
						this.StartManhunterBecauseOfPawnAction("AnimalManhunterFromDamage");
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

		// Token: 0x06003F7F RID: 16255 RVA: 0x0002F98A File Offset: 0x0002DB8A
		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0002F99C File Offset: 0x0002DB9C
		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0017FAA8 File Offset: 0x0017DCA8
		internal bool CheckStartMentalStateBecauseRecruitAttempted(Pawn tamer)
		{
			if (!this.pawn.RaceProps.Animal && (!this.pawn.IsWildMan() || this.pawn.IsPrisoner))
			{
				return false;
			}
			if (!this.mentalStateHandler.InMentalState && this.pawn.Faction == null && Rand.Chance(this.pawn.RaceProps.manhunterOnTameFailChance))
			{
				this.StartManhunterBecauseOfPawnAction("AnimalManhunterFromTaming");
				return true;
			}
			return false;
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0002F9B5 File Offset: 0x0002DBB5
		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0017FB24 File Offset: 0x0017DD24
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

		// Token: 0x06003F84 RID: 16260 RVA: 0x0002F9E1 File Offset: 0x0002DBE1
		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.WildManEverReachedOutside = false;
			}
			this.ResetLastDisturbanceTick();
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0002F9FD File Offset: 0x0002DBFD
		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0002FA0F File Offset: 0x0002DC0F
		public void Notify_PredatorHuntingPlayerNotification()
		{
			this.lastPredatorHuntingPlayerNotificationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0002FA21 File Offset: 0x0002DC21
		private IEnumerable<Pawn> GetPackmates(Pawn pawn, float radius)
		{
			Room pawnRoom = pawn.GetRoom(RegionType.Set_Passable);
			List<Pawn> raceMates = pawn.Map.mapPawns.AllPawnsSpawned;
			int num;
			for (int i = 0; i < raceMates.Count; i = num + 1)
			{
				if (pawn != raceMates[i] && raceMates[i].def == pawn.def && raceMates[i].Faction == pawn.Faction && raceMates[i].Position.InHorDistOf(pawn.Position, radius) && raceMates[i].GetRoom(RegionType.Set_Passable) == pawnRoom)
				{
					yield return raceMates[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0017FB88 File Offset: 0x0017DD88
		private void StartManhunterBecauseOfPawnAction(string letterTextKey)
		{
			if (!this.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
			{
				return;
			}
			string text = letterTextKey.Translate(this.pawn.Label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			GlobalTargetInfo target = this.pawn;
			int num = 1;
			if (Find.Storyteller.difficultyValues.allowBigThreats && Rand.Value < 0.5f)
			{
				using (IEnumerator<Pawn> enumerator = this.GetPackmates(this.pawn, 24f).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
						{
							num++;
						}
					}
				}
				if (num > 1)
				{
					target = new TargetInfo(this.pawn.Position, this.pawn.Map, false);
					text += "\n\n";
					text += "AnimalManhunterOthers".Translate(this.pawn.kindDef.GetLabelPlural(-1), this.pawn);
				}
			}
			string value = this.pawn.RaceProps.Animal ? this.pawn.Label : this.pawn.def.label;
			string str = "LetterLabelAnimalManhunterRevenge".Translate(value).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(str, text, (num == 1) ? LetterDefOf.ThreatSmall : LetterDefOf.ThreatBig, target, null, null, null, null);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0017FD70 File Offset: 0x0017DF70
		private static bool CanStartFleeingBecauseOfPawnAction(Pawn p)
		{
			return p.RaceProps.Animal && !p.InMentalState && !p.IsFighting() && !p.Downed && !p.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p) && (p.jobs.curJob == null || p.jobs.curJob.def != JobDefOf.Flee || p.jobs.curJob.startTick != Find.TickManager.TicksGame);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0017FDFC File Offset: 0x0017DFFC
		public void StartFleeingBecauseOfPawnAction(Thing instigator)
		{
			List<Thing> threats = new List<Thing>
			{
				instigator
			};
			IntVec3 fleeDest = CellFinderLoose.GetFleeDest(this.pawn, threats, this.pawn.Position.DistanceTo(instigator.Position) + 14f);
			if (fleeDest != this.pawn.Position)
			{
				this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false);
			}
			if (this.pawn.RaceProps.herdAnimal && Rand.Chance(0.1f))
			{
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(pawn))
					{
						IntVec3 fleeDest2 = CellFinderLoose.GetFleeDest(pawn, threats, pawn.Position.DistanceTo(instigator.Position) + 14f);
						if (fleeDest2 != pawn.Position)
						{
							pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest2, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false);
						}
					}
				}
			}
		}

		// Token: 0x04002BBB RID: 11195
		public Pawn pawn;

		// Token: 0x04002BBC RID: 11196
		public MentalStateHandler mentalStateHandler;

		// Token: 0x04002BBD RID: 11197
		public MentalBreaker mentalBreaker;

		// Token: 0x04002BBE RID: 11198
		public InspirationHandler inspirationHandler;

		// Token: 0x04002BBF RID: 11199
		public PriorityWork priorityWork;

		// Token: 0x04002BC0 RID: 11200
		private bool activeInt = true;

		// Token: 0x04002BC1 RID: 11201
		public JobTag lastJobTag;

		// Token: 0x04002BC2 RID: 11202
		public int lastIngestTick = -99999;

		// Token: 0x04002BC3 RID: 11203
		public int nextApparelOptimizeTick = -99999;

		// Token: 0x04002BC4 RID: 11204
		public bool canFleeIndividual = true;

		// Token: 0x04002BC5 RID: 11205
		public int exitMapAfterTick = -99999;

		// Token: 0x04002BC6 RID: 11206
		public int lastDisturbanceTick = -99999;

		// Token: 0x04002BC7 RID: 11207
		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		// Token: 0x04002BC8 RID: 11208
		public Thing knownExploder;

		// Token: 0x04002BC9 RID: 11209
		public bool wantsToTradeWithColony;

		// Token: 0x04002BCA RID: 11210
		public Thing lastMannedThing;

		// Token: 0x04002BCB RID: 11211
		public int canLovinTick = -99999;

		// Token: 0x04002BCC RID: 11212
		public int canSleepTick = -99999;

		// Token: 0x04002BCD RID: 11213
		public Pawn meleeThreat;

		// Token: 0x04002BCE RID: 11214
		public int lastMeleeThreatHarmTick = -99999;

		// Token: 0x04002BCF RID: 11215
		public int lastEngageTargetTick = -99999;

		// Token: 0x04002BD0 RID: 11216
		public int lastAttackTargetTick = -99999;

		// Token: 0x04002BD1 RID: 11217
		public LocalTargetInfo lastAttackedTarget;

		// Token: 0x04002BD2 RID: 11218
		public Thing enemyTarget;

		// Token: 0x04002BD3 RID: 11219
		public PawnDuty duty;

		// Token: 0x04002BD4 RID: 11220
		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		// Token: 0x04002BD5 RID: 11221
		public int lastAssignedInteractTime = -99999;

		// Token: 0x04002BD6 RID: 11222
		public int interactionsToday;

		// Token: 0x04002BD7 RID: 11223
		public int lastInventoryRawFoodUseTick;

		// Token: 0x04002BD8 RID: 11224
		public bool nextMoveOrderIsWait;

		// Token: 0x04002BD9 RID: 11225
		public int lastTakeCombatEnhancingDrugTick = -99999;

		// Token: 0x04002BDA RID: 11226
		public int lastHarmTick = -99999;

		// Token: 0x04002BDB RID: 11227
		public int noAidRelationsGainUntilTick = -99999;

		// Token: 0x04002BDC RID: 11228
		public bool anyCloseHostilesRecently;

		// Token: 0x04002BDD RID: 11229
		public int applyBedThoughtsTick;

		// Token: 0x04002BDE RID: 11230
		public int applyThroneThoughtsTick;

		// Token: 0x04002BDF RID: 11231
		public bool applyBedThoughtsOnLeave;

		// Token: 0x04002BE0 RID: 11232
		public bool willJoinColonyIfRescuedInt;

		// Token: 0x04002BE1 RID: 11233
		private bool wildManEverReachedOutsideInt;

		// Token: 0x04002BE2 RID: 11234
		public int timesGuestTendedToByPlayer;

		// Token: 0x04002BE3 RID: 11235
		public int lastSelfTendTick = -99999;

		// Token: 0x04002BE4 RID: 11236
		public bool spawnedByInfestationThingComp;

		// Token: 0x04002BE5 RID: 11237
		public int lastPredatorHuntingPlayerNotificationTick = -99999;

		// Token: 0x04002BE6 RID: 11238
		public float maxDistToSquadFlag = -1f;

		// Token: 0x04002BE7 RID: 11239
		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		// Token: 0x04002BE8 RID: 11240
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		// Token: 0x04002BE9 RID: 11241
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		// Token: 0x04002BEA RID: 11242
		private const float HarmForgetDistance = 3f;

		// Token: 0x04002BEB RID: 11243
		private const int MeleeHarmForgetDelay = 400;
	}
}
