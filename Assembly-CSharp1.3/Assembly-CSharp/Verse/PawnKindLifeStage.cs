using System;

namespace Verse
{
	// Token: 0x020000EA RID: 234
	public class PawnKindLifeStage
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x0001F5CF File Offset: 0x0001D7CF
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001F5F8 File Offset: 0x0001D7F8
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

		// Token: 0x04000582 RID: 1410
		[MustTranslate]
		public string label;

		// Token: 0x04000583 RID: 1411
		[MustTranslate]
		public string labelPlural;

		// Token: 0x04000584 RID: 1412
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000585 RID: 1413
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000586 RID: 1414
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000587 RID: 1415
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000588 RID: 1416
		[Unsaved(false)]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel;

		// Token: 0x04000589 RID: 1417
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale;

		// Token: 0x0400058A RID: 1418
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelFemale;

		// Token: 0x0400058B RID: 1419
		public GraphicData bodyGraphicData;

		// Token: 0x0400058C RID: 1420
		public GraphicData femaleGraphicData;

		// Token: 0x0400058D RID: 1421
		public GraphicData dessicatedBodyGraphicData;

		// Token: 0x0400058E RID: 1422
		public GraphicData femaleDessicatedBodyGraphicData;

		// Token: 0x0400058F RID: 1423
		public BodyPartToDrop butcherBodyPart;
	}
}
