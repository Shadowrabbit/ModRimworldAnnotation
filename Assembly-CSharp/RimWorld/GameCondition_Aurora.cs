using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001172 RID: 4466
	public class GameCondition_Aurora : GameCondition
	{
		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06006256 RID: 25174 RVA: 0x00043AA8 File Offset: 0x00041CA8
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_Aurora.Colors[this.prevColorIndex], GameCondition_Aurora.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06006257 RID: 25175 RVA: 0x00043AD5 File Offset: 0x00041CD5
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

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06006258 RID: 25176 RVA: 0x001EBA70 File Offset: 0x001E9C70
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

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06006259 RID: 25177 RVA: 0x000325BD File Offset: 0x000307BD
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x001EBAAC File Offset: 0x001E9CAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x00043AEA File Offset: 0x00041CEA
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_Aurora.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x00026F08 File Offset: 0x00025108
		public override float SkyGazeChanceFactor(Map map)
		{
			return 8f;
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x00043B1C File Offset: 0x00041D1C
		public override float SkyGazeJoyGainFactor(Map map)
		{
			return 5f;
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x00043B23 File Offset: 0x00041D23
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600625F RID: 25183 RVA: 0x001EBAFC File Offset: 0x001E9CFC
		public override SkyTarget? SkyTarget(Map map)
		{
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(Mathf.Max(GenCelestial.CurCelestialSunGlow(map), 0.25f), colorSet, 1f, 1f));
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x00043B37 File Offset: 0x00041D37
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x06006261 RID: 25185 RVA: 0x001EBB90 File Offset: 0x001E9D90
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

		// Token: 0x06006262 RID: 25186 RVA: 0x00043B49 File Offset: 0x00041D49
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_Aurora.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x040041E7 RID: 16871
		private int curColorIndex = -1;

		// Token: 0x040041E8 RID: 16872
		private int prevColorIndex = -1;

		// Token: 0x040041E9 RID: 16873
		private float curColorTransition;

		// Token: 0x040041EA RID: 16874
		public const float MaxSunGlow = 0.5f;

		// Token: 0x040041EB RID: 16875
		private const float Glow = 0.25f;

		// Token: 0x040041EC RID: 16876
		private const float SkyColorStrength = 0.075f;

		// Token: 0x040041ED RID: 16877
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x040041EE RID: 16878
		private const float BaseBrightness = 0.73f;

		// Token: 0x040041EF RID: 16879
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x040041F0 RID: 16880
		private const int TransitionDurationTicks_Permanent = 3750;

		// Token: 0x040041F1 RID: 16881
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
