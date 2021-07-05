using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200087A RID: 2170
	public class LordJob_FormAndSendCaravan : LordJob
	{
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x00140B2B File Offset: 0x0013ED2B
		public bool GatheringItemsNow
		{
			get
			{
				return this.lord.CurLordToil == this.gatherItems;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ManagesRopableAnimals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x00140B40 File Offset: 0x0013ED40
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
				if (curLordToil == this.gatherAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.gatherAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				if (curLordToil == this.collectAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.collectAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
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

		// Token: 0x0600394D RID: 14669 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_FormAndSendCaravan()
		{
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x00140C62 File Offset: 0x0013EE62
		public LordJob_FormAndSendCaravan(List<TransferableOneWay> transferables, List<Pawn> downedPawns, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			this.transferables = transferables;
			this.downedPawns = downedPawns;
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
			this.startingTile = startingTile;
			this.destinationTile = destinationTile;
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x00140C98 File Offset: 0x0013EE98
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.gatherAnimals = new LordToil_PrepareCaravan_GatherAnimals(this.meetingPoint);
			stateGraph.AddToil(this.gatherAnimals);
			this.gatherAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherAnimals_pause);
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
			this.collectAnimals = new LordToil_PrepareCaravan_CollectAnimals(this.exitSpot);
			stateGraph.AddToil(this.collectAnimals);
			this.collectAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.collectAnimals_pause);
			this.leave = new LordToil_PrepareCaravan_Leave(this.exitSpot);
			stateGraph.AddToil(this.leave);
			this.leave_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.leave_pause);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(this.gatherAnimals, this.gatherItems, false, true);
			transition.AddTrigger(new Trigger_Memo("AllAnimalsGathered"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(this.gatherItems, this.gatherDownedPawns, false, true);
			transition2.AddTrigger(new Trigger_Memo("AllItemsGathered"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(this.gatherDownedPawns, lordToil_PrepareCaravan_Wait, false, true);
			transition3.AddTrigger(new Trigger_Memo("AllDownedPawnsGathered"));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_PrepareCaravan_Wait, this.collectAnimals, false, true);
			transition4.AddTrigger(new Trigger_NoPawnsVeryTiredAndSleeping(0f));
			transition4.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(this.collectAnimals, this.leave, false, true);
			transition5.AddTrigger(new Trigger_Memo("AllAnimalsCollected"));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(this.leave, lordToil_End, false, true);
			transition6.AddTrigger(new Trigger_Memo("ReadyToExitMap"));
			transition6.AddPreAction(new TransitionAction_Custom(new Action(this.SendCaravan)));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = this.PauseTransition(this.gatherAnimals, this.gatherAnimals_pause);
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = this.UnpauseTransition(this.gatherAnimals_pause, this.gatherAnimals);
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = this.PauseTransition(this.gatherItems, this.gatherItems_pause);
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = this.UnpauseTransition(this.gatherItems_pause, this.gatherItems);
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = this.PauseTransition(this.gatherDownedPawns, this.gatherDownedPawns_pause);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = this.UnpauseTransition(this.gatherDownedPawns_pause, this.gatherDownedPawns);
			stateGraph.AddTransition(transition12, false);
			Transition transition13 = this.PauseTransition(this.collectAnimals, this.collectAnimals_pause);
			stateGraph.AddTransition(transition13, false);
			Transition transition14 = this.UnpauseTransition(this.collectAnimals_pause, this.collectAnimals);
			stateGraph.AddTransition(transition14, false);
			Transition transition15 = this.PauseTransition(this.leave, this.leave_pause);
			stateGraph.AddTransition(transition15, false);
			Transition transition16 = this.UnpauseTransition(this.leave_pause, this.leave);
			stateGraph.AddTransition(transition16, false);
			Transition transition17 = this.PauseTransition(lordToil_PrepareCaravan_Wait, lordToil_PrepareCaravan_Pause);
			stateGraph.AddTransition(transition17, false);
			Transition transition18 = this.UnpauseTransition(lordToil_PrepareCaravan_Pause, lordToil_PrepareCaravan_Wait);
			stateGraph.AddTransition(transition18, false);
			return stateGraph;
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x00141070 File Offset: 0x0013F270
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

		// Token: 0x06003951 RID: 14673 RVA: 0x001410F2 File Offset: 0x0013F2F2
		public override string GetReport(Pawn pawn)
		{
			return "LordReportFormingCaravan".Translate();
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x00141104 File Offset: 0x0013F304
		private Transition PauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationPaused".Translate(), MessageTypeDefOf.NegativeEvent, () => this.lord.ownedPawns.FirstOrDefault((Pawn x) => x.InMentalState), null, 1f));
			transition.AddTrigger(new Trigger_MentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x00141164 File Offset: 0x0013F364
		private Transition UnpauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationUnpaused".Translate(), MessageTypeDefOf.SilentInput, null, 1f, null));
			transition.AddTrigger(new Trigger_NoMentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x001411B8 File Offset: 0x0013F3B8
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

		// Token: 0x06003955 RID: 14677 RVA: 0x0014127C File Offset: 0x0013F47C
		private void SendCaravan()
		{
			this.caravanSent = true;
			CaravanFormingUtility.FormAndCreateCaravan(this.lord.ownedPawns.Concat(from x in this.downedPawns
			where JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(x, this.exitSpot)
			select x), this.lord.faction, base.Map.Tile, this.startingTile, this.destinationTile);
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x001412DE File Offset: 0x0013F4DE
		public override void Notify_PawnAdded(Pawn p)
		{
			base.Notify_PawnAdded(p);
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x001412ED File Offset: 0x0013F4ED
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

		// Token: 0x06003958 RID: 14680 RVA: 0x0014132A File Offset: 0x0013F52A
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return !p.RaceProps.FenceBlocked;
		}

		// Token: 0x04001F77 RID: 8055
		public List<TransferableOneWay> transferables;

		// Token: 0x04001F78 RID: 8056
		public List<Pawn> downedPawns;

		// Token: 0x04001F79 RID: 8057
		private IntVec3 meetingPoint;

		// Token: 0x04001F7A RID: 8058
		private IntVec3 exitSpot;

		// Token: 0x04001F7B RID: 8059
		private int startingTile;

		// Token: 0x04001F7C RID: 8060
		private int destinationTile;

		// Token: 0x04001F7D RID: 8061
		private bool caravanSent;

		// Token: 0x04001F7E RID: 8062
		private LordToil gatherAnimals;

		// Token: 0x04001F7F RID: 8063
		private LordToil gatherAnimals_pause;

		// Token: 0x04001F80 RID: 8064
		private LordToil gatherItems;

		// Token: 0x04001F81 RID: 8065
		private LordToil gatherItems_pause;

		// Token: 0x04001F82 RID: 8066
		private LordToil gatherDownedPawns;

		// Token: 0x04001F83 RID: 8067
		private LordToil gatherDownedPawns_pause;

		// Token: 0x04001F84 RID: 8068
		private LordToil collectAnimals;

		// Token: 0x04001F85 RID: 8069
		private LordToil collectAnimals_pause;

		// Token: 0x04001F86 RID: 8070
		private LordToil leave;

		// Token: 0x04001F87 RID: 8071
		private LordToil leave_pause;

		// Token: 0x04001F88 RID: 8072
		public const float CustomWakeThreshold = 0.5f;
	}
}
