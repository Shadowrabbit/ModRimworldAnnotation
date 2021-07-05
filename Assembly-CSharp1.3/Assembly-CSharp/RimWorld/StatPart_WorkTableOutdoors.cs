using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EC RID: 5356
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x06007F8B RID: 32651 RVA: 0x002D1010 File Offset: 0x002CF210
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.9f;
			}
		}

		// Token: 0x06007F8C RID: 32652 RVA: 0x002D1034 File Offset: 0x002CF234
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				return "Outdoors".Translate() + ": x" + 0.9f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F8D RID: 32653 RVA: 0x002D1082 File Offset: 0x002CF282
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x06007F8E RID: 32654 RVA: 0x002D109C File Offset: 0x002CF29C
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
			Room room = c.GetRoom(map);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x04004F9C RID: 20380
		public const float WorkRateFactor = 0.9f;
	}
}
