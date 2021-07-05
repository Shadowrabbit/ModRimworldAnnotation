using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B69 RID: 2921
	public class QuestPart_Choice : QuestPart
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004450 RID: 17488 RVA: 0x0016AA38 File Offset: 0x00168C38
		public override bool PreventsAutoAccept
		{
			get
			{
				return this.choices.Count >= 2;
			}
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0016AA4C File Offset: 0x00168C4C
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

		// Token: 0x06004452 RID: 17490 RVA: 0x0016AAD0 File Offset: 0x00168CD0
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

		// Token: 0x06004453 RID: 17491 RVA: 0x0016AB38 File Offset: 0x00168D38
		public override void Notify_PawnDiscarded(Pawn pawn)
		{
			foreach (QuestPart_Choice.Choice choice in this.choices)
			{
				for (int i = choice.rewards.Count - 1; i >= 0; i--)
				{
					Reward_Pawn reward_Pawn;
					if ((reward_Pawn = (choice.rewards[i] as Reward_Pawn)) != null && reward_Pawn.pawn == pawn)
					{
						choice.rewards.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0016ABC8 File Offset: 0x00168DC8
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

		// Token: 0x06004455 RID: 17493 RVA: 0x0016ACB8 File Offset: 0x00168EB8
		public override void PreQuestAccept()
		{
			base.PreQuestAccept();
			if (this.choices.Count >= 2)
			{
				Log.Error("Tried to accept a quest but " + base.GetType().Name + " still has a choice unresolved. Auto-choosing the first option.");
				this.Choose(this.choices[0]);
			}
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0016AD0C File Offset: 0x00168F0C
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

		// Token: 0x06004457 RID: 17495 RVA: 0x0016ADB4 File Offset: 0x00168FB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalChoiceUsed, "inSignalChoiceUsed", null, false);
			Scribe_Collections.Look<QuestPart_Choice.Choice>(ref this.choices, "choices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.choiceUsed, "choiceUsed", false, false);
		}

		// Token: 0x04002975 RID: 10613
		public string inSignalChoiceUsed;

		// Token: 0x04002976 RID: 10614
		public List<QuestPart_Choice.Choice> choices = new List<QuestPart_Choice.Choice>();

		// Token: 0x04002977 RID: 10615
		public bool choiceUsed;

		// Token: 0x02002084 RID: 8324
		public class Choice : IExposable
		{
			// Token: 0x0600BA32 RID: 47666 RVA: 0x003C1F63 File Offset: 0x003C0163
			public void ExposeData()
			{
				Scribe_Collections.Look<QuestPart>(ref this.questParts, "questParts", LookMode.Reference, Array.Empty<object>());
				Scribe_Collections.Look<Reward>(ref this.rewards, "rewards", LookMode.Deep, Array.Empty<object>());
			}

			// Token: 0x04007C9C RID: 31900
			public List<QuestPart> questParts = new List<QuestPart>();

			// Token: 0x04007C9D RID: 31901
			public List<Reward> rewards = new List<Reward>();
		}
	}
}
