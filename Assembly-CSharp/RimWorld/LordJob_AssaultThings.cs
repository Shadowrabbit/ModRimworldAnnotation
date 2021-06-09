using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DB3 RID: 3507
	public class LordJob_AssaultThings : LordJob
	{
		// Token: 0x06004FF3 RID: 20467 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_AssaultThings()
		{
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x000381EF File Offset: 0x000363EF
		public LordJob_AssaultThings(Faction assaulterFaction, List<Thing> things, float damageFraction = 1f, bool useAvoidGridSmart = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.things = things;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x001B5FD4 File Offset: 0x001B41D4
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

		// Token: 0x06004FF6 RID: 20470 RVA: 0x001B6048 File Offset: 0x001B4248
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<float>(ref this.damageFraction, "damageFraction", 0f, false);
		}

		// Token: 0x040033AA RID: 13226
		private Faction assaulterFaction;

		// Token: 0x040033AB RID: 13227
		private List<Thing> things;

		// Token: 0x040033AC RID: 13228
		private bool useAvoidGridSmart;

		// Token: 0x040033AD RID: 13229
		private float damageFraction;
	}
}
