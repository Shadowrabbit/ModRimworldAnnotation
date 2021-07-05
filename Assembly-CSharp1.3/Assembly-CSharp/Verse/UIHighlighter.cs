using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000429 RID: 1065
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x06002004 RID: 8196 RVA: 0x000C6404 File Offset: 0x000C4604
		public static void HighlightTag(string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (tag.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				if (UIHighlighter.liveTags[i].First == tag && UIHighlighter.liveTags[i].Second == Time.frameCount)
				{
					return;
				}
			}
			UIHighlighter.liveTags.Add(new Pair<string, int>(tag, Time.frameCount));
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x000C6488 File Offset: 0x000C4688
		public static void HighlightOpportunity(Rect rect, string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				Pair<string, int> pair = UIHighlighter.liveTags[i];
				if (tag == pair.First && Time.frameCount == pair.Second + 1)
				{
					Rect rect2 = rect.ContractedBy(-10f);
					GUI.color = new Color(1f, 1f, 1f, Pulser.PulseBrightness(1.2f, 0.7f));
					Widgets.DrawAtlas(rect2, UIHighlighter.TutorHighlightAtlas);
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000C652D File Offset: 0x000C472D
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x04001368 RID: 4968
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x04001369 RID: 4969
		private const float PulseFrequency = 1.2f;

		// Token: 0x0400136A RID: 4970
		private const float PulseAmplitude = 0.7f;

		// Token: 0x0400136B RID: 4971
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);
	}
}
