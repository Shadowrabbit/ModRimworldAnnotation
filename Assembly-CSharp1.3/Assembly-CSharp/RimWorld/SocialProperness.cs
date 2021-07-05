using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F5 RID: 2293
	public static class SocialProperness
	{
		// Token: 0x06003C15 RID: 15381 RVA: 0x0014EC50 File Offset: 0x0014CE50
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0014EC60 File Offset: 0x0014CE60
		public static bool IsSociallyProper(this Thing t, Pawn p, bool forPrisoner, bool animalsCare = false)
		{
			if (!animalsCare && p != null && !p.RaceProps.Humanlike)
			{
				return true;
			}
			if (!t.def.socialPropernessMatters)
			{
				return true;
			}
			if (!t.Spawned)
			{
				return true;
			}
			IntVec3 intVec = t.def.hasInteractionCell ? t.InteractionCell : t.Position;
			if (forPrisoner)
			{
				return p == null || intVec.GetRoom(t.Map) == p.GetRoom(RegionType.Set_All);
			}
			return !intVec.IsInPrisonCell(t.Map);
		}
	}
}
