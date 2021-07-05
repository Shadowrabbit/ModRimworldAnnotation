using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200013A RID: 314
	public class PlayLogEntry_Interaction : LogEntry
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x000282AD File Offset: 0x000264AD
		protected string InitiatorName
		{
			get
			{
				if (this.initiator == null)
				{
					return "null";
				}
				return this.initiator.LabelShort;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x000282C8 File Offset: 0x000264C8
		private string RecipientName
		{
			get
			{
				if (this.recipient == null)
				{
					return "null";
				}
				return this.recipient.LabelShort;
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00024AB6 File Offset: 0x00022CB6
		public PlayLogEntry_Interaction() : base(null)
		{
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000282E3 File Offset: 0x000264E3
		public PlayLogEntry_Interaction(InteractionDef intDef, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00028321 File Offset: 0x00026521
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipient;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00028337 File Offset: 0x00026537
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipient != null)
			{
				yield return this.recipient;
			}
			yield break;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00028347 File Offset: 0x00026547
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.recipient && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.recipient));
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00028381 File Offset: 0x00026581
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipient);
				return;
			}
			if (pov == this.recipient)
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000283BC File Offset: 0x000265BC
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.GetSymbol(this.initiatorFaction, this.initiatorIdeo);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x000283D5 File Offset: 0x000265D5
		public override Color? IconColorFromPOV(Thing pov)
		{
			return this.intDef.GetSymbolColor(this.initiatorFaction);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000283E8 File Offset: 0x000265E8
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.initiatorFaction == faction)
			{
				this.initiatorFaction = null;
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000283FA File Offset: 0x000265FA
		public override void Notify_IdeoRemoved(Ideo ideo)
		{
			if (this.initiatorIdeo == ideo)
			{
				this.initiatorIdeo = null;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002840C File Offset: 0x0002660C
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00028434 File Offset: 0x00026634
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string text;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else if (pov == this.recipient)
			{
				if (this.intDef.logRulesRecipient != null)
				{
					request.IncludesBare.Add(this.intDef.logRulesRecipient);
				}
				else
				{
					request.IncludesBare.Add(this.intDef.logRulesInitiator);
				}
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
				text = this.ToString();
			}
			if (this.extraSentencePacks != null)
			{
				for (int i = 0; i < this.extraSentencePacks.Count; i++)
				{
					request.Clear();
					request.Includes.Add(this.extraSentencePacks[i]);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
					text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].FirstRuleKeyword, request, "extraSentencePack", forceLog, this.extraSentencePacks[i].FirstUntranslatedRuleKeyword, null, null, true);
				}
			}
			Rand.PopState();
			return text;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000286AC File Offset: 0x000268AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.initiatorFaction, "initiatorFaction", false);
			Scribe_References.Look<Ideo>(ref this.initiatorIdeo, "initiatorIdeo", false);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00028729 File Offset: 0x00026929
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.intDef.label,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x04000804 RID: 2052
		protected InteractionDef intDef;

		// Token: 0x04000805 RID: 2053
		protected Pawn initiator;

		// Token: 0x04000806 RID: 2054
		protected Pawn recipient;

		// Token: 0x04000807 RID: 2055
		protected List<RulePackDef> extraSentencePacks;

		// Token: 0x04000808 RID: 2056
		public Faction initiatorFaction;

		// Token: 0x04000809 RID: 2057
		public Ideo initiatorIdeo;
	}
}
