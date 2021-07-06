using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007EA RID: 2026
	public sealed class IngredientCount
	{
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x00028252 File Offset: 0x00026452
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x00028262 File Offset: 0x00026462
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + this, false);
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x00028288 File Offset: 0x00026488
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x0014F4B0 File Offset: 0x0014D6B0
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000282AA File Offset: 0x000264AA
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000282B2 File Offset: 0x000264B2
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000282BB File Offset: 0x000264BB
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000282C8 File Offset: 0x000264C8
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x0400233B RID: 9019
		public ThingFilter filter = new ThingFilter();

		// Token: 0x0400233C RID: 9020
		private float count = 1f;
	}
}
