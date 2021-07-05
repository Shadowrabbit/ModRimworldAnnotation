using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145D RID: 5213
	public class FactionPermit : IExposable
	{
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06007081 RID: 28801 RVA: 0x0004BD3B File Offset: 0x00049F3B
		public RoyalTitleDef Title
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06007082 RID: 28802 RVA: 0x0004BD43 File Offset: 0x00049F43
		public RoyalTitlePermitDef Permit
		{
			get
			{
				return this.permit;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06007083 RID: 28803 RVA: 0x0004BD4B File Offset: 0x00049F4B
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x06007084 RID: 28804 RVA: 0x0004BD53 File Offset: 0x00049F53
		public int LastUsedTick
		{
			get
			{
				return this.lastUsedTick;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x06007085 RID: 28805 RVA: 0x0004BD5B File Offset: 0x00049F5B
		public bool OnCooldown
		{
			get
			{
				return this.LastUsedTick > 0 && Find.TickManager.TicksGame < this.LastUsedTick + this.permit.CooldownTicks;
			}
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x0004BD86 File Offset: 0x00049F86
		public FactionPermit()
		{
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x0004BD95 File Offset: 0x00049F95
		public FactionPermit(Faction faction, RoyalTitleDef title, RoyalTitlePermitDef permit)
		{
			this.title = title;
			this.faction = faction;
			this.permit = permit;
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x0004BDB9 File Offset: 0x00049FB9
		public void Notify_Used()
		{
			this.lastUsedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06007089 RID: 28809 RVA: 0x0004BDCB File Offset: 0x00049FCB
		public void ResetCooldown()
		{
			this.lastUsedTick = -1;
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x00226E9C File Offset: 0x0022509C
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.title, "title");
			Scribe_Defs.Look<RoyalTitlePermitDef>(ref this.permit, "permit");
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", -1, false);
		}

		// Token: 0x04004A26 RID: 18982
		private RoyalTitleDef title;

		// Token: 0x04004A27 RID: 18983
		private RoyalTitlePermitDef permit;

		// Token: 0x04004A28 RID: 18984
		private Faction faction;

		// Token: 0x04004A29 RID: 18985
		private int lastUsedTick = -1;
	}
}
