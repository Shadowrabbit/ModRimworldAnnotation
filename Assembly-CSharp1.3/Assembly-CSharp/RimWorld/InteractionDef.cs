using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000A83 RID: 2691
	public class InteractionDef : Def
	{
		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x0015C1C8 File Offset: 0x0015A3C8
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

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x0015C1FA File Offset: 0x0015A3FA
		private Texture2D Symbol
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

		// Token: 0x0600404C RID: 16460 RVA: 0x0015C224 File Offset: 0x0015A424
		public Texture2D GetSymbol(Faction initiatorFaction = null, Ideo initatorIdeo = null)
		{
			InteractionSymbolSource interactionSymbolSource = this.symbolSource;
			if (interactionSymbolSource != InteractionSymbolSource.InitiatorIdeo)
			{
				if (interactionSymbolSource != InteractionSymbolSource.InitiatorFaction)
				{
					return this.Symbol;
				}
				if (initiatorFaction == null)
				{
					return null;
				}
				return initiatorFaction.def.FactionIcon;
			}
			else
			{
				if (initatorIdeo == null)
				{
					return null;
				}
				return initatorIdeo.Icon;
			}
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0015C268 File Offset: 0x0015A468
		public Color? GetSymbolColor(Faction initiatorFaction = null)
		{
			if (initiatorFaction != null && this.symbolSource == InteractionSymbolSource.InitiatorFaction)
			{
				return new Color?(initiatorFaction.Color);
			}
			return null;
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0015C296 File Offset: 0x0015A496
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x040024C7 RID: 9415
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x040024C8 RID: 9416
		public ThingDef interactionMote;

		// Token: 0x040024C9 RID: 9417
		public float socialFightBaseChance;

		// Token: 0x040024CA RID: 9418
		public ThoughtDef initiatorThought;

		// Token: 0x040024CB RID: 9419
		public SkillDef initiatorXpGainSkill;

		// Token: 0x040024CC RID: 9420
		public int initiatorXpGainAmount;

		// Token: 0x040024CD RID: 9421
		public ThoughtDef recipientThought;

		// Token: 0x040024CE RID: 9422
		public SkillDef recipientXpGainSkill;

		// Token: 0x040024CF RID: 9423
		public int recipientXpGainAmount;

		// Token: 0x040024D0 RID: 9424
		public bool ignoreTimeSinceLastInteraction;

		// Token: 0x040024D1 RID: 9425
		[NoTranslate]
		private string symbol;

		// Token: 0x040024D2 RID: 9426
		public InteractionSymbolSource symbolSource;

		// Token: 0x040024D3 RID: 9427
		public RulePack logRulesInitiator;

		// Token: 0x040024D4 RID: 9428
		public RulePack logRulesRecipient;

		// Token: 0x040024D5 RID: 9429
		[Unsaved(false)]
		private InteractionWorker workerInt;

		// Token: 0x040024D6 RID: 9430
		[Unsaved(false)]
		private Texture2D symbolTex;
	}
}
