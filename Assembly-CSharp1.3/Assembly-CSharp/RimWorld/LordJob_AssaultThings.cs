using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200086D RID: 2157
	public class LordJob_AssaultThings : LordJob
	{
		// Token: 0x060038EF RID: 14575 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_AssaultThings()
		{
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x0013ED2A File Offset: 0x0013CF2A
		public LordJob_AssaultThings(Faction assaulterFaction, List<Thing> things, float damageFraction = 1f, bool useAvoidGridSmart = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.things = things;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0013ED50 File Offset: 0x0013CF50
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = new LordToil_AssaultThings(this.things);
			if (this.useAvoidGridSmart)
			{
				lordToil.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			lordToil_ExitMapAndDefendSelf.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			Transition transition = new Transition(lordToil, lordToil_ExitMapAndDefendSelf, false, true);
			transition.AddTrigger(new Trigger_ThingsDamageTaken(this.things, this.damageFraction));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0013EDC4 File Offset: 0x0013CFC4
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<float>(ref this.damageFraction, "damageFraction", 0f, false);
		}

		// Token: 0x04001F43 RID: 8003
		private Faction assaulterFaction;

		// Token: 0x04001F44 RID: 8004
		private List<Thing> things;

		// Token: 0x04001F45 RID: 8005
		private bool useAvoidGridSmart;

		// Token: 0x04001F46 RID: 8006
		private float damageFraction;
	}
}
