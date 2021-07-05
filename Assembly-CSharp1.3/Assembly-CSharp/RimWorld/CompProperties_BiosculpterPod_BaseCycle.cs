using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001109 RID: 4361
	public abstract class CompProperties_BiosculpterPod_BaseCycle : CompProperties
	{
		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x060068D4 RID: 26836 RVA: 0x002364B1 File Offset: 0x002346B1
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

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x060068D5 RID: 26837 RVA: 0x002364D9 File Offset: 0x002346D9
		public string LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x04003AB8 RID: 15032
		[NoTranslate]
		public string key;

		// Token: 0x04003AB9 RID: 15033
		[MustTranslate]
		public string label;

		// Token: 0x04003ABA RID: 15034
		[MustTranslate]
		public string description;

		// Token: 0x04003ABB RID: 15035
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003ABC RID: 15036
		public float nutritionRequired;

		// Token: 0x04003ABD RID: 15037
		public float durationDays;

		// Token: 0x04003ABE RID: 15038
		public Color operatingColor = new Color(0.5f, 0.7f, 0.5f);

		// Token: 0x04003ABF RID: 15039
		public ThoughtDef gainThoughtOnCompletion;

		// Token: 0x04003AC0 RID: 15040
		private Texture2D icon;
	}
}
