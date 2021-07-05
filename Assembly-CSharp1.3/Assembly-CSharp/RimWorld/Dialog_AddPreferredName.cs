using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D6 RID: 4822
	public class Dialog_AddPreferredName : Window
	{
		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x002678E4 File Offset: 0x00265AE4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		// Token: 0x06007341 RID: 29505 RVA: 0x002678F8 File Offset: 0x00265AF8
		public Dialog_AddPreferredName()
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
		}

		// Token: 0x06007342 RID: 29506 RVA: 0x00267988 File Offset: 0x00265B88
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
					if (listing_Standard.CurHeight + 30f > inRect.height - (Window.CloseButSize.y + 8f))
					{
						break;
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06007343 RID: 29507 RVA: 0x00267AAC File Offset: 0x00265CAC
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

		// Token: 0x06007344 RID: 29508 RVA: 0x00267B82 File Offset: 0x00265D82
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

		// Token: 0x06007345 RID: 29509 RVA: 0x00267BBF File Offset: 0x00265DBF
		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}

		// Token: 0x04003EDA RID: 16090
		private string searchName = "";

		// Token: 0x04003EDB RID: 16091
		private string[] searchWords;

		// Token: 0x04003EDC RID: 16092
		private List<NameTriple> cachedNames;
	}
}
