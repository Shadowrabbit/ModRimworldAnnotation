using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B5 RID: 4533
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06006D30 RID: 27952 RVA: 0x00249D20 File Offset: 0x00247F20
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x00249D2D File Offset: 0x00247F2D
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06006D32 RID: 27954 RVA: 0x00249D3C File Offset: 0x00247F3C
		protected float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06006D33 RID: 27955 RVA: 0x00249D6C File Offset: 0x00247F6C
		protected bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06006D34 RID: 27956 RVA: 0x00249D84 File Offset: 0x00247F84
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

		// Token: 0x06006D35 RID: 27957 RVA: 0x00249DD4 File Offset: 0x00247FD4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06006D36 RID: 27958 RVA: 0x00249DE7 File Offset: 0x00247FE7
		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		// Token: 0x06006D37 RID: 27959 RVA: 0x00249DF0 File Offset: 0x00247FF0
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

		// Token: 0x06006D38 RID: 27960
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x06006D39 RID: 27961 RVA: 0x00249E50 File Offset: 0x00248050
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x00249E64 File Offset: 0x00248064
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x00249E98 File Offset: 0x00248098
		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true, true) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true, true);
			}
			return text;
		}

		// Token: 0x06006D3C RID: 27964 RVA: 0x00249F80 File Offset: 0x00248180
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

		// Token: 0x04003CAD RID: 15533
		private CompPowerTrader powerComp;

		// Token: 0x04003CAE RID: 15534
		private int progressTicks;
	}
}
