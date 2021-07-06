using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000521 RID: 1313
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x0001D248 File Offset: 0x0001B448
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x001076C8 File Offset: 0x001058C8
		public virtual float LerpFactor
		{
			get
			{
				if (this.HasAutoAnimation)
				{
					int ticksGame = Find.TickManager.TicksGame;
					float num;
					if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration)
					{
						num = (float)(ticksGame - this.autoAnimationStartTick) / (float)this.fadeInDuration;
					}
					else if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration)
					{
						num = 1f;
					}
					else
					{
						num = 1f - (float)(ticksGame - this.autoAnimationStartTick - this.fadeInDuration - this.holdDuration) / (float)this.fadeOutDuration;
					}
					return Mathf.Clamp01(num * this.autoAnimationTarget);
				}
				return 0f;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x0001D255 File Offset: 0x0001B455
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x0001D27E File Offset: 0x0001B47E
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x000CE1E8 File Offset: 0x000CC3E8
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00107768 File Offset: 0x00105968
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0001D2B1 File Offset: 0x0001B4B1
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x040016F2 RID: 5874
		private int autoAnimationStartTick;

		// Token: 0x040016F3 RID: 5875
		private int fadeInDuration;

		// Token: 0x040016F4 RID: 5876
		private int holdDuration;

		// Token: 0x040016F5 RID: 5877
		private int fadeOutDuration;

		// Token: 0x040016F6 RID: 5878
		private float autoAnimationTarget;
	}
}
