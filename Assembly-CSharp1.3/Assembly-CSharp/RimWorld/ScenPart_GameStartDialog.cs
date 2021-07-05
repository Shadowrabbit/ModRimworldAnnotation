using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100A RID: 4106
	public class ScenPart_GameStartDialog : ScenPart
	{
		// Token: 0x060060C9 RID: 24777 RVA: 0x0020EA18 File Offset: 0x0020CC18
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 5f);
			this.text = Widgets.TextArea(scenPartRect, this.text, false);
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x0020EA4B File Offset: 0x0020CC4B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.textKey, "textKey", null, false);
			Scribe_Defs.Look<SoundDef>(ref this.closeSound, "closeSound");
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x0020EA88 File Offset: 0x0020CC88
		public override void PostGameStart()
		{
			if (Find.GameInitData.startedFromEntry)
			{
				Find.MusicManagerPlay.disabled = true;
				Find.WindowStack.Notify_GameStartDialogOpened();
				DiaNode diaNode = new DiaNode(this.text.NullOrEmpty() ? this.textKey.TranslateSimple() : this.text);
				DiaOption diaOption = new DiaOption();
				diaOption.resolveTree = true;
				diaOption.clickSound = null;
				diaNode.options.Add(diaOption);
				Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
				dialog_NodeTree.soundClose = ((this.closeSound != null) ? this.closeSound : SoundDefOf.GameStartSting);
				dialog_NodeTree.closeAction = delegate()
				{
					Find.MusicManagerPlay.ForceSilenceFor(7f);
					Find.MusicManagerPlay.disabled = false;
					Find.WindowStack.Notify_GameStartDialogClosed();
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					TutorSystem.Notify_Event("GameStartDialogClosed");
				};
				Find.WindowStack.Add(dialog_NodeTree);
				Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
			}
		}

		// Token: 0x04003749 RID: 14153
		private string text;

		// Token: 0x0400374A RID: 14154
		private string textKey;

		// Token: 0x0400374B RID: 14155
		private SoundDef closeSound;
	}
}
