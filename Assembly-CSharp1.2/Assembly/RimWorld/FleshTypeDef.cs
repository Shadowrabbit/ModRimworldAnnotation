using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F92 RID: 3986
	public class FleshTypeDef : Def
	{
		// Token: 0x06005774 RID: 22388 RVA: 0x001CD76C File Offset: 0x001CB96C
		public Material ChooseWoundOverlay()
		{
			if (this.wounds == null)
			{
				return null;
			}
			if (this.woundsResolved == null)
			{
				this.woundsResolved = (from wound in this.wounds
				select wound.GetMaterial()).ToList<Material>();
			}
			return this.woundsResolved.RandomElement<Material>();
		}

		// Token: 0x0400392B RID: 14635
		public ThoughtDef ateDirect;

		// Token: 0x0400392C RID: 14636
		public ThoughtDef ateAsIngredient;

		// Token: 0x0400392D RID: 14637
		public ThingCategoryDef corpseCategory;

		// Token: 0x0400392E RID: 14638
		public EffecterDef damageEffecter;

		// Token: 0x0400392F RID: 14639
		public List<FleshTypeDef.Wound> wounds;

		// Token: 0x04003930 RID: 14640
		private List<Material> woundsResolved;

		// Token: 0x02000F93 RID: 3987
		public class Wound
		{
			// Token: 0x06005776 RID: 22390 RVA: 0x0003CA59 File Offset: 0x0003AC59
			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}

			// Token: 0x04003931 RID: 14641
			[NoTranslate]
			public string texture;

			// Token: 0x04003932 RID: 14642
			public Color color = Color.white;
		}
	}
}
