using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7D RID: 2941
	public class QuestPart_Hyperlinks : QuestPart
	{
		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x0016C547 File Offset: 0x0016A747
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = this.GetHyperlinks();
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0016C563 File Offset: 0x0016A763
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			if (this.thingDefs != null)
			{
				int num;
				for (int i = 0; i < this.thingDefs.Count; i = num + 1)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.thingDefs[i], -1);
					num = i;
				}
			}
			if (this.pawns != null)
			{
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					if (this.pawns[i].royalty != null && this.pawns[i].royalty.AllTitlesForReading.Any<RoyalTitle>())
					{
						RoyalTitle mostSeniorTitle = this.pawns[i].royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							yield return new Dialog_InfoCard.Hyperlink(mostSeniorTitle.def, mostSeniorTitle.faction, -1);
						}
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x0016C574 File Offset: 0x0016A774
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.thingDefs, "thingDefs", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingDefs == null)
				{
					this.thingDefs = new List<ThingDef>();
				}
				this.thingDefs.RemoveAll((ThingDef x) => x == null);
				if (this.pawns == null)
				{
					this.pawns = new List<Pawn>();
				}
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x0016C637 File Offset: 0x0016A837
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040029B7 RID: 10679
		public List<ThingDef> thingDefs = new List<ThingDef>();

		// Token: 0x040029B8 RID: 10680
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040029B9 RID: 10681
		private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;
	}
}
