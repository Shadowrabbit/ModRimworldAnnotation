using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200173E RID: 5950
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x06008942 RID: 35138 RVA: 0x00315549 File Offset: 0x00313749
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06008943 RID: 35139 RVA: 0x00315549 File Offset: 0x00313749
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06008944 RID: 35140 RVA: 0x00315549 File Offset: 0x00313749
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06008945 RID: 35141 RVA: 0x00315549 File Offset: 0x00313749
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06008946 RID: 35142 RVA: 0x00315549 File Offset: 0x00313749
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06008947 RID: 35143 RVA: 0x00315550 File Offset: 0x00313750
		private static void TweakChanged()
		{
			Find.WorldFeatures.textsCreated = false;
			WorldFeature.EffectiveDrawSizeCurve[0] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[0].x, WorldFeature.FeatureSizePoint10);
			WorldFeature.EffectiveDrawSizeCurve[1] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[1].x, WorldFeature.FeatureSizePoint25);
			WorldFeature.EffectiveDrawSizeCurve[2] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[2].x, WorldFeature.FeatureSizePoint50);
			WorldFeature.EffectiveDrawSizeCurve[3] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[3].x, WorldFeature.FeatureSizePoint100);
			WorldFeature.EffectiveDrawSizeCurve[4] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[4].x, WorldFeature.FeatureSizePoint200);
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06008948 RID: 35144 RVA: 0x00315630 File Offset: 0x00313830
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x06008949 RID: 35145 RVA: 0x00315644 File Offset: 0x00313844
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", 0, false);
			Scribe_Defs.Look<FeatureDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<Vector3>(ref this.drawCenter, "drawCenter", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.drawAngle, "drawAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.maxDrawSizeInTiles, "maxDrawSizeInTiles", 0f, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600894A RID: 35146 RVA: 0x003156D1 File Offset: 0x003138D1
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID;
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x0600894B RID: 35147 RVA: 0x003156E8 File Offset: 0x003138E8
		public IEnumerable<int> Tiles
		{
			get
			{
				WorldGrid worldGrid = Find.WorldGrid;
				int tilesCount = worldGrid.TilesCount;
				int num;
				for (int i = 0; i < tilesCount; i = num + 1)
				{
					if (worldGrid[i].feature == this)
					{
						yield return i;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x04005714 RID: 22292
		public int uniqueID;

		// Token: 0x04005715 RID: 22293
		public FeatureDef def;

		// Token: 0x04005716 RID: 22294
		public string name;

		// Token: 0x04005717 RID: 22295
		public Vector3 drawCenter;

		// Token: 0x04005718 RID: 22296
		public float drawAngle;

		// Token: 0x04005719 RID: 22297
		public float maxDrawSizeInTiles;

		// Token: 0x0400571A RID: 22298
		public float alpha;

		// Token: 0x0400571B RID: 22299
		protected static SimpleCurve EffectiveDrawSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 15f),
				true
			},
			{
				new CurvePoint(25f, 40f),
				true
			},
			{
				new CurvePoint(50f, 90f),
				true
			},
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(200f, 200f),
				true
			}
		};

		// Token: 0x0400571C RID: 22300
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x0400571D RID: 22301
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x0400571E RID: 22302
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 90f;

		// Token: 0x0400571F RID: 22303
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 150f;

		// Token: 0x04005720 RID: 22304
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 200f;
	}
}
