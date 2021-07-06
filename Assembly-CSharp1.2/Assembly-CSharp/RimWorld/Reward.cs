using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CDD RID: 7389
	public abstract class Reward : IExposable
	{
		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x0600A08D RID: 41101 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x0600A08E RID: 41102 RVA: 0x0006AFBF File Offset: 0x000691BF
		public virtual IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x0600A08F RID: 41103 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float TotalMarketValue
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600A090 RID: 41104
		public abstract void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed);

		// Token: 0x0600A091 RID: 41105
		public abstract IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules);

		// Token: 0x0600A092 RID: 41106
		public abstract string GetDescription(RewardsGeneratorParams parms);

		// Token: 0x0600A093 RID: 41107 RVA: 0x0006AFC8 File Offset: 0x000691C8
		public virtual void Notify_Used()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x0600A094 RID: 41108 RVA: 0x0006AFC8 File Offset: 0x000691C8
		public virtual void Notify_PreCleanup()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x0600A095 RID: 41109 RVA: 0x0006AFD1 File Offset: 0x000691D1
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.usedOrCleanedUp, "usedOrCleanedUp", false, false);
		}

		// Token: 0x04006D12 RID: 27922
		protected bool usedOrCleanedUp;
	}
}
