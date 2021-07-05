using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200013B RID: 315
	public class PlayLogEntry_InteractionWithMany : PlayLogEntry_Interaction
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x00028766 File Offset: 0x00026966
		public PlayLogEntry_InteractionWithMany()
		{
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002876E File Offset: 0x0002696E
		public PlayLogEntry_InteractionWithMany(InteractionDef intDef, Pawn initiator, List<Pawn> recipients, List<RulePackDef> extraSentencePacks)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipients = recipients;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000287AC File Offset: 0x000269AC
		public override bool Concerns(Thing t)
		{
			Pawn item;
			return (item = (t as Pawn)) != null && (t == this.initiator || this.recipients.Contains(item));
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000287DC File Offset: 0x000269DC
		public override IEnumerable<Thing> GetConcerns()
		{
			yield return this.initiator;
			foreach (Pawn pawn in this.recipients)
			{
				yield return pawn;
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000287EC File Offset: 0x000269EC
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipients == null || this.recipients.Contains(null))
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string result;
			Pawn pawn;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				result = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else if ((pawn = (pov as Pawn)) != null && this.recipients.Contains(pawn))
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
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", pawn, request.Constants, true, true));
				result = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
				result = this.ToString();
			}
			Rand.PopState();
			return result;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002898C File Offset: 0x00026B8C
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipients.Contains(pov as Pawn) && CameraJumper.CanJump(this.initiator);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000289B4 File Offset: 0x00026BB4
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipients.RandomElement<Pawn>());
				return;
			}
			if (this.recipients.Contains(pov as Pawn))
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00028A09 File Offset: 0x00026C09
		public override string ToString()
		{
			return this.intDef.label + ": " + base.InitiatorName + "-> Many";
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00028A2C File Offset: 0x00026C2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.recipients, "recipients", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.recipients.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400080A RID: 2058
		private List<Pawn> recipients;
	}
}
