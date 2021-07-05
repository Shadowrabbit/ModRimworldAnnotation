using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001498 RID: 5272
	public abstract class Reward : IExposable
	{
		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06007DF6 RID: 32246 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06007DF7 RID: 32247 RVA: 0x002CAB5D File Offset: 0x002C8D5D
		public virtual IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06007DF8 RID: 32248 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float TotalMarketValue
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06007DF9 RID: 32249
		public abstract void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed);

		// Token: 0x06007DFA RID: 32250
		public abstract IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules);

		// Token: 0x06007DFB RID: 32251
		public abstract string GetDescription(RewardsGeneratorParams parms);

		// Token: 0x06007DFC RID: 32252 RVA: 0x002CAB66 File Offset: 0x002C8D66
		public virtual void Notify_Used()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x06007DFD RID: 32253 RVA: 0x002CAB66 File Offset: 0x002C8D66
		public virtual void Notify_PreCleanup()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x002CAB6F File Offset: 0x002C8D6F
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.usedOrCleanedUp, "usedOrCleanedUp", false, false);
		}

		// Token: 0x04004E88 RID: 20104
		protected bool usedOrCleanedUp;
	}
}
