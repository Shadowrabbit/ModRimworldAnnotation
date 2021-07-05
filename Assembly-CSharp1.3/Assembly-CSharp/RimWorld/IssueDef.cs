using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A85 RID: 2693
	public class IssueDef : Def
	{
		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x0015C2DB File Offset: 0x0015A4DB
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004054 RID: 16468 RVA: 0x0015C303 File Offset: 0x0015A503
		public bool HasDefaultPrecept
		{
			get
			{
				return DefDatabase<PreceptDef>.AllDefs.Any((PreceptDef x) => x.issue == this && x.defaultSelectionWeight > 0f && x.visible);
			}
		}

		// Token: 0x040024D8 RID: 9432
		public bool allowMultiplePrecepts;

		// Token: 0x040024D9 RID: 9433
		public string iconPath;

		// Token: 0x040024DA RID: 9434
		private Texture2D icon;
	}
}
