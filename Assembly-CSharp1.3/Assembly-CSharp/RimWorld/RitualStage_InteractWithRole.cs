using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1E RID: 3870
	public class RitualStage_InteractWithRole : RitualStage
	{
		// Token: 0x06005C13 RID: 23571 RVA: 0x001FC9CE File Offset: 0x001FABCE
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			if (this.targetId != null)
			{
				return ritual.PawnWithRole(this.targetId);
			}
			return base.GetSecondFocus(ritual);
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x001FC9F1 File Offset: 0x001FABF1
		public override IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
		{
			if (this.targets.NullOrEmpty<RitualStage_InteractWithRole.PawnTarget>())
			{
				yield break;
			}
			foreach (RitualStage_InteractWithRole.PawnTarget pawnTarget in this.targets)
			{
				Pawn pawn = ritual.assignments.FirstAssignedPawn(pawnTarget.pawnId);
				Pawn t = ritual.assignments.FirstAssignedPawn(pawnTarget.targetId);
				yield return new RitualStagePawnSecondFocus
				{
					pawn = pawn,
					target = t
				};
			}
			List<RitualStage_InteractWithRole.PawnTarget>.Enumerator enumerator = default(List<RitualStage_InteractWithRole.PawnTarget>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x001FCA08 File Offset: 0x001FAC08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetId, "targetId", null, false);
			Scribe_Collections.Look<RitualStage_InteractWithRole.PawnTarget>(ref this.targets, "targets", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x040035A7 RID: 13735
		[NoTranslate]
		public string targetId;

		// Token: 0x040035A8 RID: 13736
		public List<RitualStage_InteractWithRole.PawnTarget> targets;

		// Token: 0x020023B5 RID: 9141
		public class PawnTarget : IExposable
		{
			// Token: 0x0600C855 RID: 51285 RVA: 0x003E3E56 File Offset: 0x003E2056
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
				Scribe_Values.Look<string>(ref this.targetId, "targetId", null, false);
			}

			// Token: 0x040087DF RID: 34783
			[NoTranslate]
			public string pawnId;

			// Token: 0x040087E0 RID: 34784
			[NoTranslate]
			public string targetId;
		}
	}
}
