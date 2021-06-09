using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001899 RID: 6297
	public class CompQuality : ThingComp
	{
		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x06008BBD RID: 35773 RVA: 0x0005DB0B File Offset: 0x0005BD0B
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x06008BBE RID: 35774 RVA: 0x0028AF34 File Offset: 0x00289134
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x06008BBF RID: 35775 RVA: 0x0005DB13 File Offset: 0x0005BD13
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x06008BC0 RID: 35776 RVA: 0x0005DB2D File Offset: 0x0005BD2D
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x06008BC1 RID: 35777 RVA: 0x0028AF60 File Offset: 0x00289160
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x06008BC2 RID: 35778 RVA: 0x0005DB3B File Offset: 0x0005BD3B
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x06008BC3 RID: 35779 RVA: 0x0005DB55 File Offset: 0x0005BD55
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(this.Quality.GetLabel().CapitalizeFirst());
		}

		// Token: 0x0400598E RID: 22926
		private QualityCategory qualityInt = QualityCategory.Normal;
	}
}
