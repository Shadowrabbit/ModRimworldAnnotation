using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000940 RID: 2368
	public static class MouseoverSounds
	{
		// Token: 0x06003A18 RID: 14872 RVA: 0x0002CC69 File Offset: 0x0002AE69
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x0002CC77 File Offset: 0x0002AE77
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00168DA8 File Offset: 0x00166FA8
		public static void DoRegion(Rect rect, SoundDef sound)
		{
			if (sound == null)
			{
				return;
			}
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect2 = new Rect(GUIUtility.GUIToScreenPoint(rect.position), rect.size);
			MouseoverSounds.MouseoverRegionCall item = default(MouseoverSounds.MouseoverRegionCall);
			item.rect = rect2;
			item.sound = sound;
			item.mouseIsOver = Mouse.IsOver(rect);
			MouseoverSounds.frameCalls.Add(item);
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00168E14 File Offset: 0x00167014
		public static void ResolveFrame()
		{
			for (int i = 0; i < MouseoverSounds.frameCalls.Count; i++)
			{
				if (MouseoverSounds.frameCalls[i].mouseIsOver)
				{
					if (MouseoverSounds.lastUsedCallInd != i && !MouseoverSounds.frameCalls[i].Matches(MouseoverSounds.lastUsedCall) && MouseoverSounds.forceSilenceUntilFrame < Time.frameCount)
					{
						MouseoverSounds.frameCalls[i].sound.PlayOneShotOnCamera(null);
					}
					MouseoverSounds.lastUsedCallInd = i;
					MouseoverSounds.lastUsedCall = MouseoverSounds.frameCalls[i];
					MouseoverSounds.frameCalls.Clear();
					return;
				}
			}
			MouseoverSounds.lastUsedCall = MouseoverSounds.MouseoverRegionCall.Invalid;
			MouseoverSounds.lastUsedCallInd = -1;
			MouseoverSounds.frameCalls.Clear();
		}

		// Token: 0x04002856 RID: 10326
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04002857 RID: 10327
		private static int lastUsedCallInd = -1;

		// Token: 0x04002858 RID: 10328
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x04002859 RID: 10329
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02000941 RID: 2369
		private struct MouseoverRegionCall
		{
			// Token: 0x17000944 RID: 2372
			// (get) Token: 0x06003A1D RID: 14877 RVA: 0x0002CC9C File Offset: 0x0002AE9C
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000945 RID: 2373
			// (get) Token: 0x06003A1E RID: 14878 RVA: 0x00168ECC File Offset: 0x001670CC
			public static MouseoverSounds.MouseoverRegionCall Invalid
			{
				get
				{
					return new MouseoverSounds.MouseoverRegionCall
					{
						rect = new Rect(-1000f, -1000f, 0f, 0f)
					};
				}
			}

			// Token: 0x06003A1F RID: 14879 RVA: 0x0002CCB3 File Offset: 0x0002AEB3
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06003A20 RID: 14880 RVA: 0x00168F04 File Offset: 0x00167104
			public override string ToString()
			{
				if (!this.IsValid)
				{
					return "(Invalid)";
				}
				return string.Concat(new object[]
				{
					"(rect=",
					this.rect,
					this.mouseIsOver ? "mouseIsOver" : "",
					")"
				});
			}

			// Token: 0x0400285A RID: 10330
			public bool mouseIsOver;

			// Token: 0x0400285B RID: 10331
			public Rect rect;

			// Token: 0x0400285C RID: 10332
			public SoundDef sound;
		}
	}
}
