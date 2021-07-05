using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public class LordJob_PrisonBreak : LordJob
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003976 RID: 14710 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003977 RID: 14711 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x00141AAA File Offset: 0x0013FCAA
		public LordJob_PrisonBreak()
		{
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x00141AB9 File Offset: 0x0013FCB9
		public LordJob_PrisonBreak(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x00141AE0 File Offset: 0x0013FCE0
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

		// Token: 0x0600397B RID: 14715 RVA: 0x00141C64 File Offset: 0x0013FE64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x00141CBD File Offset: 0x0013FEBD
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00141CBD File Offset: 0x0013FEBD
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00141CC8 File Offset: 0x0013FEC8
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

		// Token: 0x04001F94 RID: 8084
		private IntVec3 groupUpLoc;

		// Token: 0x04001F95 RID: 8085
		private IntVec3 exitPoint;

		// Token: 0x04001F96 RID: 8086
		private int sapperThingID = -1;
	}
}
