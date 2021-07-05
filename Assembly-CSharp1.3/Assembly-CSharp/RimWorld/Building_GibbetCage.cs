using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200107A RID: 4218
	public class Building_GibbetCage : Building_CorpseCasket, IObservedThoughtGiver
	{
		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06006450 RID: 25680 RVA: 0x0021D45F File Offset: 0x0021B65F
		private float RandomCorpseRotation
		{
			get
			{
				return Rand.Range(-45f, 45f);
			}
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x0021D470 File Offset: 0x0021B670
		public Building_GibbetCage()
		{
			if (this.corpseRotation == 0f)
			{
				this.corpseRotation = Rand.Range(-45f, 45f);
			}
		}

		// Token: 0x06006452 RID: 25682 RVA: 0x0021D49A File Offset: 0x0021B69A
		public override bool Accepts(Thing thing)
		{
			return base.Accepts(thing) && !base.HasCorpse && this.storageSettings.AllowedToAccept(thing);
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x0021D4C4 File Offset: 0x0021B6C4
		public void Notify_CorpseAdded()
		{
			Building_GibbetCage.tmpRadialPositions.Clear();
			this.corpseRotation = this.RandomCorpseRotation;
			if (base.Corpse.GetRotStage() != RotStage.Dessicated)
			{
				int num = GenRadial.NumCellsInRadius(1.5f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 item = base.Position + GenRadial.RadialPattern[i];
					Building_GibbetCage.tmpRadialPositions.Add(item);
				}
				int num2 = Mathf.Min(Building_GibbetCage.tmpRadialPositions.Count, Rand.Range(2, 4));
				for (int j = 0; j < num2; j++)
				{
					IntVec3 intVec = Building_GibbetCage.tmpRadialPositions.RandomElement<IntVec3>();
					Building_GibbetCage.tmpRadialPositions.Remove(intVec);
					FilthMaker.TryMakeFilth(intVec, base.Map, ThingDefOf.Filth_Blood, 1, FilthSourceFlags.None);
				}
			}
			if (this.def.building.gibbetCagePlaceCorpseEffecter != null)
			{
				this.def.building.gibbetCagePlaceCorpseEffecter.Spawn(this, base.Map, 1f);
			}
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x0021D5BA File Offset: 0x0021B7BA
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.HasCorpse)
			{
				this.EjectContents();
			}
			base.DeSpawn(mode);
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x0021D5D1 File Offset: 0x0021B7D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.corpseRotation, "corpseRotation", 0f, false);
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x0021D5F0 File Offset: 0x0021B7F0
		public static Building_GibbetCage FindGibbetCageFor(Corpse c, Pawn traveler, bool ignoreOtherReservations = false)
		{
			Predicate<Thing> <>9__0;
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.IsGibbetCage)
				{
					IntVec3 position = c.Position;
					Map map = c.Map;
					ThingRequest thingReq = ThingRequest.ForDef(thingDef);
					PathEndMode peMode = PathEndMode.InteractionCell;
					TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
					float maxDistance = 9999f;
					Predicate<Thing> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((Thing x) => !((Building_GibbetCage)x).HasCorpse && ((Building_GibbetCage)x).Accepts(x) && traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations)));
					}
					Building_GibbetCage building_GibbetCage = (Building_GibbetCage)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false);
					if (building_GibbetCage != null)
					{
						return building_GibbetCage;
					}
				}
			}
			return null;
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x0021D6B8 File Offset: 0x0021B8B8
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if (observer.Ideo != null && observer.Ideo.IdeoApprovesOfSlavery())
			{
				return null;
			}
			Thought_MemoryObservation thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedGibbetCage);
			thought_MemoryObservation.Target = this;
			return thought_MemoryObservation;
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x00002688 File Offset: 0x00000888
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x0021D6E8 File Offset: 0x0021B8E8
		public override void Draw()
		{
			base.Draw();
			if (base.HasCorpse)
			{
				base.Corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(this.corpseRotation);
				base.Corpse.DrawAt(base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop) + this.def.building.gibbetCorposeDrawOffset, false);
			}
			if (this.cageTopGraphic == null)
			{
				this.cageTopGraphic = this.def.building.gibbetCageTopGraphicData.GraphicColoredFor(this);
			}
			this.cageTopGraphic.Draw(base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Item), Rot4.North, this, 0f);
		}

		// Token: 0x0400389C RID: 14492
		[Unsaved(false)]
		private Graphic cageTopGraphic;

		// Token: 0x0400389D RID: 14493
		private float corpseRotation;

		// Token: 0x0400389E RID: 14494
		private static List<IntVec3> tmpRadialPositions = new List<IntVec3>();
	}
}
