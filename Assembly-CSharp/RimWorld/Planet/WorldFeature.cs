using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002029 RID: 8233
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x0600AE61 RID: 44641 RVA: 0x000717D7 File Offset: 0x0006F9D7
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600AE62 RID: 44642 RVA: 0x000717D7 File Offset: 0x0006F9D7
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600AE63 RID: 44643 RVA: 0x000717D7 File Offset: 0x0006F9D7
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600AE64 RID: 44644 RVA: 0x000717D7 File Offset: 0x0006F9D7
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600AE65 RID: 44645 RVA: 0x000717D7 File Offset: 0x0006F9D7
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600AE66 RID: 44646 RVA: 0x0032B9C8 File Offset: 0x00329BC8
		private static void TweakChanged()
		{
			Find.WorldFeatures.textsCreated = false;
			WorldFeature.EffectiveDrawSizeCurve[0] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[0].x, WorldFeature.FeatureSizePoint10);
			WorldFeature.EffectiveDrawSizeCurve[1] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[1].x, WorldFeature.FeatureSizePoint25);
			WorldFeature.EffectiveDrawSizeCurve[2] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[2].x, WorldFeature.FeatureSizePoint50);
			WorldFeature.EffectiveDrawSizeCurve[3] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[3].x, WorldFeature.FeatureSizePoint100);
			WorldFeature.EffectiveDrawSizeCurve[4] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[4].x, WorldFeature.FeatureSizePoint200);
		}

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x0600AE67 RID: 44647 RVA: 0x000717DE File Offset: 0x0006F9DE
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x0600AE68 RID: 44648 RVA: 0x0032BAA8 File Offset: 0x00329CA8
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

		// Token: 0x0600AE69 RID: 44649 RVA: 0x000717F0 File Offset: 0x0006F9F0
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID;
		}

		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x0600AE6A RID: 44650 RVA: 0x00071807 File Offset: 0x0006FA07
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

		// Token: 0x040077B8 RID: 30648
		public int uniqueID;

		// Token: 0x040077B9 RID: 30649
		public FeatureDef def;

		// Token: 0x040077BA RID: 30650
		public string name;

		// Token: 0x040077BB RID: 30651
		public Vector3 drawCenter;

		// Token: 0x040077BC RID: 30652
		public float drawAngle;

		// Token: 0x040077BD RID: 30653
		public float maxDrawSizeInTiles;

		// Token: 0x040077BE RID: 30654
		public float alpha;

		// Token: 0x040077BF RID: 30655
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

		// Token: 0x040077C0 RID: 30656
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x040077C1 RID: 30657
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x040077C2 RID: 30658
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 90f;

		// Token: 0x040077C3 RID: 30659
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 150f;

		// Token: 0x040077C4 RID: 30660
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 200f;
	}
}
