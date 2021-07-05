using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FF RID: 4863
	public abstract class Dialog_FileList : Window
	{
		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x060074ED RID: 29933 RVA: 0x0027D571 File Offset: 0x0027B771
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 700f);
			}
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x060074EE RID: 29934 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060074EF RID: 29935 RVA: 0x0027D584 File Offset: 0x0027B784
		public Dialog_FileList()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.ReloadFiles();
		}

		// Token: 0x060074F0 RID: 29936 RVA: 0x0027D5EC File Offset: 0x0027B7EC
		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2(inRect.width - 16f, 40f);
			inRect.height -= 45f;
			float y = vector.y;
			float height = (float)this.files.Count * y;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
			Rect outRect = new Rect(inRect.AtZero());
			outRect.height -= this.bottomAreaHeight;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			int num2 = 0;
			foreach (SaveFileInfo saveFileInfo in this.files)
			{
				if (num + vector.y >= this.scrollPosition.y && num <= this.scrollPosition.y + outRect.height)
				{
					Rect rect = new Rect(0f, num, vector.x, vector.y);
					if (num2 % 2 == 0)
					{
						Widgets.DrawAltRect(rect);
					}
					GUI.BeginGroup(rect);
					Rect rect2 = new Rect(rect.width - 36f, (rect.height - 36f) / 2f, 36f, 36f);
					if (Widgets.ButtonImage(rect2, TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor, true))
					{
						FileInfo localFile = saveFileInfo.FileInfo;
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(localFile.Name), delegate
						{
							localFile.Delete();
							this.ReloadFiles();
						}, true, null));
					}
					TooltipHandler.TipRegionByKey(rect2, "DeleteThisSavegame");
					Text.Font = GameFont.Small;
					Rect rect3 = new Rect(rect2.x - 100f, (rect.height - 36f) / 2f, 100f, 36f);
					if (Widgets.ButtonText(rect3, this.interactButLabel, true, true, true))
					{
						this.DoFileInteraction(Path.GetFileNameWithoutExtension(saveFileInfo.FileName));
					}
					Rect rect4 = new Rect(rect3.x - 94f, 0f, 94f, rect.height);
					Dialog_FileList.DrawDateAndVersion(saveFileInfo, rect4);
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = this.FileNameColor(saveFileInfo);
					Rect rect5 = new Rect(8f, 0f, rect4.x - 8f - 4f, rect.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					Text.Font = GameFont.Small;
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(saveFileInfo.FileName);
					Widgets.Label(rect5, fileNameWithoutExtension.Truncate(rect5.width * 1.8f, null));
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.EndGroup();
				}
				num += vector.y;
				num2++;
			}
			Widgets.EndScrollView();
			if (this.ShouldDoTypeInField)
			{
				this.DoTypeInField(inRect.AtZero());
			}
		}

		// Token: 0x060074F1 RID: 29937
		protected abstract void DoFileInteraction(string fileName);

		// Token: 0x060074F2 RID: 29938
		protected abstract void ReloadFiles();

		// Token: 0x060074F3 RID: 29939 RVA: 0x0027D93C File Offset: 0x0027BB3C
		protected virtual void DoTypeInField(Rect rect)
		{
			GUI.BeginGroup(rect);
			bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
			float y = rect.height - 52f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.SetNextControlName("MapNameField");
			string str = Widgets.TextField(new Rect(5f, y, 400f, 35f), this.typingName);
			if (GenText.IsValidFilename(str))
			{
				this.typingName = str;
			}
			if (!this.focusedNameArea)
			{
				UI.FocusControl("MapNameField", this);
				this.focusedNameArea = true;
			}
			if (Widgets.ButtonText(new Rect(420f, y, rect.width - 400f - 20f, 35f), "SaveGameButton".Translate(), true, true, true) || flag)
			{
				if (this.typingName.NullOrEmpty())
				{
					Messages.Message("NeedAName".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					this.DoFileInteraction(this.typingName);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x060074F4 RID: 29940 RVA: 0x0027DA5A File Offset: 0x0027BC5A
		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		// Token: 0x060074F5 RID: 29941 RVA: 0x0027DA64 File Offset: 0x0027BC64
		public static void DrawDateAndVersion(SaveFileInfo sfi, Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 2f, rect.width, rect.height / 2f);
			GUI.color = SaveFileInfo.UnimportantTextColor;
			Widgets.Label(rect2, sfi.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
			Rect rect3 = new Rect(0f, rect2.yMax, rect.width, rect.height / 2f);
			GUI.color = sfi.VersionColor;
			Widgets.Label(rect3, sfi.GameVersion);
			if (Mouse.IsOver(rect3))
			{
				TooltipHandler.TipRegion(rect3, sfi.CompatibilityTip);
			}
			GUI.EndGroup();
		}

		// Token: 0x04004079 RID: 16505
		protected string interactButLabel = "Error";

		// Token: 0x0400407A RID: 16506
		protected float bottomAreaHeight;

		// Token: 0x0400407B RID: 16507
		protected List<SaveFileInfo> files = new List<SaveFileInfo>();

		// Token: 0x0400407C RID: 16508
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400407D RID: 16509
		protected string typingName = "";

		// Token: 0x0400407E RID: 16510
		private bool focusedNameArea;

		// Token: 0x0400407F RID: 16511
		protected const float EntryHeight = 40f;

		// Token: 0x04004080 RID: 16512
		protected const float FileNameLeftMargin = 8f;

		// Token: 0x04004081 RID: 16513
		protected const float FileNameRightMargin = 4f;

		// Token: 0x04004082 RID: 16514
		protected const float FileInfoWidth = 94f;

		// Token: 0x04004083 RID: 16515
		protected const float InteractButWidth = 100f;

		// Token: 0x04004084 RID: 16516
		protected const float InteractButHeight = 36f;

		// Token: 0x04004085 RID: 16517
		protected const float DeleteButSize = 36f;

		// Token: 0x04004086 RID: 16518
		private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

		// Token: 0x04004087 RID: 16519
		protected const float NameTextFieldWidth = 400f;

		// Token: 0x04004088 RID: 16520
		protected const float NameTextFieldHeight = 35f;

		// Token: 0x04004089 RID: 16521
		protected const float NameTextFieldButtonSpace = 20f;
	}
}
