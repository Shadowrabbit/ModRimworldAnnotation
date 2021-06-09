using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001DA RID: 474
	public class PlayLogEntry_Interaction : LogEntry
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0000F802 File Offset: 0x0000DA02
		private string InitiatorName
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

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0000F81D File Offset: 0x0000DA1D
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

		// Token: 0x06000C4F RID: 3151 RVA: 0x0000ED2E File Offset: 0x0000CF2E
		public PlayLogEntry_Interaction() : base(null)
		{
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0000F838 File Offset: 0x0000DA38
		public PlayLogEntry_Interaction(InteractionDef intDef, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.extraSentencePacks = extraSentencePacks;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0000F85E File Offset: 0x0000DA5E
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipient;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0000F874 File Offset: 0x0000DA74
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

		// Token: 0x06000C53 RID: 3155 RVA: 0x0000F884 File Offset: 0x0000DA84
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.recipient && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.recipient));
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0000F8BE File Offset: 0x0000DABE
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

		// Token: 0x06000C55 RID: 3157 RVA: 0x0000F8F9 File Offset: 0x0000DAF9
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.Symbol;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0000F906 File Offset: 0x0000DB06
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000A3C9C File Offset: 0x000A1E9C
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422, false);
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
				Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251, false);
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

		// Token: 0x06000C58 RID: 3160 RVA: 0x000A3F18 File Offset: 0x000A2118
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0000F92D File Offset: 0x0000DB2D
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

		// Token: 0x04000AAE RID: 2734
		private InteractionDef intDef;

		// Token: 0x04000AAF RID: 2735
		private Pawn initiator;

		// Token: 0x04000AB0 RID: 2736
		private Pawn recipient;

		// Token: 0x04000AB1 RID: 2737
		private List<RulePackDef> extraSentencePacks;
	}
}
