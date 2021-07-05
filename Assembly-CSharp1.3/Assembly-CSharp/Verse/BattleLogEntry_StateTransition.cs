using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200012E RID: 302
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00026215 File Offset: 0x00024415
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

		// Token: 0x06000832 RID: 2098 RVA: 0x00024AB6 File Offset: 0x00022CB6
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00026230 File Offset: 0x00024430
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

		// Token: 0x06000834 RID: 2100 RVA: 0x000262A6 File Offset: 0x000244A6
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x000262BC File Offset: 0x000244BC
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

		// Token: 0x06000836 RID: 2102 RVA: 0x000262CC File Offset: 0x000244CC
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00026306 File Offset: 0x00024506
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

		// Token: 0x06000838 RID: 2104 RVA: 0x00026344 File Offset: 0x00024544
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

		// Token: 0x06000839 RID: 2105 RVA: 0x0002639C File Offset: 0x0002459C
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

		// Token: 0x0600083A RID: 2106 RVA: 0x000264B8 File Offset: 0x000246B8
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

		// Token: 0x0600083B RID: 2107 RVA: 0x0002653F File Offset: 0x0002473F
		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x040007BC RID: 1980
		private RulePackDef transitionDef;

		// Token: 0x040007BD RID: 1981
		private Pawn subjectPawn;

		// Token: 0x040007BE RID: 1982
		private ThingDef subjectThing;

		// Token: 0x040007BF RID: 1983
		private Pawn initiator;

		// Token: 0x040007C0 RID: 1984
		private HediffDef culpritHediffDef;

		// Token: 0x040007C1 RID: 1985
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x040007C2 RID: 1986
		private BodyPartRecord culpritTargetPart;
	}
}
