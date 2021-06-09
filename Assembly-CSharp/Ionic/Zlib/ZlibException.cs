using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200221B RID: 8731
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x0600BB8E RID: 48014 RVA: 0x00079634 File Offset: 0x00077834
		public ZlibException()
		{
		}

		// Token: 0x0600BB8F RID: 48015 RVA: 0x0007963C File Offset: 0x0007783C
		public ZlibException(string s) : base(s)
		{
		}
	}
}
