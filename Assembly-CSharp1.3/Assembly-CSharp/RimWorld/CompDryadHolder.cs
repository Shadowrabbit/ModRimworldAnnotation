using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001124 RID: 4388
	public abstract class CompDryadHolder : ThingComp, IThingHolder
	{
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x0600695A RID: 26970 RVA: 0x00238423 File Offset: 0x00236623
		public CompProperties_DryadCocoon Props
		{
			get
			{
				return (CompProperties_DryadCocoon)this.props;
			}
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x00238430 File Offset: 0x00236630
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
			}
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x00238444 File Offset: 0x00236644
		public override void CompTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.tickComplete >= 0)
			{
				if (this.tree == null || this.tree.Destroyed)
				{
					this.parent.Destroy(DestroyMode.Vanish);
					return;
				}
				if (Find.TickManager.TicksGame >= this.tickComplete)
				{
					this.Complete();
				}
			}
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x002384A0 File Offset: 0x002366A0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.tickComplete >= 0 && Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Complete",
					action = delegate()
					{
						this.Complete();
					}
				};
			}
			yield break;
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x002384B0 File Offset: 0x002366B0
		public virtual void TryAcceptPawn(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			this.innerContainer.TryAddOrTransfer(p, 1, true);
			SoundDefOf.Pawn_EnterDryadPod.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			if (p.connections != null)
			{
				foreach (Thing thing in p.connections.ConnectedThings)
				{
					if (thing.TryGetComp<CompTreeConnection>() != null)
					{
						this.tree = thing;
						break;
					}
				}
			}
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x0023854C File Offset: 0x0023674C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
			if (this.tickComplete >= 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					"TimeLeft".Translate().CapitalizeFirst(),
					": ",
					(this.tickComplete - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor)
				});
			}
			return text;
		}

		// Token: 0x06006960 RID: 26976
		protected abstract void Complete();

		// Token: 0x06006961 RID: 26977 RVA: 0x00238614 File Offset: 0x00236814
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x0023861C File Offset: 0x0023681C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x0023862C File Offset: 0x0023682C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.tickComplete, "tickComplete", -1, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.tree, "tree", false);
		}

		// Token: 0x04003AF0 RID: 15088
		protected int tickComplete = -1;

		// Token: 0x04003AF1 RID: 15089
		protected ThingOwner innerContainer;

		// Token: 0x04003AF2 RID: 15090
		protected Thing tree;
	}
}
