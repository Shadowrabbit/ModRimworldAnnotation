using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFE RID: 3582
	public class InteractionWorker_SparkSlaveRebellion : InteractionWorker
	{
		// Token: 0x060052E7 RID: 21223 RVA: 0x001C0E48 File Offset: 0x001BF048
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (!SlaveRebellionUtility.CanParticipateInSlaveRebellion(recipient))
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
				lookTargets = null;
				return;
			}
			SlaveRebellionUtility.StartSlaveRebellion(recipient, out letterText, out letterLabel, out letterDef, out lookTargets, false);
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
		}
	}
}
