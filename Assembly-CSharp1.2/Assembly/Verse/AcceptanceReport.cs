using System;

namespace Verse
{
	// Token: 0x020007D7 RID: 2007
	public struct AcceptanceReport
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x00027750 File Offset: 0x00025950
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06003250 RID: 12880 RVA: 0x00027758 File Offset: 0x00025958
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x0014D3C8 File Offset: 0x0014B5C8
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

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06003252 RID: 12882 RVA: 0x0014D3EC File Offset: 0x0014B5EC
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

		// Token: 0x06003253 RID: 12883 RVA: 0x00027760 File Offset: 0x00025960
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x00027770 File Offset: 0x00025970
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x00027780 File Offset: 0x00025980
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x00027788 File Offset: 0x00025988
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x040022E9 RID: 8937
		private string reasonTextInt;

		// Token: 0x040022EA RID: 8938
		private bool acceptedInt;
	}
}
