using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF9 RID: 3577
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x060052D3 RID: 21203 RVA: 0x001BF9D6 File Offset: 0x001BDBD6
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			this.AddNuzzledThought(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001BF9F0 File Offset: 0x001BDBF0
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
