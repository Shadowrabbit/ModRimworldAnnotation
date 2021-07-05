using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E6 RID: 998
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06001E20 RID: 7712 RVA: 0x000BC894 File Offset: 0x000BAA94
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000BC8A8 File Offset: 0x000BAAA8
		public override void DoWindowContents(Rect inRect)
		{
			FloatMenuMap.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.selPawn = (Find.Selector.SingleSelectedThing as Pawn);
			if (CS$<>8__locals1.selPawn == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			bool flag = this.options.Count >= 3;
			if (Time.frameCount % 4 == 0 || this.lastOptionsForRevalidation == null)
			{
				this.lastOptionsForRevalidation = FloatMenuMakerMap.ChoicesAtFor(this.clickPos, CS$<>8__locals1.selPawn, false);
				FloatMenuMap.cachedChoices.Clear();
				FloatMenuMap.cachedChoices.Add(this.clickPos, this.lastOptionsForRevalidation);
				if (!flag)
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						this.<DoWindowContents>g__RevalidateOption|6_0(this.options[i], ref CS$<>8__locals1);
					}
				}
			}
			else if (flag)
			{
				if (this.nextOptionToRevalidate >= this.options.Count)
				{
					this.nextOptionToRevalidate = 0;
				}
				int num = Mathf.CeilToInt((float)this.options.Count / 3f);
				int num2 = this.nextOptionToRevalidate;
				int num3 = 0;
				while (num2 < this.options.Count && num3 < num)
				{
					this.<DoWindowContents>g__RevalidateOption|6_0(this.options[num2], ref CS$<>8__locals1);
					this.nextOptionToRevalidate++;
					num2++;
					num3++;
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000BCA08 File Offset: 0x000BAC08
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Pawn forPawn)
		{
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateClickTarget.Spawned)
				{
					return false;
				}
				Vector3 key = opt.revalidateClickTarget.Position.ToVector3Shifted();
				List<FloatMenuOption> list;
				if (!FloatMenuMap.cachedChoices.TryGetValue(key, out list))
				{
					List<FloatMenuOption> list2 = FloatMenuMakerMap.ChoicesAtFor(key, forPawn, false);
					FloatMenuMap.cachedChoices.Add(key, list2);
					list = list2;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000BCAC8 File Offset: 0x000BACC8
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (!opt.Disabled && (pawn == null || !FloatMenuMap.StillValid(opt, FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn, false), pawn)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000BCB14 File Offset: 0x000BAD14
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000BCB38 File Offset: 0x000BAD38
		[CompilerGenerated]
		private void <DoWindowContents>g__RevalidateOption|6_0(FloatMenuOption option, ref FloatMenuMap.<>c__DisplayClass6_0 A_2)
		{
			if (!option.Disabled && !FloatMenuMap.StillValid(option, this.lastOptionsForRevalidation, A_2.selPawn))
			{
				option.Disabled = true;
			}
		}

		// Token: 0x0400122A RID: 4650
		private Vector3 clickPos;

		// Token: 0x0400122B RID: 4651
		private static Dictionary<Vector3, List<FloatMenuOption>> cachedChoices = new Dictionary<Vector3, List<FloatMenuOption>>();

		// Token: 0x0400122C RID: 4652
		private List<FloatMenuOption> lastOptionsForRevalidation;

		// Token: 0x0400122D RID: 4653
		private int nextOptionToRevalidate;

		// Token: 0x0400122E RID: 4654
		public const int RevalidateEveryFrame = 4;
	}
}
