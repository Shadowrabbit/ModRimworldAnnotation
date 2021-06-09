using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021A9 RID: 8617
	public static class TradeRequestUtility
	{
		// Token: 0x0600B81F RID: 47135 RVA: 0x0034FF88 File Offset: 0x0034E188
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
