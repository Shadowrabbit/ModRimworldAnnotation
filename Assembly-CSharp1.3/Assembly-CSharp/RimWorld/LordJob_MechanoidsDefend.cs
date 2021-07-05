using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class LordJob_MechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x0600396F RID: 14703 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x00141788 File Offset: 0x0013F988
		public LordJob_MechanoidsDefend()
		{
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00141790 File Offset: 0x0013F990
		public LordJob_MechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			this.things.AddRange(things);
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x001417CC File Offset: 0x0013F9CC
		public LordJob_MechanoidsDefend(SpawnedPawnParams parms)
		{
			this.things.Add(parms.spawnerThing);
			this.faction = parms.spawnerThing.Faction;
			this.defendRadius = parms.defendRadius;
			this.defSpot = parms.defSpot;
			this.canAssaultColony = false;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00141820 File Offset: 0x0013FA20
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!this.defSpot.IsValid)
			{
				Log.Warning("LordJob_MechanoidsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.");
				stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph());
				return stateGraph;
			}
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, this.defendRadius, null);
			stateGraph.StartingToil = lordToil_DefendPoint;
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false, false);
			stateGraph.AddToil(lordToil_AssaultColony);
			if (this.canAssaultColony)
			{
				LordToil_AssaultColony lordToil_AssaultColony2 = new LordToil_AssaultColony(false, false);
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

		// Token: 0x06003974 RID: 14708 RVA: 0x00141A14 File Offset: 0x0013FC14
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

		// Token: 0x04001F92 RID: 8082
		private Thing shipPart;

		// Token: 0x04001F93 RID: 8083
		public static readonly string MemoDamaged = "ShipPartDamaged";
	}
}
