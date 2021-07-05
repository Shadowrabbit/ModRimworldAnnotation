using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC5 RID: 3781
	public class GoodwillSituationWorker_MemeCompatibility : GoodwillSituationWorker
	{
		// Token: 0x0600593D RID: 22845 RVA: 0x001E7004 File Offset: 0x001E5204
		public override string GetPostProcessedLabel(Faction other)
		{
			if (this.def.otherMeme != null)
			{
				return base.GetPostProcessedLabel(other);
			}
			if (this.Applies(Faction.OfPlayer, other))
			{
				return "MemeGoodwillImpact_Player".Translate(base.GetPostProcessedLabel(other));
			}
			return "MemeGoodwillImpact_Other".Translate(base.GetPostProcessedLabel(other));
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x001E706B File Offset: 0x001E526B
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (!this.Applies(other))
			{
				return 0;
			}
			return this.def.naturalGoodwillOffset;
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x001E7083 File Offset: 0x001E5283
		private bool Applies(Faction other)
		{
			return this.Applies(Faction.OfPlayer, other) || this.Applies(other, Faction.OfPlayer);
		}

		// Token: 0x06005940 RID: 22848 RVA: 0x001E70A4 File Offset: 0x001E52A4
		private bool Applies(Faction a, Faction b)
		{
			Ideo primaryIdeo = a.ideos.PrimaryIdeo;
			if (primaryIdeo == null)
			{
				return false;
			}
			if (this.def.versusAll)
			{
				return primaryIdeo.memes.Contains(this.def.meme);
			}
			Ideo primaryIdeo2 = b.ideos.PrimaryIdeo;
			return primaryIdeo2 != null && primaryIdeo.memes.Contains(this.def.meme) && primaryIdeo2.memes.Contains(this.def.otherMeme);
		}
	}
}
