using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001CC RID: 460
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0000F276 File Offset: 0x0000D476
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

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0000ED2E File Offset: 0x0000CF2E
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000A1F90 File Offset: 0x000A0190
		public BattleLogEntry_StateTransition(Thing subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartRecord culpritTargetDef) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			this.transitionDef = transitionDef;
			this.initiator = initiator;
			if (culpritHediff != null)
			{
				this.culpritHediffDef = culpritHediff.def;
				if (culpritHediff.Part != null)
				{
					this.culpritHediffTargetPart = culpritHediff.Part;
				}
			}
			this.culpritTargetPart = culpritTargetDef;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0000F291 File Offset: 0x0000D491
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0000F2A7 File Offset: 0x0000D4A7
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			yield break;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0000F2B7 File Offset: 0x0000D4B7
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0000F2F1 File Offset: 0x0000D4F1
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
				return;
			}
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000A2008 File Offset: 0x000A0208
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (pov == null || pov == this.subjectPawn)
			{
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.Skull;
				}
				return LogEntry.Downed;
			}
			else
			{
				if (pov != this.initiator)
				{
					return null;
				}
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.SkullTarget;
				}
				return LogEntry.DownedTarget;
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000A2060 File Offset: 0x000A0260
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			result.Includes.Add(this.transitionDef);
			if (this.initiator != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, true, true));
			}
			if (this.culpritHediffDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForHediffDef("CULPRITHEDIFF", this.culpritHediffDef, this.culpritHediffTargetPart));
			}
			if (this.culpritHediffTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_target", this.culpritHediffTargetPart));
			}
			if (this.culpritTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_originaltarget", this.culpritTargetPart));
			}
			return result;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000A217C File Offset: 0x000A037C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.transitionDef, "transitionDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Defs.Look<HediffDef>(ref this.culpritHediffDef, "culpritHediffDef");
			Scribe_BodyParts.Look(ref this.culpritHediffTargetPart, "culpritHediffTargetPart", null);
			Scribe_BodyParts.Look(ref this.culpritTargetPart, "culpritTargetPart", null);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0000F32C File Offset: 0x0000D52C
		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04000A5C RID: 2652
		private RulePackDef transitionDef;

		// Token: 0x04000A5D RID: 2653
		private Pawn subjectPawn;

		// Token: 0x04000A5E RID: 2654
		private ThingDef subjectThing;

		// Token: 0x04000A5F RID: 2655
		private Pawn initiator;

		// Token: 0x04000A60 RID: 2656
		private HediffDef culpritHediffDef;

		// Token: 0x04000A61 RID: 2657
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x04000A62 RID: 2658
		private BodyPartRecord culpritTargetPart;
	}
}
