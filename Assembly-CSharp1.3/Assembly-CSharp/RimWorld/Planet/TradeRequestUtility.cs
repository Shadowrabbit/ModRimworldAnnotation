using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017FD RID: 6141
	public static class TradeRequestUtility
	{
		// Token: 0x06008F5E RID: 36702 RVA: 0x003358D8 File Offset: 0x00333AD8
		public static string RequestedThingLabel(ThingDef def, int count)
		{
			string text = GenLabel.ThingLabel(def, null, count);
			if (def.HasComp(typeof(CompQuality)))
			{
				text += " (" + "NormalQualityOrBetter".Translate() + ")";
			}
			if (def.IsApparel)
			{
				text += " (" + "NotTainted".Translate() + ")";
			}
			return text;
		}
	}
}
