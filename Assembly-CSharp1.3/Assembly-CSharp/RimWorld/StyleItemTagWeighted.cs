using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ACC RID: 2764
	public class StyleItemTagWeighted
	{
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004146 RID: 16710 RVA: 0x0015F259 File Offset: 0x0015D459
		public string Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x0015F261 File Offset: 0x0015D461
		public float TotalWeight
		{
			get
			{
				return this.baseWeight * this.weightFactor;
			}
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x000033AC File Offset: 0x000015AC
		public StyleItemTagWeighted()
		{
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x0015F270 File Offset: 0x0015D470
		public StyleItemTagWeighted(string tag, float baseWeight, float weightFactor = 1f)
		{
			this.tag = tag;
			this.baseWeight = baseWeight;
			this.weightFactor = weightFactor;
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x0015F28D File Offset: 0x0015D48D
		public void Add(StyleItemTagWeighted other)
		{
			this.baseWeight += other.baseWeight;
			this.weightFactor *= other.weightFactor;
		}

		// Token: 0x0400271D RID: 10013
		[NoTranslate]
		private string tag;

		// Token: 0x0400271E RID: 10014
		private float baseWeight;

		// Token: 0x0400271F RID: 10015
		private float weightFactor;
	}
}
