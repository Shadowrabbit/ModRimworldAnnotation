using System;

namespace Verse.AI
{
	// Token: 0x0200060B RID: 1547
	public static class PawnLocalAwareness
	{
		// Token: 0x06002C8E RID: 11406 RVA: 0x0010A2A8 File Offset: 0x001084A8
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_All) == t.GetRoom(RegionType.Set_All) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}

		// Token: 0x04001B42 RID: 6978
		private const float SightRadius = 30f;
	}
}
