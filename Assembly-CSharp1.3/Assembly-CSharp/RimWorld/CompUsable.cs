using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011CA RID: 4554
	public class CompUsable : ThingComp
	{
		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06006DEF RID: 28143 RVA: 0x0024DB40 File Offset: 0x0024BD40
		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		// Token: 0x06006DF0 RID: 28144 RVA: 0x0024DB4D File Offset: 0x0024BD4D
		protected virtual string FloatMenuOptionLabel(Pawn pawn)
		{
			return this.Props.useLabel;
		}

		// Token: 0x06006DF1 RID: 28145 RVA: 0x0024DB5A File Offset: 0x0024BD5A
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			string text;
			if (!this.CanBeUsedBy(myPawn, out text))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + ((text != null) ? (" (" + text + ")") : ""), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (!myPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (!myPawn.CanReserve(this.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption(this.FloatMenuOptionLabel(myPawn), delegate()
				{
					if (myPawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						using (IEnumerator<CompUseEffect> enumerator = this.parent.GetComps<CompUseEffect>().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.SelectedUseOption(myPawn))
								{
									return;
								}
							}
						}
						this.TryStartUseJob(myPawn, LocalTargetInfo.Invalid);
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield return floatMenuOption;
			}
			yield break;
		}

		// Token: 0x06006DF2 RID: 28146 RVA: 0x0024DB74 File Offset: 0x0024BD74
		public virtual void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget)
		{
			CompUsable.<>c__DisplayClass4_0 CS$<>8__locals1 = new CompUsable.<>c__DisplayClass4_0();
			CS$<>8__locals1.extraTarget = extraTarget;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			if (!CS$<>8__locals1.pawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return;
			}
			string text;
			if (!this.CanBeUsedBy(CS$<>8__locals1.pawn, out text))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
			{
				TaggedString taggedString = compUseEffect.ConfirmMessage(CS$<>8__locals1.pawn);
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendTagged(taggedString);
				}
			}
			string str = stringBuilder.ToString();
			if (str.NullOrEmpty())
			{
				CS$<>8__locals1.<TryStartUseJob>g__StartJob|0();
				return;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, new Action(CS$<>8__locals1.<TryStartUseJob>g__StartJob|0), false, null));
		}

		// Token: 0x06006DF3 RID: 28147 RVA: 0x0024DC7C File Offset: 0x0024BE7C
		public void UsedBy(Pawn p)
		{
			string text;
			if (!this.CanBeUsedBy(p, out text))
			{
				return;
			}
			foreach (CompUseEffect compUseEffect in from x in this.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					compUseEffect.DoEffect(p);
				}
				catch (Exception arg)
				{
					Log.Error("Error in CompUseEffect: " + arg);
				}
			}
		}

		// Token: 0x06006DF4 RID: 28148 RVA: 0x0024DD20 File Offset: 0x0024BF20
		private bool CanBeUsedBy(Pawn p, out string failReason)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompUseEffect compUseEffect = allComps[i] as CompUseEffect;
				if (compUseEffect != null && !compUseEffect.CanBeUsedBy(p, out failReason))
				{
					return false;
				}
			}
			failReason = null;
			return true;
		}
	}
}
