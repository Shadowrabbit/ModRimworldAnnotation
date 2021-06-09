using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DC0 RID: 3520
	public class LordJob_ExitOnShuttle : LordJob
	{
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06005039 RID: 20537 RVA: 0x00038512 File Offset: 0x00036712
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600503A RID: 20538 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool RemoveDownedPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x0003851A File Offset: 0x0003671A
		public LordJob_ExitOnShuttle()
		{
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x00038529 File Offset: 0x00036729
		public LordJob_ExitOnShuttle(Thing shuttle, bool addFleeToil = true)
		{
			this.shuttle = shuttle;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x001B736C File Offset: 0x001B556C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle crash rescue is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 3454535, false);
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

		// Token: 0x0600503E RID: 20542 RVA: 0x00038546 File Offset: 0x00036746
		private void InitializeLoading()
		{
			if (!this.shuttle.TryGetComp<CompTransporter>().LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.shuttle.TryGetComp<CompTransporter>()));
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x00038570 File Offset: 0x00036770
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x040033D3 RID: 13267
		public Thing shuttle;

		// Token: 0x040033D4 RID: 13268
		private bool addFleeToil = true;
	}
}
