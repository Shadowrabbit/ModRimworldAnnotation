using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB2 RID: 4018
	public class LifeStageDef : Def
	{
		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x060057E5 RID: 22501 RVA: 0x0003CF74 File Offset: 0x0003B174
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x0003CF86 File Offset: 0x0003B186
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.icon.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				});
			}
		}

		// Token: 0x040039D0 RID: 14800
		[MustTranslate]
		private string adjective;

		// Token: 0x040039D1 RID: 14801
		public bool visible = true;

		// Token: 0x040039D2 RID: 14802
		public bool reproductive;

		// Token: 0x040039D3 RID: 14803
		public bool milkable;

		// Token: 0x040039D4 RID: 14804
		public bool shearable;

		// Token: 0x040039D5 RID: 14805
		public float voxPitch = 1f;

		// Token: 0x040039D6 RID: 14806
		public float voxVolume = 1f;

		// Token: 0x040039D7 RID: 14807
		[NoTranslate]
		public string icon;

		// Token: 0x040039D8 RID: 14808
		[Unsaved(false)]
		public Texture2D iconTex;

		// Token: 0x040039D9 RID: 14809
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x040039DA RID: 14810
		public float bodySizeFactor = 1f;

		// Token: 0x040039DB RID: 14811
		public float healthScaleFactor = 1f;

		// Token: 0x040039DC RID: 14812
		public float hungerRateFactor = 1f;

		// Token: 0x040039DD RID: 14813
		public float marketValueFactor = 1f;

		// Token: 0x040039DE RID: 14814
		public float foodMaxFactor = 1f;

		// Token: 0x040039DF RID: 14815
		public float meleeDamageFactor = 1f;
	}
}
