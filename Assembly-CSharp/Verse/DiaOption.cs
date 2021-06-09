using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020007A8 RID: 1960
	public class DiaOption
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003150 RID: 12624 RVA: 0x00026E4D File Offset: 0x0002504D
		public static DiaOption DefaultOK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					resolveTree = true
				};
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06003151 RID: 12625 RVA: 0x00026E6A File Offset: 0x0002506A
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x00145730 File Offset: 0x00143930
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00026E77 File Offset: 0x00025077
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x00145780 File Offset: 0x00143980
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x001457E0 File Offset: 0x001439E0
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x00026EAB File Offset: 0x000250AB
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x00026EBB File Offset: 0x000250BB
		public void SetText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x0014583C File Offset: 0x00143A3C
		public float OptOnGUI(Rect rect, bool active = true)
		{
			Color textColor = Widgets.NormalOptionColor;
			string text = this.text;
			if (this.disabled)
			{
				textColor = this.DisabledOptionColor;
				if (this.disabledReason != null)
				{
					text = text + " (" + this.disabledReason + ")";
				}
			}
			rect.height = Text.CalcHeight(text, rect.width);
			if (this.hyperlink.def != null)
			{
				Widgets.HyperlinkWithIcon(rect, this.hyperlink, text, 2f, 6f);
			}
			else if (Widgets.ButtonText(rect, text, false, !this.disabled, textColor, active && !this.disabled))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x001458F0 File Offset: 0x00143AF0
		protected void Activate()
		{
			if (this.clickSound != null && !this.resolveTree)
			{
				this.clickSound.PlayOneShotOnCamera(null);
			}
			if (this.resolveTree)
			{
				this.OwningDialog.Close(true);
			}
			if (this.action != null)
			{
				this.action();
			}
			if (this.linkLateBind != null)
			{
				this.OwningDialog.GotoNode(this.linkLateBind());
				return;
			}
			if (this.link != null)
			{
				this.OwningDialog.GotoNode(this.link);
			}
		}

		// Token: 0x04002213 RID: 8723
		public Window dialog;

		// Token: 0x04002214 RID: 8724
		protected string text;

		// Token: 0x04002215 RID: 8725
		public DiaNode link;

		// Token: 0x04002216 RID: 8726
		public Func<DiaNode> linkLateBind;

		// Token: 0x04002217 RID: 8727
		public bool resolveTree;

		// Token: 0x04002218 RID: 8728
		public Action action;

		// Token: 0x04002219 RID: 8729
		public bool disabled;

		// Token: 0x0400221A RID: 8730
		public string disabledReason;

		// Token: 0x0400221B RID: 8731
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x0400221C RID: 8732
		public Dialog_InfoCard.Hyperlink hyperlink;

		// Token: 0x0400221D RID: 8733
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
