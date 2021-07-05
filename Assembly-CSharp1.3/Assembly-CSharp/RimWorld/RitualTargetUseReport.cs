using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F31 RID: 3889
	public struct RitualTargetUseReport
	{
		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06005C7B RID: 23675 RVA: 0x001FE136 File Offset: 0x001FC336
		public bool ShouldShowGizmo
		{
			get
			{
				return this.canUse || !this.failReason.NullOrEmpty();
			}
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x001FE150 File Offset: 0x001FC350
		public static implicit operator RitualTargetUseReport(bool canUse)
		{
			return new RitualTargetUseReport
			{
				canUse = canUse,
				failReason = null
			};
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x001FE178 File Offset: 0x001FC378
		public static implicit operator RitualTargetUseReport(string failReason)
		{
			return new RitualTargetUseReport
			{
				canUse = false,
				failReason = failReason
			};
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x001FE1A0 File Offset: 0x001FC3A0
		public static implicit operator RitualTargetUseReport(TaggedString failReason)
		{
			return new RitualTargetUseReport
			{
				canUse = false,
				failReason = failReason
			};
		}

		// Token: 0x040035D7 RID: 13783
		public bool canUse;

		// Token: 0x040035D8 RID: 13784
		public string failReason;
	}
}
