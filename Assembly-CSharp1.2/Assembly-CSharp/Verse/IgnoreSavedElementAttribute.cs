using System;

namespace Verse
{
	// Token: 0x020006ED RID: 1773
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x06002D1B RID: 11547 RVA: 0x00023A2C File Offset: 0x00021C2C
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x04001E86 RID: 7814
		public string elementToIgnore;
	}
}
