using System;

namespace Verse
{
	// Token: 0x0200017F RID: 383
	public class RoomStatScoreStage
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0000D874 File Offset: 0x0000BA74
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04000841 RID: 2113
		public float minScore = float.MinValue;

		// Token: 0x04000842 RID: 2114
		public string label;

		// Token: 0x04000843 RID: 2115
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
