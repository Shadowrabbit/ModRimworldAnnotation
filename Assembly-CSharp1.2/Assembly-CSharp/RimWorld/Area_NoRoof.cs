using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125C RID: 4700
	public class Area_NoRoof : Area
	{
		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06006685 RID: 26245 RVA: 0x00046099 File Offset: 0x00044299
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06006686 RID: 26246 RVA: 0x000460AA File Offset: 0x000442AA
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06006687 RID: 26247 RVA: 0x000460C0 File Offset: 0x000442C0
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x06006688 RID: 26248 RVA: 0x00045FFC File Offset: 0x000441FC
		public Area_NoRoof()
		{
		}

		// Token: 0x06006689 RID: 26249 RVA: 0x00046004 File Offset: 0x00044204
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x000460C7 File Offset: 0x000442C7
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
