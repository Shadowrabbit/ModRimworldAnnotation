using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D52 RID: 7506
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x0600A305 RID: 41733 RVA: 0x0006C3F1 File Offset: 0x0006A5F1
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x0600A306 RID: 41734 RVA: 0x002F6A80 File Offset: 0x002F4C80
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				return "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A307 RID: 41735 RVA: 0x0006C414 File Offset: 0x0006A614
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x0600A308 RID: 41736 RVA: 0x002F6AD8 File Offset: 0x002F4CD8
		public static bool Applies(ThingDef tDef, Map map, IntVec3 c)
		{
			if (map == null)
			{
				return false;
			}
			if (tDef.building == null)
			{
				return false;
			}
			float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
			return temperatureForCell < 9f || temperatureForCell > 35f;
		}

		// Token: 0x04006EA9 RID: 28329
		public const float WorkRateFactor = 0.7f;

		// Token: 0x04006EAA RID: 28330
		public const float MinTemp = 9f;

		// Token: 0x04006EAB RID: 28331
		public const float MaxTemp = 35f;
	}
}
