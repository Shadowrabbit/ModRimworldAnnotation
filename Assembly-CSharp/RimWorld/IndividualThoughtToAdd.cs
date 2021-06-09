using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013ED RID: 5101
	public struct IndividualThoughtToAdd
	{
		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06006E4F RID: 28239 RVA: 0x0021C2E0 File Offset: 0x0021A4E0
		public string LabelCap
		{
			get
			{
				string text = this.thought.LabelCap;
				float num = this.thought.MoodOffset();
				if (num != 0f)
				{
					text = text + " " + Mathf.RoundToInt(num).ToStringWithSign();
				}
				return text;
			}
		}

		// Token: 0x06006E50 RID: 28240 RVA: 0x0021C328 File Offset: 0x0021A528
		public IndividualThoughtToAdd(ThoughtDef thoughtDef, Pawn addTo, Pawn otherPawn = null, float moodPowerFactor = 1f, float opinionOffsetFactor = 1f)
		{
			this.addTo = addTo;
			this.otherPawn = otherPawn;
			this.thought = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			this.thought.moodPowerFactor = moodPowerFactor;
			this.thought.otherPawn = otherPawn;
			this.thought.pawn = addTo;
			Thought_MemorySocial thought_MemorySocial = this.thought as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= opinionOffsetFactor;
			}
		}

		// Token: 0x06006E51 RID: 28241 RVA: 0x0021C398 File Offset: 0x0021A598
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		// Token: 0x040048BE RID: 18622
		public Thought_Memory thought;

		// Token: 0x040048BF RID: 18623
		public Pawn addTo;

		// Token: 0x040048C0 RID: 18624
		private Pawn otherPawn;
	}
}
