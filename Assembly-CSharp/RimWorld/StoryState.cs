using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001250 RID: 4688
	public class StoryState : IExposable
	{
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06006645 RID: 26181 RVA: 0x00045DE8 File Offset: 0x00043FE8
		public IIncidentTarget Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06006646 RID: 26182 RVA: 0x00045DF0 File Offset: 0x00043FF0
		public List<QuestScriptDef> RecentRandomQuests
		{
			get
			{
				return this.recentRandomQuests;
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06006647 RID: 26183 RVA: 0x00045DF8 File Offset: 0x00043FF8
		public List<QuestScriptDef> RecentRandomDecrees
		{
			get
			{
				return this.recentRandomDecrees;
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06006648 RID: 26184 RVA: 0x00045E00 File Offset: 0x00044000
		public int LastRoyalFavorQuestTick
		{
			get
			{
				return this.lastRoyalFavorQuestTick;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06006649 RID: 26185 RVA: 0x001F8F2C File Offset: 0x001F712C
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
					}), false);
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		// Token: 0x0600664A RID: 26186 RVA: 0x001F8FB4 File Offset: 0x001F71B4
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x001F9008 File Offset: 0x001F7208
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

		// Token: 0x0600664C RID: 26188 RVA: 0x001F90D0 File Offset: 0x001F72D0
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

		// Token: 0x0600664D RID: 26189 RVA: 0x001F9190 File Offset: 0x001F7390
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

		// Token: 0x0600664E RID: 26190 RVA: 0x00045E08 File Offset: 0x00044008
		public void RecordDecreeFired(QuestScriptDef questScript)
		{
			this.recentRandomDecrees.Insert(0, questScript);
			while (this.recentRandomDecrees.Count > 5)
			{
				this.recentRandomDecrees.RemoveAt(this.recentRandomDecrees.Count - 1);
			}
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x001F91EC File Offset: 0x001F73EC
		public void RecordPopulationIncrease()
		{
			int count = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x00045E3F File Offset: 0x0004403F
		public int GetTicksFromColonistCount(int count)
		{
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
			return this.colonistCountTicks[count];
		}

		// Token: 0x06006651 RID: 26193 RVA: 0x001F9228 File Offset: 0x001F7428
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

		// Token: 0x0400441D RID: 17437
		private IIncidentTarget target;

		// Token: 0x0400441E RID: 17438
		private int lastThreatBigTick = -1;

		// Token: 0x0400441F RID: 17439
		private Dictionary<int, int> colonistCountTicks = new Dictionary<int, int>();

		// Token: 0x04004420 RID: 17440
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		// Token: 0x04004421 RID: 17441
		private List<QuestScriptDef> recentRandomQuests = new List<QuestScriptDef>();

		// Token: 0x04004422 RID: 17442
		private List<QuestScriptDef> recentRandomDecrees = new List<QuestScriptDef>();

		// Token: 0x04004423 RID: 17443
		private int lastRoyalFavorQuestTick = -1;

		// Token: 0x04004424 RID: 17444
		private const int RecentRandomQuestsMaxStorage = 5;
	}
}
