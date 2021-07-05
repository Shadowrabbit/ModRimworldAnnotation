using System;

namespace Verse
{
	// Token: 0x020003DD RID: 989
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x06001DE5 RID: 7653 RVA: 0x000BAE68 File Offset: 0x000B9068
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x040011EF RID: 4591
		public string elementToIgnore;
	}
}
