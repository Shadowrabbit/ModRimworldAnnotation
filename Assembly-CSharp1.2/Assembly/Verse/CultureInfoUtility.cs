using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x0200006F RID: 111
	public static class CultureInfoUtility
	{
		// Token: 0x0600045F RID: 1119 RVA: 0x00009E30 File Offset: 0x00008030
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x040001EE RID: 494
		private const string EnglishCulture = "en-US";
	}
}
