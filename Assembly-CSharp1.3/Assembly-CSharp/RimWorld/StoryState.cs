using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2D RID: 3117
	public class StoryState : IExposable
	{
		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x001838D7 File Offset: 0x00181AD7
		public IIncidentTarget Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x0600492C RID: 18732 RVA: 0x001838DF File Offset: 0x00181ADF
		public List<QuestScriptDef> RecentRandomQuests
		{
			get
			{
				return this.recentRandomQuests;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x001838E7 File Offset: 0x00181AE7
		public List<QuestScriptDef> RecentRandomDecrees
		{
			get
			{
				return this.recentRandomDecrees;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x0600492E RID: 18734 RVA: 0x001838EF File Offset: 0x00181AEF
		public int LastRoyalFavorQuestTick
		{
			get
			{
				return this.lastRoyalFavorQuestTick;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x0600492F RID: 18735 RVA: 0x001838F8 File Offset: 0x00181AF8
		public int LastThreatBigTick
		{
			get
			{
				if (this.lastThreatBigTick > Find.TickManager.TicksGame + 1000)
				{
					Log.Error(string.Concat(new object[]
					{
						"Latest big threat queue time was ",
						this.lastThreatBigTick,
						" at tick ",
						Find.TickManager.TicksGame,
						". This is too far in the future. Resetting."
					}));
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x00183980 File Offset: 0x00181B80
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x001839D4 File Offset: 0x00181BD4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomQuests, "recentRandomQuests", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomDecrees, "recentRandomDecrees", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<int, int>(ref this.colonistCountTicks, "colonistCountTicks", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<int>(ref this.lastRoyalFavorQuestTick, "lastRoyalFavorQuestTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.recentRandomQuests == null)
				{
					this.recentRandomQuests = new List<QuestScriptDef>();
				}
				if (this.recentRandomDecrees == null)
				{
					this.recentRandomDecrees = new List<QuestScriptDef>();
				}
				if (this.colonistCountTicks == null)
				{
					this.colonistCountTicks = new Dictionary<int, int>();
				}
				this.RecordPopulationIncrease();
			}
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x00183A9C File Offset: 0x00181C9C
		public void Notify_IncidentFired(FiringIncident fi)
		{
			if (fi.parms.forced || fi.parms.target != this.target)
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (fi.def.category == IncidentCategoryDefOf.ThreatBig)
			{
				this.lastThreatBigTick = ticksGame;
				Find.StoryWatcher.statsRecord.numThreatBigs++;
			}
			if (this.lastFireTicks.ContainsKey(fi.def))
			{
				this.lastFireTicks[fi.def] = ticksGame;
			}
			else
			{
				this.lastFireTicks.Add(fi.def, ticksGame);
			}
			if (fi.def == IncidentDefOf.GiveQuest_Random)
			{
				this.RecordRandomQuestFired(fi.parms.questScriptDef);
			}
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00183B5C File Offset: 0x00181D5C
		public void RecordRandomQuestFired(QuestScriptDef questScript)
		{
			this.recentRandomQuests.Insert(0, questScript);
			while (this.recentRandomQuests.Count > 5)
			{
				this.recentRandomQuests.RemoveAt(this.recentRandomQuests.Count - 1);
			}
			if (questScript.canGiveRoyalFavor)
			{
				this.lastRoyalFavorQuestTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x00183BB6 File Offset: 0x00181DB6
		public void RecordDecreeFired(QuestScriptDef questScript)
		{
			this.recentRandomDecrees.Insert(0, questScript);
			while (this.recentRandomDecrees.Count > 5)
			{
				this.recentRandomDecrees.RemoveAt(this.recentRandomDecrees.Count - 1);
			}
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00183BF0 File Offset: 0x00181DF0
		public void RecordPopulationIncrease()
		{
			int count = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00183C2C File Offset: 0x00181E2C
		public int GetTicksFromColonistCount(int count)
		{
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
			return this.colonistCountTicks[count];
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00183C60 File Offset: 0x00181E60
		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
			other.RecentRandomQuests.Clear();
			other.recentRandomQuests.AddRange(this.RecentRandomQuests);
			other.RecentRandomDecrees.Clear();
			other.RecentRandomDecrees.AddRange(this.RecentRandomDecrees);
			other.lastRoyalFavorQuestTick = this.lastRoyalFavorQuestTick;
			other.colonistCountTicks.Clear();
			foreach (KeyValuePair<int, int> keyValuePair2 in this.colonistCountTicks)
			{
				other.colonistCountTicks.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
		}

		// Token: 0x04002C7E RID: 11390
		private IIncidentTarget target;

		// Token: 0x04002C7F RID: 11391
		private int lastThreatBigTick = -1;

		// Token: 0x04002C80 RID: 11392
		private Dictionary<int, int> colonistCountTicks = new Dictionary<int, int>();

		// Token: 0x04002C81 RID: 11393
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		// Token: 0x04002C82 RID: 11394
		private List<QuestScriptDef> recentRandomQuests = new List<QuestScriptDef>();

		// Token: 0x04002C83 RID: 11395
		private List<QuestScriptDef> recentRandomDecrees = new List<QuestScriptDef>();

		// Token: 0x04002C84 RID: 11396
		private int lastRoyalFavorQuestTick = -1;

		// Token: 0x04002C85 RID: 11397
		private const int RecentRandomQuestsMaxStorage = 5;
	}
}
