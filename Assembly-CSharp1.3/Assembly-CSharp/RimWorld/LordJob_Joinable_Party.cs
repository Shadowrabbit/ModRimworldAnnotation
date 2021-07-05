using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000896 RID: 2198
	public class LordJob_Joinable_Party : LordJob_Joinable_Gathering
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06003A43 RID: 14915 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x00146A9F File Offset: 0x00144C9F
		protected virtual ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06003A45 RID: 14917 RVA: 0x00146AA6 File Offset: 0x00144CA6
		protected virtual TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003A46 RID: 14918 RVA: 0x00146A9F File Offset: 0x00144C9F
		protected virtual ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003A47 RID: 14919 RVA: 0x00146AA6 File Offset: 0x00144CA6
		protected virtual TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x00146AAD File Offset: 0x00144CAD
		public LordJob_Joinable_Party()
		{
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00146AB5 File Offset: 0x00144CB5
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
			this.durationTicks = Rand.RangeInclusive(5000, 15000);
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00146AD5 File Offset: 0x00144CD5
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingParty".Translate();
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00146AE6 File Offset: 0x00144CE6
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Party(spot, gatheringDef, 3.5E-05f);
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00146AF4 File Offset: 0x00144CF4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil party = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(party, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
			transition.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition.AddPreAction(new TransitionAction_Message(this.gatheringDef.calledOffMessage, MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = this.GetTimeoutTrigger();
			Transition transition2 = new Transition(party, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition2.AddPreAction(new TransitionAction_Message(this.gatheringDef.finishedMessage, MessageTypeDefOf.SituationResolved, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x00146C5C File Offset: 0x00144E5C
		private void ApplyOutcome(LordToil_Party toil)
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			LordToilData_Gathering lordToilData_Gathering = (LordToilData_Gathering)toil.data;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				bool flag = pawn == this.organizer;
				int num;
				if (lordToilData_Gathering.presentForTicks.TryGetValue(pawn, out num) && num > 0)
				{
					if (ownedPawns[i].needs.mood != null)
					{
						ThoughtDef thoughtDef = flag ? this.OrganizerThought : this.AttendeeThought;
						float num2 = 0.5f / thoughtDef.stages[0].baseMoodEffect;
						float moodPowerFactor = Mathf.Min((float)num / (float)this.durationTicks + num2, 1f);
						Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
						thought_Memory.moodPowerFactor = moodPowerFactor;
						ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					}
					TaleRecorder.RecordTale(flag ? this.OrganizerTale : this.AttendeeTale, new object[]
					{
						ownedPawns[i],
						this.organizer
					});
				}
			}
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00146D8F File Offset: 0x00144F8F
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.gatheringDef == null)
			{
				this.gatheringDef = GatheringDefOf.Party;
			}
		}
	}
}
