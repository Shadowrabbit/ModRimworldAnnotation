using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A16 RID: 6678
	public abstract class Dialog_FileList : Window
	{
		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06009391 RID: 37777 RVA: 0x00062DA6 File Offset: 0x00060FA6
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 700f);
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06009392 RID: 37778 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009393 RID: 37779 RVA: 0x002A8828 File Offset: 0x002A6A28
		public Dialog_FileList()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.ReloadFiles();
		}

		// Token: 0x06009394 RID: 37780 RVA: 0x002A8890 File Offset: 0x002A6A90
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
			foreach (SaveFileInfo sfi in this.files)
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
						FileInfo localFile = sfi.FileInfo;
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
						this.DoFileInteraction(Path.GetFileNameWithoutExtension(sfi.FileInfo.Name));
					}
					Rect rect4 = new Rect(rect3.x - 94f, 0f, 94f, rect.height);
					Dialog_FileList.DrawDateAndVersion(sfi, rect4);
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = this.FileNameColor(sfi);
					Rect rect5 = new Rect(8f, 0f, rect4.x - 8f - 4f, rect.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					Text.Font = GameFont.Small;
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sfi.FileInfo.Name);
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

		// Token: 0x06009395 RID: 37781
		protected abstract void DoFileInteraction(string fileName);

		// Token: 0x06009396 RID: 37782
		protected abstract void ReloadFiles();

		// Token: 0x06009397 RID: 37783 RVA: 0x002A8BE8 File Offset: 0x002A6DE8
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

		// Token: 0x06009398 RID: 37784 RVA: 0x00062DB7 File Offset: 0x00060FB7
		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		// Token: 0x06009399 RID: 37785 RVA: 0x002A8D08 File Offset: 0x002A6F08
		public static void DrawDateAndVersion(SaveFileInfo sfi, Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 2f, rect.width, rect.height / 2f);
			GUI.color = SaveFileInfo.UnimportantTextColor;
			Widgets.Label(rect2, sfi.FileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
			Rect rect3 = new Rect(0f, rect2.yMax, rect.width, rect.height / 2f);
			GUI.color = sfi.VersionColor;
			Widgets.Label(rect3, sfi.GameVersion);
			if (Mouse.IsOver(rect3))
			{
				TooltipHandler.TipRegion(rect3, sfi.CompatibilityTip);
			}
			GUI.EndGroup();
		}

		// Token: 0x04005D7C RID: 23932
		protected string interactButLabel = "Error";

		// Token: 0x04005D7D RID: 23933
		protected float bottomAreaHeight;

		// Token: 0x04005D7E RID: 23934
		protected List<SaveFileInfo> files = new List<SaveFileInfo>();

		// Token: 0x04005D7F RID: 23935
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04005D80 RID: 23936
		protected string typingName = "";

		// Token: 0x04005D81 RID: 23937
		private bool focusedNameArea;

		// Token: 0x04005D82 RID: 23938
		protected const float EntryHeight = 40f;

		// Token: 0x04005D83 RID: 23939
		protected const float FileNameLeftMargin = 8f;

		// Token: 0x04005D84 RID: 23940
		protected const float FileNameRightMargin = 4f;

		// Token: 0x04005D85 RID: 23941
		protected const float FileInfoWidth = 94f;

		// Token: 0x04005D86 RID: 23942
		protected const float InteractButWidth = 100f;

		// Token: 0x04005D87 RID: 23943
		protected const float InteractButHeight = 36f;

		// Token: 0x04005D88 RID: 23944
		protected const float DeleteButSize = 36f;

		// Token: 0x04005D89 RID: 23945
		private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

		// Token: 0x04005D8A RID: 23946
		protected const float NameTextFieldWidth = 400f;

		// Token: 0x04005D8B RID: 23947
		protected const float NameTextFieldHeight = 35f;

		// Token: 0x04005D8C RID: 23948
		protected const float NameTextFieldButtonSpace = 20f;
	}
}
