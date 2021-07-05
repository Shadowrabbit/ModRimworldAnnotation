using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D4 RID: 2516
	public class Thought_Counselled : Thought_Memory
	{
		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x00154F7F File Offset: 0x0015317F
		public override int DurationTicks
		{
			get
			{
				return this.durationTicksOverride;
			}
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x00154F87 File Offset: 0x00153187
		public override float MoodOffset()
		{
			return this.moodOffsetOverride;
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x00154F8F File Offset: 0x0015318F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.moodOffsetOverride, "moodOffsetOverride", 0f, false);
		}

		// Token: 0x040020EB RID: 8427
		public float moodOffsetOverride;
	}
}
