using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147E RID: 5246
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x0600713E RID: 28990 RVA: 0x0004C3AB File Offset: 0x0004A5AB
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
