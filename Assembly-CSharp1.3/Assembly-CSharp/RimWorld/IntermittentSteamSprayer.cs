using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B2 RID: 4274
	public class IntermittentSteamSprayer
	{
		// Token: 0x06006618 RID: 26136 RVA: 0x002277BB File Offset: 0x002259BB
		public IntermittentSteamSprayer(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x002277D8 File Offset: 0x002259D8
		public void SteamSprayerTick()
		{
			if (this.sprayTicksLeft > 0)
			{
				this.sprayTicksLeft--;
				if (Rand.Value < 0.6f)
				{
					FleckMaker.ThrowAirPuffUp(this.parent.TrueCenter(), this.parent.Map);
				}
				if (Find.TickManager.TicksGame % 20 == 0)
				{
					GenTemperature.PushHeat(this.parent, 40f);
				}
				if (this.sprayTicksLeft <= 0)
				{
					if (this.endSprayCallback != null)
					{
						this.endSprayCallback();
					}
					this.ticksUntilSpray = Rand.RangeInclusive(500, 2000);
					return;
				}
			}
			else
			{
				this.ticksUntilSpray--;
				if (this.ticksUntilSpray <= 0)
				{
					if (this.startSprayCallback != null)
					{
						this.startSprayCallback();
					}
					this.sprayTicksLeft = Rand.RangeInclusive(200, 500);
				}
			}
		}

		// Token: 0x0400399D RID: 14749
		private Thing parent;

		// Token: 0x0400399E RID: 14750
		private int ticksUntilSpray = 500;

		// Token: 0x0400399F RID: 14751
		private int sprayTicksLeft;

		// Token: 0x040039A0 RID: 14752
		public Action startSprayCallback;

		// Token: 0x040039A1 RID: 14753
		public Action endSprayCallback;

		// Token: 0x040039A2 RID: 14754
		private const int MinTicksBetweenSprays = 500;

		// Token: 0x040039A3 RID: 14755
		private const int MaxTicksBetweenSprays = 2000;

		// Token: 0x040039A4 RID: 14756
		private const int MinSprayDuration = 200;

		// Token: 0x040039A5 RID: 14757
		private const int MaxSprayDuration = 500;

		// Token: 0x040039A6 RID: 14758
		private const float SprayThickness = 0.6f;
	}
}
