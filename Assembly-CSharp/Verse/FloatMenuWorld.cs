using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000701 RID: 1793
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x06002D7E RID: 11646 RVA: 0x00023E2C File Offset: 0x0002202C
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x00133E7C File Offset: 0x0013207C
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

		// Token: 0x06002D80 RID: 11648 RVA: 0x00133F2C File Offset: 0x0013212C
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan)
		{
			List<FloatMenuOption> list = null;
			Vector2 vector = new Vector2(-9999f, -9999f);
			return FloatMenuWorld.StillValid(opt, curOpts, forCaravan, ref list, ref vector);
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x00133F58 File Offset: 0x00132158
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

		// Token: 0x06002D82 RID: 11650 RVA: 0x0013401C File Offset: 0x0013221C
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (!opt.Disabled && (caravan == null || !FloatMenuWorld.StillValid(opt, FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan), caravan)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x00023CE8 File Offset: 0x00021EE8
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x04001EFE RID: 7934
		private Vector2 clickPos;

		// Token: 0x04001EFF RID: 7935
		private const int RevalidateEveryFrame = 4;
	}
}
