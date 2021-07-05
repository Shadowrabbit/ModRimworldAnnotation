using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2F RID: 3887
	public class RitualObligation : IExposable, ILoadReferenceable
	{
		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06005C69 RID: 23657 RVA: 0x001FDE63 File Offset: 0x001FC063
		public bool StillValid
		{
			get
			{
				return !this.expires || this.TicksUntilExpiration >= 0;
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06005C6A RID: 23658 RVA: 0x001FDE7B File Offset: 0x001FC07B
		public static int DaysToExpire
		{
			get
			{
				return RitualObligation.StageDays[RitualObligation.StageDays.Length - 1];
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06005C6B RID: 23659 RVA: 0x001FDE8C File Offset: 0x001FC08C
		public int ActiveForTicks
		{
			get
			{
				return Find.TickManager.TicksGame - this.triggeredTick;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06005C6C RID: 23660 RVA: 0x001FDE9F File Offset: 0x001FC09F
		public int TicksUntilExpiration
		{
			get
			{
				return this.triggeredTick + RitualObligation.DaysToExpire * 60000 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06005C6D RID: 23661 RVA: 0x001FDEC0 File Offset: 0x001FC0C0
		public int DelayStage
		{
			get
			{
				int num = this.ActiveForTicks / 60000;
				int result = -1;
				int num2 = RitualObligation.StageDays.Length - 1;
				while (num2 >= 0 && num < RitualObligation.StageDays[num2])
				{
					result = num2 - 1;
					num2--;
				}
				return result;
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06005C6E RID: 23662 RVA: 0x001FDEFF File Offset: 0x001FC0FF
		public Alert_RitualObligation AlertCached
		{
			get
			{
				if (this.alertCached == null)
				{
					this.alertCached = new Alert_RitualObligation(this);
				}
				return this.alertCached;
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06005C6F RID: 23663 RVA: 0x001FDF1C File Offset: 0x001FC11C
		public TargetInfo FirstValidTarget
		{
			get
			{
				if (this.targetA.IsValid)
				{
					return this.targetA;
				}
				if (this.targetB.IsValid)
				{
					return this.targetB;
				}
				if (this.targetC.IsValid)
				{
					return this.targetC;
				}
				return TargetInfo.Invalid;
			}
		}

		// Token: 0x06005C70 RID: 23664 RVA: 0x001FDF6A File Offset: 0x001FC16A
		public RitualObligation()
		{
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x001FDF80 File Offset: 0x001FC180
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, TargetInfo targetB, TargetInfo targetC)
		{
			this.precept = precept;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.triggeredTick = Find.TickManager.TicksGame;
			this.ID = Find.UniqueIDsManager.GetNextRitualObligationID();
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x001FDFDE File Offset: 0x001FC1DE
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, TargetInfo targetB, bool expires = true) : this(precept, targetA, targetB, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x001FDFF6 File Offset: 0x001FC1F6
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, bool expires = true) : this(precept, targetA, TargetInfo.Invalid, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x001FE011 File Offset: 0x001FC211
		public RitualObligation(Precept_Ritual precept, bool expires = true) : this(precept, TargetInfo.Invalid, TargetInfo.Invalid, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x001FE030 File Offset: 0x001FC230
		public void DebugOffsetTriggeredTick(int ticks)
		{
			this.triggeredTick += ticks;
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x001FE040 File Offset: 0x001FC240
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.triggeredTick, "triggeredTick", 0, false);
			Scribe_Values.Look<bool>(ref this.showAlert, "showAlert", false, false);
			Scribe_Values.Look<bool>(ref this.expires, "expires", false, false);
			Scribe_TargetInfo.Look(ref this.targetA, true, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, true, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, true, "targetC");
			Scribe_Collections.Look<Pawn>(ref this.onlyForPawns, "onlyForPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x001FE0DE File Offset: 0x001FC2DE
		public string GetUniqueLoadID()
		{
			return "RitualObligation_" + this.ID;
		}

		// Token: 0x040035C4 RID: 13764
		public Precept_Ritual precept;

		// Token: 0x040035C5 RID: 13765
		private int triggeredTick;

		// Token: 0x040035C6 RID: 13766
		public bool showAlert = true;

		// Token: 0x040035C7 RID: 13767
		public bool expires = true;

		// Token: 0x040035C8 RID: 13768
		public TargetInfo targetA;

		// Token: 0x040035C9 RID: 13769
		public TargetInfo targetB;

		// Token: 0x040035CA RID: 13770
		public TargetInfo targetC;

		// Token: 0x040035CB RID: 13771
		public List<Pawn> onlyForPawns;

		// Token: 0x040035CC RID: 13772
		public int ID;

		// Token: 0x040035CD RID: 13773
		private Alert_RitualObligation alertCached;

		// Token: 0x040035CE RID: 13774
		public static readonly int[] StageDays = new int[]
		{
			3,
			9
		};
	}
}
