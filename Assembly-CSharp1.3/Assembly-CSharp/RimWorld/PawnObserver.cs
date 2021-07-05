using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5A RID: 3674
	public class PawnObserver
	{
		// Token: 0x06005503 RID: 21763 RVA: 0x001CCA27 File Offset: 0x001CAC27
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = 0;
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x001CCA48 File Offset: 0x001CAC48
		public void ObserverInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.intervalsUntilObserve--;
			if (this.intervalsUntilObserve <= 0)
			{
				this.ObserveSurroundingThings();
				this.intervalsUntilObserve = 4 + Rand.RangeInclusive(-1, 1);
			}
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x001CCA84 File Offset: 0x001CAC84
		private void ObserveSurroundingThings()
		{
			TerrorUtility.RemoveAllTerrorThoughts(this.pawn);
			this.terrorThoughts.Clear();
			if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) || !this.pawn.Awake() || this.pawn.needs.mood == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(this.pawn.Position, this.pawn.Map, (Region from, Region to) => this.pawn.Position.InHorDistOf(to.extentsClose.ClosestCellTo(this.pawn.Position), 5f), delegate(Region reg)
			{
				foreach (Thing thing in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					if (this.PossibleToObserve(thing))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing);
					}
				}
				foreach (Thing thing2 in reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (this.PossibleToObserve(thing2))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing2);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing2);
					}
				}
				return true;
			}, 999999, RegionType.Set_Passable);
			foreach (Thought_MemoryObservationTerror newThought in TerrorUtility.TakeTopTerrorThoughts(this.terrorThoughts))
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
			this.terrorThoughts.Clear();
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x001CCB88 File Offset: 0x001CAD88
		private bool PossibleToObserve(Thing thing)
		{
			return thing.Position.InHorDistOf(this.pawn.Position, 5f) && GenSight.LineOfSight(thing.Position, this.pawn.Position, this.pawn.Map, true, null, 0, 0);
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x001CCBDC File Offset: 0x001CADDC
		[CompilerGenerated]
		private void <ObserveSurroundingThings>g__TryCreateObservedThought|7_0(Thing thing)
		{
			Thought_MemoryObservationTerror item;
			if (TerrorUtility.TryCreateTerrorThought(thing, out item))
			{
				this.terrorThoughts.Add(item);
			}
			IObservedThoughtGiver observedThoughtGiver;
			if ((observedThoughtGiver = (thing as IObservedThoughtGiver)) != null)
			{
				Thought_Memory thought_Memory = observedThoughtGiver.GiveObservedThought(this.pawn);
				if (thought_Memory != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x001CCC3C File Offset: 0x001CAE3C
		[CompilerGenerated]
		private void <ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(Thing thing)
		{
			IObservedThoughtGiver observedThoughtGiver;
			if ((observedThoughtGiver = (thing as IObservedThoughtGiver)) != null)
			{
				HistoryEventDef historyEventDef = observedThoughtGiver.GiveObservedHistoryEvent(this.pawn);
				if (historyEventDef != null)
				{
					HistoryEvent historyEvent = new HistoryEvent(historyEventDef, this.pawn.Named(HistoryEventArgsNames.Doer), thing.Named(HistoryEventArgsNames.Subject));
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
			}
		}

		// Token: 0x0400325F RID: 12895
		private Pawn pawn;

		// Token: 0x04003260 RID: 12896
		private int intervalsUntilObserve;

		// Token: 0x04003261 RID: 12897
		private List<Thought_MemoryObservationTerror> terrorThoughts = new List<Thought_MemoryObservationTerror>();

		// Token: 0x04003262 RID: 12898
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x04003263 RID: 12899
		private const float ObservationRadius = 5f;
	}
}
