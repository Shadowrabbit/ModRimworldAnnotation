using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000382 RID: 898
	public static class CompColorableUtility
	{
		// Token: 0x06001A59 RID: 6745 RVA: 0x0009963C File Offset: 0x0009783C
		public static void SetColor(this Thing t, Color newColor, bool reportFailure = true)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on non-ThingWithComps " + t);
				}
				return;
			}
			CompColorable comp = thingWithComps.GetComp<CompColorable>();
			if (comp == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on Thing without CompColorable " + t);
				}
				return;
			}
			if (comp.Color != newColor)
			{
				comp.SetColor(newColor);
			}
		}
	}
}
