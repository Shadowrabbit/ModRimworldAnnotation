using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02001837 RID: 6199
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x060091D6 RID: 37334 RVA: 0x0034764D File Offset: 0x0034584D
		public ZlibException()
		{
		}

		// Token: 0x060091D7 RID: 37335 RVA: 0x00347655 File Offset: 0x00345855
		public ZlibException(string s) : base(s)
		{
		}
	}
}
