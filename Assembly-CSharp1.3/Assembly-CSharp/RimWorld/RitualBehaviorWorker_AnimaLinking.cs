using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2A RID: 3882
	public class RitualBehaviorWorker_AnimaLinking : RitualBehaviorWorker
	{
		// Token: 0x06005C59 RID: 23641 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_AnimaLinking()
		{
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_AnimaLinking(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x001FDBB8 File Offset: 0x001FBDB8
		public override string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			int count = assignments.SpectatorsForReading.Count;
			float value = RitualOutcomeEffectWorker_AnimaTreeLinking.RestoredGrassFromQuality.Evaluate(quality);
			TaggedString taggedString = "AnimaLinkingExplanationBase".Translate(count, value);
			TaggedString psylinkAffectedByTraitsNegativelyWarning = RoyalTitleUtility.GetPsylinkAffectedByTraitsNegativelyWarning(assignments.ExtraRequiredPawnsForReading.FirstOrDefault<Pawn>());
			if (psylinkAffectedByTraitsNegativelyWarning.RawText != null)
			{
				taggedString += "\n\n" + psylinkAffectedByTraitsNegativelyWarning.Resolve();
			}
			return taggedString;
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x001FDC2C File Offset: 0x001FBE2C
		public override int ExpectedDurationOverride(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			int count = assignments.SpectatorsForReading.Count;
			return Mathf.RoundToInt((float)ritual.behavior.def.durationTicks.max / RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)(count + 1)) / 2500f);
		}
	}
}
