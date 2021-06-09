using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007C9 RID: 1993
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x06003215 RID: 12821 RVA: 0x00027531 File Offset: 0x00025731
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

		// Token: 0x06003216 RID: 12822 RVA: 0x0014C0C8 File Offset: 0x0014A2C8
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

		// Token: 0x06003217 RID: 12823 RVA: 0x00027568 File Offset: 0x00025768
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}

		// Token: 0x040022D6 RID: 8918
		public const float IconSize = 24f;

		// Token: 0x040022D7 RID: 8919
		private static readonly Texture2D ContentSourceIcon_OfficialModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/OfficialModsFolder", true);

		// Token: 0x040022D8 RID: 8920
		private static readonly Texture2D ContentSourceIcon_ModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/ModsFolder", true);

		// Token: 0x040022D9 RID: 8921
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);
	}
}
