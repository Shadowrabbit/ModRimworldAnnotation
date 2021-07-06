using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001BF RID: 447
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0000EBEE File Offset: 0x0000CDEE
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn == null)
				{
					return "null";
				}
				return this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0000EC09 File Offset: 0x0000CE09
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0000EC12 File Offset: 0x0000CE12
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0000EC30 File Offset: 0x0000CE30
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0000EC46 File Offset: 0x0000CE46
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0000EC56 File Offset: 0x0000CE56
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return CameraJumper.CanJump(this.recipientPawn);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0000EC68 File Offset: 0x0000CE68
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0000EC7A File Offset: 0x0000CE7A
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0000EC81 File Offset: 0x0000CE81
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000A0778 File Offset: 0x0009E978
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709, false);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			return result;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0000EC9D File Offset: 0x0000CE9D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0000ECD7 File Offset: 0x0000CED7
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x04000A17 RID: 2583
		private Pawn initiatorPawn;

		// Token: 0x04000A18 RID: 2584
		private Pawn recipientPawn;

		// Token: 0x04000A19 RID: 2585
		private RulePackDef ruleDef;
	}
}
