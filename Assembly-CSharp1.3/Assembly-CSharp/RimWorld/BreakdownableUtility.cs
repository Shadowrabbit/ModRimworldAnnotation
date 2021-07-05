using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001115 RID: 4373
	public static class BreakdownableUtility
	{
		// Token: 0x0600690A RID: 26890 RVA: 0x00237044 File Offset: 0x00235244
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
