using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200188D RID: 6285
	public class CompUsable : ThingComp
	{
		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x06008B91 RID: 35729 RVA: 0x0005D934 File Offset: 0x0005BB34
		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		// Token: 0x06008B92 RID: 35730 RVA: 0x0005D941 File Offset: 0x0005BB41
		protected virtual string FloatMenuOptionLabel(Pawn pawn)
		{
			return this.Props.useLabel;
		}

		// Token: 0x06008B93 RID: 35731 RVA: 0x0005D94E File Offset: 0x0005BB4E
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			string text;
			if (!this.CanBeUsedBy(myPawn, out text))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + ((text != null) ? (" (" + text + ")") : ""), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReserve(this.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
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
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return floatMenuOption;
			}
			yield break;
		}

		// Token: 0x06008B94 RID: 35732 RVA: 0x0028A4FC File Offset: 0x002886FC
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

		// Token: 0x06008B95 RID: 35733 RVA: 0x0028A604 File Offset: 0x00288804
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
					Log.Error("Error in CompUseEffect: " + arg, false);
				}
			}
		}

		// Token: 0x06008B96 RID: 35734 RVA: 0x0028A6A8 File Offset: 0x002888A8
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
