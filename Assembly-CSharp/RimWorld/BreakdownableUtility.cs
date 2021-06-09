using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A7 RID: 6055
	public static class BreakdownableUtility
	{
		// Token: 0x060085D3 RID: 34259 RVA: 0x00276F58 File Offset: 0x00275158
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
