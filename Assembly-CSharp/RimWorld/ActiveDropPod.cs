using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200172E RID: 5934
	public class ActiveDropPod : Thing, IActiveDropPod, IThingHolder
	{
		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x060082E0 RID: 33504 RVA: 0x00057D95 File Offset: 0x00055F95
		// (set) Token: 0x060082E1 RID: 33505 RVA: 0x00057D9D File Offset: 0x00055F9D
		public ActiveDropPodInfo Contents
		{
			get
			{
				return this.contents;
			}
			set
			{
				if (this.contents != null)
				{
					this.contents.parent = null;
				}
				if (value != null)
				{
					value.parent = this;
				}
				this.contents = value;
			}
		}

		// Token: 0x060082E2 RID: 33506 RVA: 0x00057DC4 File Offset: 0x00055FC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveDropPodInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x060082E3 RID: 33507 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x060082E4 RID: 33508 RVA: 0x00057DF8 File Offset: 0x00055FF8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x060082E5 RID: 33509 RVA: 0x0026CD7C File Offset: 0x0026AF7C
		public override void Tick()
		{
			if (this.contents == null)
			{
				return;
			}
			this.contents.innerContainer.ThingOwnerTick(true);
			if (base.Spawned)
			{
				this.age++;
				if (this.age > this.contents.openDelay)
				{
					this.PodOpen();
				}
			}
		}

		// Token: 0x060082E6 RID: 33510 RVA: 0x0026CDD4 File Offset: 0x0026AFD4
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				for (int i = 0; i < 1; i++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		// Token: 0x060082E7 RID: 33511 RVA: 0x0026CE3C File Offset: 0x0026B03C
		private void PodOpen()
		{
			Map map = base.Map;
			if (this.contents.despawnPodBeforeSpawningThing)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			for (int i = this.contents.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.contents.innerContainer[i];
				Rot4 rot = (this.contents.setRotation != null) ? this.contents.setRotation.Value : Rot4.North;
				if (this.contents.moveItemsAsideBeforeSpawning)
				{
					GenSpawn.CheckMoveItemsAside(base.Position, rot, thing.def, map);
				}
				Thing thing2;
				if (this.contents.spawnWipeMode == null)
				{
					GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, out thing2, delegate(Thing placedThing, int count)
					{
						if (Find.TickManager.TicksGame < 1200 && TutorSystem.TutorialMode && placedThing.def.category == ThingCategory.Item)
						{
							Find.TutorialState.AddStartingItem(placedThing);
						}
					}, null, rot);
				}
				else if (this.contents.setRotation != null)
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.setRotation.Value, this.contents.spawnWipeMode.Value, false);
				}
				else
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.spawnWipeMode.Value);
				}
				Pawn pawn = thing2 as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						TaleRecorder.RecordTale(TaleDefOf.LandedInPod, new object[]
						{
							pawn
						});
					}
					if (pawn.IsColonist && pawn.Spawned && !map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
					if (pawn.guest != null && pawn.guest.IsPrisoner)
					{
						pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
				}
			}
			this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.contents.leaveSlag)
			{
				for (int j = 0; j < 1; j++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
			SoundDefOf.DropPod_Open.PlayOneShot(new TargetInfo(base.Position, map, false));
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x040054D9 RID: 21721
		public int age;

		// Token: 0x040054DA RID: 21722
		private ActiveDropPodInfo contents;
	}
}
