using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001341 RID: 4929
	public class ITab_ContentsTransporter : ITab_ContentsBase
	{
		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x0600774F RID: 30543 RVA: 0x0029E50E File Offset: 0x0029C70E
		public override IList<Thing> container
		{
			get
			{
				return this.Transporter.innerContainer;
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x06007750 RID: 30544 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool UseDiscardMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06007751 RID: 30545 RVA: 0x0029E51B File Offset: 0x0029C71B
		public CompTransporter Transporter
		{
			get
			{
				return base.SelThing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06007752 RID: 30546 RVA: 0x0029E528 File Offset: 0x0029C728
		public override bool IsVisible
		{
			get
			{
				return (base.SelThing.Faction == null || base.SelThing.Faction == Faction.OfPlayer) && this.Transporter != null && (this.Transporter.LoadingInProgressOrReadyToLaunch || this.Transporter.innerContainer.Any);
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06007753 RID: 30547 RVA: 0x0029E57D File Offset: 0x0029C77D
		public override IntVec3 DropOffset
		{
			get
			{
				if (this.Transporter.Shuttle != null)
				{
					return ShipJob_Unload.DropoffSpotOffset;
				}
				return base.DropOffset;
			}
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x0029E598 File Offset: 0x0029C798
		public ITab_ContentsTransporter()
		{
			this.labelKey = "TabTransporterContents";
			this.containedItemsKey = "ContainedItems";
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x0029E5B8 File Offset: 0x0029C7B8
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			CompTransporter transporter = this.Transporter;
			Rect position = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			Text.Font = GameFont.Small;
			bool flag = false;
			float a = 0f;
			GUI.BeginGroup(position);
			Widgets.ListSeparator(ref a, position.width, "ItemsToLoad".Translate());
			if (transporter.leftToLoad != null)
			{
				for (int i = 0; i < transporter.leftToLoad.Count; i++)
				{
					TransferableOneWay t = transporter.leftToLoad[i];
					if (t.CountToTransfer > 0 && t.HasAnyThing)
					{
						flag = true;
						base.DoThingRow(t.ThingDef, t.CountToTransfer, t.things, position.width, ref a, delegate(int x)
						{
							this.OnDropToLoadThing(t, x);
						});
					}
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, position.width, null);
			}
			GUI.EndGroup();
			Rect inRect2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			base.DoItemsLists(inRect2, ref b);
			curY += Mathf.Max(a, b);
		}

		// Token: 0x06007756 RID: 30550 RVA: 0x0029E740 File Offset: 0x0029C940
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn;
			if ((pawn = (t as Pawn)) != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
		}

		// Token: 0x06007757 RID: 30551 RVA: 0x0029E768 File Offset: 0x0029C968
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x0029E7A0 File Offset: 0x0029C9A0
		private void OnDropToLoadThing(TransferableOneWay t, int count)
		{
			t.ForceTo(t.CountToTransfer - count);
			this.EndJobForEveryoneHauling(t);
			foreach (Thing thing in t.things)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					this.RemovePawnFromLoadLord(pawn);
				}
			}
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x0029E810 File Offset: 0x0029CA10
		private void EndJobForEveryoneHauling(TransferableOneWay t)
		{
			List<Pawn> allPawnsSpawned = base.SelThing.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
				{
					JobDriver_HaulToTransporter jobDriver_HaulToTransporter = (JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver;
					if (jobDriver_HaulToTransporter.Transporter == this.Transporter && jobDriver_HaulToTransporter.ThingToCarry != null && jobDriver_HaulToTransporter.ThingToCarry.def == t.ThingDef)
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}
	}
}
