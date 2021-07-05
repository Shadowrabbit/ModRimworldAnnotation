using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D5 RID: 4565
	public abstract class CompScanner : ThingComp
	{
		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06006E30 RID: 28208 RVA: 0x0024F0EF File Offset: 0x0024D2EF
		public CompProperties_Scanner Props
		{
			get
			{
				return (CompProperties_Scanner)this.props;
			}
		}

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06006E31 RID: 28209 RVA: 0x0024F0FC File Offset: 0x0024D2FC
		public bool CanUseNow
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && !RoofUtility.IsAnyCellUnderRoof(this.parent) && (this.forbiddable == null || !this.forbiddable.Forbidden) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06006E32 RID: 28210 RVA: 0x0024F168 File Offset: 0x0024D368
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.lastScanTick, "lastScanTick", 0f, false);
		}

		// Token: 0x06006E33 RID: 28211 RVA: 0x0024F1BD File Offset: 0x0024D3BD
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.forbiddable = this.parent.GetComp<CompForbiddable>();
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x0024F1E8 File Offset: 0x0024D3E8
		public void Used(Pawn worker)
		{
			if (!this.CanUseNow)
			{
				Log.Error("Used while CanUseNow is false.");
			}
			this.lastScanTick = (float)Find.TickManager.TicksGame;
			this.lastUserSpeed = 1f;
			if (this.Props.scanSpeedStat != null)
			{
				this.lastUserSpeed = worker.GetStatValue(this.Props.scanSpeedStat, true);
			}
			this.daysWorkingSinceLastFinding += this.lastUserSpeed / 60000f;
			if (this.TickDoesFind(this.lastUserSpeed))
			{
				this.DoFind(worker);
				this.daysWorkingSinceLastFinding = 0f;
			}
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x0024F284 File Offset: 0x0024D484
		protected virtual bool TickDoesFind(float scanSpeed)
		{
			return Find.TickManager.TicksGame % 59 == 0 && (Rand.MTBEventOccurs(this.Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (this.Props.scanFindGuaranteedDays > 0f && this.daysWorkingSinceLastFinding >= this.Props.scanFindGuaranteedDays));
		}

		// Token: 0x06006E36 RID: 28214 RVA: 0x0024F2E8 File Offset: 0x0024D4E8
		public override string CompInspectStringExtra()
		{
			string t = "";
			if (this.lastScanTick > (float)(Find.TickManager.TicksGame - 30))
			{
				t += "UserScanAbility".Translate() + ": " + this.lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((this.Props.scanFindMtbDays / this.lastUserSpeed).ToString("F1")) + "\n";
			}
			return t + "ScanningProgressToGuaranteedFind".Translate() + ": " + (this.daysWorkingSinceLastFinding / this.Props.scanFindGuaranteedDays).ToStringPercent();
		}

		// Token: 0x06006E37 RID: 28215
		protected abstract void DoFind(Pawn worker);

		// Token: 0x06006E38 RID: 28216 RVA: 0x0024F3DF File Offset: 0x0024D5DF
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Find now",
					action = delegate()
					{
						this.DoFind(PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>());
					}
				};
			}
			yield break;
		}

		// Token: 0x04003D29 RID: 15657
		protected float daysWorkingSinceLastFinding;

		// Token: 0x04003D2A RID: 15658
		protected float lastUserSpeed = 1f;

		// Token: 0x04003D2B RID: 15659
		protected float lastScanTick = -1f;

		// Token: 0x04003D2C RID: 15660
		protected CompPowerTrader powerComp;

		// Token: 0x04003D2D RID: 15661
		protected CompForbiddable forbiddable;
	}
}
