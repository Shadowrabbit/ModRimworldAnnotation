using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A84 RID: 2692
	public class InteractionWorker
	{
		// Token: 0x06004050 RID: 16464 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0015C2C9 File Offset: 0x0015A4C9
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x040024D7 RID: 9431
		public InteractionDef interaction;
	}
}
