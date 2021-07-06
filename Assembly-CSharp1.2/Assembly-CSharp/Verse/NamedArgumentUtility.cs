using System;

namespace Verse
{
	// Token: 0x020001F7 RID: 503
	public static class NamedArgumentUtility
	{
		// Token: 0x06000D21 RID: 3361 RVA: 0x0000FFC7 File Offset: 0x0000E1C7
		public static NamedArgument Named(this object arg, string label)
		{
			return new NamedArgument(arg, label);
		}
	}
}
