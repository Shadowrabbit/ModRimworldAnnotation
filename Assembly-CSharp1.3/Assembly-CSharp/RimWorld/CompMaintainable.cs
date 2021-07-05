using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114B RID: 4427
	public class CompMaintainable : ThingComp
	{
		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06006A69 RID: 27241 RVA: 0x0023CD56 File Offset: 0x0023AF56
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06006A6A RID: 27242 RVA: 0x0023CD63 File Offset: 0x0023AF63
		public MaintainableStage CurStage
		{
			get
			{
				if (this.ticksSinceMaintain < this.Props.ticksHealthy)
				{
					return MaintainableStage.Healthy;
				}
				if (this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance)
				{
					return MaintainableStage.NeedsMaintenance;
				}
				return MaintainableStage.Damaging;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06006A6B RID: 27243 RVA: 0x0023CD9C File Offset: 0x0023AF9C
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.CompDormant.Awake;
			}
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x0023CDC5 File Offset: 0x0023AFC5
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x0023CDD9 File Offset: 0x0023AFD9
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain++;
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06006A6E RID: 27246 RVA: 0x0023CE10 File Offset: 0x0023B010
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain += 250;
			this.CheckTakeDamage();
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x0023CE3C File Offset: 0x0023B03C
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x06006A70 RID: 27248 RVA: 0x0023CE85 File Offset: 0x0023B085
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x06006A71 RID: 27249 RVA: 0x0023CE90 File Offset: 0x0023B090
		public override string CompInspectStringExtra()
		{
			MaintainableStage curStage = this.CurStage;
			if (curStage == MaintainableStage.NeedsMaintenance)
			{
				return "DueForMaintenance".Translate();
			}
			if (curStage != MaintainableStage.Damaging)
			{
				return null;
			}
			return "DeterioratingDueToLackOfMaintenance".Translate();
		}

		// Token: 0x04003B50 RID: 15184
		public int ticksSinceMaintain;
	}
}
