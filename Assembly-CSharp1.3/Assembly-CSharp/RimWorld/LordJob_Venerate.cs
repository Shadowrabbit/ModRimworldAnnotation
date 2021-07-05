using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000889 RID: 2185
	public class LordJob_Venerate : LordJob
	{
		// Token: 0x060039B1 RID: 14769 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_Venerate()
		{
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x00142E3C File Offset: 0x0014103C
		public LordJob_Venerate(Thing target, int venerateDurationTicks, string outSignalVenerationCompleted = null, string forceExitSignal = null)
		{
			this.target = target;
			this.venerateDurationTicks = venerateDurationTicks;
			this.outSignalVenerationCompleted = outSignalVenerationCompleted;
			this.forceExitSignal = forceExitSignal;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x00142E64 File Offset: 0x00141064
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.target.InteractionCell);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_Venerate lordToil_Venerate = new LordToil_Venerate(this.target);
			stateGraph.AddToil(lordToil_Venerate);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMapAndDefendSelf toil = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(toil);
			Transition transition = new Transition(lordToil_Travel, lordToil_Venerate, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Venerate, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(this.venerateDurationTicks));
			if (!this.outSignalVenerationCompleted.NullOrEmpty())
			{
				transition2.AddPostAction(new TransitionAction_Custom(delegate()
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalVenerationCompleted));
				}));
			}
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Venerate, toil, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMap);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_ExitMap, false, true);
			transition4.AddSource(lordToil_Venerate);
			transition4.AddTrigger(new Trigger_AnyThingDamageTaken(new List<Thing>
			{
				this.target
			}, 1f));
			if (this.forceExitSignal != null)
			{
				transition4.AddTrigger(new Trigger_Signal(this.forceExitSignal));
			}
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4, false);
			return stateGraph;
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x00142FE4 File Offset: 0x001411E4
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<int>(ref this.venerateDurationTicks, "venerateDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalVenerationCompleted, "outSignalVenerationCompleted", null, false);
			Scribe_Values.Look<string>(ref this.forceExitSignal, "forceExitSignal", null, false);
		}

		// Token: 0x04001FA9 RID: 8105
		private Thing target;

		// Token: 0x04001FAA RID: 8106
		private int venerateDurationTicks;

		// Token: 0x04001FAB RID: 8107
		private string outSignalVenerationCompleted;

		// Token: 0x04001FAC RID: 8108
		private string forceExitSignal;
	}
}
