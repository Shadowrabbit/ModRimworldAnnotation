using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153A RID: 5434
	public class PlaceWorker_ReportWorkSpeedPenalties : PlaceWorker
	{
		// Token: 0x06008148 RID: 33096 RVA: 0x002DBE80 File Offset: 0x002DA080
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef == null)
			{
				return;
			}
			bool flag = StatPart_WorkTableOutdoors.Applies(thingDef, map, loc);
			bool flag2 = StatPart_WorkTableTemperature.Applies(thingDef, map, loc);
			if (flag || flag2)
			{
				string str = "WillGetWorkSpeedPenalty".Translate(def.label).CapitalizeFirst() + ": ";
				string text = "";
				if (flag)
				{
					text += "Outdoors".Translate();
				}
				if (flag2)
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += "BadTemperature".Translate();
				}
				Messages.Message(str + text.CapitalizeFirst() + ".", new TargetInfo(loc, map, false), MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
