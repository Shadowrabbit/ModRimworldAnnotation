using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4B RID: 3915
	public abstract class RitualObligationTrigger_EveryMember : RitualObligationTrigger
	{
		// Token: 0x06005CFB RID: 23803
		protected abstract void Recache();

		// Token: 0x06005CFC RID: 23804 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_MemberSpawned(Pawn p)
		{
			this.Recache();
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_MemberDied(Pawn p)
		{
			this.Recache();
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			this.Recache();
		}

		// Token: 0x06005CFF RID: 23807 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_MemberGenerated(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x06005D00 RID: 23808 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_GameStarted()
		{
			this.Recache();
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_RitualExecuted(LordJob_Ritual ritual)
		{
			this.Recache();
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x001FF091 File Offset: 0x001FD291
		public override void Notify_MemberLost(Pawn pawn)
		{
			this.Recache();
		}
	}
}
