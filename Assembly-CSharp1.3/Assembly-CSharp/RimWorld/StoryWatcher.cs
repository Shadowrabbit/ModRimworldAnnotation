using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2F RID: 3119
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x0600493C RID: 18748 RVA: 0x00183E32 File Offset: 0x00182032
		public void StoryWatcherTick()
		{
			this.watcherAdaptation.AdaptationWatcherTick();
			this.watcherPopAdaptation.PopAdaptationWatcherTick();
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00183E4C File Offset: 0x0018204C
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_Adaptation>(ref this.watcherAdaptation, "watcherAdaptation", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_PopAdaptation>(ref this.watcherPopAdaptation, "watcherPopAdaptation", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04002C8B RID: 11403
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x04002C8C RID: 11404
		public StoryWatcher_Adaptation watcherAdaptation = new StoryWatcher_Adaptation();

		// Token: 0x04002C8D RID: 11405
		public StoryWatcher_PopAdaptation watcherPopAdaptation = new StoryWatcher_PopAdaptation();
	}
}
