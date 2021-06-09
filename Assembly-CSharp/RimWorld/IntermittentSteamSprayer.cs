using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001710 RID: 5904
	public class IntermittentSteamSprayer
	{
		// Token: 0x060081F7 RID: 33271 RVA: 0x000574B3 File Offset: 0x000556B3
		public IntermittentSteamSprayer(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x060081F8 RID: 33272 RVA: 0x0026846C File Offset: 0x0026666C
		public void SteamSprayerTick()
		{
			if (this.sprayTicksLeft > 0)
			{
				this.sprayTicksLeft--;
				if (Rand.Value < 0.6f)
				{
					MoteMaker.ThrowAirPuffUp(this.parent.TrueCenter(), this.parent.Map);
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

		// Token: 0x04005450 RID: 21584
		private Thing parent;

		// Token: 0x04005451 RID: 21585
		private int ticksUntilSpray = 500;

		// Token: 0x04005452 RID: 21586
		private int sprayTicksLeft;

		// Token: 0x04005453 RID: 21587
		public Action startSprayCallback;

		// Token: 0x04005454 RID: 21588
		public Action endSprayCallback;

		// Token: 0x04005455 RID: 21589
		private const int MinTicksBetweenSprays = 500;

		// Token: 0x04005456 RID: 21590
		private const int MaxTicksBetweenSprays = 2000;

		// Token: 0x04005457 RID: 21591
		private const int MinSprayDuration = 200;

		// Token: 0x04005458 RID: 21592
		private const int MaxSprayDuration = 500;

		// Token: 0x04005459 RID: 21593
		private const float SprayThickness = 0.6f;
	}
}
