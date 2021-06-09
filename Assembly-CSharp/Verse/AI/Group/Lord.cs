using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000AC4 RID: 2756
	[StaticConstructorOnStartup]
	public class Lord : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x00030978 File Offset: 0x0002EB78
		public Map Map
		{
			get
			{
				return this.lordManager.map;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06004104 RID: 16644 RVA: 0x00030985 File Offset: 0x0002EB85
		public StateGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x0003098D File Offset: 0x0002EB8D
		public LordToil CurLordToil
		{
			get
			{
				return this.curLordToil;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06004106 RID: 16646 RVA: 0x00030995 File Offset: 0x0002EB95
		public LordJob LordJob
		{
			get
			{
				return this.curJob;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06004107 RID: 16647 RVA: 0x0003099D File Offset: 0x0002EB9D
		private bool CanExistWithoutPawns
		{
			get
			{
				return this.curJob is LordJob_VoluntarilyJoinable;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06004108 RID: 16648 RVA: 0x000309AD File Offset: 0x0002EBAD
		private bool ShouldExist
		{
			get
			{
				return this.ownedPawns.Count > 0 || this.CanExistWithoutPawns || (this.ownedBuildings.Count > 0 && this.curJob.KeepExistingWhileHasAnyBuilding);
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x001860E4 File Offset: 0x001842E4
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

		// Token: 0x0600410A RID: 16650 RVA: 0x000309E2 File Offset: 0x0002EBE2
		private void Init()
		{
			this.initialized = true;
			this.initialColonyHealthTotal = this.Map.wealthWatcher.HealthTotal;
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00030A01 File Offset: 0x0002EC01
		public string GetUniqueLoadID()
		{
			return "Lord_" + this.loadID;
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00186138 File Offset: 0x00184338
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

		// Token: 0x0600410D RID: 16653 RVA: 0x001862EC File Offset: 0x001844EC
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
				LordJob job = this.curJob;
				this.curJob = null;
				this.SetJob(job);
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
						}), false);
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
						}), false);
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
					}), false);
					return;
				}
				this.curLordToil = this.graph.lordToils[this.tmpCurLordToilIdx];
			}
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x001866C8 File Offset: 0x001848C8
		public void SetJob(LordJob lordJob)
		{
			if (this.curJob != null)
			{
				this.curJob.Cleanup();
			}
			this.curJob = lordJob;
			this.curLordToil = null;
			lordJob.lord = this;
			Rand.PushState();
			Rand.Seed = this.loadID * 193;
			this.graph = lordJob.CreateGraph();
			Rand.PopState();
			this.graph.ErrorCheck();
			if (this.faction != null && !this.faction.IsPlayer && this.faction.def.autoFlee && lordJob.AddFleeToil)
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

		// Token: 0x0600410F RID: 16655 RVA: 0x001868A4 File Offset: 0x00184AA4
		public void Cleanup()
		{
			this.curJob.Cleanup();
			if (this.curLordToil != null)
			{
				this.curLordToil.Cleanup();
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
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x00186970 File Offset: 0x00184B70
		public void AddPawn(Pawn p)
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
				}), false);
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
				}), false);
				return;
			}
			this.ownedPawns.Add(p);
			this.numPawnsEverGained++;
			this.Map.attackTargetsCache.UpdateTarget(p);
			this.curLordToil.UpdateAllDuties();
			this.curJob.Notify_PawnAdded(p);
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00186A54 File Offset: 0x00184C54
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
				}), false);
				return;
			}
			this.ownedBuildings.Add(b);
			this.curLordToil.UpdateAllDuties();
			this.curJob.Notify_BuildingAdded(b);
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00030A18 File Offset: 0x0002EC18
		private void RemovePawn(Pawn p)
		{
			this.ownedPawns.Remove(p);
			if (p.mindState != null)
			{
				p.mindState.duty = null;
			}
			this.Map.attackTargetsCache.UpdateTarget(p);
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x00186AD0 File Offset: 0x00184CD0
		public void GotoToil(LordToil newLordToil)
		{
			LordToil previousToil = this.curLordToil;
			if (this.curLordToil != null)
			{
				this.curLordToil.Cleanup();
			}
			this.curLordToil = newLordToil;
			this.ticksInToil = 0;
			if (this.curLordToil.lord != this)
			{
				Log.Error("curLordToil lord is " + ((this.curLordToil.lord == null) ? "null (forgot to add toil to graph?)" : this.curLordToil.lord.ToString()), false);
				this.curLordToil.lord = this;
			}
			this.curLordToil.Init();
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil))
				{
					this.graph.transitions[i].SourceToilBecameActive(this.graph.transitions[i], previousToil);
				}
			}
			this.curLordToil.UpdateAllDuties();
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x00030A4C File Offset: 0x0002EC4C
		public void LordTick()
		{
			if (!this.initialized)
			{
				this.Init();
			}
			this.curJob.LordJobTick();
			this.curLordToil.LordToilTick();
			this.CheckTransitionOnSignal(TriggerSignal.ForTick);
			this.ticksInToil++;
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00186BD0 File Offset: 0x00184DD0
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

		// Token: 0x06004116 RID: 16662 RVA: 0x00030A8C File Offset: 0x0002EC8C
		public void ReceiveMemo(string memo)
		{
			this.CheckTransitionOnSignal(TriggerSignal.ForMemo(memo));
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00186C48 File Offset: 0x00184E48
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

		// Token: 0x06004118 RID: 16664 RVA: 0x00030A9B File Offset: 0x0002EC9B
		private void Destroy()
		{
			this.lordManager.RemoveLord(this);
			this.curJob.Notify_LordDestroyed();
			if (this.faction != null)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated");
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x00186CC4 File Offset: 0x00184EC4
		public void Notify_PawnLost(Pawn pawn, PawnLostCondition cond, DamageInfo? dinfo = null)
		{
			if (this.ownedPawns.Contains(pawn))
			{
				this.RemovePawn(pawn);
				if (cond == PawnLostCondition.IncappedOrKilled || cond == PawnLostCondition.MadePrisoner)
				{
					this.numPawnsLostViolently++;
				}
				this.curJob.Notify_PawnLost(pawn, cond);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_PawnLost(pawn, cond);
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
			}), false);
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x00186DA8 File Offset: 0x00184FA8
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
			Log.Error("Lord lost building " + building + " it didn't have.", false);
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00186E5C File Offset: 0x0018505C
		public void Notify_BuildingDamaged(Building building, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.BuildingDamaged,
				thing = building,
				dinfo = dinfo
			});
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x00186E94 File Offset: 0x00185094
		public void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnDamaged,
				thing = victim,
				dinfo = dinfo
			});
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x00186ECC File Offset: 0x001850CC
		public void Notify_PawnAttemptArrested(Pawn victim)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnArrestAttempted,
				thing = victim
			});
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00186EFC File Offset: 0x001850FC
		public void Notify_Clamor(Thing source, ClamorDef clamorType)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.Clamor,
				thing = source,
				clamorType = clamorType
			});
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x00006A05 File Offset: 0x00004C05
		public void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x00030ACC File Offset: 0x0002ECCC
		public void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.curLordToil.Notify_ReachedDutyLocation(pawn);
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x00030ADA File Offset: 0x0002ECDA
		public void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.curLordToil.Notify_ConstructionFailed(pawn, frame, newBlueprint);
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00186F34 File Offset: 0x00185134
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

		// Token: 0x06004123 RID: 16675 RVA: 0x00186FFC File Offset: 0x001851FC
		public void Notify_DormancyWakeup()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.DormancyWakeup
			});
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x00187024 File Offset: 0x00185224
		public void Notify_MechClusterDefeated()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.MechClusterDefeated
			});
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0018704C File Offset: 0x0018524C
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

		// Token: 0x06004126 RID: 16678 RVA: 0x001870CC File Offset: 0x001852CC
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

		// Token: 0x06004127 RID: 16679 RVA: 0x001871D0 File Offset: 0x001853D0
		public void DebugDraw()
		{
			Vector3 a = this.DebugCenter();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid)
			{
				Graphics.DrawMesh(MeshPool.plane14, flagLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Building), Quaternion.identity, Lord.FlagTex, 0);
			}
			GenDraw.DrawLineBetween(a, flagLoc.ToVector3Shifted(), SimpleColor.Red);
			foreach (Pawn pawn in this.ownedPawns)
			{
				SimpleColor color = (!pawn.InMentalState) ? SimpleColor.White : SimpleColor.Yellow;
				GenDraw.DrawLineBetween(a, pawn.DrawPos, color);
			}
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00187280 File Offset: 0x00185480
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

		// Token: 0x06004129 RID: 16681 RVA: 0x00187330 File Offset: 0x00185530
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

		// Token: 0x0600412A RID: 16682 RVA: 0x00187410 File Offset: 0x00185610
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

		// Token: 0x04002CF3 RID: 11507
		public LordManager lordManager;

		// Token: 0x04002CF4 RID: 11508
		private LordToil curLordToil;

		// Token: 0x04002CF5 RID: 11509
		private StateGraph graph;

		// Token: 0x04002CF6 RID: 11510
		public int loadID = -1;

		// Token: 0x04002CF7 RID: 11511
		private LordJob curJob;

		// Token: 0x04002CF8 RID: 11512
		public Faction faction;

		// Token: 0x04002CF9 RID: 11513
		public List<Pawn> ownedPawns = new List<Pawn>();

		// Token: 0x04002CFA RID: 11514
		public List<Building> ownedBuildings = new List<Building>();

		// Token: 0x04002CFB RID: 11515
		public List<Thing> extraForbiddenThings = new List<Thing>();

		// Token: 0x04002CFC RID: 11516
		public List<string> questTags;

		// Token: 0x04002CFD RID: 11517
		public string inSignalLeave;

		// Token: 0x04002CFE RID: 11518
		private bool initialized;

		// Token: 0x04002CFF RID: 11519
		public int ticksInToil;

		// Token: 0x04002D00 RID: 11520
		public int numPawnsLostViolently;

		// Token: 0x04002D01 RID: 11521
		public int numPawnsEverGained;

		// Token: 0x04002D02 RID: 11522
		public int initialColonyHealthTotal;

		// Token: 0x04002D03 RID: 11523
		public int lastPawnHarmTick = -99999;

		// Token: 0x04002D04 RID: 11524
		private const int AttackTargetCacheInterval = 60;

		// Token: 0x04002D05 RID: 11525
		private static readonly Material FlagTex = MaterialPool.MatFrom("UI/Overlays/SquadFlag");

		// Token: 0x04002D06 RID: 11526
		private int tmpCurLordToilIdx = -1;

		// Token: 0x04002D07 RID: 11527
		private Dictionary<int, LordToilData> tmpLordToilData = new Dictionary<int, LordToilData>();

		// Token: 0x04002D08 RID: 11528
		private Dictionary<int, TriggerData> tmpTriggerData = new Dictionary<int, TriggerData>();
	}
}
