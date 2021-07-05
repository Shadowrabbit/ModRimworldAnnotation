using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014ED RID: 5357
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x06007F90 RID: 32656 RVA: 0x002D10CB File Offset: 0x002CF2CB
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x06007F91 RID: 32657 RVA: 0x002D10F0 File Offset: 0x002CF2F0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				return "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x002D1146 File Offset: 0x002CF346
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x06007F93 RID: 32659 RVA: 0x002D116C File Offset: 0x002CF36C
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

		// Token: 0x04004F9D RID: 20381
		public const float WorkRateFactor = 0.7f;

		// Token: 0x04004F9E RID: 20382
		public const float MinTemp = 9f;

		// Token: 0x04004F9F RID: 20383
		public const float MaxTemp = 35f;
	}
}
