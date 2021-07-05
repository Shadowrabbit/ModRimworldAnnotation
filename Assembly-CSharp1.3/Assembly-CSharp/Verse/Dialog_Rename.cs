using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000446 RID: 1094
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x000CFD43 File Offset: 0x000CDF43
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x000CFD55 File Offset: 0x000CDF55
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002134 RID: 8500 RVA: 0x000CFD59 File Offset: 0x000CDF59
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000CFD6A File Offset: 0x000CDF6A
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000CFD95 File Offset: 0x000CDF95
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000CFDA4 File Offset: 0x000CDFA4
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			if (name.Length == 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000CFDBC File Offset: 0x000CDFBC
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

		// Token: 0x06002139 RID: 8505
		protected abstract void SetName(string name);

		// Token: 0x0400149B RID: 5275
		protected string curName;

		// Token: 0x0400149C RID: 5276
		private bool focusedRenameField;

		// Token: 0x0400149D RID: 5277
		private int startAcceptingInputAtFrame;
	}
}
