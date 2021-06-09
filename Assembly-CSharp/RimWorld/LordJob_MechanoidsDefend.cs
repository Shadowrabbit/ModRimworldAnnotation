using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DC8 RID: 3528
	public class LordJob_MechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x0600506F RID: 20591 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06005070 RID: 20592 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x00038772 File Offset: 0x00036972
		public LordJob_MechanoidsDefend()
		{
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x0003877A File Offset: 0x0003697A
		public LordJob_MechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			this.things.AddRange(things);
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x001B7CEC File Offset: 0x001B5EEC
		public LordJob_MechanoidsDefend(SpawnedPawnParams parms)
		{
			this.things.Add(parms.spawnerThing);
			this.faction = parms.spawnerThing.Faction;
			this.defendRadius = parms.defendRadius;
			this.defSpot = parms.defSpot;
			this.canAssaultColony = false;
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x001B7D40 File Offset: 0x001B5F40
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!this.defSpot.IsValid)
			{
				Log.Warning("LordJob_MechanoidsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.", false);
				stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph());
				return stateGraph;
			}
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, this.defendRadius, null);
			stateGraph.StartingToil = lordToil_DefendPoint;
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false);
			stateGraph.AddToil(lordToil_AssaultColony);
			if (this.canAssaultColony)
			{
				LordToil_AssaultColony lordToil_AssaultColony2 = new LordToil_AssaultColony(false);
				stateGraph.AddToil(lordToil_AssaultColony2);
				Transition transition = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition.AddSource(lordToil_AssaultColony2);
				transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
				transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
				transition2.AddTrigger(new Trigger_PawnLostViolently(true));
				transition2.AddTrigger(new Trigger_Memo(LordJob_MechanoidsDefend.MemoDamaged));
				transition2.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition2, false);
				Transition transition3 = new Transition(lordToil_AssaultColony2, lordToil_DefendPoint, false, true);
				transition3.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1380, new string[]
				{
					LordJob_MechanoidsDefend.MemoDamaged
				}));
				transition3.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
				stateGraph.AddTransition(transition3, false);
				Transition transition4 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition4.AddSource(lordToil_AssaultColony2);
				transition4.AddTrigger(new Trigger_AnyThingDamageTaken(this.things, 0.5f));
				transition4.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
				stateGraph.AddTransition(transition4, false);
			}
			Transition transition5 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
			transition5.AddTrigger(new Trigger_ChanceOnSignal(TriggerSignalType.MechClusterDefeated, 1f));
			stateGraph.AddTransition(transition5, false);
			if (!this.isMechCluster)
			{
				Transition transition6 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition6.AddTrigger(new Trigger_AnyThingDamageTaken(this.things, 1f));
				stateGraph.AddTransition(transition6, false);
			}
			return stateGraph;
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x001B7F30 File Offset: 0x001B6130
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shipPart, "shipPart", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				if (this.shipPart != null)
				{
					if (this.things == null)
					{
						this.things = new List<Thing>();
					}
					this.things.Add(this.shipPart);
					this.shipPart = null;
				}
			}
		}

		// Token: 0x040033F2 RID: 13298
		private Thing shipPart;

		// Token: 0x040033F3 RID: 13299
		public static readonly string MemoDamaged = "ShipPartDamaged";
	}
}
