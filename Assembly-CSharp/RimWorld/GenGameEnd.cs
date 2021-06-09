using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB7 RID: 7351
	public static class GenGameEnd
	{
		// Token: 0x06009FE9 RID: 40937 RVA: 0x0006AA76 File Offset: 0x00068C76
		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying = true)
		{
			GenGameEnd.EndGameDialogMessage(msg, allowKeepPlaying, Color.clear);
		}

		// Token: 0x06009FEA RID: 40938 RVA: 0x002EC218 File Offset: 0x002EA418
		public static void EndGameDialogMessage(string msg, bool allowKeepPlaying, Color screenFillColor)
		{
			DiaNode diaNode = new DiaNode(msg);
			if (allowKeepPlaying)
			{
				DiaOption diaOption = new DiaOption("GameOverKeepPlaying".Translate());
				diaOption.resolveTree = true;
				diaNode.options.Add(diaOption);
			}
			DiaOption diaOption2 = new DiaOption("GameOverMainMenu".Translate());
			diaOption2.action = delegate()
			{
				GenScene.GoToMainMenu();
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, true, false, null);
			dialog_NodeTree.screenFillColor = screenFillColor;
			dialog_NodeTree.silenceAmbientSound = !allowKeepPlaying;
			dialog_NodeTree.closeOnAccept = allowKeepPlaying;
			dialog_NodeTree.closeOnCancel = allowKeepPlaying;
			Find.WindowStack.Add(dialog_NodeTree);
			Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
		}
	}
}
