using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBA RID: 3258
	public static class ManhunterPackGenStepUtility
	{
		// Token: 0x06004BE5 RID: 19429 RVA: 0x001945D5 File Offset: 0x001927D5
		public static bool TryGetAnimalsKind(float points, int tile, out PawnKindDef animalKind)
		{
			return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, tile, out animalKind) || ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, -1, out animalKind);
		}
	}
}
