using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200086F RID: 2159
	public class LordJob_BegForItems : LordJob
	{
		// Token: 0x060038F7 RID: 14583 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_BegForItems()
		{
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0013F0A8 File Offset: 0x0013D2A8
		public LordJob_BegForItems(Faction faction, IntVec3 idleSpot, Pawn target, ThingDef thingDef, int amount, string outSignalItemsReceived = null)
		{
			this.idleSpot = idleSpot;
			this.faction = faction;
			this.target = target;
			this.thingDef = thingDef;
			this.amount = amount;
			this.outSignalItemsReceived = outSignalItemsReceived;
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x0013F0E0 File Offset: 0x0013D2E0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckIdeology("Beg for items"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.idleSpot);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_WaitForItems waitForItems = new LordToil_WaitForItems(this.target, this.thingDef, this.amount, this.idleSpot);
			stateGraph.AddToil(waitForItems);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMapAndDefendSelf toil = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(toil);
			Transition transition = new Transition(lordToil_Travel, waitForItems, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(waitForItems, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_Custom((TriggerSignal s) => waitForItems.HasAllRequestedItems));
			if (!this.outSignalItemsReceived.NullOrEmpty())
			{
				transition2.AddPostAction(new TransitionAction_Custom(delegate()
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalItemsReceived));
				}));
			}
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(waitForItems, toil, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMap);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0013F248 File Offset: 0x0013D448
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.idleSpot, "idleSpot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalItemsReceived, "outSignalItemsReceived", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x04001F49 RID: 8009
		private IntVec3 idleSpot;

		// Token: 0x04001F4A RID: 8010
		private Faction faction;

		// Token: 0x04001F4B RID: 8011
		private Pawn target;

		// Token: 0x04001F4C RID: 8012
		private ThingDef thingDef;

		// Token: 0x04001F4D RID: 8013
		private int amount;

		// Token: 0x04001F4E RID: 8014
		private string outSignalItemsReceived;
	}
}
