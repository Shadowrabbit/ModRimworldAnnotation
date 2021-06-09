using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001256 RID: 4694
	public class StoryWatcher_PopAdaptation : IExposable
	{
		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06006663 RID: 26211 RVA: 0x00045F29 File Offset: 0x00044129
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x001F9664 File Offset: 0x001F7864
		public void Notify_PawnEvent(Pawn p, PopAdaptationEvent ev)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message(string.Concat(new object[]
				{
					"PopAdaptation event: ",
					ev,
					" - ",
					p
				}), false);
			}
			if (ev == PopAdaptationEvent.GainedColonist)
			{
				this.adaptDays = 0f;
			}
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x001F96C4 File Offset: 0x001F78C4
		public void PopAdaptationWatcherTick()
		{
			if (Find.TickManager.TicksGame % 30000 == 171)
			{
				float num = 0.5f;
				this.adaptDays += num;
			}
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x00045F31 File Offset: 0x00044131
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x06006667 RID: 26215 RVA: 0x00045F49 File Offset: 0x00044149
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x04004437 RID: 17463
		private float adaptDays;

		// Token: 0x04004438 RID: 17464
		private const int UpdateInterval = 30000;
	}
}
