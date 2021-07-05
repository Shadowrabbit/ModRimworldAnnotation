using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5E RID: 3166
	public class Area_Home : Area
	{
		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x001870F0 File Offset: 0x001852F0
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x00187101 File Offset: 0x00185301
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x00187117 File Offset: 0x00185317
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x0018711E File Offset: 0x0018531E
		public Area_Home()
		{
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x00187126 File Offset: 0x00185326
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0018712F File Offset: 0x0018532F
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0018714B File Offset: 0x0018534B
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
