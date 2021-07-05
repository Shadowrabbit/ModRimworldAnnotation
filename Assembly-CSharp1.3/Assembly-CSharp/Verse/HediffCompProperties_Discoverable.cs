using System;

namespace Verse
{
	// Token: 0x0200028B RID: 651
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x0600124B RID: 4683 RVA: 0x00069AE5 File Offset: 0x00067CE5
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x04000DDE RID: 3550
		public bool sendLetterWhenDiscovered;

		// Token: 0x04000DDF RID: 3551
		public string discoverLetterLabel;

		// Token: 0x04000DE0 RID: 3552
		public string discoverLetterText;

		// Token: 0x04000DE1 RID: 3553
		public MessageTypeDef messageType;

		// Token: 0x04000DE2 RID: 3554
		public LetterDef letterType;
	}
}
