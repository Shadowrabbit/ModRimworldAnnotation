using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E9 RID: 1001
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x06001E3B RID: 7739 RVA: 0x000BD2F8 File Offset: 0x000BB4F8
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000BD30C File Offset: 0x000BB50C
		public override void DoWindowContents(Rect inRect)
		{
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (caravan == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			if (Time.frameCount % 4 == 0)
			{
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan);
				List<FloatMenuOption> list2 = list;
				Vector2 vector = this.clickPos;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuWorld.StillValid(this.options[i], list, caravan, ref list2, ref vector))
					{
						this.options[i].Disabled = true;
					}
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000BD3BC File Offset: 0x000BB5BC
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan)
		{
			List<FloatMenuOption> list = null;
			Vector2 vector = new Vector2(-9999f, -9999f);
			return FloatMenuWorld.StillValid(opt, curOpts, forCaravan, ref list, ref vector);
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000BD3E8 File Offset: 0x000BB5E8
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan, ref List<FloatMenuOption> cachedChoices, ref Vector2 cachedChoicesForPos)
		{
			if (opt.revalidateWorldClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateWorldClickTarget.Spawned)
				{
					return false;
				}
				Vector2 vector = opt.revalidateWorldClickTarget.ScreenPos();
				vector.y = (float)UI.screenHeight - vector.y;
				List<FloatMenuOption> list;
				if (vector == cachedChoicesForPos)
				{
					list = cachedChoices;
				}
				else
				{
					cachedChoices = FloatMenuMakerWorld.ChoicesAtFor(vector, forCaravan);
					cachedChoicesForPos = vector;
					list = cachedChoices;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000BD4AC File Offset: 0x000BB6AC
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (!opt.Disabled && (caravan == null || !FloatMenuWorld.StillValid(opt, FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan), caravan)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000BCB14 File Offset: 0x000BAD14
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x0400125E RID: 4702
		private Vector2 clickPos;

		// Token: 0x0400125F RID: 4703
		private const int RevalidateEveryFrame = 4;
	}
}
