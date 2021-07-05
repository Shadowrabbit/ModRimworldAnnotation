using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000127 RID: 295
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002493A File Offset: 0x00022B3A
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

		// Token: 0x060007DE RID: 2014 RVA: 0x00024955 File Offset: 0x00022B55
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002495E File Offset: 0x00022B5E
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002497C File Offset: 0x00022B7C
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00024992 File Offset: 0x00022B92
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

		// Token: 0x060007E2 RID: 2018 RVA: 0x000249A2 File Offset: 0x00022BA2
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return CameraJumper.CanJump(this.recipientPawn);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000249B4 File Offset: 0x00022BB4
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000249C6 File Offset: 0x00022BC6
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000249CD File Offset: 0x00022BCD
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000249EC File Offset: 0x00022BEC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			return result;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00024A4F File Offset: 0x00022C4F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00024A89 File Offset: 0x00022C89
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x0400078F RID: 1935
		private Pawn initiatorPawn;

		// Token: 0x04000790 RID: 1936
		private Pawn recipientPawn;

		// Token: 0x04000791 RID: 1937
		private RulePackDef ruleDef;
	}
}
