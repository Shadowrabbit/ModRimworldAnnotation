using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF6 RID: 2806
	public class ExternalHistory : IExposable
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x00161AB8 File Offset: 0x0015FCB8
		public string AllInformation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("storyteller: ");
				stringBuilder.Append(this.storytellerName);
				stringBuilder.Append("   userName: ");
				stringBuilder.Append(this.userName);
				stringBuilder.Append("   realWorldDate(UTC): ");
				stringBuilder.Append(this.realWorldDate);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00161B1C File Offset: 0x0015FD1C
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.gameVersion, "gameVersion", null, false);
			Scribe_Values.Look<string>(ref this.gameplayID, "gameplayID", null, false);
			Scribe_Values.Look<string>(ref this.userName, "userName", null, false);
			Scribe_Values.Look<string>(ref this.storytellerName, "storytellerName", null, false);
			Scribe_Values.Look<string>(ref this.realWorldDate, "realWorldDate", null, false);
			Scribe_Values.Look<string>(ref this.firstUploadDate, "firstUploadDate", null, false);
			Scribe_Values.Look<int>(ref this.firstUploadTime, "firstUploadTime", 0, false);
			Scribe_Values.Look<bool>(ref this.devMode, "devMode", false, false);
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
		}

		// Token: 0x04002839 RID: 10297
		public string gameVersion = "?";

		// Token: 0x0400283A RID: 10298
		public string gameplayID = "?";

		// Token: 0x0400283B RID: 10299
		public string userName = "?";

		// Token: 0x0400283C RID: 10300
		public string storytellerName = "?";

		// Token: 0x0400283D RID: 10301
		public string realWorldDate = "?";

		// Token: 0x0400283E RID: 10302
		public string firstUploadDate = "?";

		// Token: 0x0400283F RID: 10303
		public int firstUploadTime;

		// Token: 0x04002840 RID: 10304
		public bool devMode;

		// Token: 0x04002841 RID: 10305
		public History history = new History();

		// Token: 0x04002842 RID: 10306
		public static string defaultUserName = "Anonymous";
	}
}
