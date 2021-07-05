using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005EA RID: 1514
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x06002BAF RID: 11183 RVA: 0x00104574 File Offset: 0x00102774
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (this.pawn.ownership.OwnedBed != null)
			{
				this.target = this.pawn.ownership.OwnedBed.Position;
				return;
			}
			this.target = this.pawn.Position;
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x001045C8 File Offset: 0x001027C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04001A99 RID: 6809
		public IntVec3 target;
	}
}
