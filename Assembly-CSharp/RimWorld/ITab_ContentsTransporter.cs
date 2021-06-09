using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B01 RID: 6913
	public class ITab_ContentsTransporter : ITab_ContentsBase
	{
		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x0600982C RID: 38956 RVA: 0x0006565C File Offset: 0x0006385C
		public override IList<Thing> container
		{
			get
			{
				return this.Transporter.innerContainer;
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x0600982D RID: 38957 RVA: 0x00065669 File Offset: 0x00063869
		public CompTransporter Transporter
		{
			get
			{
				return base.SelThing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600982E RID: 38958 RVA: 0x002CAB7C File Offset: 0x002C8D7C
		public override bool IsVisible
		{
			get
			{
				return (base.SelThing.Faction == null || base.SelThing.Faction == Faction.OfPlayer) && this.Transporter != null && (this.Transporter.LoadingInProgressOrReadyToLaunch || this.Transporter.innerContainer.Any);
			}
		}

		// Token: 0x0600982F RID: 38959 RVA: 0x00065676 File Offset: 0x00063876
		public ITab_ContentsTransporter()
		{
			this.labelKey = "TabTransporterContents";
			this.containedItemsKey = "ContainedItems";
		}

		// Token: 0x06009830 RID: 38960 RVA: 0x002CABD4 File Offset: 0x002C8DD4
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

		// Token: 0x06009831 RID: 38961 RVA: 0x002CAD5C File Offset: 0x002C8F5C
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
		}

		// Token: 0x06009832 RID: 38962 RVA: 0x002CAD84 File Offset: 0x002C8F84
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x06009833 RID: 38963 RVA: 0x002CADBC File Offset: 0x002C8FBC
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

		// Token: 0x06009834 RID: 38964 RVA: 0x002CAE2C File Offset: 0x002C902C
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
