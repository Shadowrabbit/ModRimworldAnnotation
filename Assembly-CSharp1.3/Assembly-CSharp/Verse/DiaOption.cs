using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000454 RID: 1108
	public class DiaOption
	{
		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000D1889 File Offset: 0x000CFA89
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

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000D18A6 File Offset: 0x000CFAA6
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000D18B4 File Offset: 0x000CFAB4
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000D1901 File Offset: 0x000CFB01
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000D1938 File Offset: 0x000CFB38
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000D1998 File Offset: 0x000CFB98
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000D19F2 File Offset: 0x000CFBF2
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000D1A02 File Offset: 0x000CFC02
		public void SetText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000D1A0C File Offset: 0x000CFC0C
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
				Widgets.HyperlinkWithIcon(rect, this.hyperlink, text, 2f, 6f, null, false, null);
			}
			else if (Widgets.ButtonText(rect, text, false, !this.disabled, textColor, active && !this.disabled))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000D1ACC File Offset: 0x000CFCCC
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

		// Token: 0x040014E9 RID: 5353
		public Window dialog;

		// Token: 0x040014EA RID: 5354
		protected string text;

		// Token: 0x040014EB RID: 5355
		public DiaNode link;

		// Token: 0x040014EC RID: 5356
		public Func<DiaNode> linkLateBind;

		// Token: 0x040014ED RID: 5357
		public bool resolveTree;

		// Token: 0x040014EE RID: 5358
		public Action action;

		// Token: 0x040014EF RID: 5359
		public bool disabled;

		// Token: 0x040014F0 RID: 5360
		public string disabledReason;

		// Token: 0x040014F1 RID: 5361
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x040014F2 RID: 5362
		public Dialog_InfoCard.Hyperlink hyperlink;

		// Token: 0x040014F3 RID: 5363
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
