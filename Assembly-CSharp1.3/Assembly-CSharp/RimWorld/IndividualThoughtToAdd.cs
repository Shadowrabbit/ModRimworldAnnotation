using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB2 RID: 3506
	public struct IndividualThoughtToAdd
	{
		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x001B26D4 File Offset: 0x001B08D4
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

		// Token: 0x06005124 RID: 20772 RVA: 0x001B271C File Offset: 0x001B091C
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

		// Token: 0x06005125 RID: 20773 RVA: 0x001B278C File Offset: 0x001B098C
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		// Token: 0x0400300F RID: 12303
		public Thought_Memory thought;

		// Token: 0x04003010 RID: 12304
		public Pawn addTo;

		// Token: 0x04003011 RID: 12305
		private Pawn otherPawn;
	}
}
