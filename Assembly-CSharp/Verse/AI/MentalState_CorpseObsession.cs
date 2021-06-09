using System;

namespace Verse.AI
{
	// Token: 0x02000A26 RID: 2598
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x06003E18 RID: 15896 RVA: 0x0002EB56 File Offset: 0x0002CD56
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<bool>(ref this.alreadyHauledCorpse, "alreadyHauledCorpse", false, false);
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x00177C34 File Offset: 0x00175E34
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

		// Token: 0x06003E1A RID: 15898 RVA: 0x0002EB81 File Offset: 0x0002CD81
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0002EB9B File Offset: 0x0002CD9B
		public void Notify_CorpseHauled()
		{
			this.alreadyHauledCorpse = true;
		}

		// Token: 0x04002AEA RID: 10986
		public Corpse corpse;

		// Token: 0x04002AEB RID: 10987
		public bool alreadyHauledCorpse;

		// Token: 0x04002AEC RID: 10988
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}
