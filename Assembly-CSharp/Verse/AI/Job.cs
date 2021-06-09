using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000968 RID: 2408
	public class Job : IExposable, ILoadReferenceable
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x0002D3F6 File Offset: 0x0002B5F6
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

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x0002D40D File Offset: 0x0002B60D
		public JobDriver GetCachedDriverDirect
		{
			get
			{
				return this.cachedDriver;
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0016BD3C File Offset: 0x00169F3C
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
			this.canBash = false;
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
			this.jobGiverThinkTree = null;
			this.jobGiver = null;
			this.workGiverDef = null;
			this.ability = null;
			if (this.cachedDriver != null)
			{
				this.cachedDriver.job = null;
			}
			this.cachedDriver = null;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0016BF08 File Offset: 0x0016A108
		public Job()
		{
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0002D415 File Offset: 0x0002B615
		public Job(JobDef def) : this(def, null)
		{
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0002D424 File Offset: 0x0002B624
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0016BF9C File Offset: 0x0016A19C
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x0016C054 File Offset: 0x0016A254
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0016C114 File Offset: 0x0016A314
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0016C1D4 File Offset: 0x0016A3D4
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0002D434 File Offset: 0x0002B634
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

		// Token: 0x06003AEB RID: 15083 RVA: 0x0016C28C File Offset: 0x0016A48C
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

		// Token: 0x06003AEC RID: 15084 RVA: 0x0002D466 File Offset: 0x0002B666
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

		// Token: 0x06003AED RID: 15085 RVA: 0x0002D49B File Offset: 0x0002B69B
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x0016C2DC File Offset: 0x0016A4DC
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
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
			Scribe_Values.Look<bool>(ref this.canBash, "canBash", false, false);
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
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.jobGiverKey = ((this.jobGiver != null) ? this.jobGiver.UniqueSaveKey : -1);
			}
			Scribe_Values.Look<int>(ref this.jobGiverKey, "lastJobGiverKey", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.jobGiverKey != -1 && !this.jobGiverThinkTree.TryGetThinkNodeWithSaveKey(this.jobGiverKey, out this.jobGiver))
			{
				Log.Warning("Could not find think node with key " + this.jobGiverKey, false);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbToUse != null && this.verbToUse.BuggedAfterLoading)
			{
				this.verbToUse = null;
				Log.Warning(base.GetType() + " had a bugged verbToUse after loading.", false);
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x0002D4AA File Offset: 0x0002B6AA
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			return jobDriver;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x0016C788 File Offset: 0x0016A988
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
				}), false);
			}
			return this.cachedDriver;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x0002D4CF File Offset: 0x0002B6CF
		public bool TryMakePreToilReservations(Pawn driverPawn, bool errorOnFailed)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations(errorOnFailed);
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x0002D4DE File Offset: 0x0002B6DE
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x0002D4EC File Offset: 0x0002B6EC
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x0002D4F4 File Offset: 0x0002B6F4
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0016C814 File Offset: 0x0016AA14
		public bool JobIsSameAs(Job other)
		{
			return other != null && (this == other || (this.def == other.def && !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && this.verbToUse == other.verbToUse && !(this.targetC != other.targetC) && this.commTarget == other.commTarget && this.bill == other.bill));
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x0016C8A0 File Offset: 0x0016AAA0
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x0016C910 File Offset: 0x0016AB10
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

		// Token: 0x06003AF8 RID: 15096 RVA: 0x0016C9EC File Offset: 0x0016ABEC
		private static bool IsTargetOutsideArea(LocalTargetInfo target, Area zone)
		{
			IntVec3 cell = target.Cell;
			return cell.IsValid && !zone[cell];
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x0016CA18 File Offset: 0x0016AC18
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

		// Token: 0x06003AFA RID: 15098 RVA: 0x0002D512 File Offset: 0x0002B712
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID;
		}

		// Token: 0x040028CF RID: 10447
		public JobDef def;

		// Token: 0x040028D0 RID: 10448
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		// Token: 0x040028D1 RID: 10449
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		// Token: 0x040028D2 RID: 10450
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		// Token: 0x040028D3 RID: 10451
		public List<LocalTargetInfo> targetQueueA;

		// Token: 0x040028D4 RID: 10452
		public List<LocalTargetInfo> targetQueueB;

		// Token: 0x040028D5 RID: 10453
		public GlobalTargetInfo globalTarget = GlobalTargetInfo.Invalid;

		// Token: 0x040028D6 RID: 10454
		public int count = -1;

		// Token: 0x040028D7 RID: 10455
		public List<int> countQueue;

		// Token: 0x040028D8 RID: 10456
		public int loadID = -1;

		// Token: 0x040028D9 RID: 10457
		public int startTick = -1;

		// Token: 0x040028DA RID: 10458
		public int expiryInterval = -1;

		// Token: 0x040028DB RID: 10459
		public bool checkOverrideOnExpire;

		// Token: 0x040028DC RID: 10460
		public bool playerForced;

		// Token: 0x040028DD RID: 10461
		public List<ThingCountClass> placedThings;

		// Token: 0x040028DE RID: 10462
		public int maxNumMeleeAttacks = int.MaxValue;

		// Token: 0x040028DF RID: 10463
		public int maxNumStaticAttacks = int.MaxValue;

		// Token: 0x040028E0 RID: 10464
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		// Token: 0x040028E1 RID: 10465
		public HaulMode haulMode;

		// Token: 0x040028E2 RID: 10466
		public Bill bill;

		// Token: 0x040028E3 RID: 10467
		public ICommunicable commTarget;

		// Token: 0x040028E4 RID: 10468
		public ThingDef plantDefToSow;

		// Token: 0x040028E5 RID: 10469
		public Verb verbToUse;

		// Token: 0x040028E6 RID: 10470
		public bool haulOpportunisticDuplicates;

		// Token: 0x040028E7 RID: 10471
		public bool exitMapOnArrival;

		// Token: 0x040028E8 RID: 10472
		public bool failIfCantJoinOrCreateCaravan;

		// Token: 0x040028E9 RID: 10473
		public bool killIncappedTarget;

		// Token: 0x040028EA RID: 10474
		public bool ignoreForbidden;

		// Token: 0x040028EB RID: 10475
		public bool ignoreDesignations;

		// Token: 0x040028EC RID: 10476
		public bool canBash;

		// Token: 0x040028ED RID: 10477
		public bool canUseRangedWeapon = true;

		// Token: 0x040028EE RID: 10478
		public bool haulDroppedApparel;

		// Token: 0x040028EF RID: 10479
		public bool restUntilHealed;

		// Token: 0x040028F0 RID: 10480
		public bool ignoreJoyTimeAssignment;

		// Token: 0x040028F1 RID: 10481
		public bool doUntilGatheringEnded;

		// Token: 0x040028F2 RID: 10482
		public bool overeat;

		// Token: 0x040028F3 RID: 10483
		public bool attackDoorIfTargetLost;

		// Token: 0x040028F4 RID: 10484
		public int takeExtraIngestibles;

		// Token: 0x040028F5 RID: 10485
		public bool expireRequiresEnemiesNearby;

		// Token: 0x040028F6 RID: 10486
		public Lord lord;

		// Token: 0x040028F7 RID: 10487
		public bool collideWithPawns;

		// Token: 0x040028F8 RID: 10488
		public bool forceSleep;

		// Token: 0x040028F9 RID: 10489
		public InteractionDef interaction;

		// Token: 0x040028FA RID: 10490
		public bool endIfCantShootTargetFromCurPos;

		// Token: 0x040028FB RID: 10491
		public bool endIfCantShootInMelee;

		// Token: 0x040028FC RID: 10492
		public bool checkEncumbrance;

		// Token: 0x040028FD RID: 10493
		public float followRadius;

		// Token: 0x040028FE RID: 10494
		public bool endAfterTendedOnce;

		// Token: 0x040028FF RID: 10495
		public Quest quest;

		// Token: 0x04002900 RID: 10496
		public Mote mote;

		// Token: 0x04002901 RID: 10497
		public float psyfocusTargetLast = -1f;

		// Token: 0x04002902 RID: 10498
		public bool wasOnMeditationTimeAssignment;

		// Token: 0x04002903 RID: 10499
		public bool reactingToMeleeThreat;

		// Token: 0x04002904 RID: 10500
		public ThinkTreeDef jobGiverThinkTree;

		// Token: 0x04002905 RID: 10501
		public ThinkNode jobGiver;

		// Token: 0x04002906 RID: 10502
		public WorkGiverDef workGiverDef;

		// Token: 0x04002907 RID: 10503
		public Ability ability;

		// Token: 0x04002908 RID: 10504
		private JobDriver cachedDriver;

		// Token: 0x04002909 RID: 10505
		private int jobGiverKey = -1;
	}
}
