using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046F RID: 1135
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x0600224F RID: 8783 RVA: 0x000D9864 File Offset: 0x000D7A64
		public static Texture2D GetIcon(this ContentSource s)
		{
			switch (s)
			{
			case ContentSource.Undefined:
				return BaseContent.BadTex;
			case ContentSource.OfficialModsFolder:
				return ContentSourceUtility.ContentSourceIcon_OfficialModsFolder;
			case ContentSource.ModsFolder:
				return ContentSourceUtility.ContentSourceIcon_ModsFolder;
			case ContentSource.SteamWorkshop:
				return ContentSourceUtility.ContentSourceIcon_SteamWorkshop;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000D989C File Offset: 0x000D7A9C
		public static void DrawContentSource(Rect r, ContentSource source, Action clickAction = null)
		{
			Rect rect = new Rect(r.x, r.y + r.height / 2f - 12f, 24f, 24f);
			GUI.DrawTexture(rect, source.GetIcon());
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "Source".Translate() + ": " + source.HumanLabel(), (int)(r.x + r.y * 56161f));
				Widgets.DrawHighlight(rect);
			}
			if (clickAction != null && Widgets.ButtonInvisible(rect, true))
			{
				clickAction();
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000D9942 File Offset: 0x000D7B42
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}

		// Token: 0x040015B0 RID: 5552
		public const float IconSize = 24f;

		// Token: 0x040015B1 RID: 5553
		private static readonly Texture2D ContentSourceIcon_OfficialModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/OfficialModsFolder", true);

		// Token: 0x040015B2 RID: 5554
		private static readonly Texture2D ContentSourceIcon_ModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/ModsFolder", true);

		// Token: 0x040015B3 RID: 5555
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);
	}
}
