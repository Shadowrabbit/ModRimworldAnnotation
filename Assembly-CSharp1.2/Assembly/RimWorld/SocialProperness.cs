using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3E RID: 3646
	public static class SocialProperness
	{
		// Token: 0x060052A9 RID: 21161 RVA: 0x00039C96 File Offset: 0x00037E96
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x001BF59C File Offset: 0x001BD79C
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
				return p == null || intVec.GetRoom(t.Map, RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable);
			}
			return !intVec.IsInPrisonCell(t.Map);
		}
	}
}
