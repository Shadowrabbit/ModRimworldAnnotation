using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000445 RID: 1093
	public class Dialog_NamePawn : Window
	{
		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x000CF988 File Offset: 0x000CDB88
		private Name CurPawnName
		{
			get
			{
				NameTriple nameTriple = this.pawn.Name as NameTriple;
				if (nameTriple != null)
				{
					return new NameTriple(nameTriple.First, this.curName, nameTriple.Last);
				}
				if (this.pawn.Name is NameSingle)
				{
					return new NameSingle(this.curName, false);
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x000CF9E5 File Offset: 0x000CDBE5
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 175f);
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000CF9F8 File Offset: 0x000CDBF8
		public Dialog_NamePawn(Pawn pawn)
		{
			this.pawn = pawn;
			this.curName = pawn.Name.ToStringShort;
			if (pawn.story != null)
			{
				if (pawn.story.title != null)
				{
					this.curTitle = pawn.story.title;
				}
				else
				{
					this.curTitle = "";
				}
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.closeOnAccept = false;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000CFA74 File Offset: 0x000CDC74
		public override void DoWindowContents(Rect inRect)
		{
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			Text.Font = GameFont.Medium;
			string text = this.CurPawnName.ToString().Replace(" '' ", " ");
			if (this.curTitle == "")
			{
				text = text + ", " + this.pawn.story.TitleDefaultCap;
			}
			else if (this.curTitle != null)
			{
				text = text + ", " + this.curTitle.CapitalizeFirst();
			}
			Widgets.Label(new Rect(15f, 15f, 500f, 50f), text);
			Text.Font = GameFont.Small;
			string text2 = Widgets.TextField(new Rect(15f, 50f, inRect.width / 2f - 20f, 35f), this.curName);
			if (text2.Length < 16 && CharacterCardUtility.ValidNameRegex.IsMatch(text2))
			{
				this.curName = text2;
			}
			if (this.curTitle != null)
			{
				string text3 = Widgets.TextField(new Rect(inRect.width / 2f, 50f, inRect.width / 2f - 20f, 35f), this.curTitle);
				if (text3.Length < 25 && CharacterCardUtility.ValidNameRegex.IsMatch(text3))
				{
					this.curTitle = text3;
				}
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "OK", true, true, true) || flag)
			{
				if (string.IsNullOrEmpty(this.curName))
				{
					this.curName = ((NameTriple)this.pawn.Name).First;
				}
				this.pawn.Name = this.CurPawnName;
				if (this.pawn.story != null)
				{
					this.pawn.story.Title = this.curTitle;
				}
				Find.WindowStack.TryRemove(this, true);
				Messages.Message(this.pawn.def.race.Animal ? "AnimalGainsName".Translate(this.curName) : "PawnGainsName".Translate(this.curName, this.pawn.story.Title, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.PositiveEvent, false);
			}
		}

		// Token: 0x04001498 RID: 5272
		private Pawn pawn;

		// Token: 0x04001499 RID: 5273
		private string curName;

		// Token: 0x0400149A RID: 5274
		private string curTitle;
	}
}
