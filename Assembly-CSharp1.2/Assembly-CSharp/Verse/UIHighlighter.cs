using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000760 RID: 1888
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x06002F9D RID: 12189 RVA: 0x0013BC48 File Offset: 0x00139E48
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

		// Token: 0x06002F9E RID: 12190 RVA: 0x0013BCCC File Offset: 0x00139ECC
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

		// Token: 0x06002F9F RID: 12191 RVA: 0x000256A9 File Offset: 0x000238A9
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x04002045 RID: 8261
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x04002046 RID: 8262
		private const float PulseFrequency = 1.2f;

		// Token: 0x04002047 RID: 8263
		private const float PulseAmplitude = 0.7f;

		// Token: 0x04002048 RID: 8264
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);
	}
}
