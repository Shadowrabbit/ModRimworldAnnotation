using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D0 RID: 4560
	public class CompQuality : ThingComp
	{
		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06006E07 RID: 28167 RVA: 0x0024E301 File Offset: 0x0024C501
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x06006E08 RID: 28168 RVA: 0x0024E30C File Offset: 0x0024C50C
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x06006E09 RID: 28169 RVA: 0x0024E336 File Offset: 0x0024C536
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x0024E350 File Offset: 0x0024C550
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x06006E0B RID: 28171 RVA: 0x0024E360 File Offset: 0x0024C560
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x06006E0C RID: 28172 RVA: 0x0024E382 File Offset: 0x0024C582
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x06006E0D RID: 28173 RVA: 0x0024E39C File Offset: 0x0024C59C
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(this.Quality.GetLabel().CapitalizeFirst());
		}

		// Token: 0x04003D19 RID: 15641
		private QualityCategory qualityInt = QualityCategory.Normal;
	}
}
