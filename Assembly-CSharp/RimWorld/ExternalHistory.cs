using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001020 RID: 4128
	public class ExternalHistory : IExposable
	{
		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06005A1B RID: 23067 RVA: 0x001D447C File Offset: 0x001D267C
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

		// Token: 0x06005A1C RID: 23068 RVA: 0x001D44E0 File Offset: 0x001D26E0
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

		// Token: 0x04003C9E RID: 15518
		public string gameVersion = "?";

		// Token: 0x04003C9F RID: 15519
		public string gameplayID = "?";

		// Token: 0x04003CA0 RID: 15520
		public string userName = "?";

		// Token: 0x04003CA1 RID: 15521
		public string storytellerName = "?";

		// Token: 0x04003CA2 RID: 15522
		public string realWorldDate = "?";

		// Token: 0x04003CA3 RID: 15523
		public string firstUploadDate = "?";

		// Token: 0x04003CA4 RID: 15524
		public int firstUploadTime;

		// Token: 0x04003CA5 RID: 15525
		public bool devMode;

		// Token: 0x04003CA6 RID: 15526
		public History history = new History();

		// Token: 0x04003CA7 RID: 15527
		public static string defaultUserName = "Anonymous";
	}
}
