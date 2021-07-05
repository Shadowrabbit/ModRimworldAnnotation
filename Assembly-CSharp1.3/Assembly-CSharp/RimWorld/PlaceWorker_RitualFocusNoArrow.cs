using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001563 RID: 5475
	[StaticConstructorOnStartup]
	public class PlaceWorker_RitualFocusNoArrow : PlaceWorker_RitualFocus
	{
		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x060081AF RID: 33199 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool UseArrow
		{
			get
			{
				return false;
			}
		}
	}
}
