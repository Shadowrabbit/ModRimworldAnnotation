using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D1 RID: 4305
	public class QuestPart_Hyperlinks : QuestPart
	{
		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005DDE RID: 24030 RVA: 0x00041129 File Offset: 0x0003F329
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

		// Token: 0x06005DDF RID: 24031 RVA: 0x00041145 File Offset: 0x0003F345
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

		// Token: 0x06005DE0 RID: 24032 RVA: 0x001DDC38 File Offset: 0x001DBE38
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

		// Token: 0x06005DE1 RID: 24033 RVA: 0x00041155 File Offset: 0x0003F355
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003EC6 RID: 16070
		public List<ThingDef> thingDefs = new List<ThingDef>();

		// Token: 0x04003EC7 RID: 16071
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003EC8 RID: 16072
		private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;
	}
}
