using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AC RID: 4268
	public class QuestPart_Choice : QuestPart
	{
		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005D12 RID: 23826 RVA: 0x000408D5 File Offset: 0x0003EAD5
		public override bool PreventsAutoAccept
		{
			get
			{
				return this.choices.Count >= 2;
			}
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x001DBE94 File Offset: 0x001DA094
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalChoiceUsed)
			{
				this.choiceUsed = true;
				for (int i = 0; i < this.choices.Count; i++)
				{
					for (int j = 0; j < this.choices[i].rewards.Count; j++)
					{
						this.choices[i].rewards[j].Notify_Used();
					}
				}
			}
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x001DBF18 File Offset: 0x001DA118
		public override void Notify_PreCleanup()
		{
			base.Notify_PreCleanup();
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					this.choices[i].rewards[j].Notify_PreCleanup();
				}
			}
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x001DBF80 File Offset: 0x001DA180
		public void Choose(QuestPart_Choice.Choice choice)
		{
			for (int i = this.choices.Count - 1; i >= 0; i--)
			{
				if (this.choices[i] != choice)
				{
					for (int j = 0; j < this.choices[i].questParts.Count; j++)
					{
						if (!choice.questParts.Contains(this.choices[i].questParts[j]))
						{
							this.choices[i].questParts[j].Notify_PreCleanup();
							this.choices[i].questParts[j].Cleanup();
							this.quest.RemovePart(this.choices[i].questParts[j]);
						}
					}
					this.choices.RemoveAt(i);
				}
			}
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x001DC070 File Offset: 0x001DA270
		public override void PreQuestAccept()
		{
			base.PreQuestAccept();
			if (this.choices.Count >= 2)
			{
				Log.Error("Tried to accept a quest but " + base.GetType().Name + " still has a choice unresolved. Auto-choosing the first option.", false);
				this.Choose(this.choices[0]);
			}
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x001DC0C4 File Offset: 0x001DA2C4
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					Reward_Items reward_Items;
					if ((reward_Items = (this.choices[i].rewards[j] as Reward_Items)) != null)
					{
						for (int k = 0; k < reward_Items.items.Count; k++)
						{
							if (reward_Items.items[k].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x001DC16C File Offset: 0x001DA36C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalChoiceUsed, "inSignalChoiceUsed", null, false);
			Scribe_Collections.Look<QuestPart_Choice.Choice>(ref this.choices, "choices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.choiceUsed, "choiceUsed", false, false);
		}

		// Token: 0x04003E4C RID: 15948
		public string inSignalChoiceUsed;

		// Token: 0x04003E4D RID: 15949
		public List<QuestPart_Choice.Choice> choices = new List<QuestPart_Choice.Choice>();

		// Token: 0x04003E4E RID: 15950
		public bool choiceUsed;

		// Token: 0x020010AD RID: 4269
		public class Choice : IExposable
		{
			// Token: 0x06005D1A RID: 23834 RVA: 0x000408FB File Offset: 0x0003EAFB
			public void ExposeData()
			{
				Scribe_Collections.Look<QuestPart>(ref this.questParts, "questParts", LookMode.Reference, Array.Empty<object>());
				Scribe_Collections.Look<Reward>(ref this.rewards, "rewards", LookMode.Deep, Array.Empty<object>());
			}

			// Token: 0x04003E4F RID: 15951
			public List<QuestPart> questParts = new List<QuestPart>();

			// Token: 0x04003E50 RID: 15952
			public List<Reward> rewards = new List<Reward>();
		}
	}
}
