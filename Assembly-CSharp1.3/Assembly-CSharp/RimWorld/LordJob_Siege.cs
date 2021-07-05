using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000882 RID: 2178
	public class LordJob_Siege : LordJob
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06003984 RID: 14724 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_Siege()
		{
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x00141D83 File Offset: 0x0013FF83
		public LordJob_Siege(Faction faction, IntVec3 siegeSpot, float blueprintPoints)
		{
			this.faction = faction;
			this.siegeSpot = siegeSpot;
			this.blueprintPoints = blueprintPoints;
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x00141DA0 File Offset: 0x0013FFA0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.siegeSpot).CreateGraph()).StartingToil;
			LordToil_Siege lordToil_Siege = new LordToil_Siege(this.siegeSpot, this.blueprintPoints);
			stateGraph.AddToil(lordToil_Siege);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(startingToil, lordToil_Siege, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.AddTrigger(new Trigger_TicksPassed(5000));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Siege, startingToil2, false, true);
			transition2.AddTrigger(new Trigger_Memo("NoBuilders"));
			transition2.AddTrigger(new Trigger_Memo("NoArtillery"));
			transition2.AddTrigger(new Trigger_PawnHarmed(0.08f, false, null));
			transition2.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition2.AddTrigger(new Trigger_TicksPassed((int)(60000f * Rand.Range(1.5f, 3f))));
			transition2.AddPreAction(new TransitionAction_Message("MessageSiegersAssaulting".Translate(this.faction.def.pawnsPlural, this.faction), MessageTypeDefOf.ThreatBig, null, 1f, null));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Siege, lordToil_ExitMap, false, true);
			transition3.AddSource(startingToil2);
			transition3.AddSource(startingToil);
			transition3.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition3.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x00141FA4 File Offset: 0x001401A4
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.siegeSpot, "siegeSpot", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
		}

		// Token: 0x04001F98 RID: 8088
		private Faction faction;

		// Token: 0x04001F99 RID: 8089
		private IntVec3 siegeSpot;

		// Token: 0x04001F9A RID: 8090
		private float blueprintPoints;
	}
}
