using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BDC RID: 3036
	public class GameCondition_Aurora : GameCondition
	{
		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004772 RID: 18290 RVA: 0x0017A2F0 File Offset: 0x001784F0
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_Aurora.Colors[this.prevColorIndex], GameCondition_Aurora.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004773 RID: 18291 RVA: 0x0017A31D File Offset: 0x0017851D
		private int TransitionDurationTicks
		{
			get
			{
				if (!base.Permanent)
				{
					return 280;
				}
				return 3750;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06004774 RID: 18292 RVA: 0x0017A334 File Offset: 0x00178534
		private bool BrightInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (GenCelestial.CurCelestialSunGlow(maps[i]) <= 0.5f)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06004775 RID: 18293 RVA: 0x0011EB3C File Offset: 0x0011CD3C
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0017A370 File Offset: 0x00178570
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0017A3BD File Offset: 0x001785BD
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_Aurora.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x000D26A9 File Offset: 0x000D08A9
		public override float SkyGazeChanceFactor(Map map)
		{
			return 8f;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0017A3EF File Offset: 0x001785EF
		public override float SkyGazeJoyGainFactor(Map map)
		{
			return 5f;
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0017A3F6 File Offset: 0x001785F6
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0017A40C File Offset: 0x0017860C
		public override SkyTarget? SkyTarget(Map map)
		{
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(Mathf.Max(GenCelestial.CurCelestialSunGlow(map), 0.25f), colorSet, 1f, 1f));
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0017A49D File Offset: 0x0017869D
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0017A4B0 File Offset: 0x001786B0
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			if (!base.Permanent && base.TicksLeft > this.TransitionTicks && this.BrightInAllMaps)
			{
				base.TicksLeft = this.TransitionTicks;
			}
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x0017A531 File Offset: 0x00178731
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_Aurora.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x04002BD7 RID: 11223
		private int curColorIndex = -1;

		// Token: 0x04002BD8 RID: 11224
		private int prevColorIndex = -1;

		// Token: 0x04002BD9 RID: 11225
		private float curColorTransition;

		// Token: 0x04002BDA RID: 11226
		public const float MaxSunGlow = 0.5f;

		// Token: 0x04002BDB RID: 11227
		private const float Glow = 0.25f;

		// Token: 0x04002BDC RID: 11228
		private const float SkyColorStrength = 0.075f;

		// Token: 0x04002BDD RID: 11229
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x04002BDE RID: 11230
		private const float BaseBrightness = 0.73f;

		// Token: 0x04002BDF RID: 11231
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x04002BE0 RID: 11232
		private const int TransitionDurationTicks_Permanent = 3750;

		// Token: 0x04002BE1 RID: 11233
		private static readonly Color[] Colors = new Color[]
		{
			new Color(0f, 1f, 0f),
			new Color(0.3f, 1f, 0f),
			new Color(0f, 1f, 0.7f),
			new Color(0.3f, 1f, 0.7f),
			new Color(0f, 0.5f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.87f, 0f, 1f),
			new Color(0.75f, 0f, 1f)
		};
	}
}
