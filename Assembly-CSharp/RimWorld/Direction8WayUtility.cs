using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAC RID: 7340
	public static class Direction8WayUtility
	{
		// Token: 0x06009FAF RID: 40879 RVA: 0x002EA908 File Offset: 0x002E8B08
		public static string LabelShort(this Direction8Way dir)
		{
			switch (dir)
			{
			case Direction8Way.North:
				return "Direction8Way_North_Short".Translate();
			case Direction8Way.NorthEast:
				return "Direction8Way_NorthEast_Short".Translate();
			case Direction8Way.East:
				return "Direction8Way_East_Short".Translate();
			case Direction8Way.SouthEast:
				return "Direction8Way_SouthEast_Short".Translate();
			case Direction8Way.South:
				return "Direction8Way_South_Short".Translate();
			case Direction8Way.SouthWest:
				return "Direction8Way_SouthWest_Short".Translate();
			case Direction8Way.West:
				return "Direction8Way_West_Short".Translate();
			case Direction8Way.NorthWest:
				return "Direction8Way_NorthWest_Short".Translate();
			default:
				return "Unknown Direction8Way";
			}
		}
	}
}
