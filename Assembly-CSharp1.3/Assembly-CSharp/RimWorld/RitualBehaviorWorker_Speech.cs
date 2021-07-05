using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F25 RID: 3877
	public class RitualBehaviorWorker_Speech : RitualBehaviorWorker
	{
		// Token: 0x06005C41 RID: 23617 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_Speech()
		{
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_Speech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C43 RID: 23619 RVA: 0x001FD608 File Offset: 0x001FB808
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn organizer2 = assignments.AssignedPawns("speaker").First<Pawn>();
			return new LordJob_Joinable_Speech(target, organizer2, ritual, this.def.stages, assignments, false);
		}

		// Token: 0x06005C44 RID: 23620 RVA: 0x001FD640 File Offset: 0x001FB840
		protected override void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn arg = assignments.AssignedPawns("speaker").First<Pawn>();
			Find.LetterStack.ReceiveLetter(this.def.letterTitle.Formatted(ritual.Named("RITUAL")), this.def.letterText.Formatted(arg.Named("SPEAKER"), ritual.Named("RITUAL"), ritual.ideo.MemberNamePlural.Named("IDEOMEMBERS")) + "\n\n" + ritual.outcomeEffect.ExtraAlertParagraph(ritual), LetterDefOf.PositiveEvent, target, null, null, null, null);
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x001FD6EC File Offset: 0x001FB8EC
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Precept_Role precept_Role = ritual.ideo.RolesListForReading.First((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Leader);
			if (precept_Role.ChosenPawnSingle() == null)
			{
				return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}
	}
}
