using System;

namespace Verse
{
	// Token: 0x020003E0 RID: 992
	public static class ModToolUtility
	{
		// Token: 0x06001DFA RID: 7674 RVA: 0x000BB6FF File Offset: 0x000B98FF
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
