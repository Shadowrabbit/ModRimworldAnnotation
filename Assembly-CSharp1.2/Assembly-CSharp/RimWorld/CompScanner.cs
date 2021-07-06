using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A4 RID: 6308
	public abstract class CompScanner : ThingComp
	{
		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x06008C04 RID: 35844 RVA: 0x0005DE38 File Offset: 0x0005C038
		public CompProperties_Scanner Props
		{
			get
			{
				return (CompProperties_Scanner)this.props;
			}
		}

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x06008C05 RID: 35845 RVA: 0x0028BB20 File Offset: 0x00289D20
		public bool CanUseNow
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && !RoofUtility.IsAnyCellUnderRoof(this.parent) && (this.forbiddable == null || !this.forbiddable.Forbidden) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06008C06 RID: 35846 RVA: 0x0028BB8C File Offset: 0x00289D8C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.lastScanTick, "lastScanTick", 0f, false);
		}

		// Token: 0x06008C07 RID: 35847 RVA: 0x0005DE45 File Offset: 0x0005C045
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.forbiddable = this.parent.GetComp<CompForbiddable>();
		}

		// Token: 0x06008C08 RID: 35848 RVA: 0x0028BBE4 File Offset: 0x00289DE4
		public void Used(Pawn worker)
		{
			if (!this.CanUseNow)
			{
				Log.Error("Used while CanUseNow is false.", false);
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

		// Token: 0x06008C09 RID: 35849 RVA: 0x0028BC80 File Offset: 0x00289E80
		protected virtual bool TickDoesFind(float scanSpeed)
		{
			return Find.TickManager.TicksGame % 59 == 0 && (Rand.MTBEventOccurs(this.Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (this.Props.scanFindGuaranteedDays > 0f && this.daysWorkingSinceLastFinding >= this.Props.scanFindGuaranteedDays));
		}

		// Token: 0x06008C0A RID: 35850 RVA: 0x0028BCE4 File Offset: 0x00289EE4
		public override string CompInspectStringExtra()
		{
			string t = "";
			if (this.lastScanTick > (float)(Find.TickManager.TicksGame - 30))
			{
				t += "UserScanAbility".Translate() + ": " + this.lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((this.Props.scanFindMtbDays / this.lastUserSpeed).ToString("F1")) + "\n";
			}
			return t + "ScanningProgressToGuaranteedFind".Translate() + ": " + (this.daysWorkingSinceLastFinding / this.Props.scanFindGuaranteedDays).ToStringPercent();
		}

		// Token: 0x06008C0B RID: 35851
		protected abstract void DoFind(Pawn worker);

		// Token: 0x06008C0C RID: 35852 RVA: 0x0005DE70 File Offset: 0x0005C070
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

		// Token: 0x040059B3 RID: 22963
		protected float daysWorkingSinceLastFinding;

		// Token: 0x040059B4 RID: 22964
		protected float lastUserSpeed = 1f;

		// Token: 0x040059B5 RID: 22965
		protected float lastScanTick = -1f;

		// Token: 0x040059B6 RID: 22966
		protected CompPowerTrader powerComp;

		// Token: 0x040059B7 RID: 22967
		protected CompForbiddable forbiddable;
	}
}
