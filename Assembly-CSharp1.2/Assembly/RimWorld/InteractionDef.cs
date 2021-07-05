using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FAD RID: 4013
	public class InteractionDef : Def
	{
		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x060057CF RID: 22479 RVA: 0x0003CE12 File Offset: 0x0003B012
		public InteractionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InteractionWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.interaction = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x060057D0 RID: 22480 RVA: 0x0003CE44 File Offset: 0x0003B044
		public Texture2D Symbol
		{
			get
			{
				if (this.symbolTex == null)
				{
					this.symbolTex = ContentFinder<Texture2D>.Get(this.symbol, true);
				}
				return this.symbolTex;
			}
		}

		// Token: 0x060057D1 RID: 22481 RVA: 0x0003CE6C File Offset: 0x0003B06C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x040039AD RID: 14765
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x040039AE RID: 14766
		public ThingDef interactionMote;

		// Token: 0x040039AF RID: 14767
		public float socialFightBaseChance;

		// Token: 0x040039B0 RID: 14768
		public ThoughtDef initiatorThought;

		// Token: 0x040039B1 RID: 14769
		public SkillDef initiatorXpGainSkill;

		// Token: 0x040039B2 RID: 14770
		public int initiatorXpGainAmount;

		// Token: 0x040039B3 RID: 14771
		public ThoughtDef recipientThought;

		// Token: 0x040039B4 RID: 14772
		public SkillDef recipientXpGainSkill;

		// Token: 0x040039B5 RID: 14773
		public int recipientXpGainAmount;

		// Token: 0x040039B6 RID: 14774
		public bool ignoreTimeSinceLastInteraction;

		// Token: 0x040039B7 RID: 14775
		[NoTranslate]
		private string symbol;

		// Token: 0x040039B8 RID: 14776
		public RulePack logRulesInitiator;

		// Token: 0x040039B9 RID: 14777
		public RulePack logRulesRecipient;

		// Token: 0x040039BA RID: 14778
		[Unsaved(false)]
		private InteractionWorker workerInt;

		// Token: 0x040039BB RID: 14779
		[Unsaved(false)]
		private Texture2D symbolTex;
	}
}
