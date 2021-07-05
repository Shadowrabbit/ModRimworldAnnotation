using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE0 RID: 3808
	public class PreceptComp_SelfTookMemoryThought : PreceptComp_Thought
	{
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06005A86 RID: 23174 RVA: 0x001F4F58 File Offset: 0x001F3158
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}

		// Token: 0x06005A87 RID: 23175 RVA: 0x001F5108 File Offset: 0x001F3308
		public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
			if (ev.def != this.eventDef || !canApplySelfTookThoughts)
			{
				return;
			}
			Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			if (arg.needs != null && arg.needs.mood != null && (!this.onlyForNonSlaves || !arg.IsSlave))
			{
				if (this.thought.minExpectationForNegativeThought != null && ExpectationsUtility.CurrentExpectationFor(arg).order < this.thought.minExpectationForNegativeThought.order)
				{
					return;
				}
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				Thought_KilledInnocentAnimal thought_KilledInnocentAnimal;
				Pawn animal;
				if ((thought_KilledInnocentAnimal = (thought_Memory as Thought_KilledInnocentAnimal)) != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				Thought_MemoryObservation thought_MemoryObservation;
				Corpse target;
				if ((thought_MemoryObservation = (thought_Memory as Thought_MemoryObservation)) != null && ev.args.TryGetArg<Corpse>(HistoryEventArgsNames.Subject, out target))
				{
					thought_MemoryObservation.Target = target;
				}
				arg.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
			}
		}

		// Token: 0x04003502 RID: 13570
		public HistoryEventDef eventDef;

		// Token: 0x04003503 RID: 13571
		public bool onlyForNonSlaves;
	}
}
