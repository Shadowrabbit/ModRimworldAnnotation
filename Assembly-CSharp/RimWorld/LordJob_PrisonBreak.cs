using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DCA RID: 3530
	public class LordJob_PrisonBreak : LordJob
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x000387CC File Offset: 0x000369CC
		public LordJob_PrisonBreak()
		{
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x000387DB File Offset: 0x000369DB
		public LordJob_PrisonBreak(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x001B7FBC File Offset: 0x001B61BC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.useAvoidGrid = true;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap2);
			Transition transition = new Transition(lordToil_Travel, lordToil_ExitMap2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_PrisonerEscape,
				lordToil_ExitMap
			});
			transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ExitMap2, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition4.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_PrisonerEscape, lordToil_PrisonerEscape, true, true);
			transition5.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			transition5.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap, false, true);
			transition6.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001B8140 File Offset: 0x001B6340
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x000387FF File Offset: 0x000369FF
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x000387FF File Offset: 0x000369FF
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001B819C File Offset: 0x001B639C
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn == null)
			{
				return true;
			}
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef == null || !mentalStateDef.escapingPrisonersIgnore;
		}

		// Token: 0x040033F6 RID: 13302
		private IntVec3 groupUpLoc;

		// Token: 0x040033F7 RID: 13303
		private IntVec3 exitPoint;

		// Token: 0x040033F8 RID: 13304
		private int sapperThingID = -1;
	}
}
