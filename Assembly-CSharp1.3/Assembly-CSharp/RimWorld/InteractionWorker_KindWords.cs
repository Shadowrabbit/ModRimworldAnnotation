using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF2 RID: 3570
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x060052BF RID: 21183 RVA: 0x001BEB45 File Offset: 0x001BCD45
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return 0.01f;
			}
			return 0f;
		}
	}
}
