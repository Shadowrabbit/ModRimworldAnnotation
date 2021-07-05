using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000487 RID: 1159
	public sealed class IngredientCount
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x000DD81F File Offset: 0x000DBA1F
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000DD82F File Offset: 0x000DBA2F
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + this);
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x000DD854 File Offset: 0x000DBA54
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000DD878 File Offset: 0x000DBA78
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000DD89F File Offset: 0x000DBA9F
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000DD8A7 File Offset: 0x000DBAA7
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000DD8B0 File Offset: 0x000DBAB0
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000DD8BD File Offset: 0x000DBABD
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x04001607 RID: 5639
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04001608 RID: 5640
		private float count = 1f;
	}
}
