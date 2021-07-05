using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDF RID: 3807
	public class PreceptComp_KnowsMemoryThought : PreceptComp_Thought
	{
		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06005A83 RID: 23171 RVA: 0x001F4F58 File Offset: 0x001F3158
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x001F4F68 File Offset: 0x001F3168
		public override void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
			if (ev.def != this.eventDef)
			{
				return;
			}
			Pawn pawn;
			bool flag = ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn);
			bool flag2 = false;
			if (this.doerMustBeMyFaction != null)
			{
				flag2 = this.doerMustBeMyFaction.Value;
			}
			else
			{
				if (flag)
				{
					using (List<ThoughtStage>.Enumerator enumerator = this.thought.stages.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.baseMoodEffect != 0f)
							{
								flag2 = true;
								break;
							}
						}
						goto IL_88;
					}
				}
				flag2 = false;
			}
			IL_88:
			if (member.needs != null && member.needs.mood != null && (!flag2 || (flag && pawn.Faction == member.Faction)) && (!this.onlyForNonSlaves || !member.IsSlave))
			{
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				int val;
				if (ev.args.TryGetArg<int>(HistoryEventArgsNames.ExecutionThoughtStage, out val))
				{
					thought_Memory.SetForcedStage(Math.Min(val, this.thought.stages.Count - 1));
				}
				Thought_KilledInnocentAnimal thought_KilledInnocentAnimal;
				Pawn animal;
				if ((thought_KilledInnocentAnimal = (thought_Memory as Thought_KilledInnocentAnimal)) != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				member.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, pawn);
				if (this.removesThought != null)
				{
					member.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.removesThought);
				}
			}
		}

		// Token: 0x040034FE RID: 13566
		public HistoryEventDef eventDef;

		// Token: 0x040034FF RID: 13567
		public bool? doerMustBeMyFaction;

		// Token: 0x04003500 RID: 13568
		public ThoughtDef removesThought;

		// Token: 0x04003501 RID: 13569
		public bool onlyForNonSlaves;
	}
}
