using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017D9 RID: 6105
	public class CompMaintainable : ThingComp
	{
		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06008721 RID: 34593 RVA: 0x0005ABA5 File Offset: 0x00058DA5
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06008722 RID: 34594 RVA: 0x0005ABB2 File Offset: 0x00058DB2
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

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06008723 RID: 34595 RVA: 0x0027B2B4 File Offset: 0x002794B4
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.CompDormant.Awake;
			}
		}

		// Token: 0x06008724 RID: 34596 RVA: 0x0005ABEB File Offset: 0x00058DEB
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x06008725 RID: 34597 RVA: 0x0005ABFF File Offset: 0x00058DFF
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

		// Token: 0x06008726 RID: 34598 RVA: 0x0005AC36 File Offset: 0x00058E36
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

		// Token: 0x06008727 RID: 34599 RVA: 0x0027B2E0 File Offset: 0x002794E0
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x06008728 RID: 34600 RVA: 0x0005AC5F File Offset: 0x00058E5F
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x06008729 RID: 34601 RVA: 0x0027B328 File Offset: 0x00279528
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

		// Token: 0x040056DC RID: 22236
		public int ticksSinceMaintain;
	}
}
