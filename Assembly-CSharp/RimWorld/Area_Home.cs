using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125A RID: 4698
	public class Area_Home : Area
	{
		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06006677 RID: 26231 RVA: 0x00045FCE File Offset: 0x000441CE
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06006678 RID: 26232 RVA: 0x00045FDF File Offset: 0x000441DF
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06006679 RID: 26233 RVA: 0x00045FF5 File Offset: 0x000441F5
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x00045FFC File Offset: 0x000441FC
		public Area_Home()
		{
		}

		// Token: 0x0600667B RID: 26235 RVA: 0x00046004 File Offset: 0x00044204
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600667C RID: 26236 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x0600667D RID: 26237 RVA: 0x0004600D File Offset: 0x0004420D
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		// Token: 0x0600667E RID: 26238 RVA: 0x00046029 File Offset: 0x00044229
		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] == val)
			{
				return;
			}
			base.Set(c, val);
			base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
		}
	}
}
