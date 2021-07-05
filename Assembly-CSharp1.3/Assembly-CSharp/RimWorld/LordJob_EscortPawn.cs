using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000878 RID: 2168
	public class LordJob_EscortPawn : LordJob
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_EscortPawn()
		{
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x0014080C File Offset: 0x0013EA0C
		public LordJob_EscortPawn(Pawn escortee, Thing shuttle = null)
		{
			this.escortee = escortee;
			this.shuttle = shuttle;
			this.escorteeFaction = escortee.Faction;
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x00140830 File Offset: 0x0013EA30
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
			Trigger_Custom trigger2 = new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && (this.escortee.MapHeld != this.lord.Map || (this.shuttle != null && this.escortee.ParentHolder == this.shuttle.TryGetComp<CompTransporter>() && this.shuttle.TryGetComp<CompShuttle>().shipParent.Waiting)));
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

		// Token: 0x0600393B RID: 14651 RVA: 0x00140929 File Offset: 0x0013EB29
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.escorteeFaction, "escorteeFaction", false);
		}

		// Token: 0x04001F72 RID: 8050
		public Pawn escortee;

		// Token: 0x04001F73 RID: 8051
		public Thing shuttle;

		// Token: 0x04001F74 RID: 8052
		private Faction escorteeFaction;
	}
}
