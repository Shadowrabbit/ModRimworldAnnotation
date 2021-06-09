using System;

namespace Verse
{
	// Token: 0x0200015F RID: 351
	public class PawnKindLifeStage
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x0000D036 File Offset: 0x0000B236
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00097AAC File Offset: 0x00095CAC
		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleDessicatedBodyGraphicData != null && this.femaleDessicatedBodyGraphicData.graphicClass == null)
			{
				this.femaleDessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}

		// Token: 0x0400077B RID: 1915
		[MustTranslate]
		public string label;

		// Token: 0x0400077C RID: 1916
		[MustTranslate]
		public string labelPlural;

		// Token: 0x0400077D RID: 1917
		[MustTranslate]
		public string labelMale;

		// Token: 0x0400077E RID: 1918
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x0400077F RID: 1919
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000780 RID: 1920
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000781 RID: 1921
		[Unsaved(false)]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel;

		// Token: 0x04000782 RID: 1922
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale;

		// Token: 0x04000783 RID: 1923
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelFemale;

		// Token: 0x04000784 RID: 1924
		public GraphicData bodyGraphicData;

		// Token: 0x04000785 RID: 1925
		public GraphicData femaleGraphicData;

		// Token: 0x04000786 RID: 1926
		public GraphicData dessicatedBodyGraphicData;

		// Token: 0x04000787 RID: 1927
		public GraphicData femaleDessicatedBodyGraphicData;

		// Token: 0x04000788 RID: 1928
		public BodyPartToDrop butcherBodyPart;
	}
}
