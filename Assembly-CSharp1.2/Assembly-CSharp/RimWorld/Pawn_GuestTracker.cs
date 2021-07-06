using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020014D4 RID: 5332
	public class Pawn_GuestTracker : IExposable
	{
		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x060072D6 RID: 29398 RVA: 0x0004D3B0 File Offset: 0x0004B5B0
		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x060072D7 RID: 29399 RVA: 0x0004D3B8 File Offset: 0x0004B5B8
		public bool CanBeBroughtFood
		{
			get
			{
				return this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x060072D8 RID: 29400 RVA: 0x0004D3E3 File Offset: 0x0004B5E3
		public bool IsPrisoner
		{
			get
			{
				return this.isPrisonerInt;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x060072D9 RID: 29401 RVA: 0x0004D3EB File Offset: 0x0004B5EB
		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - 10000 && this.pawn.mindState.interactionsToday < 2;
			}
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x060072DA RID: 29402 RVA: 0x0004D424 File Offset: 0x0004B624
		// (set) Token: 0x060072DB RID: 29403 RVA: 0x0004D42C File Offset: 0x0004B62C
		public bool Released
		{
			get
			{
				return this.releasedInt;
			}
			set
			{
				if (value == this.releasedInt)
				{
					return;
				}
				this.releasedInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060072DC RID: 29404 RVA: 0x00230E20 File Offset: 0x0022F020
		public bool PrisonerIsSecure
		{
			get
			{
				if (this.Released)
				{
					return false;
				}
				if (this.pawn.HostFaction == null)
				{
					return false;
				}
				if (this.pawn.InMentalState)
				{
					return false;
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
					{
						return false;
					}
					if (PrisonBreakUtility.IsPrisonBreaking(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060072DD RID: 29405 RVA: 0x00230EA0 File Offset: 0x0022F0A0
		public bool ShouldWaitInsteadOfEscaping
		{
			get
			{
				if (!this.IsPrisoner)
				{
					return false;
				}
				Map mapHeld = this.pawn.MapHeld;
				return mapHeld != null && mapHeld.mapPawns.FreeColonistsSpawnedCount != 0 && Find.TickManager.TicksGame < this.ticksWhenAllowedToEscapeAgain;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x060072DE RID: 29406 RVA: 0x0004D44A File Offset: 0x0004B64A
		public float Resistance
		{
			get
			{
				return this.resistance;
			}
		}

		// Token: 0x060072DF RID: 29407 RVA: 0x0004D452 File Offset: 0x0004B652
		public Pawn_GuestTracker()
		{
		}

		// Token: 0x060072E0 RID: 29408 RVA: 0x0004D482 File Offset: 0x0004B682
		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060072E1 RID: 29409 RVA: 0x00230EEC File Offset: 0x0022F0EC
		public void GuestTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(2500))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
		}

		// Token: 0x060072E2 RID: 29410 RVA: 0x00230F3C File Offset: 0x0022F13C
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_Values.Look<bool>(ref this.isPrisonerInt, "prisoner", false, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
			Scribe_Values.Look<bool>(ref this.getRescuedThoughtOnUndownedBecauseOfPlayer, "getRescuedThoughtOnUndownedBecauseOfPlayer", false, false);
			Scribe_Values.Look<float>(ref this.resistance, "resistance", -1f, false);
			Scribe_Values.Look<string>(ref this.lastRecruiterName, "lastRecruiterName", null, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterNegotiationAbilityFactor, "lastRecruiterNegotiationAbilityFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasOpinionOfLastRecruiter, "hasOpinionOfLastRecruiter", false, false);
			Scribe_Values.Look<int>(ref this.lastRecruiterOpinion, "lastRecruiterOpinion", 0, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterFinalChance, "lastRecruiterFinalChance", 0f, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterOpinionChanceFactor, "lastRecruiterOpinionChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterResistanceReduce, "lastRecruiterResistanceReduce", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.interactionMode.allowOnWildMan && this.pawn.IsWildMan())
			{
				this.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
			}
		}

		// Token: 0x060072E3 RID: 29411 RVA: 0x002310C4 File Offset: 0x0022F2C4
		public void ClearLastRecruiterData()
		{
			this.lastRecruiterName = null;
			this.lastRecruiterNegotiationAbilityFactor = 0f;
			this.lastRecruiterOpinion = 0;
			this.lastRecruiterOpinionChanceFactor = 0f;
			this.lastRecruiterResistanceReduce = 0f;
			this.hasOpinionOfLastRecruiter = false;
			this.lastRecruiterFinalChance = 0f;
		}

		// Token: 0x060072E4 RID: 29412 RVA: 0x00231114 File Offset: 0x0022F314
		public void SetLastRecruiterData(Pawn recruiter, float resistanceReduce)
		{
			this.lastRecruiterName = recruiter.LabelShort;
			this.lastRecruiterNegotiationAbilityFactor = RecruitUtility.RecruitChanceFactorForRecruiterNegotiationAbility(recruiter);
			this.lastRecruiterOpinionChanceFactor = RecruitUtility.RecruitChanceFactorForOpinion(recruiter, this.pawn);
			this.lastRecruiterResistanceReduce = resistanceReduce;
			this.hasOpinionOfLastRecruiter = (this.pawn.relations != null);
			this.lastRecruiterOpinion = (this.hasOpinionOfLastRecruiter ? this.pawn.relations.OpinionOf(recruiter) : 0);
			this.lastRecruiterFinalChance = this.pawn.RecruitChanceFinalByPawn(recruiter);
		}

		// Token: 0x060072E5 RID: 29413 RVA: 0x0023119C File Offset: 0x0022F39C
		public void SetGuestStatus(Faction newHost, bool prisoner = false)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			if (newHost == this.HostFaction && prisoner == this.IsPrisoner)
			{
				return;
			}
			if (!prisoner && this.pawn.Faction.HostileTo(newHost))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to make ",
					this.pawn,
					" a guest of ",
					newHost,
					" but their faction ",
					this.pawn.Faction,
					" is hostile to ",
					newHost
				}), false);
				return;
			}
			if (newHost != null && newHost == this.pawn.Faction && !prisoner)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to make ",
					this.pawn,
					" a guest of their own faction ",
					this.pawn.Faction
				}), false);
				return;
			}
			bool flag = prisoner && (!this.IsPrisoner || this.HostFaction != newHost);
			this.isPrisonerInt = prisoner;
			Faction faction = this.hostFactionInt;
			this.hostFactionInt = newHost;
			this.pawn.ClearMind(newHost != null, false, prisoner);
			if (flag)
			{
				this.pawn.DropAndForbidEverything(false);
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.MadePrisoner, null);
				}
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				float x = this.pawn.RecruitDifficulty(Faction.OfPlayer);
				this.resistance = Pawn_GuestTracker.StartingResistancePerRecruitDifficultyCurve.Evaluate(x);
				this.resistance *= Pawn_GuestTracker.StartingResistanceFactorFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
				this.resistance *= Pawn_GuestTracker.StartingResistanceRandomFactorRange.RandomInRange;
				if (this.pawn.royalty != null)
				{
					RoyalTitle mostSeniorTitle = this.pawn.royalty.MostSeniorTitle;
					if (mostSeniorTitle != null)
					{
						this.resistance *= mostSeniorTitle.def.recruitmentResistanceFactor;
						this.resistance += mostSeniorTitle.def.recruitmentResistanceOffset;
					}
				}
				this.resistance = (float)GenMath.RoundRandom(this.resistance);
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			this.pawn.health.surgeryBills.Clear();
			if (this.pawn.ownership != null)
			{
				this.pawn.ownership.Notify_ChangedGuestStatus();
			}
			ReachabilityUtility.ClearCacheFor(this.pawn);
			if (this.pawn.Spawned)
			{
				this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this.pawn);
			if (prisoner && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.Notify_MadePrisoner();
			}
			if (faction != this.hostFactionInt)
			{
				QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ChangedHostFaction", this.pawn.Named("SUBJECT"), this.hostFactionInt.Named("FACTION"));
			}
		}

		// Token: 0x060072E6 RID: 29414 RVA: 0x002314CC File Offset: 0x0022F6CC
		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			Faction factionOrExtraMiniOrHomeFaction = this.pawn.FactionOrExtraMiniOrHomeFaction;
			if (factionOrExtraMiniOrHomeFaction != null)
			{
				factionOrExtraMiniOrHomeFaction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, true);
			if (this.IsPrisoner && byPawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
				{
					byPawn,
					this.pawn
				});
				byPawn.records.Increment(RecordDefOf.PeopleCaptured);
			}
		}

		// Token: 0x060072E7 RID: 29415 RVA: 0x0004D4B9 File Offset: 0x0004B6B9
		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		// Token: 0x060072E8 RID: 29416 RVA: 0x0004D4C6 File Offset: 0x0004B6C6
		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (!this.IsPrisoner)
			{
				return;
			}
			this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
			this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
		}

		// Token: 0x060072E9 RID: 29417 RVA: 0x00231538 File Offset: 0x0022F738
		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && (this.HostFaction == Faction.OfPlayer || (this.pawn.IsWildMan() && this.pawn.InBed() && this.pawn.CurrentBed().Faction == Faction.OfPlayer)) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.getRescuedThoughtOnUndownedBecauseOfPlayer && this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null);
				}
				if (this.pawn.Faction == null || this.pawn.Faction.def.rescueesCanJoin)
				{
					Map mapHeld = this.pawn.MapHeld;
					float num;
					if (!this.pawn.SafeTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp) || mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
					{
						num = 1f;
					}
					else
					{
						num = 0.5f;
					}
					if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 8976612) < num)
					{
						this.pawn.SetFaction(Faction.OfPlayer, null);
						Find.LetterStack.ReceiveLetter("LetterLabelRescueeJoins".Translate(this.pawn.Named("PAWN")), "LetterRescueeJoins".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
					}
					else
					{
						Messages.Message("MessageRescueeDidntJoin".Translate().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
					}
				}
			}
			this.getRescuedThoughtOnUndownedBecauseOfPlayer = false;
		}

		// Token: 0x04004B95 RID: 19349
		private Pawn pawn;

		// Token: 0x04004B96 RID: 19350
		public PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.NoInteraction;

		// Token: 0x04004B97 RID: 19351
		private Faction hostFactionInt;

		// Token: 0x04004B98 RID: 19352
		public bool isPrisonerInt;

		// Token: 0x04004B99 RID: 19353
		public string lastRecruiterName;

		// Token: 0x04004B9A RID: 19354
		public int lastRecruiterOpinion;

		// Token: 0x04004B9B RID: 19355
		public float lastRecruiterOpinionChanceFactor;

		// Token: 0x04004B9C RID: 19356
		public float lastRecruiterNegotiationAbilityFactor;

		// Token: 0x04004B9D RID: 19357
		public bool hasOpinionOfLastRecruiter;

		// Token: 0x04004B9E RID: 19358
		public float lastRecruiterResistanceReduce;

		// Token: 0x04004B9F RID: 19359
		public float lastRecruiterFinalChance;

		// Token: 0x04004BA0 RID: 19360
		private bool releasedInt;

		// Token: 0x04004BA1 RID: 19361
		private int ticksWhenAllowedToEscapeAgain;

		// Token: 0x04004BA2 RID: 19362
		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		// Token: 0x04004BA3 RID: 19363
		public int lastPrisonBreakTicks = -1;

		// Token: 0x04004BA4 RID: 19364
		public bool everParticipatedInPrisonBreak;

		// Token: 0x04004BA5 RID: 19365
		public float resistance = -1f;

		// Token: 0x04004BA6 RID: 19366
		public bool getRescuedThoughtOnUndownedBecauseOfPlayer;

		// Token: 0x04004BA7 RID: 19367
		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		// Token: 0x04004BA8 RID: 19368
		public const int MinInteractionInterval = 10000;

		// Token: 0x04004BA9 RID: 19369
		public const int MaxInteractionsPerDay = 2;

		// Token: 0x04004BAA RID: 19370
		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		// Token: 0x04004BAB RID: 19371
		private static readonly SimpleCurve StartingResistancePerRecruitDifficultyCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 15f),
				true
			},
			{
				new CurvePoint(0.9f, 25f),
				true
			},
			{
				new CurvePoint(1f, 50f),
				true
			}
		};

		// Token: 0x04004BAC RID: 19372
		private static readonly SimpleCurve StartingResistanceFactorFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 2f),
				true
			},
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 0.8f),
				true
			}
		};

		// Token: 0x04004BAD RID: 19373
		private static readonly FloatRange StartingResistanceRandomFactorRange = new FloatRange(0.8f, 1.2f);
	}
}
