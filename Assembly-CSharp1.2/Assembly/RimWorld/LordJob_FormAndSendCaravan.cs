using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DC1 RID: 3521
	public class LordJob_FormAndSendCaravan : LordJob
	{
		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x000385AD File Offset: 0x000367AD
		public bool GatheringItemsNow
		{
			get
			{
				return this.lord.CurLordToil == this.gatherItems;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06005044 RID: 20548 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06005045 RID: 20549 RVA: 0x001B7404 File Offset: 0x001B5604
		public string Status
		{
			get
			{
				LordToil curLordToil = this.lord.CurLordToil;
				if (curLordToil == this.gatherItems)
				{
					return "FormingCaravanStatus_GatheringItems".Translate();
				}
				if (curLordToil == this.gatherItems_pause)
				{
					return "FormingCaravanStatus_GatheringItems_Pause".Translate();
				}
				if (curLordToil == this.gatherDownedPawns)
				{
					return "FormingCaravanStatus_GatheringDownedPawns".Translate();
				}
				if (curLordToil == this.gatherDownedPawns_pause)
				{
					return "FormingCaravanStatus_GatheringDownedPawns_Pause".Translate();
				}
				if (curLordToil == this.leave)
				{
					return "FormingCaravanStatus_Leaving".Translate();
				}
				if (curLordToil == this.leave_pause)
				{
					return "FormingCaravanStatus_Leaving_Pause".Translate();
				}
				return "FormingCaravanStatus_Waiting".Translate();
			}
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_FormAndSendCaravan()
		{
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x000385C2 File Offset: 0x000367C2
		public LordJob_FormAndSendCaravan(List<TransferableOneWay> transferables, List<Pawn> downedPawns, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			this.transferables = transferables;
			this.downedPawns = downedPawns;
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
			this.startingTile = startingTile;
			this.destinationTile = destinationTile;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x001B74C4 File Offset: 0x001B56C4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.gatherItems = new LordToil_PrepareCaravan_GatherItems(this.meetingPoint);
			stateGraph.AddToil(this.gatherItems);
			this.gatherItems_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherItems_pause);
			this.gatherDownedPawns = new LordToil_PrepareCaravan_GatherDownedPawns(this.meetingPoint, this.exitSpot);
			stateGraph.AddToil(this.gatherDownedPawns);
			this.gatherDownedPawns_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherDownedPawns_pause);
			LordToil_PrepareCaravan_Wait lordToil_PrepareCaravan_Wait = new LordToil_PrepareCaravan_Wait(this.meetingPoint);
			stateGraph.AddToil(lordToil_PrepareCaravan_Wait);
			LordToil_PrepareCaravan_Pause lordToil_PrepareCaravan_Pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(lordToil_PrepareCaravan_Pause);
			this.leave = new LordToil_PrepareCaravan_Leave(this.exitSpot);
			stateGraph.AddToil(this.leave);
			this.leave_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.leave_pause);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(this.gatherItems, this.gatherDownedPawns, false, true);
			transition.AddTrigger(new Trigger_Memo("AllItemsGathered"));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(this.gatherDownedPawns, lordToil_PrepareCaravan_Wait, false, true);
			transition2.AddTrigger(new Trigger_Memo("AllDownedPawnsGathered"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_PrepareCaravan_Wait, this.leave, false, true);
			transition3.AddTrigger(new Trigger_NoPawnsVeryTiredAndSleeping(0f));
			transition3.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(this.leave, lordToil_End, false, true);
			transition4.AddTrigger(new Trigger_Memo("ReadyToExitMap"));
			transition4.AddPreAction(new TransitionAction_Custom(new Action(this.SendCaravan)));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = this.PauseTransition(this.gatherItems, this.gatherItems_pause);
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = this.UnpauseTransition(this.gatherItems_pause, this.gatherItems);
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = this.PauseTransition(this.gatherDownedPawns, this.gatherDownedPawns_pause);
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = this.UnpauseTransition(this.gatherDownedPawns_pause, this.gatherDownedPawns);
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = this.PauseTransition(this.leave, this.leave_pause);
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = this.UnpauseTransition(this.leave_pause, this.leave);
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = this.PauseTransition(lordToil_PrepareCaravan_Wait, lordToil_PrepareCaravan_Pause);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = this.UnpauseTransition(lordToil_PrepareCaravan_Pause, lordToil_PrepareCaravan_Wait);
			stateGraph.AddTransition(transition12, false);
			return stateGraph;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x001B7760 File Offset: 0x001B5960
		public override void LordJobTick()
		{
			base.LordJobTick();
			for (int i = this.downedPawns.Count - 1; i >= 0; i--)
			{
				if (this.downedPawns[i].Destroyed)
				{
					this.downedPawns.RemoveAt(i);
				}
				else if (!this.downedPawns[i].Downed)
				{
					this.lord.AddPawn(this.downedPawns[i]);
					this.downedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x000385F7 File Offset: 0x000367F7
		public override string GetReport(Pawn pawn)
		{
			return "LordReportFormingCaravan".Translate();
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x001B77E4 File Offset: 0x001B59E4
		private Transition PauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationPaused".Translate(), MessageTypeDefOf.NegativeEvent, () => this.lord.ownedPawns.FirstOrDefault((Pawn x) => x.InMentalState), null, 1f));
			transition.AddTrigger(new Trigger_MentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x001B7844 File Offset: 0x001B5A44
		private Transition UnpauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationUnpaused".Translate(), MessageTypeDefOf.SilentInput, null, 1f));
			transition.AddTrigger(new Trigger_NoMentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x001B7898 File Offset: 0x001B5A98
		public override void ExposeData()
		{
			Scribe_Collections.Look<TransferableOneWay>(ref this.transferables, "transferables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.downedPawns, "downedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.meetingPoint, "meetingPoint", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitSpot, "exitSpot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.startingTile, "startingTile", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.downedPawns.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x001B795C File Offset: 0x001B5B5C
		private void SendCaravan()
		{
			this.caravanSent = true;
			CaravanFormingUtility.FormAndCreateCaravan(this.lord.ownedPawns.Concat(from x in this.downedPawns
			where JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(x, this.exitSpot)
			select x), this.lord.faction, base.Map.Tile, this.startingTile, this.destinationTile);
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x00038608 File Offset: 0x00036808
		public override void Notify_PawnAdded(Pawn p)
		{
			base.Notify_PawnAdded(p);
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x00038617 File Offset: 0x00036817
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			ReachabilityUtility.ClearCacheFor(p);
			if (!this.caravanSent)
			{
				if (condition == PawnLostCondition.IncappedOrKilled && p.Downed)
				{
					this.downedPawns.Add(p);
				}
				CaravanFormingUtility.RemovePawnFromCaravan(p, this.lord, false);
			}
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x040033D5 RID: 13269
		public List<TransferableOneWay> transferables;

		// Token: 0x040033D6 RID: 13270
		public List<Pawn> downedPawns;

		// Token: 0x040033D7 RID: 13271
		private IntVec3 meetingPoint;

		// Token: 0x040033D8 RID: 13272
		private IntVec3 exitSpot;

		// Token: 0x040033D9 RID: 13273
		private int startingTile;

		// Token: 0x040033DA RID: 13274
		private int destinationTile;

		// Token: 0x040033DB RID: 13275
		private bool caravanSent;

		// Token: 0x040033DC RID: 13276
		private LordToil gatherItems;

		// Token: 0x040033DD RID: 13277
		private LordToil gatherItems_pause;

		// Token: 0x040033DE RID: 13278
		private LordToil gatherDownedPawns;

		// Token: 0x040033DF RID: 13279
		private LordToil gatherDownedPawns_pause;

		// Token: 0x040033E0 RID: 13280
		private LordToil leave;

		// Token: 0x040033E1 RID: 13281
		private LordToil leave_pause;

		// Token: 0x040033E2 RID: 13282
		public const float CustomWakeThreshold = 0.5f;
	}
}
