using System;

namespace Verse
{
	// Token: 0x020000FF RID: 255
	public class RoomStatScoreStage
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x000211C4 File Offset: 0x0001F3C4
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0400061A RID: 1562
		public float minScore = float.MinValue;

		// Token: 0x0400061B RID: 1563
		public string label;

		// Token: 0x0400061C RID: 1564
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
