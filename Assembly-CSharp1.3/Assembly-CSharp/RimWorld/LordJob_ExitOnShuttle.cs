using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000879 RID: 2169
	public class LordJob_ExitOnShuttle : LordJob
	{
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x00140A04 File Offset: 0x0013EC04
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003940 RID: 14656 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool RemoveDownedPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x00140A0C File Offset: 0x0013EC0C
		public LordJob_ExitOnShuttle()
		{
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x00140A1B File Offset: 0x0013EC1B
		public LordJob_ExitOnShuttle(Thing shuttle, bool addFleeToil = true)
		{
			this.shuttle = shuttle;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x00140A38 File Offset: 0x0013EC38
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return stateGraph;
			}
			LordToil_Wait lordToil_Wait = new LordToil_Wait();
			stateGraph.AddToil(lordToil_Wait);
			stateGraph.StartingToil = lordToil_Wait;
			LordToil_EnterShuttleOrLeave lordToil_EnterShuttleOrLeave = new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Sprint, false, false);
			stateGraph.AddToil(lordToil_EnterShuttleOrLeave);
			Transition transition = new Transition(lordToil_Wait, lordToil_EnterShuttleOrLeave, false, true);
			transition.AddPreAction(new TransitionAction_Custom(new Action(this.InitializeLoading)));
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.shuttle.Spawned));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x00140AC4 File Offset: 0x0013ECC4
		private void InitializeLoading()
		{
			if (!this.shuttle.TryGetComp<CompTransporter>().LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.shuttle.TryGetComp<CompTransporter>()));
			}
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x00140AEE File Offset: 0x0013ECEE
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04001F75 RID: 8053
		public Thing shuttle;

		// Token: 0x04001F76 RID: 8054
		private bool addFleeToil = true;
	}
}
