using System;

namespace RimWorld
{
	// Token: 0x020015B1 RID: 5553
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x060078A2 RID: 30882 RVA: 0x00051493 File Offset: 0x0004F693
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x060078A3 RID: 30883 RVA: 0x0005149B File Offset: 0x0004F69B
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x060078A4 RID: 30884 RVA: 0x000514A3 File Offset: 0x0004F6A3
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x060078A5 RID: 30885 RVA: 0x0024AA50 File Offset: 0x00248C50
		public static FloatMenuAcceptanceReport WasAccepted
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x060078A6 RID: 30886 RVA: 0x0024AA70 File Offset: 0x00248C70
		public static FloatMenuAcceptanceReport WasRejected
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x060078A7 RID: 30887 RVA: 0x000514AB File Offset: 0x0004F6AB
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			if (value)
			{
				return FloatMenuAcceptanceReport.WasAccepted;
			}
			return FloatMenuAcceptanceReport.WasRejected;
		}

		// Token: 0x060078A8 RID: 30888 RVA: 0x000514BB File Offset: 0x0004F6BB
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x060078A9 RID: 30889 RVA: 0x0024AA90 File Offset: 0x00248C90
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x060078AA RID: 30890 RVA: 0x0024AAB8 File Offset: 0x00248CB8
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x060078AB RID: 30891 RVA: 0x0024AAE0 File Offset: 0x00248CE0
		public static FloatMenuAcceptanceReport WithFailReasonAndMessage(string failReason, string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason,
				failMessageInt = failMessage
			};
		}

		// Token: 0x04004F7F RID: 20351
		private string failMessageInt;

		// Token: 0x04004F80 RID: 20352
		private string failReasonInt;

		// Token: 0x04004F81 RID: 20353
		private bool acceptedInt;
	}
}
