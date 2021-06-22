using System;
using Verse;

namespace RimWorld
{
    // Token: 0x020012C8 RID: 4808
    public static class ManhunterPackGenStepUtility
    {
        // Token: 0x06006833 RID: 26675 RVA: 0x00046F1E File Offset: 0x0004511E
        public static bool TryGetAnimalsKind(float points, int tile, out PawnKindDef animalKind)
        {
            return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, tile, out animalKind) ||
                   ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, -1, out animalKind);
        }
    }
}