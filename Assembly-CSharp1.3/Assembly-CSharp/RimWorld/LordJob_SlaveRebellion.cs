using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000883 RID: 2179
	public class LordJob_SlaveRebellion : LordJob
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06003989 RID: 14729 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600398A RID: 14730 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x00141FF2 File Offset: 0x001401F2
		public bool IsAggressiveRebellion
		{
			get
			{
				return !this.passive;
			}
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x00141FFD File Offset: 0x001401FD
		public LordJob_SlaveRebellion()
		{
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0014200C File Offset: 0x0014020C
		public LordJob_SlaveRebellion(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID, bool passive = true)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
			this.passive = passive;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x00142038 File Offset: 0x00140238
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckIdeology("Slave rebellion"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.useAvoidGrid = true;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_AssaultColonyPrisoners lordToil_AssaultColonyPrisoners = new LordToil_AssaultColonyPrisoners();
			lordToil_AssaultColonyPrisoners.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColonyPrisoners);
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, true, false);
			lordToil_ExitMapFighting.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMapFighting);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, false, false);
			lordToil_ExitMap2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap2);
			if (!this.passive)
			{
				Transition transition = new Transition(lordToil_Travel, lordToil_AssaultColonyPrisoners, false, true);
				transition.AddTrigger(new Trigger_Memo("TravelArrived"));
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_AssaultColonyPrisoners, lordToil_ExitMapFighting, false, true);
				transition2.AddTrigger(new Trigger_FractionColonyDamageTaken(LordJob_SlaveRebellion.DesiredDamageRange.RandomInRange, 900f));
				transition2.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition2, false);
				Transition transition3 = new Transition(lordToil_AssaultColonyPrisoners, lordToil_ExitMapFighting, false, true);
				transition3.AddTrigger(new Trigger_TicksPassed(LordJob_SlaveRebellion.AssaultTimeBeforeGiveUp.RandomInRange));
				transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition3, false);
			}
			else
			{
				Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
				transition4.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition4, false);
				Transition transition5 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
				transition5.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition5, false);
				Transition transition6 = new Transition(lordToil_Travel, lordToil_ExitMap, false, true);
				transition6.AddSources(new LordToil[]
				{
					lordToil_AssaultColonyPrisoners,
					lordToil_PrisonerEscape,
					lordToil_ExitMapFighting
				});
				transition6.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition6, false);
				Transition transition7 = new Transition(lordToil_ExitMap, lordToil_PrisonerEscape, false, true);
				transition7.AddTrigger(new Trigger_PawnCanReachMapEdge());
				transition7.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition7, false);
			}
			Transition transition8 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap2, false, true);
			transition8.AddSource(lordToil_ExitMapFighting);
			transition8.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition8, false);
			return stateGraph;
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0014229C File Offset: 0x0014049C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
			Scribe_Values.Look<bool>(ref this.passive, "passive", false, false);
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x00141CBD File Offset: 0x0013FEBD
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x00141CBD File Offset: 0x0013FEBD
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x00142308 File Offset: 0x00140508
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

		// Token: 0x04001F9B RID: 8091
		private IntVec3 groupUpLoc;

		// Token: 0x04001F9C RID: 8092
		private IntVec3 exitPoint;

		// Token: 0x04001F9D RID: 8093
		private bool passive;

		// Token: 0x04001F9E RID: 8094
		private int sapperThingID = -1;

		// Token: 0x04001F9F RID: 8095
		private static FloatRange DesiredDamageRange = new FloatRange(0.25f, 0.35f);

		// Token: 0x04001FA0 RID: 8096
		private const float MinDamage = 900f;

		// Token: 0x04001FA1 RID: 8097
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);
	}
}
