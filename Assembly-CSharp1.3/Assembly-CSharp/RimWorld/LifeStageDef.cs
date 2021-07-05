using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A88 RID: 2696
	public class LifeStageDef : Def
	{
		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x0015C49F File Offset: 0x0015A69F
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0015C4B1 File Offset: 0x0015A6B1
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

		// Token: 0x040024E9 RID: 9449
		[MustTranslate]
		private string adjective;

		// Token: 0x040024EA RID: 9450
		public bool visible = true;

		// Token: 0x040024EB RID: 9451
		public bool reproductive;

		// Token: 0x040024EC RID: 9452
		public bool milkable;

		// Token: 0x040024ED RID: 9453
		public bool shearable;

		// Token: 0x040024EE RID: 9454
		public bool caravanRideable;

		// Token: 0x040024EF RID: 9455
		public float voxPitch = 1f;

		// Token: 0x040024F0 RID: 9456
		public float voxVolume = 1f;

		// Token: 0x040024F1 RID: 9457
		[NoTranslate]
		public string icon;

		// Token: 0x040024F2 RID: 9458
		[Unsaved(false)]
		public Texture2D iconTex;

		// Token: 0x040024F3 RID: 9459
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x040024F4 RID: 9460
		public float bodySizeFactor = 1f;

		// Token: 0x040024F5 RID: 9461
		public float healthScaleFactor = 1f;

		// Token: 0x040024F6 RID: 9462
		public float hungerRateFactor = 1f;

		// Token: 0x040024F7 RID: 9463
		public float marketValueFactor = 1f;

		// Token: 0x040024F8 RID: 9464
		public float foodMaxFactor = 1f;

		// Token: 0x040024F9 RID: 9465
		public float meleeDamageFactor = 1f;
	}
}
