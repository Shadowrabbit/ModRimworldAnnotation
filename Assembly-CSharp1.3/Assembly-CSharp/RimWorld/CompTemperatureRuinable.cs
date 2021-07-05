using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B4 RID: 4532
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06006D24 RID: 27940 RVA: 0x00249AD1 File Offset: 0x00247CD1
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x00249ADE File Offset: 0x00247CDE
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x06006D26 RID: 27942 RVA: 0x00249AF0 File Offset: 0x00247CF0
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x06006D27 RID: 27943 RVA: 0x00249B08 File Offset: 0x00247D08
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x06006D28 RID: 27944 RVA: 0x00249B15 File Offset: 0x00247D15
		public override void CompTick()
		{
			this.DoTicks(1);
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x00249B1E File Offset: 0x00247D1E
		public override void CompTickRare()
		{
			this.DoTicks(250);
		}

		// Token: 0x06006D2A RID: 27946 RVA: 0x00249B2C File Offset: 0x00247D2C
		private void DoTicks(int ticks)
		{
			if (!this.Ruined)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					this.ruinedPercent += (ambientTemperature - this.Props.maxSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				else if (ambientTemperature < this.Props.minSafeTemperature)
				{
					this.ruinedPercent -= (ambientTemperature - this.Props.minSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				if (this.ruinedPercent >= 1f)
				{
					this.ruinedPercent = 1f;
					this.parent.BroadcastCompSignal("RuinedByTemperature");
					return;
				}
				if (this.ruinedPercent < 0f)
				{
					this.ruinedPercent = 0f;
				}
			}
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x00249C04 File Offset: 0x00247E04
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x00249C48 File Offset: 0x00247E48
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x00249C6F File Offset: 0x00247E6F
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompTemperatureRuinable>().ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00249C88 File Offset: 0x00247E88
		public override string CompInspectStringExtra()
		{
			if (this.Ruined)
			{
				return "RuinedByTemperature".Translate();
			}
			if (this.ruinedPercent > 0f)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				string str;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					str = "Overheating".Translate();
				}
				else
				{
					if (ambientTemperature >= this.Props.minSafeTemperature)
					{
						return null;
					}
					str = "Freezing".Translate();
				}
				return str + ": " + this.ruinedPercent.ToStringPercent();
			}
			return null;
		}

		// Token: 0x04003CAB RID: 15531
		protected float ruinedPercent;

		// Token: 0x04003CAC RID: 15532
		public const string RuinedSignal = "RuinedByTemperature";
	}
}
