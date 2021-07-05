using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000660 RID: 1632
	[StaticConstructorOnStartup]
	public class Lord : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002E20 RID: 11808 RVA: 0x00114763 File Offset: 0x00112963
		public Map Map
		{
			get
			{
				return this.lordManager.map;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002E21 RID: 11809 RVA: 0x00114770 File Offset: 0x00112970
		public StateGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002E22 RID: 11810 RVA: 0x00114778 File Offset: 0x00112978
		public LordToil CurLordToil
		{
			get
			{
				return this.curLordToil;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002E23 RID: 11811 RVA: 0x00114780 File Offset: 0x00112980
		public LordJob LordJob
		{
			get
			{
				return this.curJob;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x00114788 File Offset: 0x00112988
		private bool CanExistWithoutPawns
		{
			get
			{
				return this.curJob is LordJob_VoluntarilyJoinable;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x00114798 File Offset: 0x00112998
		private bool ShouldExist
		{
			get
			{
				return this.ownedPawns.Count > 0 || this.CanExistWithoutPawns || (this.ownedBuildings.Count > 0 && this.curJob.KeepExistingWhileHasAnyBuilding);
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x001147D0 File Offset: 0x001129D0
		public bool AnyActivePawn
		{
			get
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					if (this.ownedPawns[i].mindState != null && this.ownedPawns[i].mindState.Active)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x00114821 File Offset: 0x00112A21
		private void Init()
		{
			this.initialized = true;
			this.initialColonyHealthTotal = this.Map.wealthWatcher.HealthTotal;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x00114840 File Offset: 0x00112A40
		public string GetUniqueLoadID()
		{
			return "Lord_" + this.loadID;
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x00114858 File Offset: 0x00112A58
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.extraForbiddenThings, "extraForbiddenThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.ownedPawns, "ownedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Building>(ref this.ownedBuildings, "ownedBuildings", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<LordJob>(ref this.curJob, "lordJob", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
			Scribe_Values.Look<int>(ref this.ticksInToil, "ticksInToil", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsEverGained, "numPawnsEverGained", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsLostViolently, "numPawnsLostViolently", 0, false);
			Scribe_Values.Look<int>(ref this.initialColonyHealthTotal, "initialColonyHealthTotal", 0, false);
			Scribe_Values.Look<int>(ref this.lastPawnHarmTick, "lastPawnHarmTick", -99999, false);
			Scribe_Values.Look<string>(ref this.inSignalLeave, "inSignalLeave", null, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.extraForbiddenThings.RemoveAll((Thing x) => x == null);
				this.ownedPawns.RemoveAll((Pawn x) => x == null);
				this.ownedBuildings.RemoveAll((Building x) => x == null);
			}
			this.ExposeData_StateGraph();
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x00114A0C File Offset: 0x00112C0C
		private void ExposeData_StateGraph()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpLordToilData.Clear();
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					if (this.graph.lordToils[i].data != null)
					{
						this.tmpLordToilData.Add(i, this.graph.lordToils[i].data);
					}
				}
				this.tmpTriggerData.Clear();
				int num = 0;
				for (int j = 0; j < this.graph.transitions.Count; j++)
				{
					for (int k = 0; k < this.graph.transitions[j].triggers.Count; k++)
					{
						if (this.graph.transitions[j].triggers[k].data != null)
						{
							this.tmpTriggerData.Add(num, this.graph.transitions[j].triggers[k].data);
						}
						num++;
					}
				}
				this.tmpCurLordToilIdx = this.graph.lordToils.IndexOf(this.curLordToil);
			}
			Scribe_Collections.Look<int, LordToilData>(ref this.tmpLordToilData, "lordToilData", LookMode.Value, LookMode.Deep);
			Scribe_Collections.Look<int, TriggerData>(ref this.tmpTriggerData, "triggerData", LookMode.Value, LookMode.Deep);
			Scribe_Values.Look<int>(ref this.tmpCurLordToilIdx, "curLordToilIdx", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curJob.LostImportantReferenceDuringLoading)
				{
					this.lordManager.RemoveLord(this);
					return;
				}
				LordJob lordJob = this.curJob;
				this.curJob = null;
				this.SetJob(lordJob, true);
				foreach (KeyValuePair<int, LordToilData> keyValuePair in this.tmpLordToilData)
				{
					if (keyValuePair.Key < 0 || keyValuePair.Key >= this.graph.lordToils.Count)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not find lord toil for lord toil data of type \"",
							keyValuePair.Value.GetType(),
							"\" (lord job: \"",
							this.curJob.GetType(),
							"\"), because lord toil index is out of bounds: ",
							keyValuePair.Key
						}));
					}
					else
					{
						this.graph.lordToils[keyValuePair.Key].data = keyValuePair.Value;
					}
				}
				this.tmpLordToilData.Clear();
				foreach (KeyValuePair<int, TriggerData> keyValuePair2 in this.tmpTriggerData)
				{
					Trigger triggerByIndex = this.GetTriggerByIndex(keyValuePair2.Key);
					if (triggerByIndex == null)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not find trigger for trigger data of type \"",
							keyValuePair2.Value.GetType(),
							"\" (lord job: \"",
							this.curJob.GetType(),
							"\"), because trigger index is out of bounds: ",
							keyValuePair2.Key
						}));
					}
					else
					{
						triggerByIndex.data = keyValuePair2.Value;
					}
				}
				this.tmpTriggerData.Clear();
				if (this.tmpCurLordToilIdx < 0 || this.tmpCurLordToilIdx >= this.graph.lordToils.Count)
				{
					Log.Error(string.Concat(new object[]
					{
						"Current lord toil index out of bounds (lord job: \"",
						this.curJob.GetType(),
						"\"): ",
						this.tmpCurLordToilIdx
					}));
					return;
				}
				this.curLordToil = this.graph.lordToils[this.tmpCurLordToilIdx];
			}
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x00114DE8 File Offset: 0x00112FE8
		public void SetJob(LordJob lordJob, bool loading = false)
		{
			if (this.curJob != null)
			{
				this.curJob.Cleanup();
			}
			this.curJob = lordJob;
			this.curLordToil = null;
			lordJob.lord = this;
			if (!loading)
			{
				lordJob.Notify_AddedToLord();
			}
			Rand.PushState();
			Rand.Seed = this.loadID * 193;
			this.graph = lordJob.CreateGraph();
			Rand.PopState();
			this.graph.ErrorCheck();
			if (this.faction != null && !this.faction.IsPlayer && this.faction.def.autoFlee && !this.faction.neverFlee && lordJob.AddFleeToil)
			{
				LordToil_PanicFlee lordToil_PanicFlee = new LordToil_PanicFlee();
				lordToil_PanicFlee.useAvoidGrid = true;
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					Transition transition = new Transition(this.graph.lordToils[i], lordToil_PanicFlee, false, true);
					transition.AddPreAction(new TransitionAction_Message("MessageFightersFleeing".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
					transition.AddTrigger(new Trigger_FractionPawnsLost(this.faction.def.attackersDownPercentageRangeForAutoFlee.RandomInRangeSeeded(this.loadID)));
					this.graph.AddTransition(transition, true);
				}
				this.graph.AddToil(lordToil_PanicFlee);
			}
			for (int j = 0; j < this.graph.lordToils.Count; j++)
			{
				this.graph.lordToils[j].lord = this;
			}
			for (int k = 0; k < this.ownedPawns.Count; k++)
			{
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[k]);
			}
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x00114FE0 File Offset: 0x001131E0
		public void Cleanup()
		{
			try
			{
				this.curJob.Cleanup();
			}
			catch (Exception arg)
			{
				Log.Error("Error in LordJob.Cleanup(): " + arg);
			}
			if (this.curLordToil != null)
			{
				try
				{
					this.curLordToil.Cleanup();
				}
				catch (Exception arg2)
				{
					Log.Error("Error in LordToil.Cleanup(): " + arg2);
				}
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].mindState != null)
				{
					this.ownedPawns[i].mindState.duty = null;
				}
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[i]);
				if (this.ownedPawns[i].Spawned && this.ownedPawns[i].CurJob != null)
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			try
			{
				this.curJob.PostCleanup();
			}
			catch (Exception arg3)
			{
				Log.Error("Error in LordJob.PostCleanup(): " + arg3);
			}
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x00115120 File Offset: 0x00113320
		public void AddPawns(IEnumerable<Pawn> pawns)
		{
			foreach (Pawn p in pawns)
			{
				this.AddPawnInternal(p, false);
			}
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception arg)
			{
				Log.Error("Error in LordToil.UpdateAllDuties(): " + arg);
			}
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x00115198 File Offset: 0x00113398
		public void AddPawn(Pawn p)
		{
			this.AddPawnInternal(p, true);
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x001151A4 File Offset: 0x001133A4
		private void AddPawnInternal(Pawn p, bool updateDuties)
		{
			if (this.ownedPawns.Contains(p))
			{
				Log.Error(string.Concat(new object[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					p,
					" whom it already controls."
				}));
				return;
			}
			if (p.GetLord() != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add pawn ",
					p,
					" to lord ",
					this,
					" but this pawn is already a member of lord ",
					p.GetLord(),
					". Pawns can't be members of more than one lord at the same time."
				}));
				return;
			}
			this.ownedPawns.Add(p);
			this.numPawnsEverGained++;
			this.Map.attackTargetsCache.UpdateTarget(p);
			if (updateDuties)
			{
				try
				{
					this.curLordToil.UpdateAllDuties();
				}
				catch (Exception arg)
				{
					Log.Error("Error in LordToil.UpdateAllDuties(): " + arg);
				}
			}
			try
			{
				this.curJob.Notify_PawnAdded(p);
			}
			catch (Exception arg2)
			{
				Log.Error("Error in LordJob.Notify_PawnAdded(): " + arg2);
			}
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x001152D0 File Offset: 0x001134D0
		public void AddBuilding(Building b)
		{
			if (this.ownedBuildings.Contains(b))
			{
				Log.Error(string.Concat(new object[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					b,
					" which it already controls."
				}));
				return;
			}
			this.ownedBuildings.Add(b);
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception arg)
			{
				Log.Error("Error in LordToil.UpdateAllDuties(): " + arg);
			}
			try
			{
				this.curJob.Notify_BuildingAdded(b);
			}
			catch (Exception arg2)
			{
				Log.Error("Error in LordJob.Notify_BuildingAdded(): " + arg2);
			}
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x00115390 File Offset: 0x00113590
		public void RemovePawn(Pawn p)
		{
			this.ownedPawns.Remove(p);
			if (p.mindState != null)
			{
				p.mindState.duty = null;
			}
			this.Map.attackTargetsCache.UpdateTarget(p);
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x001153C4 File Offset: 0x001135C4
		public void RemovePawns(List<Pawn> pawns)
		{
			foreach (Pawn p in pawns)
			{
				this.RemovePawn(p);
			}
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x00115414 File Offset: 0x00113614
		public void GotoToil(LordToil newLordToil)
		{
			LordToil previousToil = this.curLordToil;
			if (this.curLordToil != null)
			{
				try
				{
					this.curLordToil.Cleanup();
				}
				catch (Exception arg)
				{
					Log.Error("Error in LordToil.Cleanup(): " + arg);
				}
			}
			this.curLordToil = newLordToil;
			this.ticksInToil = 0;
			if (this.curLordToil.lord != this)
			{
				Log.Error("curLordToil lord is " + ((this.curLordToil.lord == null) ? "null (forgot to add toil to graph?)" : this.curLordToil.lord.ToString()));
				this.curLordToil.lord = this;
			}
			try
			{
				this.curLordToil.Init();
			}
			catch (Exception arg2)
			{
				Log.Error("Error in LordToil.Init(): " + arg2);
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil))
				{
					this.graph.transitions[i].SourceToilBecameActive(this.graph.transitions[i], previousToil);
				}
			}
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception arg3)
			{
				Log.Error("Error in LordToil.UpdateAllDuties(): " + arg3);
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x00115580 File Offset: 0x00113780
		public void LordTick()
		{
			if (this.ticksInToil % 60 == 0)
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					Pawn pawn = this.ownedPawns[i];
					if (Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.Free)
					{
						Log.ErrorOnce(string.Concat(new object[]
						{
							"Lord ",
							this,
							" (",
							this.curJob,
							") owns a free world pawn ",
							pawn.LabelShort,
							". Is there WorldPawnSituation to be defined?"
						}), this.loadID ^ 1357461551);
					}
				}
			}
			if (!this.initialized)
			{
				this.Init();
			}
			this.curJob.LordJobTick();
			this.curLordToil.LordToilTick();
			this.CheckTransitionOnSignal(TriggerSignal.ForTick);
			this.ticksInToil++;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0011565C File Offset: 0x0011385C
		private Trigger GetTriggerByIndex(int index)
		{
			int num = 0;
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				for (int j = 0; j < this.graph.transitions[i].triggers.Count; j++)
				{
					if (num == index)
					{
						return this.graph.transitions[i].triggers[j];
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x001156D2 File Offset: 0x001138D2
		public void ReceiveMemo(string memo)
		{
			this.CheckTransitionOnSignal(TriggerSignal.ForMemo(memo));
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x001156E4 File Offset: 0x001138E4
		public void Notify_FactionRelationsChanged(Faction otherFaction, FactionRelationKind previousRelationKind)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.FactionRelationsChanged,
				faction = otherFaction,
				previousRelationKind = new FactionRelationKind?(previousRelationKind)
			});
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].Spawned)
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x00115760 File Offset: 0x00113960
		private void Destroy()
		{
			this.lordManager.RemoveLord(this);
			this.curJob.Notify_LordDestroyed();
			if (this.faction != null)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated");
			}
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x00115794 File Offset: 0x00113994
		public void Notify_PawnLost(Pawn pawn, PawnLostCondition cond, DamageInfo? dinfo = null)
		{
			if (this.ownedPawns.Contains(pawn))
			{
				this.RemovePawn(pawn);
				if (cond == PawnLostCondition.IncappedOrKilled || cond == PawnLostCondition.MadePrisoner)
				{
					this.numPawnsLostViolently++;
				}
				try
				{
					this.curJob.Notify_PawnLost(pawn, cond);
				}
				catch (Exception arg)
				{
					Log.Error("Error in LordJob.Notify_PawnLost(): " + arg);
				}
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					try
					{
						this.curLordToil.Notify_PawnLost(pawn, cond);
					}
					catch (Exception arg2)
					{
						Log.Error("Error in LordToil.Notify_PawnLost(): " + arg2);
					}
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.PawnLost;
					signal.thing = pawn;
					signal.condition = cond;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Lord lost pawn ",
				pawn,
				" it didn't have. Condition=",
				cond
			}));
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x001158C0 File Offset: 0x00113AC0
		public void Notify_InMentalState(Pawn pawn, MentalStateDef def)
		{
			this.curJob.Notify_InMentalState(pawn, def);
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x001158D0 File Offset: 0x00113AD0
		public void Notify_BuildingLost(Building building, DamageInfo? dinfo = null)
		{
			if (this.ownedBuildings.Contains(building))
			{
				this.ownedBuildings.Remove(building);
				this.curJob.Notify_BuildingLost(building);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_BuildingLost(building);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.BuildingLost;
					signal.thing = building;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost building " + building + " it didn't have.");
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x00115984 File Offset: 0x00113B84
		public void Notify_BuildingDamaged(Building building, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.BuildingDamaged,
				thing = building,
				dinfo = dinfo
			});
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x001159BC File Offset: 0x00113BBC
		public void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnDamaged,
				thing = victim,
				dinfo = dinfo
			});
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x001159F4 File Offset: 0x00113BF4
		public void Notify_PawnAttemptArrested(Pawn victim)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnArrestAttempted,
				thing = victim
			});
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x00115A24 File Offset: 0x00113C24
		public void Notify_Clamor(Thing source, ClamorDef clamorType)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.Clamor,
				thing = source,
				clamorType = clamorType
			});
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x0000313F File Offset: 0x0000133F
		public void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x00115A5A File Offset: 0x00113C5A
		public void Notify_BuildingSpawnedOnMap(Building b)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil == null)
			{
				return;
			}
			lordToil.Notify_BuildingSpawnedOnMap(b);
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x00115A6D File Offset: 0x00113C6D
		public void Notify_BuildingDespawnedOnMap(Building b)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil == null)
			{
				return;
			}
			lordToil.Notify_BuildingDespawnedOnMap(b);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x00115A80 File Offset: 0x00113C80
		public void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.curLordToil.Notify_ReachedDutyLocation(pawn);
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x00115A8E File Offset: 0x00113C8E
		public void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.curLordToil.Notify_ConstructionFailed(pawn, frame, newBlueprint);
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x00115AA0 File Offset: 0x00113CA0
		public void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalLeave)
			{
				if (this.ownedPawns.Any<Pawn>() && this.faction != null)
				{
					Messages.Message("MessagePawnsLeaving".Translate(this.faction.def.pawnsPlural), this.ownedPawns, MessageTypeDefOf.NeutralEvent, true);
				}
				LordToil lordToil = this.Graph.lordToils.Find((LordToil st) => st is LordToil_PanicFlee);
				if (lordToil != null)
				{
					this.GotoToil(lordToil);
				}
				else
				{
					this.lordManager.RemoveLord(this);
				}
			}
			this.CheckTransitionOnSignal(TriggerSignal.ForSignal(signal));
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x00115B68 File Offset: 0x00113D68
		public void Notify_DormancyWakeup()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.DormancyWakeup
			});
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x00115B90 File Offset: 0x00113D90
		public void Notify_MechClusterDefeated()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.MechClusterDefeated
			});
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x00115BB8 File Offset: 0x00113DB8
		private bool CheckTransitionOnSignal(TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				this.lastPawnHarmTick = Find.TickManager.TicksGame;
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil) && this.graph.transitions[i].CheckSignal(this, signal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x00115C38 File Offset: 0x00113E38
		private Vector3 DebugCenter()
		{
			Vector3 result = this.Map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			if ((from p in this.ownedPawns
			where p.Spawned
			select p).Any<Pawn>())
			{
				result.x = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.x);
				result.z = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.z);
			}
			return result;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x00115D3C File Offset: 0x00113F3C
		public void DebugDraw()
		{
			Vector3 a = this.DebugCenter();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid)
			{
				Graphics.DrawMesh(MeshPool.plane14, flagLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Building), Quaternion.identity, Lord.FlagTex, 0);
			}
			GenDraw.DrawLineBetween(a, flagLoc.ToVector3Shifted(), SimpleColor.Red, 0.2f);
			foreach (Pawn pawn in this.ownedPawns)
			{
				SimpleColor color = (!pawn.InMentalState) ? SimpleColor.White : SimpleColor.Yellow;
				GenDraw.DrawLineBetween(a, pawn.DrawPos, color, 0.2f);
			}
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x00115DF8 File Offset: 0x00113FF8
		public void DebugOnGUI()
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			string label;
			if (this.CurLordToil != null)
			{
				label = string.Concat(new object[]
				{
					"toil ",
					this.graph.lordToils.IndexOf(this.CurLordToil),
					"\n",
					this.CurLordToil.ToString()
				});
			}
			else
			{
				label = "toil=NULL";
			}
			Vector2 vector = this.DebugCenter().MapToUIPosition();
			Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x00115EA8 File Offset: 0x001140A8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Start steal threshold: " + StealAIUtility.StartStealingMarketValueThreshold(this).ToString("F0"));
			stringBuilder.AppendLine("Duties:");
			foreach (Pawn pawn in this.ownedPawns)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					pawn.LabelCap,
					" - ",
					pawn.mindState.duty
				}));
			}
			stringBuilder.AppendLine("Raw save data:");
			stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(this));
			return stringBuilder.ToString();
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x00115F88 File Offset: 0x00114188
		private bool ShouldDoDebugOutput()
		{
			IntVec3 a = UI.MouseCell();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid && a == flagLoc)
			{
				return true;
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (a == this.ownedPawns[i].Position)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001C6D RID: 7277
		public LordManager lordManager;

		// Token: 0x04001C6E RID: 7278
		private LordToil curLordToil;

		// Token: 0x04001C6F RID: 7279
		private StateGraph graph;

		// Token: 0x04001C70 RID: 7280
		public int loadID = -1;

		// Token: 0x04001C71 RID: 7281
		private LordJob curJob;

		// Token: 0x04001C72 RID: 7282
		public Faction faction;

		// Token: 0x04001C73 RID: 7283
		public List<Pawn> ownedPawns = new List<Pawn>();

		// Token: 0x04001C74 RID: 7284
		public List<Building> ownedBuildings = new List<Building>();

		// Token: 0x04001C75 RID: 7285
		public List<Thing> extraForbiddenThings = new List<Thing>();

		// Token: 0x04001C76 RID: 7286
		public List<string> questTags;

		// Token: 0x04001C77 RID: 7287
		public string inSignalLeave;

		// Token: 0x04001C78 RID: 7288
		private bool initialized;

		// Token: 0x04001C79 RID: 7289
		public int ticksInToil;

		// Token: 0x04001C7A RID: 7290
		public int numPawnsLostViolently;

		// Token: 0x04001C7B RID: 7291
		public int numPawnsEverGained;

		// Token: 0x04001C7C RID: 7292
		public int initialColonyHealthTotal;

		// Token: 0x04001C7D RID: 7293
		public int lastPawnHarmTick = -99999;

		// Token: 0x04001C7E RID: 7294
		private const int AttackTargetCacheInterval = 60;

		// Token: 0x04001C7F RID: 7295
		private static readonly Material FlagTex = MaterialPool.MatFrom("UI/Overlays/SquadFlag");

		// Token: 0x04001C80 RID: 7296
		private int tmpCurLordToilIdx = -1;

		// Token: 0x04001C81 RID: 7297
		private Dictionary<int, LordToilData> tmpLordToilData = new Dictionary<int, LordToilData>();

		// Token: 0x04001C82 RID: 7298
		private Dictionary<int, TriggerData> tmpTriggerData = new Dictionary<int, TriggerData>();
	}
}
