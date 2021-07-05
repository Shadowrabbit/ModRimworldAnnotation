using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200157B RID: 5499
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>, IExposable
	{
		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x06008204 RID: 33284 RVA: 0x002DF1A4 File Offset: 0x002DD3A4
		public QualityCategory Quality
		{
			get
			{
				QualityCategory? qualityCategory = this.quality;
				if (qualityCategory == null)
				{
					return QualityCategory.Normal;
				}
				return qualityCategory.GetValueOrDefault();
			}
		}

		// Token: 0x06008205 RID: 33285 RVA: 0x002DF1CC File Offset: 0x002DD3CC
		public ThingStuffPairWithQuality(ThingDef thing, ThingDef stuff, QualityCategory quality)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.quality = new QualityCategory?(quality);
			if (quality != QualityCategory.Normal && !thing.HasComp(typeof(CompQuality)))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with quality",
					quality,
					" but ",
					thing,
					" doesn't have CompQuality."
				}));
				quality = QualityCategory.Normal;
			}
			if (stuff != null && !thing.MadeFromStuff)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with stuff ",
					stuff,
					" but ",
					thing,
					" is not made from stuff."
				}));
				stuff = null;
			}
		}

		// Token: 0x06008206 RID: 33286 RVA: 0x002DF27F File Offset: 0x002DD47F
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x06008207 RID: 33287 RVA: 0x002DF2A4 File Offset: 0x002DD4A4
		public static bool operator ==(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			if (a.thing == b.thing && a.stuff == b.stuff)
			{
				QualityCategory? qualityCategory = a.quality;
				QualityCategory? qualityCategory2 = b.quality;
				return qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null);
			}
			return false;
		}

		// Token: 0x06008208 RID: 33288 RVA: 0x002DF2FE File Offset: 0x002DD4FE
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x06008209 RID: 33289 RVA: 0x002DF30A File Offset: 0x002DD50A
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x0600820A RID: 33290 RVA: 0x002DF322 File Offset: 0x002DD522
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x0600820B RID: 33291 RVA: 0x002DF330 File Offset: 0x002DD530
		public override int GetHashCode()
		{
			return Gen.HashCombine<QualityCategory?>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.quality);
		}

		// Token: 0x0600820C RID: 33292 RVA: 0x002DF354 File Offset: 0x002DD554
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x0600820D RID: 33293 RVA: 0x002DF368 File Offset: 0x002DD568
		public Thing MakeThing()
		{
			Thing result = ThingMaker.MakeThing(this.thing, this.stuff);
			CompQuality compQuality = result.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(this.Quality, ArtGenerationContext.Outsider);
			}
			return result;
		}

		// Token: 0x0600820E RID: 33294 RVA: 0x002DF3A0 File Offset: 0x002DD5A0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thing, "thing");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
		}

		// Token: 0x040050E1 RID: 20705
		public ThingDef thing;

		// Token: 0x040050E2 RID: 20706
		public ThingDef stuff;

		// Token: 0x040050E3 RID: 20707
		public QualityCategory? quality;
	}
}
