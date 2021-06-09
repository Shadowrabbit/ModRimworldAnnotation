using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D6 RID: 6614
	public class Dialog_AddPreferredName : Window
	{
		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06009230 RID: 37424 RVA: 0x00061F18 File Offset: 0x00060118
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		// Token: 0x06009231 RID: 37425 RVA: 0x0029FBE4 File Offset: 0x0029DDE4
		public Dialog_AddPreferredName()
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
		}

		// Token: 0x06009232 RID: 37426 RVA: 0x0029FC74 File Offset: 0x0029DE74
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label("TypeFirstNickOrLastName".Translate(), -1f, null);
			string text = listing_Standard.TextEntry(this.searchName, 1);
			if (text.Length < 20)
			{
				this.searchName = text;
				this.searchWords = this.searchName.Replace("'", "").Split(new char[]
				{
					' '
				});
			}
			listing_Standard.Gap(4f);
			if (this.searchName.Length > 1)
			{
				foreach (NameTriple nameTriple in this.cachedNames.Where(new Func<NameTriple, bool>(this.FilterMatch)))
				{
					if (listing_Standard.ButtonText(nameTriple.ToString(), null))
					{
						this.TryChooseName(nameTriple);
					}
					if (listing_Standard.CurHeight + 30f > inRect.height - (this.CloseButSize.y + 8f))
					{
						break;
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06009233 RID: 37427 RVA: 0x0029FD98 File Offset: 0x0029DF98
		private bool FilterMatch(NameTriple n)
		{
			if (n.First == "Tynan" && n.Last == "Sylvester")
			{
				return false;
			}
			if (this.searchWords.Length == 0)
			{
				return false;
			}
			if (this.searchWords.Length == 1)
			{
				return n.Last.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.First.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase);
			}
			return this.searchWords.Length == 2 && n.First.EqualsIgnoreCase(this.searchWords[0]) && (n.Last.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06009234 RID: 37428 RVA: 0x00061F29 File Offset: 0x00060129
		private void TryChooseName(NameTriple name)
		{
			if (this.AlreadyPreferred(name))
			{
				Messages.Message("MessageAlreadyPreferredName".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Prefs.PreferredNames.Add(name.ToString());
			this.Close(true);
		}

		// Token: 0x06009235 RID: 37429 RVA: 0x00061F66 File Offset: 0x00060166
		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}

		// Token: 0x04005C69 RID: 23657
		private string searchName = "";

		// Token: 0x04005C6A RID: 23658
		private string[] searchWords;

		// Token: 0x04005C6B RID: 23659
		private List<NameTriple> cachedNames;
	}
}
