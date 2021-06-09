using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DBF RID: 3519
	public class LordJob_EscortPawn : LordJob
	{
		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06005031 RID: 20529 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_EscortPawn()
		{
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x00038480 File Offset: 0x00036680
		public LordJob_EscortPawn(Pawn escortee, Thing shuttle = null)
		{
			this.escortee = escortee;
			this.shuttle = shuttle;
			this.escorteeFaction = escortee.Faction;
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x001B7208 File Offset: 0x001B5408
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_EscortPawn lordToil_EscortPawn = new LordToil_EscortPawn(this.escortee, 7f);
			stateGraph.AddToil(lordToil_EscortPawn);
			LordToil lordToil = (this.shuttle == null) ? new LordToil_ExitMap(LocomotionUrgency.None, false, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil);
			Transition transition = new Transition(lordToil_EscortPawn, lordToil, false, true);
			Trigger_Custom trigger = new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.escortee.Dead);
			transition.AddTrigger(trigger);
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_EscortPawn, lordToil, false, true);
			Trigger_Custom trigger2 = new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && (this.escortee.MapHeld != this.lord.Map || (this.shuttle != null && this.escortee.ParentHolder == this.shuttle.TryGetComp<CompTransporter>() && !this.shuttle.TryGetComp<CompShuttle>().dropEverythingOnArrival)));
			transition2.AddTrigger(trigger2);
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_EscortPawn, lordToil, false, true);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_EscortPawn, lordToil, false, true);
			transition4.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.escortee.Faction != this.escorteeFaction));
			stateGraph.AddTransition(transition4, false);
			return stateGraph;
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x000384A2 File Offset: 0x000366A2
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.escorteeFaction, "escorteeFaction", false);
		}

		// Token: 0x040033D0 RID: 13264
		public Pawn escortee;

		// Token: 0x040033D1 RID: 13265
		public Thing shuttle;

		// Token: 0x040033D2 RID: 13266
		private Faction escorteeFaction;
	}
}
