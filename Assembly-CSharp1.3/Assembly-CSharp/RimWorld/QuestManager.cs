using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B10 RID: 2832
	public class QuestManager : IExposable
	{
		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600428E RID: 17038 RVA: 0x0016485C File Offset: 0x00162A5C
		public List<Quest> QuestsListForReading
		{
			get
			{
				return this.quests;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x00164864 File Offset: 0x00162A64
		public List<QuestPart_SituationalThought> SituationalThoughtQuestParts
		{
			get
			{
				return this.cachedSituationalThoughtQuestParts;
			}
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x0016486C File Offset: 0x00162A6C
		public void Add(Quest quest)
		{
			if (quest == null)
			{
				Log.Error("Tried to add a null quest.");
				return;
			}
			if (this.Contains(quest))
			{
				Log.Error("Tried to add the same quest twice: " + quest.ToStringSafe<Quest>());
				return;
			}
			this.quests.Add(quest);
			this.AddToCache(quest);
			Find.SignalManager.RegisterReceiver(quest);
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				partsListForReading[i].PostQuestAdded();
			}
			quest.PostAdded();
			if (quest.initiallyAccepted)
			{
				quest.Initiate();
			}
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x001648FC File Offset: 0x00162AFC
		public void Remove(Quest quest)
		{
			if (!this.Contains(quest))
			{
				Log.Error("Tried to remove non-existent quest: " + quest.ToStringSafe<Quest>());
				return;
			}
			this.quests.Remove(quest);
			this.RemoveFromCache(quest);
			Find.SignalManager.DeregisterReceiver(quest);
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x0016493C File Offset: 0x00162B3C
		public bool Contains(Quest quest)
		{
			return this.quests.Contains(quest);
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x0016494C File Offset: 0x00162B4C
		public void QuestManagerTick()
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].QuestTick();
			}
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x00164980 File Offset: 0x00162B80
		public bool IsReservedByAnyQuest(Pawn p)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].QuestReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x001649BC File Offset: 0x00162BBC
		public bool IsReservedByAnyQuest(Faction f)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].QuestReserves(f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x001649F8 File Offset: 0x00162BF8
		private void AddToCache(Quest quest)
		{
			this.questsInDisplayOrder.Add(quest);
			this.questsInDisplayOrder.SortBy((Quest x) => x.TicksSinceAppeared);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Add(questPart_SituationalThought);
				}
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00164A74 File Offset: 0x00162C74
		private void RemoveFromCache(Quest quest)
		{
			this.questsInDisplayOrder.Remove(quest);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Remove(questPart_SituationalThought);
				}
			}
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00164AC8 File Offset: 0x00162CC8
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x00164B00 File Offset: 0x00162D00
		public void ExposeData()
		{
			Scribe_Collections.Look<Quest>(ref this.quests, "quests", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = this.quests.RemoveAll((Quest x) => x == null);
				if (num != 0)
				{
					Log.Error(num + " quest(s) were null after loading.");
				}
				int num2 = this.quests.RemoveAll((Quest q) => q.root == null);
				if (num2 != 0)
				{
					Log.Error(num2 + " quest(s) had null roots after loading.");
				}
				this.cachedSituationalThoughtQuestParts.Clear();
				this.questsInDisplayOrder.Clear();
				for (int i = 0; i < this.quests.Count; i++)
				{
					this.AddToCache(this.quests[i]);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = 0; j < this.quests.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.quests[j]);
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x00164C2C File Offset: 0x00162E2C
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].State == QuestState.Ongoing)
				{
					this.quests[i].Notify_ThingsProduced(worker, things);
				}
			}
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x00164C78 File Offset: 0x00162E78
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].State == QuestState.Ongoing)
				{
					this.quests[i].Notify_PlantHarvested(worker, harvested);
				}
			}
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00164CC4 File Offset: 0x00162EC4
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].State == QuestState.Ongoing)
				{
					this.quests[i].Notify_PawnKilled(pawn, dinfo);
				}
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x00164D10 File Offset: 0x00162F10
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x04002891 RID: 10385
		private List<Quest> quests = new List<Quest>();

		// Token: 0x04002892 RID: 10386
		public List<Quest> questsInDisplayOrder = new List<Quest>();

		// Token: 0x04002893 RID: 10387
		private List<QuestPart_SituationalThought> cachedSituationalThoughtQuestParts = new List<QuestPart_SituationalThought>();
	}
}
