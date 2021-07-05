using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x0200058C RID: 1420
	public class Job : IExposable, ILoadReferenceable
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002987 RID: 10631 RVA: 0x000FB000 File Offset: 0x000F9200
		public RecipeDef RecipeDef
		{
			get
			{
				if (this.bill == null)
				{
					return null;
				}
				return this.bill.recipe;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x000FB017 File Offset: 0x000F9217
		public JobDriver GetCachedDriverDirect
		{
			get
			{
				return this.cachedDriver;
			}
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000FB020 File Offset: 0x000F9220
		public void Clear()
		{
			this.def = null;
			this.targetA = LocalTargetInfo.Invalid;
			this.targetB = LocalTargetInfo.Invalid;
			this.targetC = LocalTargetInfo.Invalid;
			this.targetQueueA = null;
			this.targetQueueB = null;
			this.count = -1;
			this.countQueue = null;
			this.loadID = -1;
			this.startTick = -1;
			this.expiryInterval = -1;
			this.checkOverrideOnExpire = false;
			this.playerForced = false;
			this.placedThings = null;
			this.maxNumMeleeAttacks = int.MaxValue;
			this.maxNumStaticAttacks = int.MaxValue;
			this.locomotionUrgency = LocomotionUrgency.Jog;
			this.haulMode = HaulMode.Undefined;
			this.bill = null;
			this.commTarget = null;
			this.plantDefToSow = null;
			this.verbToUse = null;
			this.haulOpportunisticDuplicates = false;
			this.exitMapOnArrival = false;
			this.failIfCantJoinOrCreateCaravan = false;
			this.killIncappedTarget = false;
			this.ignoreForbidden = false;
			this.ignoreDesignations = false;
			this.canBashDoors = false;
			this.canBashFences = false;
			this.canUseRangedWeapon = true;
			this.haulDroppedApparel = false;
			this.restUntilHealed = false;
			this.ignoreJoyTimeAssignment = false;
			this.doUntilGatheringEnded = false;
			this.overeat = false;
			this.attackDoorIfTargetLost = false;
			this.takeExtraIngestibles = 0;
			this.expireRequiresEnemiesNearby = false;
			this.lord = null;
			this.collideWithPawns = false;
			this.forceSleep = false;
			this.interaction = null;
			this.endIfCantShootTargetFromCurPos = false;
			this.endIfCantShootInMelee = false;
			this.checkEncumbrance = false;
			this.followRadius = 0f;
			this.endAfterTendedOnce = false;
			this.quest = null;
			this.mote = null;
			this.reactingToMeleeThreat = false;
			this.wasOnMeditationTimeAssignment = false;
			this.psyfocusTargetLast = -1f;
			this.preventFriendlyFire = false;
			this.endIfAllyNotAThreatAnymore = false;
			this.ropingPriority = RopingPriority.Closest;
			this.ropeToUnenclosedPens = false;
			this.thingDefToCarry = null;
			this.ritualTag = null;
			this.lookDirection = Direction8Way.Invalid;
			this.overrideFacing = Rot4.Invalid;
			this.takeInventoryDelay = 0;
			this.draftedTend = false;
			this.showSpeechBubbles = true;
			this.speechSoundFemale = null;
			this.speechSoundMale = null;
			this.jobGiverThinkTree = null;
			this.jobGiver = null;
			this.workGiverDef = null;
			this.ability = null;
			if (this.cachedDriver != null)
			{
				this.cachedDriver.job = null;
			}
			this.cachedDriver = null;
			if (this.lastJobDriverMade != null)
			{
				this.lastJobDriverMade.job = null;
			}
			this.lastJobDriverMade = null;
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000FB270 File Offset: 0x000F9470
		public Job()
		{
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000FB31A File Offset: 0x000F951A
		public Job(JobDef def) : this(def, null)
		{
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000FB329 File Offset: 0x000F9529
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000FB33C File Offset: 0x000F953C
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000FB40C File Offset: 0x000F960C
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000FB4E4 File Offset: 0x000F96E4
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000FB5BC File Offset: 0x000F97BC
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000FB68B File Offset: 0x000F988B
		public LocalTargetInfo GetTarget(TargetIndex ind)
		{
			switch (ind)
			{
			case TargetIndex.A:
				return this.targetA;
			case TargetIndex.B:
				return this.targetB;
			case TargetIndex.C:
				return this.targetC;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000FB6C0 File Offset: 0x000F98C0
		public List<LocalTargetInfo> GetTargetQueue(TargetIndex ind)
		{
			if (ind == TargetIndex.A)
			{
				if (this.targetQueueA == null)
				{
					this.targetQueueA = new List<LocalTargetInfo>();
				}
				return this.targetQueueA;
			}
			if (ind != TargetIndex.B)
			{
				throw new ArgumentException();
			}
			if (this.targetQueueB == null)
			{
				this.targetQueueB = new List<LocalTargetInfo>();
			}
			return this.targetQueueB;
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000FB710 File Offset: 0x000F9910
		public void SetTarget(TargetIndex ind, LocalTargetInfo pack)
		{
			switch (ind)
			{
			case TargetIndex.A:
				this.targetA = pack;
				return;
			case TargetIndex.B:
				this.targetB = pack;
				return;
			case TargetIndex.C:
				this.targetC = pack;
				return;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000FB745 File Offset: 0x000F9945
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000FB754 File Offset: 0x000F9954
		public void ExposeData()
		{
			ILoadReferenceable loadReferenceable = (ILoadReferenceable)this.commTarget;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "commTarget", false);
			this.commTarget = (ICommunicable)loadReferenceable;
			Scribe_References.Look<Verb>(ref this.verbToUse, "verbToUse", false);
			Scribe_References.Look<Bill>(ref this.bill, "bill", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Defs.Look<JobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_TargetInfo.Look(ref this.targetA, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, "targetC");
			Scribe_TargetInfo.Look(ref this.globalTarget, "globalTarget");
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueA, "targetQueueA", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueB, "targetQueueB", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.count, "count", -1, false);
			Scribe_Collections.Look<int>(ref this.countQueue, "countQueue", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_Values.Look<int>(ref this.expiryInterval, "expiryInterval", -1, false);
			Scribe_Values.Look<bool>(ref this.checkOverrideOnExpire, "checkOverrideOnExpire", false, false);
			Scribe_Values.Look<bool>(ref this.playerForced, "playerForced", false, false);
			Scribe_Collections.Look<ThingCountClass>(ref this.placedThings, "placedThings", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.maxNumMeleeAttacks, "maxNumMeleeAttacks", int.MaxValue, false);
			Scribe_Values.Look<int>(ref this.maxNumStaticAttacks, "maxNumStaticAttacks", int.MaxValue, false);
			Scribe_Values.Look<bool>(ref this.exitMapOnArrival, "exitMapOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.failIfCantJoinOrCreateCaravan, "failIfCantJoinOrCreateCaravan", false, false);
			Scribe_Values.Look<bool>(ref this.killIncappedTarget, "killIncappedTarget", false, false);
			Scribe_Values.Look<bool>(ref this.haulOpportunisticDuplicates, "haulOpportunisticDuplicates", false, false);
			Scribe_Values.Look<HaulMode>(ref this.haulMode, "haulMode", HaulMode.Undefined, false);
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToSow, "plantDefToSow");
			Scribe_Defs.Look<ThingDef>(ref this.thingDefToCarry, "thingDefToCarry");
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
			Scribe_Values.Look<bool>(ref this.canBashDoors, "canBash", false, false);
			Scribe_Values.Look<bool>(ref this.canBashFences, "canBashFences", false, false);
			Scribe_Values.Look<bool>(ref this.canUseRangedWeapon, "canUseRangedWeapon", true, false);
			Scribe_Values.Look<bool>(ref this.haulDroppedApparel, "haulDroppedApparel", false, false);
			Scribe_Values.Look<bool>(ref this.restUntilHealed, "restUntilHealed", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreJoyTimeAssignment, "ignoreJoyTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.overeat, "overeat", false, false);
			Scribe_Values.Look<bool>(ref this.attackDoorIfTargetLost, "attackDoorIfTargetLost", false, false);
			Scribe_Values.Look<int>(ref this.takeExtraIngestibles, "takeExtraIngestibles", 0, false);
			Scribe_Values.Look<bool>(ref this.expireRequiresEnemiesNearby, "expireRequiresEnemiesNearby", false, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_Values.Look<bool>(ref this.forceSleep, "forceSleep", false, false);
			Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
			Scribe_Values.Look<bool>(ref this.endIfCantShootTargetFromCurPos, "endIfCantShootTargetFromCurPos", false, false);
			Scribe_Values.Look<bool>(ref this.endIfCantShootInMelee, "endIfCantShootInMelee", false, false);
			Scribe_Values.Look<bool>(ref this.checkEncumbrance, "checkEncumbrance", false, false);
			Scribe_Values.Look<float>(ref this.followRadius, "followRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.endAfterTendedOnce, "endAfterTendedOnce", false, false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.workGiverDef, "workGiverDef");
			Scribe_Defs.Look<ThinkTreeDef>(ref this.jobGiverThinkTree, "jobGiverThinkTree");
			Scribe_Values.Look<bool>(ref this.doUntilGatheringEnded, "doUntilGatheringEnded", false, false);
			Scribe_Values.Look<float>(ref this.psyfocusTargetLast, "psyfocusTargetLast", 0f, false);
			Scribe_Values.Look<bool>(ref this.wasOnMeditationTimeAssignment, "wasOnMeditationTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.reactingToMeleeThreat, "reactingToMeleeThreat", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.endIfAllyNotAThreatAnymore, "endIfAllyNotAThreatAnymore", false, false);
			Scribe_Values.Look<RopingPriority>(ref this.ropingPriority, "ropingPriority", RopingPriority.Closest, false);
			Scribe_Values.Look<bool>(ref this.ropeToUnenclosedPens, "ropeToUnenclosedPens", false, false);
			Scribe_Values.Look<Direction8Way>(ref this.lookDirection, "lookDirection", Direction8Way.Invalid, false);
			Scribe_Values.Look<string>(ref this.ritualTag, "ritualTag", null, false);
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
			Scribe_Values.Look<int>(ref this.takeInventoryDelay, "takeInventoryDelay", 0, false);
			Scribe_Values.Look<bool>(ref this.draftedTend, "draftedTend", false, false);
			Scribe_Values.Look<bool>(ref this.showSpeechBubbles, "showSpeechBubbles", true, false);
			Scribe_Values.Look<Rot4>(ref this.overrideFacing, "overrideFacing", Rot4.Invalid, false);
			Scribe_Defs.Look<SoundDef>(ref this.speechSoundMale, "speechSoundMale");
			Scribe_Defs.Look<SoundDef>(ref this.speechSoundFemale, "speechSoundFemale");
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.jobGiverKey = ((this.jobGiver != null) ? this.jobGiver.UniqueSaveKey : -1);
			}
			Scribe_Values.Look<int>(ref this.jobGiverKey, "lastJobGiverKey", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.jobGiverKey != -1 && !this.jobGiverThinkTree.TryGetThinkNodeWithSaveKey(this.jobGiverKey, out this.jobGiver))
			{
				Log.Warning("Could not find think node with key " + this.jobGiverKey);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbToUse != null && this.verbToUse.BuggedAfterLoading)
			{
				this.verbToUse = null;
				Log.Warning(base.GetType() + " had a bugged verbToUse after loading.");
			}
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000FBCF8 File Offset: 0x000F9EF8
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			this.lastJobDriverMade = jobDriver;
			return jobDriver;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000FBD34 File Offset: 0x000F9F34
		public JobDriver GetCachedDriver(Pawn driverPawn)
		{
			if (this.cachedDriver == null)
			{
				this.cachedDriver = this.MakeDriver(driverPawn);
			}
			if (this.cachedDriver.pawn != driverPawn)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to use the same driver for 2 pawns: ",
					this.cachedDriver.ToStringSafe<JobDriver>(),
					", first pawn= ",
					this.cachedDriver.pawn.ToStringSafe<Pawn>(),
					", second pawn=",
					driverPawn.ToStringSafe<Pawn>()
				}));
			}
			return this.cachedDriver;
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000FBDBC File Offset: 0x000F9FBC
		public bool TryMakePreToilReservations(Pawn driverPawn, bool errorOnFailed)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations(errorOnFailed);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000FBDCB File Offset: 0x000F9FCB
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000FBDD9 File Offset: 0x000F9FD9
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000FBDE1 File Offset: 0x000F9FE1
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000FBE00 File Offset: 0x000FA000
		public bool JobIsSameAs(Job other)
		{
			return other != null && (this == other || (this.def == other.def && !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && this.verbToUse == other.verbToUse && !(this.targetC != other.targetC) && this.commTarget == other.commTarget && this.bill == other.bill));
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000FBE8C File Offset: 0x000FA08C
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000FBEFC File Offset: 0x000FA0FC
		public bool AnyTargetOutsideArea(Area zone)
		{
			if (Job.IsTargetOutsideArea(this.targetA, zone) || Job.IsTargetOutsideArea(this.targetB, zone) || Job.IsTargetOutsideArea(this.targetC, zone))
			{
				return true;
			}
			if (this.targetQueueA != null)
			{
				using (List<LocalTargetInfo>.Enumerator enumerator = this.targetQueueA.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Job.IsTargetOutsideArea(enumerator.Current, zone))
						{
							return true;
						}
					}
				}
			}
			if (this.targetQueueB != null)
			{
				using (List<LocalTargetInfo>.Enumerator enumerator = this.targetQueueB.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Job.IsTargetOutsideArea(enumerator.Current, zone))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000FBFD8 File Offset: 0x000FA1D8
		private static bool IsTargetOutsideArea(LocalTargetInfo target, Area zone)
		{
			IntVec3 cell = target.Cell;
			return cell.IsValid && !zone[cell];
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000FC004 File Offset: 0x000FA204
		public override string ToString()
		{
			string text = this.def.ToString() + " (" + this.GetUniqueLoadID() + ")";
			if (this.targetA.IsValid)
			{
				text = text + " A=" + this.targetA.ToString();
			}
			if (this.targetB.IsValid)
			{
				text = text + " B=" + this.targetB.ToString();
			}
			if (this.targetC.IsValid)
			{
				text = text + " C=" + this.targetC.ToString();
			}
			return text;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000FC0B1 File Offset: 0x000FA2B1
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID;
		}

		// Token: 0x040019BB RID: 6587
		public JobDef def;

		// Token: 0x040019BC RID: 6588
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		// Token: 0x040019BD RID: 6589
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		// Token: 0x040019BE RID: 6590
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		// Token: 0x040019BF RID: 6591
		public List<LocalTargetInfo> targetQueueA;

		// Token: 0x040019C0 RID: 6592
		public List<LocalTargetInfo> targetQueueB;

		// Token: 0x040019C1 RID: 6593
		public GlobalTargetInfo globalTarget = GlobalTargetInfo.Invalid;

		// Token: 0x040019C2 RID: 6594
		public int count = -1;

		// Token: 0x040019C3 RID: 6595
		public List<int> countQueue;

		// Token: 0x040019C4 RID: 6596
		public int loadID = -1;

		// Token: 0x040019C5 RID: 6597
		public int startTick = -1;

		// Token: 0x040019C6 RID: 6598
		public int expiryInterval = -1;

		// Token: 0x040019C7 RID: 6599
		public bool checkOverrideOnExpire;

		// Token: 0x040019C8 RID: 6600
		public bool playerForced;

		// Token: 0x040019C9 RID: 6601
		public List<ThingCountClass> placedThings;

		// Token: 0x040019CA RID: 6602
		public int maxNumMeleeAttacks = int.MaxValue;

		// Token: 0x040019CB RID: 6603
		public int maxNumStaticAttacks = int.MaxValue;

		// Token: 0x040019CC RID: 6604
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		// Token: 0x040019CD RID: 6605
		public HaulMode haulMode;

		// Token: 0x040019CE RID: 6606
		public Bill bill;

		// Token: 0x040019CF RID: 6607
		public ICommunicable commTarget;

		// Token: 0x040019D0 RID: 6608
		public ThingDef plantDefToSow;

		// Token: 0x040019D1 RID: 6609
		public ThingDef thingDefToCarry;

		// Token: 0x040019D2 RID: 6610
		public Verb verbToUse;

		// Token: 0x040019D3 RID: 6611
		public bool haulOpportunisticDuplicates;

		// Token: 0x040019D4 RID: 6612
		public bool exitMapOnArrival;

		// Token: 0x040019D5 RID: 6613
		public bool failIfCantJoinOrCreateCaravan;

		// Token: 0x040019D6 RID: 6614
		public bool killIncappedTarget;

		// Token: 0x040019D7 RID: 6615
		public bool ignoreForbidden;

		// Token: 0x040019D8 RID: 6616
		public bool ignoreDesignations;

		// Token: 0x040019D9 RID: 6617
		public bool canBashDoors;

		// Token: 0x040019DA RID: 6618
		public bool canBashFences;

		// Token: 0x040019DB RID: 6619
		public bool canUseRangedWeapon = true;

		// Token: 0x040019DC RID: 6620
		public bool haulDroppedApparel;

		// Token: 0x040019DD RID: 6621
		public bool restUntilHealed;

		// Token: 0x040019DE RID: 6622
		public bool ignoreJoyTimeAssignment;

		// Token: 0x040019DF RID: 6623
		public bool doUntilGatheringEnded;

		// Token: 0x040019E0 RID: 6624
		public bool overeat;

		// Token: 0x040019E1 RID: 6625
		public bool attackDoorIfTargetLost;

		// Token: 0x040019E2 RID: 6626
		public int takeExtraIngestibles;

		// Token: 0x040019E3 RID: 6627
		public bool expireRequiresEnemiesNearby;

		// Token: 0x040019E4 RID: 6628
		public Lord lord;

		// Token: 0x040019E5 RID: 6629
		public bool collideWithPawns;

		// Token: 0x040019E6 RID: 6630
		public bool forceSleep;

		// Token: 0x040019E7 RID: 6631
		public InteractionDef interaction;

		// Token: 0x040019E8 RID: 6632
		public bool endIfCantShootTargetFromCurPos;

		// Token: 0x040019E9 RID: 6633
		public bool endIfCantShootInMelee;

		// Token: 0x040019EA RID: 6634
		public bool checkEncumbrance;

		// Token: 0x040019EB RID: 6635
		public float followRadius;

		// Token: 0x040019EC RID: 6636
		public bool endAfterTendedOnce;

		// Token: 0x040019ED RID: 6637
		public Quest quest;

		// Token: 0x040019EE RID: 6638
		public Mote mote;

		// Token: 0x040019EF RID: 6639
		public float psyfocusTargetLast = -1f;

		// Token: 0x040019F0 RID: 6640
		public bool wasOnMeditationTimeAssignment;

		// Token: 0x040019F1 RID: 6641
		public bool reactingToMeleeThreat;

		// Token: 0x040019F2 RID: 6642
		public bool preventFriendlyFire;

		// Token: 0x040019F3 RID: 6643
		public bool endIfAllyNotAThreatAnymore;

		// Token: 0x040019F4 RID: 6644
		public RopingPriority ropingPriority;

		// Token: 0x040019F5 RID: 6645
		public bool ropeToUnenclosedPens;

		// Token: 0x040019F6 RID: 6646
		public bool showSpeechBubbles = true;

		// Token: 0x040019F7 RID: 6647
		public Direction8Way lookDirection = Direction8Way.Invalid;

		// Token: 0x040019F8 RID: 6648
		public Rot4 overrideFacing = Rot4.Invalid;

		// Token: 0x040019F9 RID: 6649
		public string ritualTag;

		// Token: 0x040019FA RID: 6650
		public int takeInventoryDelay;

		// Token: 0x040019FB RID: 6651
		public bool draftedTend;

		// Token: 0x040019FC RID: 6652
		public SoundDef speechSoundMale;

		// Token: 0x040019FD RID: 6653
		public SoundDef speechSoundFemale;

		// Token: 0x040019FE RID: 6654
		public ThinkTreeDef jobGiverThinkTree;

		// Token: 0x040019FF RID: 6655
		public ThinkNode jobGiver;

		// Token: 0x04001A00 RID: 6656
		public WorkGiverDef workGiverDef;

		// Token: 0x04001A01 RID: 6657
		public Ability ability;

		// Token: 0x04001A02 RID: 6658
		private JobDriver cachedDriver;

		// Token: 0x04001A03 RID: 6659
		private JobDriver lastJobDriverMade;

		// Token: 0x04001A04 RID: 6660
		private int jobGiverKey = -1;
	}
}
