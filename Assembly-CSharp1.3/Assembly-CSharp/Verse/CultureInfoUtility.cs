using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000038 RID: 56
	public static class CultureInfoUtility
	{
		// Token: 0x0600032E RID: 814 RVA: 0x00011515 File Offset: 0x0000F715
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x040000AC RID: 172
		private const string EnglishCulture = "en-US";
	}
}
