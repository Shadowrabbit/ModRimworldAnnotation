using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125D RID: 4701
	public class Area_SnowClear : Area
	{
		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x0600668B RID: 26251 RVA: 0x000460E3 File Offset: 0x000442E3
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x0600668C RID: 26252 RVA: 0x000460F4 File Offset: 0x000442F4
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x0600668D RID: 26253 RVA: 0x00043D77 File Offset: 0x00041F77
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x00045FFC File Offset: 0x000441FC
		public Area_SnowClear()
		{
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x00046004 File Offset: 0x00044204
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x0004610A File Offset: 0x0004430A
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
