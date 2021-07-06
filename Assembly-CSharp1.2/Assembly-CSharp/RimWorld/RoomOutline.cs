using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BA RID: 4794
	public class RoomOutline
	{
		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06006805 RID: 26629 RVA: 0x00046D97 File Offset: 0x00044F97
		public int CellsCountIgnoringWalls
		{
			get
			{
				if (this.rect.Width <= 2 || this.rect.Height <= 2)
				{
					return 0;
				}
				return (this.rect.Width - 2) * (this.rect.Height - 2);
			}
		}

		// Token: 0x06006806 RID: 26630 RVA: 0x00046DD2 File Offset: 0x00044FD2
		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		// Token: 0x0400454B RID: 17739
		public CellRect rect;
	}
}
