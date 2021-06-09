using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125B RID: 4699
	public class Area_BuildRoof : Area
	{
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x0600667F RID: 26239 RVA: 0x0004604F File Offset: 0x0004424F
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06006680 RID: 26240 RVA: 0x00046060 File Offset: 0x00044260
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06006681 RID: 26241 RVA: 0x00046076 File Offset: 0x00044276
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x06006682 RID: 26242 RVA: 0x00045FFC File Offset: 0x000441FC
		public Area_BuildRoof()
		{
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x00046004 File Offset: 0x00044204
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x0004607D File Offset: 0x0004427D
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
