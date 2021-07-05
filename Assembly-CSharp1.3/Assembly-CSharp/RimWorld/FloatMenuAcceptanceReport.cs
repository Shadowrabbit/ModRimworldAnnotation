using System;

namespace RimWorld
{
	// Token: 0x02000FE2 RID: 4066
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06005FDC RID: 24540 RVA: 0x0020BBFA File Offset: 0x00209DFA
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06005FDD RID: 24541 RVA: 0x0020BC02 File Offset: 0x00209E02
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06005FDE RID: 24542 RVA: 0x0020BC0A File Offset: 0x00209E0A
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06005FDF RID: 24543 RVA: 0x0020BC14 File Offset: 0x00209E14
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

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06005FE0 RID: 24544 RVA: 0x0020BC34 File Offset: 0x00209E34
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

		// Token: 0x06005FE1 RID: 24545 RVA: 0x0020BC52 File Offset: 0x00209E52
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			if (value)
			{
				return FloatMenuAcceptanceReport.WasAccepted;
			}
			return FloatMenuAcceptanceReport.WasRejected;
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0020BC62 File Offset: 0x00209E62
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0020BC6C File Offset: 0x00209E6C
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0020BC94 File Offset: 0x00209E94
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0020BCBC File Offset: 0x00209EBC
		public static FloatMenuAcceptanceReport WithFailReasonAndMessage(string failReason, string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason,
				failMessageInt = failMessage
			};
		}

		// Token: 0x04003719 RID: 14105
		private string failMessageInt;

		// Token: 0x0400371A RID: 14106
		private string failReasonInt;

		// Token: 0x0400371B RID: 14107
		private bool acceptedInt;
	}
}
