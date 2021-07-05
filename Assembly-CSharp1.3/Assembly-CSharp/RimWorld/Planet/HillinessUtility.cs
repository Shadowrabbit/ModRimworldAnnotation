using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001772 RID: 6002
	public static class HillinessUtility
	{
		// Token: 0x06008A70 RID: 35440 RVA: 0x0031AE8C File Offset: 0x0031908C
		public static string GetLabel(this Hilliness h)
		{
			switch (h)
			{
			case Hilliness.Flat:
				return "Hilliness_Flat".Translate();
			case Hilliness.SmallHills:
				return "Hilliness_SmallHills".Translate();
			case Hilliness.LargeHills:
				return "Hilliness_LargeHills".Translate();
			case Hilliness.Mountainous:
				return "Hilliness_Mountainous".Translate();
			case Hilliness.Impassable:
				return "Hilliness_Impassable".Translate();
			default:
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362);
				return h.ToString();
			}
		}

		// Token: 0x06008A71 RID: 35441 RVA: 0x0031AF35 File Offset: 0x00319135
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
