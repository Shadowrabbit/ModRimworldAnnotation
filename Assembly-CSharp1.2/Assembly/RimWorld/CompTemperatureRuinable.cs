using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001877 RID: 6263
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06008AE4 RID: 35556 RVA: 0x0005D272 File Offset: 0x0005B472
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06008AE5 RID: 35557 RVA: 0x0005D27F File Offset: 0x0005B47F
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x06008AE6 RID: 35558 RVA: 0x0005D291 File Offset: 0x0005B491
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x06008AE7 RID: 35559 RVA: 0x0005D2A9 File Offset: 0x0005B4A9
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x0005D2B6 File Offset: 0x0005B4B6
		public override void CompTick()
		{
			this.DoTicks(1);
		}

		// Token: 0x06008AE9 RID: 35561 RVA: 0x0005D2BF File Offset: 0x0005B4BF
		public override void CompTickRare()
		{
			this.DoTicks(250);
		}

		// Token: 0x06008AEA RID: 35562 RVA: 0x002882CC File Offset: 0x002864CC
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

		// Token: 0x06008AEB RID: 35563 RVA: 0x002883A4 File Offset: 0x002865A4
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x06008AEC RID: 35564 RVA: 0x002883E8 File Offset: 0x002865E8
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x06008AED RID: 35565 RVA: 0x0005D2CC File Offset: 0x0005B4CC
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompTemperatureRuinable>().ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x06008AEE RID: 35566 RVA: 0x00288410 File Offset: 0x00286610
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

		// Token: 0x04005915 RID: 22805
		protected float ruinedPercent;

		// Token: 0x04005916 RID: 22806
		public const string RuinedSignal = "RuinedByTemperature";
	}
}
