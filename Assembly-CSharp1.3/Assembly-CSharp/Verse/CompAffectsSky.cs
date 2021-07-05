using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037E RID: 894
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x000990F8 File Offset: 0x000972F8
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x00099108 File Offset: 0x00097308
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

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001A3E RID: 6718 RVA: 0x000991A7 File Offset: 0x000973A7
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x000991D0 File Offset: 0x000973D0
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x00099204 File Offset: 0x00097404
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0009921C File Offset: 0x0009741C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0009928D File Offset: 0x0009748D
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x04001125 RID: 4389
		private int autoAnimationStartTick;

		// Token: 0x04001126 RID: 4390
		private int fadeInDuration;

		// Token: 0x04001127 RID: 4391
		private int holdDuration;

		// Token: 0x04001128 RID: 4392
		private int fadeOutDuration;

		// Token: 0x04001129 RID: 4393
		private float autoAnimationTarget;
	}
}
