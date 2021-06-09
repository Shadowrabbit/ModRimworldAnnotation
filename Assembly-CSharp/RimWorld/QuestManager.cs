using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001041 RID: 4161
	public class QuestManager : IExposable
	{
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x0003ED78 File Offset: 0x0003CF78
		public List<Quest> QuestsListForReading
		{
			get
			{
				return this.quests;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x0003ED80 File Offset: 0x0003CF80
		public List<QuestPart_SituationalThought> SituationalThoughtQuestParts
		{
			get
			{
				return this.cachedSituationalThoughtQuestParts;
			}
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x001D6334 File Offset: 0x001D4534
		public void Add(Quest quest)
		{
			if (quest == null)
			{
				Log.Error("Tried to add a null quest.", false);
				return;
			}
			if (this.Contains(quest))
			{
				Log.Error("Tried to add the same quest twice: " + quest.ToStringSafe<Quest>(), false);
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
			if (quest.initiallyAccepted)
			{
				quest.Initiate();
			}
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x001D63C0 File Offset: 0x001D45C0
		public void Remove(Quest quest)
		{
			if (!this.Contains(quest))
			{
				Log.Error("Tried to remove non-existent quest: " + quest.ToStringSafe<Quest>(), false);
				return;
			}
			this.quests.Remove(quest);
			this.RemoveFromCache(quest);
			Find.SignalManager.DeregisterReceiver(quest);
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x0003ED88 File Offset: 0x0003CF88
		public bool Contains(Quest quest)
		{
			return this.quests.Contains(quest);
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x001D640C File Offset: 0x001D460C
		public void QuestManagerTick()
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].QuestTick();
			}
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x001D6440 File Offset: 0x001D4640
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

		// Token: 0x06005AB0 RID: 23216 RVA: 0x001D647C File Offset: 0x001D467C
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

		// Token: 0x06005AB1 RID: 23217 RVA: 0x001D64B8 File Offset: 0x001D46B8
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

		// Token: 0x06005AB2 RID: 23218 RVA: 0x001D6534 File Offset: 0x001D4734
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

		// Token: 0x06005AB3 RID: 23219 RVA: 0x001D6588 File Offset: 0x001D4788
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x001D65C0 File Offset: 0x001D47C0
		public void ExposeData()
		{
			Scribe_Collections.Look<Quest>(ref this.quests, "quests", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = this.quests.RemoveAll((Quest x) => x == null);
				if (num != 0)
				{
					Log.Error(num + " quest(s) were null after loading.", false);
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

		// Token: 0x06005AB5 RID: 23221 RVA: 0x001D66AC File Offset: 0x001D48AC
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

		// Token: 0x06005AB6 RID: 23222 RVA: 0x001D66F8 File Offset: 0x001D48F8
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

		// Token: 0x06005AB7 RID: 23223 RVA: 0x001D6744 File Offset: 0x001D4944
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

		// Token: 0x06005AB8 RID: 23224 RVA: 0x001D6790 File Offset: 0x001D4990
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x04003CFA RID: 15610
		private List<Quest> quests = new List<Quest>();

		// Token: 0x04003CFB RID: 15611
		public List<Quest> questsInDisplayOrder = new List<Quest>();

		// Token: 0x04003CFC RID: 15612
		private List<QuestPart_SituationalThought> cachedSituationalThoughtQuestParts = new List<QuestPart_SituationalThought>();
	}
}
