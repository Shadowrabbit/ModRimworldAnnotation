using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F9 RID: 5369
	public class PawnObserver
	{
		// Token: 0x060073A2 RID: 29602 RVA: 0x0004DDBA File Offset: 0x0004BFBA
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = Rand.Range(0, 4);
		}

		// Token: 0x060073A3 RID: 29603 RVA: 0x0004DDD6 File Offset: 0x0004BFD6
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

		// Token: 0x060073A4 RID: 29604 RVA: 0x00234C5C File Offset: 0x00232E5C
		private void ObserveSurroundingThings()
		{
			if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) || this.pawn.needs.mood == null)
			{
				return;
			}
			Map map = this.pawn.Map;
			int num = 0;
			while ((float)num < 100f)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(map) && GenSight.LineOfSight(intVec, this.pawn.Position, map, true, null, 0, 0))
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						IThoughtGiver thoughtGiver = thingList[i] as IThoughtGiver;
						if (thoughtGiver != null)
						{
							Thought_Memory thought_Memory = thoughtGiver.GiveObservedThought();
							if (thought_Memory != null)
							{
								this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
							}
						}
					}
				}
				num++;
			}
		}

		// Token: 0x04004C68 RID: 19560
		private Pawn pawn;

		// Token: 0x04004C69 RID: 19561
		private int intervalsUntilObserve;

		// Token: 0x04004C6A RID: 19562
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x04004C6B RID: 19563
		private const float SampleNumCells = 100f;
	}
}
