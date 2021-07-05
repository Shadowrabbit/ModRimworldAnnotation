using System;

namespace Verse.AI
{
	// Token: 0x020005DE RID: 1502
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x06002B78 RID: 11128 RVA: 0x0010385A File Offset: 0x00101A5A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<bool>(ref this.alreadyHauledCorpse, "alreadyHauledCorpse", false, false);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x00103888 File Offset: 0x00101A88
		public override void MentalStateTick()
		{
			if (this.alreadyHauledCorpse)
			{
				base.MentalStateTick();
				return;
			}
			bool flag = false;
			if (this.pawn.IsHashIntervalTick(500) && !CorpseObsessionMentalStateUtility.IsCorpseValid(this.corpse, this.pawn, false))
			{
				this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
				if (this.corpse == null)
				{
					base.RecoverFromState();
					flag = true;
				}
			}
			if (!flag)
			{
				base.MentalStateTick();
			}
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x001038F6 File Offset: 0x00101AF6
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x00103910 File Offset: 0x00101B10
		public void Notify_CorpseHauled()
		{
			this.alreadyHauledCorpse = true;
		}

		// Token: 0x04001A84 RID: 6788
		public Corpse corpse;

		// Token: 0x04001A85 RID: 6789
		public bool alreadyHauledCorpse;

		// Token: 0x04001A86 RID: 6790
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}
