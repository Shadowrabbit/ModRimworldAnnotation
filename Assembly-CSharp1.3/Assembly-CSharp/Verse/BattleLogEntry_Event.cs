using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000128 RID: 296
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00024A9B File Offset: 0x00022C9B
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

		// Token: 0x060007EA RID: 2026 RVA: 0x00024AB6 File Offset: 0x00022CB6
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00024AC0 File Offset: 0x00022CC0
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

		// Token: 0x060007EC RID: 2028 RVA: 0x00024B25 File Offset: 0x00022D25
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00024B3B File Offset: 0x00022D3B
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

		// Token: 0x060007EE RID: 2030 RVA: 0x00024B4B File Offset: 0x00022D4B
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiatorPawn)) || (pov == this.initiatorPawn && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00024B85 File Offset: 0x00022D85
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

		// Token: 0x060007F0 RID: 2032 RVA: 0x00024BC0 File Offset: 0x00022DC0
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

		// Token: 0x060007F1 RID: 2033 RVA: 0x00024C90 File Offset: 0x00022E90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00024CF5 File Offset: 0x00022EF5
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04000792 RID: 1938
		protected RulePackDef eventDef;

		// Token: 0x04000793 RID: 1939
		protected Pawn subjectPawn;

		// Token: 0x04000794 RID: 1940
		protected ThingDef subjectThing;

		// Token: 0x04000795 RID: 1941
		protected Pawn initiatorPawn;

		// Token: 0x04000796 RID: 1942
		protected ThingDef initiatorThing;
	}
}
