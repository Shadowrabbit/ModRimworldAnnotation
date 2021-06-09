using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001878 RID: 6264
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x06008AF0 RID: 35568 RVA: 0x0005D2E4 File Offset: 0x0005B4E4
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x06008AF1 RID: 35569 RVA: 0x0005D2F1 File Offset: 0x0005B4F1
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06008AF2 RID: 35570 RVA: 0x0005D300 File Offset: 0x0005B500
		protected float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06008AF3 RID: 35571 RVA: 0x0005D330 File Offset: 0x0005B530
		protected bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06008AF4 RID: 35572 RVA: 0x002884A8 File Offset: 0x002866A8
		private int TicksUntilRadiusInteger
		{
			get
			{
				float num = Mathf.Ceil(this.CurrentRadius) - this.CurrentRadius;
				if (num < 1E-05f)
				{
					num = 1f;
				}
				float num2 = this.Props.radius / this.Props.daysToRadius;
				return (int)(num / num2 * 60000f);
			}
		}

		// Token: 0x06008AF5 RID: 35573 RVA: 0x0005D347 File Offset: 0x0005B547
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06008AF6 RID: 35574 RVA: 0x0005D35A File Offset: 0x0005B55A
		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		// Token: 0x06008AF7 RID: 35575 RVA: 0x002884F8 File Offset: 0x002866F8
		public override void CompTickRare()
		{
			if (this.Working)
			{
				this.progressTicks += 250;
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius);
				for (int i = 0; i < num; i++)
				{
					this.AffectCell(this.parent.Position + GenRadial.RadialPattern[i]);
				}
			}
		}

		// Token: 0x06008AF8 RID: 35576
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x06008AF9 RID: 35577 RVA: 0x0005D363 File Offset: 0x0005B563
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x06008AFA RID: 35578 RVA: 0x0005D377 File Offset: 0x0005B577
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x06008AFB RID: 35579 RVA: 0x00288558 File Offset: 0x00286758
		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true, true) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true, true);
			}
			return text;
		}

		// Token: 0x06008AFC RID: 35580 RVA: 0x0005D3A8 File Offset: 0x0005B5A8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Progress 1 day",
					action = delegate()
					{
						this.progressTicks += 60000;
					}
				};
			}
			yield break;
		}

		// Token: 0x04005917 RID: 22807
		private CompPowerTrader powerComp;

		// Token: 0x04005918 RID: 22808
		private int progressTicks;
	}
}
