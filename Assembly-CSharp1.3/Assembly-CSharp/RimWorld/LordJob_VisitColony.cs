using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200088A RID: 2186
	public class LordJob_VisitColony : LordJob
	{
		// Token: 0x060039B6 RID: 14774 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_VisitColony()
		{
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0014304F File Offset: 0x0014124F
		public LordJob_VisitColony(Faction faction, IntVec3 chillSpot, int? durationTicks = null)
		{
			this.faction = faction;
			this.chillSpot = chillSpot;
			this.durationTicks = durationTicks;
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0014306C File Offset: 0x0014126C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.chillSpot).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.chillSpot, 28f, null);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil_TakeWoundedGuest lordToil_TakeWoundedGuest = new LordToil_TakeWoundedGuest();
			stateGraph.AddToil(lordToil_TakeWoundedGuest);
			this.exitSubgraph = new LordJob_TravelAndExit(IntVec3.Invalid).CreateGraph();
			LordToil startingToil2 = stateGraph.AttachSubgraph(this.exitSubgraph).StartingToil;
			LordToil target = this.exitSubgraph.lordToils[1];
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Walk, true, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(startingToil, startingToil2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_DefendPoint
			});
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			if (this.faction != null)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			}
			transition.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(startingToil, lordToil_ExitMap, false, true);
			transition2.AddSources(new LordToil[]
			{
				lordToil_DefendPoint,
				lordToil_TakeWoundedGuest
			});
			transition2.AddSources(this.exitSubgraph.lordToils);
			transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			if (this.faction != null)
			{
				transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			}
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_ExitMap, startingToil2, false, true);
			transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition3.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(startingToil, lordToil_DefendPoint, false, true);
			transition4.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition4, false);
			if (this.faction != null)
			{
				Transition transition5 = new Transition(lordToil_DefendPoint, lordToil_TakeWoundedGuest, false, true);
				transition5.AddTrigger(new Trigger_WoundedGuestPresent());
				transition5.AddPreAction(new TransitionAction_Message("MessageVisitorsTakingWounded".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
				stateGraph.AddTransition(transition5, false);
			}
			Transition transition6 = new Transition(lordToil_DefendPoint, target, false, true);
			transition6.AddSources(new LordToil[]
			{
				lordToil_TakeWoundedGuest,
				startingToil
			});
			transition6.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition6.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition6.AddPostAction(new TransitionAction_WakeAll());
			transition6.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_DefendPoint, startingToil2, false, true);
			int tickLimit;
			if (DebugSettings.instantVisitorsGift && this.faction != null)
			{
				tickLimit = 0;
			}
			else if (this.durationTicks != null)
			{
				tickLimit = this.durationTicks.Value;
			}
			else
			{
				tickLimit = Rand.Range(8000, 22000);
			}
			transition7.AddTrigger(new Trigger_TicksPassed(tickLimit));
			if (this.faction != null)
			{
				transition7.AddPreAction(new TransitionAction_Message("VisitorsLeaving".Translate(this.faction.Name), null, 1f));
			}
			if (this.gifts != null)
			{
				transition7.AddPreAction(new TransitionAction_GiveGift
				{
					gifts = this.gifts
				});
			}
			else
			{
				transition7.AddPreAction(new TransitionAction_CheckGiveGift());
			}
			transition7.AddPostAction(new TransitionAction_WakeAll());
			transition7.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition7, false);
			return stateGraph;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00143468 File Offset: 0x00141668
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.chillSpot, "chillSpot", default(IntVec3), false);
			Scribe_Values.Look<int?>(ref this.durationTicks, "durationTicks", null, false);
			Scribe_Collections.Look<Thing>(ref this.gifts, "gifts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.gifts;
				if (list == null)
				{
					return;
				}
				list.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04001FAD RID: 8109
		private Faction faction;

		// Token: 0x04001FAE RID: 8110
		private IntVec3 chillSpot;

		// Token: 0x04001FAF RID: 8111
		private int? durationTicks;

		// Token: 0x04001FB0 RID: 8112
		public List<Thing> gifts;

		// Token: 0x04001FB1 RID: 8113
		public StateGraph exitSubgraph;
	}
}
