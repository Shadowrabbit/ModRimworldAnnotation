using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E35 RID: 3637
	public class Pawn_GuestTracker : IExposable
	{
		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06005415 RID: 21525 RVA: 0x001C7316 File Offset: 0x001C5516
		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06005416 RID: 21526 RVA: 0x001C731E File Offset: 0x001C551E
		public Faction SlaveFaction
		{
			get
			{
				return this.slaveFactionInt;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06005417 RID: 21527 RVA: 0x001C7326 File Offset: 0x001C5526
		public GuestStatus GuestStatus
		{
			get
			{
				return this.guestStatusInt;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06005418 RID: 21528 RVA: 0x001C732E File Offset: 0x001C552E
		public bool CanBeBroughtFood
		{
			get
			{
				return this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06005419 RID: 21529 RVA: 0x001C7359 File Offset: 0x001C5559
		public bool IsPrisoner
		{
			get
			{
				return this.guestStatusInt == GuestStatus.Prisoner;
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x0600541A RID: 21530 RVA: 0x001C7364 File Offset: 0x001C5564
		public bool IsSlave
		{
			get
			{
				return this.guestStatusInt == GuestStatus.Slave;
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x0600541B RID: 21531 RVA: 0x001C736F File Offset: 0x001C556F
		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - 10000 && this.pawn.mindState.interactionsToday < 2;
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x0600541C RID: 21532 RVA: 0x001C73A8 File Offset: 0x001C55A8
		// (set) Token: 0x0600541D RID: 21533 RVA: 0x001C73B0 File Offset: 0x001C55B0
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

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x0600541E RID: 21534 RVA: 0x001C73D0 File Offset: 0x001C55D0
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

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x0600541F RID: 21535 RVA: 0x001C7450 File Offset: 0x001C5650
		public bool SlaveIsSecure
		{
			get
			{
				if (this.Released)
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
					if (SlaveRebellionUtility.IsRebelling(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005420 RID: 21536 RVA: 0x001C74C0 File Offset: 0x001C56C0
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

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005421 RID: 21537 RVA: 0x001C7509 File Offset: 0x001C5709
		public float Resistance
		{
			get
			{
				return this.resistance;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005422 RID: 21538 RVA: 0x001C7511 File Offset: 0x001C5711
		public bool ScheduledForSlaveSuppression
		{
			get
			{
				return this.pawn.mindState.lastSlaveSuppressedTick < Find.TickManager.TicksGame - 60000;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005423 RID: 21539 RVA: 0x001C7535 File Offset: 0x001C5735
		public bool EverEnslaved
		{
			get
			{
				return this.everEnslaved;
			}
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x001C7540 File Offset: 0x001C5740
		public Pawn_GuestTracker()
		{
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x001C7594 File Offset: 0x001C5794
		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001C75EC File Offset: 0x001C57EC
		public void GuestTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(2500))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn, null);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
			if (this.pawn.IsHashIntervalTick(2500))
			{
				float num2 = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(this.pawn);
				if (num2 >= 0f && Rand.MTBEventOccurs(num2, 60000f, 2500f))
				{
					SlaveRebellionUtility.StartSlaveRebellion(this.pawn, false);
				}
			}
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x001C7682 File Offset: 0x001C5882
		public void RandomizeJoinStatus()
		{
			if (ModsConfig.IdeologyActive)
			{
				this.joinStatus = ((Rand.Value < 0.75f) ? JoinStatus.JoinAsSlave : JoinStatus.JoinAsColonist);
				return;
			}
			this.joinStatus = JoinStatus.JoinAsColonist;
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x001C76AC File Offset: 0x001C58AC
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_References.Look<Faction>(ref this.slaveFactionInt, "slaveFaction", false);
			Scribe_Values.Look<GuestStatus>(ref this.guestStatusInt, "guestStatus", GuestStatus.Guest, false);
			Scribe_Values.Look<JoinStatus>(ref this.joinStatus, "joinStatus", JoinStatus.Undefined, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Defs.Look<SlaveInteractionModeDef>(ref this.slaveInteractionMode, "slaveInteractionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
			Scribe_Values.Look<bool>(ref this.getRescuedThoughtOnUndownedBecauseOfPlayer, "getRescuedThoughtOnUndownedBecauseOfPlayer", false, false);
			Scribe_Values.Look<float>(ref this.resistance, "resistance", -1f, false);
			Scribe_Values.Look<float>(ref this.will, "will", -1f, false);
			Scribe_Values.Look<string>(ref this.lastRecruiterName, "lastRecruiterName", null, false);
			Scribe_Values.Look<bool>(ref this.hasOpinionOfLastRecruiter, "hasOpinionOfLastRecruiter", false, false);
			Scribe_Values.Look<int>(ref this.lastRecruiterOpinion, "lastRecruiterOpinion", 0, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterResistanceReduce, "lastRecruiterResistanceReduce", 0f, false);
			Scribe_Values.Look<bool>(ref this.everEnslaved, "everEnslaved", false, false);
			Scribe_References.Look<Ideo>(ref this.ideoForConversion, "ideoForConversion", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.interactionMode.allowOnWildMan && this.pawn.IsWildMan())
				{
					this.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
				if (this.joinStatus == JoinStatus.Undefined)
				{
					this.RandomizeJoinStatus();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x001C786F File Offset: 0x001C5A6F
		public void ClearLastRecruiterData()
		{
			this.lastRecruiterName = null;
			this.lastRecruiterOpinion = 0;
			this.lastRecruiterResistanceReduce = 0f;
			this.hasOpinionOfLastRecruiter = false;
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x001C7894 File Offset: 0x001C5A94
		public void SetLastRecruiterData(Pawn recruiter, float resistanceReduce)
		{
			this.lastRecruiterName = recruiter.LabelShort;
			this.lastRecruiterResistanceReduce = resistanceReduce;
			this.hasOpinionOfLastRecruiter = (this.pawn.relations != null);
			this.lastRecruiterOpinion = (this.hasOpinionOfLastRecruiter ? this.pawn.relations.OpinionOf(recruiter) : 0);
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x001C78EC File Offset: 0x001C5AEC
		public void Notify_WardensOfIdeoLost(Ideo ideo)
		{
			if (!ModsConfig.IdeologyActive || this.interactionMode != PrisonerInteractionModeDefOf.Convert || this.ideoForConversion == null || this.ideoForConversion != ideo)
			{
				return;
			}
			this.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
			this.ideoForConversion = null;
			Messages.Message("MessageNoWardenOfIdeo".Translate(this.pawn.Named("PRISONER"), ideo.memberName.Named("MEMBERNAME")), new LookTargets(this.pawn), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x001C7978 File Offset: 0x001C5B78
		public void SetGuestStatus(Faction newHost, GuestStatus guestStatus = GuestStatus.Guest)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			switch (guestStatus)
			{
			case GuestStatus.Guest:
				if (this.pawn.Faction.HostileTo(newHost))
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
					}));
					return;
				}
				if (newHost != null && newHost == this.pawn.Faction)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to make ",
						this.pawn,
						" a guest of their own faction ",
						this.pawn.Faction
					}));
					return;
				}
				break;
			case GuestStatus.Prisoner:
			{
				if (newHost == this.HostFaction && this.IsPrisoner)
				{
					return;
				}
				this.pawn.ClearMind(newHost != null, false, true);
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
				float num = this.pawn.kindDef.initialResistanceRange.Value.RandomInRange;
				if (this.pawn.royalty != null)
				{
					RoyalTitle mostSeniorTitle = this.pawn.royalty.MostSeniorTitle;
					if (mostSeniorTitle != null)
					{
						num += mostSeniorTitle.def.recruitmentResistanceOffset;
					}
				}
				this.resistance = (float)GenMath.RoundRandom(num);
				this.will = (this.everEnslaved ? 2.5f : this.pawn.kindDef.initialWillRange.Value.RandomInRange);
				if (ModsConfig.IdeologyActive && this.interactionMode == PrisonerInteractionModeDefOf.Enslave)
				{
					this.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
				if (this.slaveFactionInt != null)
				{
					this.pawn.SetFaction(this.slaveFactionInt, null);
					this.slaveFactionInt = null;
				}
				break;
			}
			case GuestStatus.Slave:
			{
				if (newHost == this.pawn.Faction && this.IsSlave)
				{
					return;
				}
				Lord lord2 = this.pawn.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_PawnLost(this.pawn, PawnLostCondition.MadeSlave, null);
				}
				if (this.slaveFactionInt == null)
				{
					this.slaveFactionInt = this.pawn.Faction;
				}
				if (this.pawn.Faction != newHost)
				{
					this.pawn.SetFaction(newHost, null);
				}
				if (!this.everEnslaved || this.slaveInteractionMode == SlaveInteractionModeDefOf.Imprison || this.slaveInteractionMode == SlaveInteractionModeDefOf.Emancipate)
				{
					this.slaveInteractionMode = SlaveInteractionModeDefOf.Suppress;
				}
				this.everEnslaved = true;
				break;
			}
			default:
				Log.Error(string.Format("Unknown GuestStatus type {0}", guestStatus));
				return;
			}
			this.guestStatusInt = guestStatus;
			Faction faction = this.hostFactionInt;
			if (guestStatus != GuestStatus.Slave)
			{
				this.hostFactionInt = newHost;
			}
			else
			{
				this.hostFactionInt = null;
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
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
			if ((guestStatus == GuestStatus.Prisoner || guestStatus == GuestStatus.Slave) && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.ResetMedicalCare();
			}
			if (faction != this.hostFactionInt)
			{
				QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ChangedHostFaction", this.pawn.Named("SUBJECT"), this.hostFactionInt.Named("FACTION"));
			}
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x001C7D84 File Offset: 0x001C5F84
		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			Faction homeFaction = this.pawn.HomeFaction;
			if (homeFaction != null)
			{
				homeFaction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, GuestStatus.Prisoner);
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

		// Token: 0x0600542E RID: 21550 RVA: 0x001C7DEE File Offset: 0x001C5FEE
		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x001C7DFB File Offset: 0x001C5FFB
		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (!this.IsPrisoner)
			{
				return;
			}
			this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
			this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x001C7E23 File Offset: 0x001C6023
		public Texture2D GetIcon()
		{
			return GuestUtility.GetGuestIcon(this.guestStatusInt);
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x001C7E30 File Offset: 0x001C6030
		public string GetLabel()
		{
			if (this.IsSlave)
			{
				return "Slave".Translate();
			}
			return null;
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x001C7E4C File Offset: 0x001C604C
		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && (this.HostFaction.IsPlayerSafe() || (this.pawn.IsWildMan() && this.pawn.InBed() && this.pawn.CurrentBed().Faction == Faction.OfPlayer)) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.getRescuedThoughtOnUndownedBecauseOfPlayer && this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null, null);
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

		// Token: 0x06005433 RID: 21555 RVA: 0x001C804E File Offset: 0x001C624E
		public void Notify_PawnRecruited()
		{
			this.slaveFactionInt = null;
		}

		// Token: 0x0400317C RID: 12668
		private Pawn pawn;

		// Token: 0x0400317D RID: 12669
		public PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.NoInteraction;

		// Token: 0x0400317E RID: 12670
		public SlaveInteractionModeDef slaveInteractionMode = SlaveInteractionModeDefOf.NoInteraction;

		// Token: 0x0400317F RID: 12671
		private Faction hostFactionInt;

		// Token: 0x04003180 RID: 12672
		public GuestStatus guestStatusInt;

		// Token: 0x04003181 RID: 12673
		public JoinStatus joinStatus;

		// Token: 0x04003182 RID: 12674
		private Faction slaveFactionInt;

		// Token: 0x04003183 RID: 12675
		public string lastRecruiterName;

		// Token: 0x04003184 RID: 12676
		public int lastRecruiterOpinion;

		// Token: 0x04003185 RID: 12677
		public bool hasOpinionOfLastRecruiter;

		// Token: 0x04003186 RID: 12678
		public float lastRecruiterResistanceReduce;

		// Token: 0x04003187 RID: 12679
		private bool releasedInt;

		// Token: 0x04003188 RID: 12680
		private int ticksWhenAllowedToEscapeAgain;

		// Token: 0x04003189 RID: 12681
		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		// Token: 0x0400318A RID: 12682
		public int lastPrisonBreakTicks = -1;

		// Token: 0x0400318B RID: 12683
		public bool everParticipatedInPrisonBreak;

		// Token: 0x0400318C RID: 12684
		public float resistance = -1f;

		// Token: 0x0400318D RID: 12685
		public float will = -1f;

		// Token: 0x0400318E RID: 12686
		public Ideo ideoForConversion;

		// Token: 0x0400318F RID: 12687
		private bool everEnslaved;

		// Token: 0x04003190 RID: 12688
		public bool getRescuedThoughtOnUndownedBecauseOfPlayer;

		// Token: 0x04003191 RID: 12689
		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		// Token: 0x04003192 RID: 12690
		public const int MinInteractionInterval = 10000;

		// Token: 0x04003193 RID: 12691
		public const int MaxInteractionsPerDay = 2;

		// Token: 0x04003194 RID: 12692
		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		// Token: 0x04003195 RID: 12693
		private const int CheckInitiateSlaveRebellionIntervalTicks = 2500;

		// Token: 0x04003196 RID: 12694
		private const int MinSlaveSuppressionIntervalTicks = 60000;

		// Token: 0x04003197 RID: 12695
		public const float DefaultWillIfPreviouslyEnslaved = 2.5f;
	}
}
