using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C60 RID: 3168
	public class Area_NoRoof : Area
	{
		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06004A02 RID: 18946 RVA: 0x001871BB File Offset: 0x001853BB
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06004A03 RID: 18947 RVA: 0x001871CC File Offset: 0x001853CC
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x001871E2 File Offset: 0x001853E2
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0018711E File Offset: 0x0018531E
		public Area_NoRoof()
		{
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x00187126 File Offset: 0x00185326
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x001871E9 File Offset: 0x001853E9
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
