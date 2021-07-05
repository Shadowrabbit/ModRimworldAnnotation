using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5F RID: 3167
	public class Area_BuildRoof : Area
	{
		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x00187171 File Offset: 0x00185371
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060049FD RID: 18941 RVA: 0x00187182 File Offset: 0x00185382
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x00187198 File Offset: 0x00185398
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0018711E File Offset: 0x0018531E
		public Area_BuildRoof()
		{
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x00187126 File Offset: 0x00185326
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0018719F File Offset: 0x0018539F
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
