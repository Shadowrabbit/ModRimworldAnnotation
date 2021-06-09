using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200078F RID: 1935
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x00026744 File Offset: 0x00024944
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060030D8 RID: 12504 RVA: 0x00026756 File Offset: 0x00024956
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x0002675A File Offset: 0x0002495A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x0002676B File Offset: 0x0002496B
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x00026796 File Offset: 0x00024996
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000267A5 File Offset: 0x000249A5
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			if (name.Length == 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x0014366C File Offset: 0x0014186C
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			GUI.SetNextControlName("RenameField");
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.curName);
			if (this.AcceptsInput && text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			else if (!this.AcceptsInput)
			{
				((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl)).SelectAll();
			}
			if (!this.focusedRenameField)
			{
				UI.FocusControl("RenameField", this);
				this.focusedRenameField = true;
			}
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK", true, true, true) || flag)
			{
				AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					return;
				}
				else
				{
					this.SetName(this.curName);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}

		// Token: 0x060030DE RID: 12510
		protected abstract void SetName(string name);

		// Token: 0x0400219E RID: 8606
		protected string curName;

		// Token: 0x0400219F RID: 8607
		private bool focusedRenameField;

		// Token: 0x040021A0 RID: 8608
		private int startAcceptingInputAtFrame;
	}
}
