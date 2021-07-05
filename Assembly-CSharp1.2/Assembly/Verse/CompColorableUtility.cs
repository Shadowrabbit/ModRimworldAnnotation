using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000525 RID: 1317
	public static class CompColorableUtility
	{
		// Token: 0x060021C6 RID: 8646 RVA: 0x001079EC File Offset: 0x00105BEC
		public static void SetColor(this Thing t, Color newColor, bool reportFailure = true)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on non-ThingWithComps " + t, false);
				}
				return;
			}
			CompColorable comp = thingWithComps.GetComp<CompColorable>();
			if (comp == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on Thing without CompColorable " + t, false);
				}
				return;
			}
			if (comp.Color != newColor)
			{
				comp.Color = newColor;
			}
		}
	}
}
