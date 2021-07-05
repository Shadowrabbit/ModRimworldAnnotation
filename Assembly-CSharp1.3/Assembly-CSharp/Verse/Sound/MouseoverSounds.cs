using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200056A RID: 1386
	public static class MouseoverSounds
	{
		// Token: 0x060028D5 RID: 10453 RVA: 0x000F7AC4 File Offset: 0x000F5CC4
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000F7AD2 File Offset: 0x000F5CD2
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000F7AE0 File Offset: 0x000F5CE0
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

		// Token: 0x060028D8 RID: 10456 RVA: 0x000F7B4C File Offset: 0x000F5D4C
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

		// Token: 0x0400195A RID: 6490
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x0400195B RID: 6491
		private static int lastUsedCallInd = -1;

		// Token: 0x0400195C RID: 6492
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x0400195D RID: 6493
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02001D05 RID: 7429
		private struct MouseoverRegionCall
		{
			// Token: 0x17001A36 RID: 6710
			// (get) Token: 0x0600A92E RID: 43310 RVA: 0x00389537 File Offset: 0x00387737
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17001A37 RID: 6711
			// (get) Token: 0x0600A92F RID: 43311 RVA: 0x00389550 File Offset: 0x00387750
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

			// Token: 0x0600A930 RID: 43312 RVA: 0x00389586 File Offset: 0x00387786
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x0600A931 RID: 43313 RVA: 0x0038959C File Offset: 0x0038779C
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

			// Token: 0x04007022 RID: 28706
			public bool mouseIsOver;

			// Token: 0x04007023 RID: 28707
			public Rect rect;

			// Token: 0x04007024 RID: 28708
			public SoundDef sound;
		}
	}
}
