using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C61 RID: 3169
	public class Area_SnowClear : Area
	{
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004A08 RID: 18952 RVA: 0x00187205 File Offset: 0x00185405
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06004A09 RID: 18953 RVA: 0x00187216 File Offset: 0x00185416
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06004A0A RID: 18954 RVA: 0x0017AEF3 File Offset: 0x001790F3
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0018711E File Offset: 0x0018531E
		public Area_SnowClear()
		{
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x00187126 File Offset: 0x00185326
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0018722C File Offset: 0x0018542C
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
