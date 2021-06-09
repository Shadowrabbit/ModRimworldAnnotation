using System;

namespace Verse.AI
{
	// Token: 0x02000A6A RID: 2666
	public static class PawnLocalAwareness
	{
		// Token: 0x06003FAB RID: 16299 RVA: 0x001805E4 File Offset: 0x0017E7E4
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}

		// Token: 0x04002C0A RID: 11274
		private const float SightRadius = 30f;
	}
}
