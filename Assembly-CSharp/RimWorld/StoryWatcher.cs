using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001252 RID: 4690
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x06006656 RID: 26198 RVA: 0x00045E81 File Offset: 0x00044081
		public void StoryWatcherTick()
		{
			this.watcherAdaptation.AdaptationWatcherTick();
			this.watcherPopAdaptation.PopAdaptationWatcherTick();
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x001F93EC File Offset: 0x001F75EC
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_Adaptation>(ref this.watcherAdaptation, "watcherAdaptation", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_PopAdaptation>(ref this.watcherPopAdaptation, "watcherPopAdaptation", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0400442A RID: 17450
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x0400442B RID: 17451
		public StoryWatcher_Adaptation watcherAdaptation = new StoryWatcher_Adaptation();

		// Token: 0x0400442C RID: 17452
		public StoryWatcher_PopAdaptation watcherPopAdaptation = new StoryWatcher_PopAdaptation();
	}
}
