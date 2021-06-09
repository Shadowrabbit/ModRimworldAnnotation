using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E01 RID: 7681
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>, IExposable
	{
		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x0600A663 RID: 42595 RVA: 0x00303AB8 File Offset: 0x00301CB8
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

		// Token: 0x0600A664 RID: 42596 RVA: 0x00303AE0 File Offset: 0x00301CE0
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
				}), false);
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
				}), false);
				stuff = null;
			}
		}

		// Token: 0x0600A665 RID: 42597 RVA: 0x0006E107 File Offset: 0x0006C307
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x0600A666 RID: 42598 RVA: 0x00303B98 File Offset: 0x00301D98
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

		// Token: 0x0600A667 RID: 42599 RVA: 0x0006E12C File Offset: 0x0006C32C
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x0600A668 RID: 42600 RVA: 0x0006E138 File Offset: 0x0006C338
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x0600A669 RID: 42601 RVA: 0x0006E150 File Offset: 0x0006C350
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x0600A66A RID: 42602 RVA: 0x0006E15E File Offset: 0x0006C35E
		public override int GetHashCode()
		{
			return Gen.HashCombine<QualityCategory?>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.quality);
		}

		// Token: 0x0600A66B RID: 42603 RVA: 0x0006E182 File Offset: 0x0006C382
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x0600A66C RID: 42604 RVA: 0x00303BF4 File Offset: 0x00301DF4
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

		// Token: 0x0600A66D RID: 42605 RVA: 0x00303C2C File Offset: 0x00301E2C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thing, "thing");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
		}

		// Token: 0x040070BF RID: 28863
		public ThingDef thing;

		// Token: 0x040070C0 RID: 28864
		public ThingDef stuff;

		// Token: 0x040070C1 RID: 28865
		public QualityCategory? quality;
	}
}
