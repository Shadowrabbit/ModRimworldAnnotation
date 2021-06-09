using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D30 RID: 7472
	public static class PawnOrCorpseStatUtility
	{
		// Token: 0x0600A26D RID: 41581 RVA: 0x002F5488 File Offset: 0x002F3688
		public static bool TryGetPawnOrCorpseStat(StatRequest req, Func<Pawn, float> pawnStatGetter, Func<ThingDef, float> pawnDefStatGetter, out float stat)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					stat = pawnStatGetter(pawn);
					return true;
				}
				Corpse corpse = req.Thing as Corpse;
				if (corpse != null)
				{
					stat = pawnStatGetter(corpse.InnerPawn);
					return true;
				}
			}
			else
			{
				ThingDef thingDef = req.Def as ThingDef;
				if (thingDef != null)
				{
					if (thingDef.category == ThingCategory.Pawn)
					{
						stat = pawnDefStatGetter(thingDef);
						return true;
					}
					if (thingDef.IsCorpse)
					{
						stat = pawnDefStatGetter(thingDef.ingestible.sourceDef);
						return true;
					}
				}
			}
			stat = 0f;
			return false;
		}
	}
}
