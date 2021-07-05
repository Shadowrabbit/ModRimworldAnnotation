using System;

namespace Verse
{
	// Token: 0x02000479 RID: 1145
	public struct AcceptanceReport
	{
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x000DAE4D File Offset: 0x000D904D
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x000DAE55 File Offset: 0x000D9055
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x000DAE60 File Offset: 0x000D9060
		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002288 RID: 8840 RVA: 0x000DAE84 File Offset: 0x000D9084
		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000DAEA6 File Offset: 0x000D90A6
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000DAEB6 File Offset: 0x000D90B6
		public static implicit operator bool(AcceptanceReport report)
		{
			return report.Accepted;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000DAEBF File Offset: 0x000D90BF
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000DAECF File Offset: 0x000D90CF
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000DAED7 File Offset: 0x000D90D7
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x040015C2 RID: 5570
		private string reasonTextInt;

		// Token: 0x040015C3 RID: 5571
		private bool acceptedInt;
	}
}
