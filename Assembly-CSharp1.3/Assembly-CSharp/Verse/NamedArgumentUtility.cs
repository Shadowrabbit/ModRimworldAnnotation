using System;

namespace Verse
{
	// Token: 0x0200014C RID: 332
	public static class NamedArgumentUtility
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x0002E8F1 File Offset: 0x0002CAF1
		public static NamedArgument Named(this object arg, string label)
		{
			return new NamedArgument(arg, label);
		}
	}
}
