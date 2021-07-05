using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001C1 RID: 449
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0000ED13 File Offset: 0x0000CF13
		private string SubjectName
		{
			get
			{
				if (this.subjectPawn == null)
				{
					return "null";
				}
				return this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0000ED2E File Offset: 0x0000CF2E
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000A08A0 File Offset: 0x0009EAA0
		public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			this.eventDef = eventDef;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0000ED37 File Offset: 0x0000CF37
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0000ED4D File Offset: 0x0000CF4D
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			yield break;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0000ED5D File Offset: 0x0000CF5D
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiatorPawn)) || (pov == this.initiatorPawn && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0000ED97 File Offset: 0x0000CF97
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn);
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000A0908 File Offset: 0x0009EB08
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(this.eventDef);
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			return result;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000A09D8 File Offset: 0x0009EBD8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0000EDD2 File Offset: 0x0000CFD2
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04000A1E RID: 2590
		protected RulePackDef eventDef;

		// Token: 0x04000A1F RID: 2591
		protected Pawn subjectPawn;

		// Token: 0x04000A20 RID: 2592
		protected ThingDef subjectThing;

		// Token: 0x04000A21 RID: 2593
		protected Pawn initiatorPawn;

		// Token: 0x04000A22 RID: 2594
		protected ThingDef initiatorThing;
	}
}
