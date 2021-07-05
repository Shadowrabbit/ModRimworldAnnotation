using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006FD RID: 1789
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06002D62 RID: 11618 RVA: 0x00023CCC File Offset: 0x00021ECC
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x00133644 File Offset: 0x00131844
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
				this.lastOptionsForRevalidation = FloatMenuMakerMap.ChoicesAtFor(this.clickPos, CS$<>8__locals1.selPawn);
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

		// Token: 0x06002D64 RID: 11620 RVA: 0x00023CDE File Offset: 0x00021EDE
		[Obsolete("Only need this overload to not break mod compatibility.")]
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Pawn forPawn, ref List<FloatMenuOption> cachedChoices, ref Vector3 cachedChoicesForPos)
		{
			return FloatMenuMap.StillValid(opt, curOpts, forPawn);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x001337A4 File Offset: 0x001319A4
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
					List<FloatMenuOption> list2 = FloatMenuMakerMap.ChoicesAtFor(key, forPawn);
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

		// Token: 0x06002D66 RID: 11622 RVA: 0x00133864 File Offset: 0x00131A64
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (!opt.Disabled && (pawn == null || !FloatMenuMap.StillValid(opt, FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn), pawn)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x00023CE8 File Offset: 0x00021EE8
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x00023D0C File Offset: 0x00021F0C
		[CompilerGenerated]
		private void <DoWindowContents>g__RevalidateOption|6_0(FloatMenuOption option, ref FloatMenuMap.<>c__DisplayClass6_0 A_2)
		{
			if (!option.Disabled && !FloatMenuMap.StillValid(option, this.lastOptionsForRevalidation, A_2.selPawn))
			{
				option.Disabled = true;
			}
		}

		// Token: 0x04001ECD RID: 7885
		private Vector3 clickPos;

		// Token: 0x04001ECE RID: 7886
		private static Dictionary<Vector3, List<FloatMenuOption>> cachedChoices = new Dictionary<Vector3, List<FloatMenuOption>>();

		// Token: 0x04001ECF RID: 7887
		private List<FloatMenuOption> lastOptionsForRevalidation;

		// Token: 0x04001ED0 RID: 7888
		private int nextOptionToRevalidate;

		// Token: 0x04001ED1 RID: 7889
		public const int RevalidateEveryFrame = 4;
	}
}
