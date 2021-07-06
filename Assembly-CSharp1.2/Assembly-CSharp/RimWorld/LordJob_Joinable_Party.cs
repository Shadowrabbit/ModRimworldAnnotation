using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DDA RID: 3546
	public class LordJob_Joinable_Party : LordJob_Joinable_Gathering
	{
		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060050E4 RID: 20708 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x060050E5 RID: 20709 RVA: 0x00038BBC File Offset: 0x00036DBC
		protected virtual ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x060050E6 RID: 20710 RVA: 0x00038BC3 File Offset: 0x00036DC3
		protected virtual TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x060050E7 RID: 20711 RVA: 0x00038BBC File Offset: 0x00036DBC
		protected virtual ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x060050E8 RID: 20712 RVA: 0x00038BC3 File Offset: 0x00036DC3
		protected virtual TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x060050E9 RID: 20713 RVA: 0x00038BCA File Offset: 0x00036DCA
		public int DurationTicks
		{
			get
			{
				return this.durationTicks;
			}
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x00038BD2 File Offset: 0x00036DD2
		public LordJob_Joinable_Party()
		{
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x00038BDA File Offset: 0x00036DDA
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
			this.durationTicks = Rand.RangeInclusive(5000, 15000);
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x00038BFA File Offset: 0x00036DFA
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingParty".Translate();
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x00038C0B File Offset: 0x00036E0B
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Party(spot, gatheringDef, 3.5E-05f);
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x001B9DEC File Offset: 0x001B7FEC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil party = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(party, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.ShouldBeCalledOff(), 1));
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

		// Token: 0x060050EF RID: 20719 RVA: 0x00038C19 File Offset: 0x00036E19
		protected virtual Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassed(this.durationTicks);
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x001B9F54 File Offset: 0x001B8154
		private void ApplyOutcome(LordToil_Party toil)
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			LordToilData_Party lordToilData_Party = (LordToilData_Party)toil.data;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				bool flag = pawn == this.organizer;
				int num;
				if (lordToilData_Party.presentForTicks.TryGetValue(pawn, out num) && num > 0)
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

		// Token: 0x060050F1 RID: 20721 RVA: 0x00038C26 File Offset: 0x00036E26
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.gatheringDef == null)
			{
				this.gatheringDef = GatheringDefOf.Party;
			}
		}

		// Token: 0x04003415 RID: 13333
		private int durationTicks;
	}
}
