using System;

namespace RimWorld
{
	// Token: 0x020010B7 RID: 4279
	public class MoteProgressBarAlwaysVisible : MoteProgressBar
	{
		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06006640 RID: 26176 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool OnlyShowForClosestZoom
		{
			get
			{
				return false;
			}
		}
	}
}
