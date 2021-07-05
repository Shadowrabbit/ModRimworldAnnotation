using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012DF RID: 4831
	public class Dialog_ChooseIdeoSymbols : Window
	{
		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06007399 RID: 29593 RVA: 0x0026D7C4 File Offset: 0x0026B9C4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 620f);
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x0600739A RID: 29594 RVA: 0x0026D7D8 File Offset: 0x0026B9D8
		private static List<ColorDef> IdeoColorsSorted
		{
			get
			{
				if (Dialog_ChooseIdeoSymbols.allColors == null)
				{
					Dialog_ChooseIdeoSymbols.allColors = new List<ColorDef>();
					Dialog_ChooseIdeoSymbols.allColors.AddRange(DefDatabase<ColorDef>.AllDefsListForReading);
					Dialog_ChooseIdeoSymbols.allColors.SortByColor((ColorDef x) => x.color);
				}
				return Dialog_ChooseIdeoSymbols.allColors;
			}
		}

		// Token: 0x0600739B RID: 29595 RVA: 0x0026D834 File Offset: 0x0026BA34
		public Dialog_ChooseIdeoSymbols(Ideo ideo)
		{
			this.ideo = ideo;
			this.absorbInputAroundWindow = true;
			this.newName = ideo.name;
			this.newAdjective = ideo.adjective;
			this.newMemberName = ideo.memberName;
			this.newWorshipRoomLabel = ideo.WorshipRoomLabel;
			this.newIconDef = ideo.iconDef;
			this.newColorDef = ideo.colorDef;
		}

		// Token: 0x0600739C RID: 29596 RVA: 0x0026D89D File Offset: 0x0026BA9D
		public override void OnAcceptKeyPressed()
		{
			this.TryAccept();
			Event.current.Use();
		}

		// Token: 0x0600739D RID: 29597 RVA: 0x0026D8B0 File Offset: 0x0026BAB0
		public override void DoWindowContents(Rect rect)
		{
			Rect mainRect = rect;
			mainRect.height -= Window.CloseButSize.y;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(mainRect.x, mainRect.y, rect.width, 35f), "EditSymbols".Translate());
			Text.Font = GameFont.Small;
			mainRect.yMin += 45f;
			float num = mainRect.y;
			float num2 = mainRect.x + mainRect.width / 3f;
			float num3 = mainRect.xMax - num2;
			float num4 = num;
			Widgets.Label(mainRect.x, ref num4, mainRect.width, "Name".Translate(), default(TipSignal));
			this.newName = Widgets.TextField(new Rect(num2, num, num3, Dialog_ChooseIdeoSymbols.EditFieldHeight), this.newName);
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			float num5 = num;
			Widgets.Label(mainRect.x, ref num5, mainRect.width, "Adjective".Translate(), default(TipSignal));
			this.newAdjective = Widgets.TextField(new Rect(num2, num, num3, Dialog_ChooseIdeoSymbols.EditFieldHeight), this.newAdjective);
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			float num6 = num;
			Widgets.Label(mainRect.x, ref num6, mainRect.width, "IdeoMembers".Translate(), default(TipSignal));
			this.newMemberName = Widgets.TextField(new Rect(num2, num, num3, Dialog_ChooseIdeoSymbols.EditFieldHeight), this.newMemberName);
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			float num7 = num;
			Widgets.Label(mainRect.x, ref num7, mainRect.width, "WorshipRoom".Translate(), default(TipSignal));
			Rect rect2 = new Rect(num2, num, num3 - Dialog_ChooseIdeoSymbols.ResetButtonWidth - 10f, Dialog_ChooseIdeoSymbols.EditFieldHeight);
			Rect rect3 = new Rect(rect2.xMax + 10f, num, Dialog_ChooseIdeoSymbols.ResetButtonWidth, Dialog_ChooseIdeoSymbols.EditFieldHeight);
			this.newWorshipRoomLabel = Widgets.TextField(rect2, this.newWorshipRoomLabel);
			if (Widgets.ButtonText(rect3, "Reset".Translate(), true, true, true))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				this.ideo.WorshipRoomLabel = null;
				this.newWorshipRoomLabel = this.ideo.WorshipRoomLabel;
			}
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			Widgets.Label(mainRect.x, ref num, mainRect.width, "Icon".Translate(), default(TipSignal));
			mainRect.yMin = num;
			this.DoColorSelector(mainRect, ref num);
			mainRect.yMin = num;
			this.DoIconSelector(mainRect);
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_ChooseIdeoSymbols.ButSize.y, Dialog_ChooseIdeoSymbols.ButSize.x, Dialog_ChooseIdeoSymbols.ButSize.y), "Back".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_ChooseIdeoSymbols.ButSize.x, rect.height - Dialog_ChooseIdeoSymbols.ButSize.y, Dialog_ChooseIdeoSymbols.ButSize.x, Dialog_ChooseIdeoSymbols.ButSize.y), "DoneButton".Translate(), true, true, true))
			{
				this.TryAccept();
			}
		}

		// Token: 0x0600739E RID: 29598 RVA: 0x0026DC28 File Offset: 0x0026BE28
		private void DoIconSelector(Rect mainRect)
		{
			int num = 50;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(mainRect, ref this.scrollPos, viewRect, true);
			IEnumerable<IdeoIconDef> allDefs = DefDatabase<IdeoIconDef>.AllDefs;
			int num2 = Mathf.FloorToInt(viewRect.width / (float)(num + 5));
			int num3 = allDefs.Count<IdeoIconDef>();
			int num4 = 0;
			foreach (IdeoIconDef ideoIconDef in allDefs)
			{
				int num5 = num4 / num2;
				int num6 = num4 % num2;
				int num7 = (num4 >= num3 - num3 % num2) ? (num3 % num2) : num2;
				float num8 = (viewRect.width - (float)(num7 * num) - (float)((num7 - 1) * 5)) / 2f;
				Rect rect = new Rect(num8 + (float)(num6 * num) + (float)(num6 * 5), (float)(num5 * num + num5 * 5), (float)num, (float)num);
				Widgets.DrawLightHighlight(rect);
				Widgets.DrawHighlightIfMouseover(rect);
				if (ideoIconDef == this.newIconDef)
				{
					Widgets.DrawBox(rect, 1, null);
				}
				GUI.color = this.newColorDef.color;
				GUI.DrawTexture(new Rect(rect.x + 5f, rect.y + 5f, 40f, 40f), ideoIconDef.Icon);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.newIconDef = ideoIconDef;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				this.viewHeight = Mathf.Max(this.viewHeight, rect.yMax);
				num4++;
			}
			GUI.color = Color.white;
			Widgets.EndScrollView();
		}

		// Token: 0x0600739F RID: 29599 RVA: 0x0026DDF0 File Offset: 0x0026BFF0
		private void DoColorSelector(Rect mainRect, ref float curY)
		{
			int num = 26;
			float num2 = 98f;
			int num3 = Mathf.FloorToInt((mainRect.width - num2) / (float)(num + 2));
			int num4 = Mathf.CeilToInt((float)Dialog_ChooseIdeoSymbols.IdeoColorsSorted.Count / (float)num3);
			GUI.BeginGroup(mainRect);
			GUI.color = this.newColorDef.color;
			GUI.DrawTexture(new Rect(5f, 5f, 88f, 88f), this.newIconDef.Icon);
			GUI.color = Color.white;
			curY += num2;
			int num5 = 0;
			foreach (ColorDef colorDef in Dialog_ChooseIdeoSymbols.IdeoColorsSorted)
			{
				int num6 = num5 / num3;
				int num7 = num5 % num3;
				float num8 = (num2 - (float)(num * num4) - 2f) / 2f;
				Rect rect = new Rect(num2 + (float)(num7 * num) + (float)(num7 * 2), num8 + (float)(num6 * num) + (float)(num6 * 2), (float)num, (float)num);
				Widgets.DrawLightHighlight(rect);
				Widgets.DrawHighlightIfMouseover(rect);
				if (this.newColorDef == colorDef)
				{
					Widgets.DrawBox(rect, 1, null);
				}
				Widgets.DrawBoxSolid(new Rect(rect.x + 2f, rect.y + 2f, 22f, 22f), colorDef.color);
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.newColorDef = colorDef;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				curY = Mathf.Max(curY, mainRect.yMin + rect.yMax);
				num5++;
			}
			GUI.EndGroup();
			curY += 4f;
		}

		// Token: 0x060073A0 RID: 29600 RVA: 0x0026DFAC File Offset: 0x0026C1AC
		private void TryAccept()
		{
			bool flag = this.ideo.name != this.newName || this.ideo.adjective != this.newAdjective || this.ideo.memberName != this.newMemberName;
			if (!this.newName.NullOrEmpty())
			{
				this.ideo.name = this.newName;
			}
			if (!this.newAdjective.NullOrEmpty())
			{
				this.ideo.adjective = this.newAdjective;
			}
			if (!this.newMemberName.NullOrEmpty())
			{
				this.ideo.memberName = this.newMemberName;
			}
			if (this.ideo.WorshipRoomLabel != this.newWorshipRoomLabel && !this.newWorshipRoomLabel.NullOrEmpty())
			{
				this.ideo.WorshipRoomLabel = this.newWorshipRoomLabel;
			}
			this.ideo.SetIcon(this.newIconDef, this.newColorDef);
			if (flag)
			{
				this.ideo.MakeMemeberNamePluralDirty();
				this.ideo.RegenerateAllPreceptNames();
			}
			this.Close(true);
		}

		// Token: 0x04003F49 RID: 16201
		private Ideo ideo;

		// Token: 0x04003F4A RID: 16202
		private string newName;

		// Token: 0x04003F4B RID: 16203
		private string newAdjective;

		// Token: 0x04003F4C RID: 16204
		private string newMemberName;

		// Token: 0x04003F4D RID: 16205
		private string newWorshipRoomLabel;

		// Token: 0x04003F4E RID: 16206
		private IdeoIconDef newIconDef;

		// Token: 0x04003F4F RID: 16207
		private ColorDef newColorDef;

		// Token: 0x04003F50 RID: 16208
		private Vector2 scrollPos;

		// Token: 0x04003F51 RID: 16209
		private float viewHeight;

		// Token: 0x04003F52 RID: 16210
		private static List<ColorDef> allColors;

		// Token: 0x04003F53 RID: 16211
		private const int IconSize = 40;

		// Token: 0x04003F54 RID: 16212
		private const int IconPadding = 5;

		// Token: 0x04003F55 RID: 16213
		private const int IconMargin = 5;

		// Token: 0x04003F56 RID: 16214
		private const int ColorSize = 22;

		// Token: 0x04003F57 RID: 16215
		private const int ColorPadding = 2;

		// Token: 0x04003F58 RID: 16216
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04003F59 RID: 16217
		private static readonly Color IconColor = new Color(0.95f, 0.95f, 0.95f);

		// Token: 0x04003F5A RID: 16218
		private static readonly float EditFieldHeight = 30f;

		// Token: 0x04003F5B RID: 16219
		private static readonly float ResetButtonWidth = 120f;
	}
}
