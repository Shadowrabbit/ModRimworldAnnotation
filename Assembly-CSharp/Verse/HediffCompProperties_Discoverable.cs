using System;

namespace Verse
{
	// Token: 0x020003CD RID: 973
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x06001818 RID: 6168 RVA: 0x00016EDB File Offset: 0x000150DB
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x04001248 RID: 4680
		public bool sendLetterWhenDiscovered;

		// Token: 0x04001249 RID: 4681
		public string discoverLetterLabel;

		// Token: 0x0400124A RID: 4682
		public string discoverLetterText;

		// Token: 0x0400124B RID: 4683
		public MessageTypeDef messageType;

		// Token: 0x0400124C RID: 4684
		public LetterDef letterType;
	}
}
