using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE6 RID: 3558
	public class FactionPermit : IExposable
	{
		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x001BCA8F File Offset: 0x001BAC8F
		public RoyalTitleDef Title
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06005263 RID: 21091 RVA: 0x001BCA97 File Offset: 0x001BAC97
		public RoyalTitlePermitDef Permit
		{
			get
			{
				return this.permit;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06005264 RID: 21092 RVA: 0x001BCA9F File Offset: 0x001BAC9F
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x001BCAA7 File Offset: 0x001BACA7
		public int LastUsedTick
		{
			get
			{
				return this.lastUsedTick;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06005266 RID: 21094 RVA: 0x001BCAAF File Offset: 0x001BACAF
		public bool OnCooldown
		{
			get
			{
				return this.LastUsedTick > 0 && Find.TickManager.TicksGame < this.LastUsedTick + this.permit.CooldownTicks;
			}
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x001BCADA File Offset: 0x001BACDA
		public FactionPermit()
		{
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x001BCAE9 File Offset: 0x001BACE9
		public FactionPermit(Faction faction, RoyalTitleDef title, RoyalTitlePermitDef permit)
		{
			this.title = title;
			this.faction = faction;
			this.permit = permit;
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x001BCB0D File Offset: 0x001BAD0D
		public void Notify_Used()
		{
			this.lastUsedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x001BCB1F File Offset: 0x001BAD1F
		public void ResetCooldown()
		{
			this.lastUsedTick = -1;
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x001BCB28 File Offset: 0x001BAD28
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.title, "title");
			Scribe_Defs.Look<RoyalTitlePermitDef>(ref this.permit, "permit");
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", -1, false);
		}

		// Token: 0x040030A7 RID: 12455
		private RoyalTitleDef title;

		// Token: 0x040030A8 RID: 12456
		private RoyalTitlePermitDef permit;

		// Token: 0x040030A9 RID: 12457
		private Faction faction;

		// Token: 0x040030AA RID: 12458
		private int lastUsedTick = -1;
	}
}
