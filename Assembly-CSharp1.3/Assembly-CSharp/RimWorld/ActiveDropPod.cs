using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010CB RID: 4299
	public class ActiveDropPod : Thing, IActiveDropPod, IThingHolder
	{
		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x060066D8 RID: 26328 RVA: 0x0022BD25 File Offset: 0x00229F25
		// (set) Token: 0x060066D9 RID: 26329 RVA: 0x0022BD2D File Offset: 0x00229F2D
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

		// Token: 0x060066DA RID: 26330 RVA: 0x0022BD54 File Offset: 0x00229F54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveDropPodInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x060066DB RID: 26331 RVA: 0x00002688 File Offset: 0x00000888
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x0022BD88 File Offset: 0x00229F88
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x0022BDAC File Offset: 0x00229FAC
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

		// Token: 0x060066DE RID: 26334 RVA: 0x0022BE04 File Offset: 0x0022A004
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

		// Token: 0x060066DF RID: 26335 RVA: 0x0022BE6C File Offset: 0x0022A06C
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

		// Token: 0x04003A13 RID: 14867
		public int age;

		// Token: 0x04003A14 RID: 14868
		private ActiveDropPodInfo contents;
	}
}
