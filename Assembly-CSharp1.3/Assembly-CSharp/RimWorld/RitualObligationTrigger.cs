using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F46 RID: 3910
	public abstract class RitualObligationTrigger : IExposable
	{
		// Token: 0x06005CE9 RID: 23785 RVA: 0x001FEF38 File Offset: 0x001FD138
		public virtual void Init(RitualObligationTriggerProperties props)
		{
			this.mustBePlayerIdeo = props.mustBePlayerIdeo;
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Tick()
		{
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberSpawned(Pawn p)
		{
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
		}

		// Token: 0x06005CED RID: 23789 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberGenerated(Pawn pawn)
		{
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberDied(Pawn p)
		{
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberCorpseDestroyed(Pawn p)
		{
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberLost(Pawn p)
		{
		}

		// Token: 0x06005CF1 RID: 23793 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_GameStarted()
		{
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_RitualExecuted(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x001FEF46 File Offset: 0x001FD146
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.mustBePlayerIdeo, "mustBePlayerIdeo", false, false);
		}

		// Token: 0x040035DD RID: 13789
		public Precept_Ritual ritual;

		// Token: 0x040035DE RID: 13790
		protected bool mustBePlayerIdeo;
	}
}
