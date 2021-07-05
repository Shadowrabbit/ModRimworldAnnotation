using System;

namespace Verse
{
	// Token: 0x020006F0 RID: 1776
	public static class ModToolUtility
	{
		// Token: 0x06002D30 RID: 11568 RVA: 0x00023B64 File Offset: 0x00021D64
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
