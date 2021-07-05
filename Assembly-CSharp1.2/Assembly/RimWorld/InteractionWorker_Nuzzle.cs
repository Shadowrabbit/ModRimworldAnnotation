using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001483 RID: 5251
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x0600714A RID: 29002 RVA: 0x0004C435 File Offset: 0x0004A635
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			this.AddNuzzledThought(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x0600714B RID: 29003 RVA: 0x0022A484 File Offset: 0x00228684
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
		}
	}
}
