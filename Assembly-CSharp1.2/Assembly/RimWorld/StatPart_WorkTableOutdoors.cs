using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D51 RID: 7505
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x0600A300 RID: 41728 RVA: 0x0006C3B5 File Offset: 0x0006A5B5
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.9f;
			}
		}

		// Token: 0x0600A301 RID: 41729 RVA: 0x002F6A00 File Offset: 0x002F4C00
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				return "Outdoors".Translate() + ": x" + 0.9f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A302 RID: 41730 RVA: 0x0006C3D8 File Offset: 0x0006A5D8
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x0600A303 RID: 41731 RVA: 0x002F6A50 File Offset: 0x002F4C50
		public static bool Applies(ThingDef def, Map map, IntVec3 c)
		{
			if (def.building == null)
			{
				return false;
			}
			if (map == null)
			{
				return false;
			}
			Room room = c.GetRoom(map, RegionType.Set_All);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x04006EA8 RID: 28328
		public const float WorkRateFactor = 0.9f;
	}
}
