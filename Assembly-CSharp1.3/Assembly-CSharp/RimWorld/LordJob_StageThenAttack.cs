using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000886 RID: 2182
	public class LordJob_StageThenAttack : LordJob
	{
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x060039A2 RID: 14754 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_StageThenAttack()
		{
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x0014278E File Offset: 0x0014098E
		public LordJob_StageThenAttack(Faction faction, IntVec3 stageLoc, int raidSeed)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.raidSeed = raidSeed;
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x001427AC File Offset: 0x001409AC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			int tickLimit = Rand.RangeSeeded(5000, 15000, this.raidSeed);
			Transition transition = new Transition(lordToil_Stage, startingToil, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(tickLimit));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), MessageTypeDefOf.ThreatBig, "MessageRaidersBeginningAssault-" + this.raidSeed, 1f, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition, false);
			stateGraph.transitions.Find((Transition x) => x.triggers.Any((Trigger y) => y is Trigger_BecameNonHostileToPlayer)).AddSource(lordToil_Stage);
			return stateGraph;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001428D8 File Offset: 0x00140AD8
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.raidSeed, "raidSeed", 0, false);
		}

		// Token: 0x04001FA4 RID: 8100
		private Faction faction;

		// Token: 0x04001FA5 RID: 8101
		private IntVec3 stageLoc;

		// Token: 0x04001FA6 RID: 8102
		private int raidSeed;
	}
}
