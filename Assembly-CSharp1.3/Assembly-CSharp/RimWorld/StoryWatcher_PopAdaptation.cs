using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C33 RID: 3123
	public class StoryWatcher_PopAdaptation : IExposable
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004949 RID: 18761 RVA: 0x0018415C File Offset: 0x0018235C
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x00184164 File Offset: 0x00182364
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
				}));
			}
			if (ev == PopAdaptationEvent.GainedColonist)
			{
				this.adaptDays = 0f;
			}
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x001841C4 File Offset: 0x001823C4
		public void PopAdaptationWatcherTick()
		{
			if (Find.TickManager.TicksGame % 30000 == 171)
			{
				float num = 0.5f;
				this.adaptDays += num;
			}
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x001841FC File Offset: 0x001823FC
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x00184214 File Offset: 0x00182414
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x04002C98 RID: 11416
		private float adaptDays;

		// Token: 0x04002C99 RID: 11417
		private const int UpdateInterval = 30000;
	}
}
