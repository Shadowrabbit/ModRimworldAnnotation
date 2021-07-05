using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAE RID: 4014
	public class InteractionWorker
	{
		// Token: 0x060057D3 RID: 22483 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x0003CE9F File Offset: 0x0003B09F
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x040039BC RID: 14780
		public InteractionDef interaction;
	}
}
