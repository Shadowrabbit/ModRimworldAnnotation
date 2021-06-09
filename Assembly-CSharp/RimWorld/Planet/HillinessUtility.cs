using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002071 RID: 8305
	public static class HillinessUtility
	{
		// Token: 0x0600B020 RID: 45088 RVA: 0x0033294C File Offset: 0x00330B4C
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
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362, false);
				return h.ToString();
			}
		}

		// Token: 0x0600B021 RID: 45089 RVA: 0x0007272A File Offset: 0x0007092A
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
