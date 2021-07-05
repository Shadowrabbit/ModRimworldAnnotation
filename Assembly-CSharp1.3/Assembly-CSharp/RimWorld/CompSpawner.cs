using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119F RID: 4511
	public class CompSpawner : ThingComp
	{
		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06006C9A RID: 27802 RVA: 0x00247551 File Offset: 0x00245751
		public CompProperties_Spawner PropsSpawner
		{
			get
			{
				return (CompProperties_Spawner)this.props;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06006C9B RID: 27803 RVA: 0x00247560 File Offset: 0x00245760
		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x00247584 File Offset: 0x00245784
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x06006C9D RID: 27805 RVA: 0x0024758F File Offset: 0x0024578F
		public override void CompTick()
		{
			this.TickInterval(1);
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x00247598 File Offset: 0x00245798
		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x002475A8 File Offset: 0x002457A8
		private void TickInterval(int interval)
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if (comp != null)
			{
				if (!comp.Awake)
				{
					return;
				}
			}
			else if (this.parent.Position.Fogged(this.parent.Map))
			{
				return;
			}
			if (this.PropsSpawner.requiresPower && !this.PowerOn)
			{
				return;
			}
			this.ticksUntilSpawn -= interval;
			this.CheckShouldSpawn();
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x00247623 File Offset: 0x00245823
		private void CheckShouldSpawn()
		{
			if (this.ticksUntilSpawn <= 0)
			{
				this.ResetCountdown();
				this.TryDoSpawn();
			}
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x0024763C File Offset: 0x0024583C
		public bool TryDoSpawn()
		{
			if (!this.parent.Spawned)
			{
				return false;
			}
			if (this.PropsSpawner.spawnMaxAdjacent >= 0)
			{
				int num = 0;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.parent.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.parent.Map))
					{
						List<Thing> thingList = c.GetThingList(this.parent.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def == this.PropsSpawner.thingToSpawn)
							{
								num += thingList[j].stackCount;
								if (num >= this.PropsSpawner.spawnMaxAdjacent)
								{
									return false;
								}
							}
						}
					}
				}
			}
			IntVec3 center;
			if (CompSpawner.TryFindSpawnCell(this.parent, this.PropsSpawner.thingToSpawn, this.PropsSpawner.spawnCount, out center))
			{
				Thing thing = ThingMaker.MakeThing(this.PropsSpawner.thingToSpawn, null);
				thing.stackCount = this.PropsSpawner.spawnCount;
				if (thing == null)
				{
					Log.Error("Could not spawn anything for " + this.parent);
				}
				if (this.PropsSpawner.inheritFaction && thing.Faction != this.parent.Faction)
				{
					thing.SetFaction(this.parent.Faction, null);
				}
				Thing t;
				GenPlace.TryPlaceThing(thing, center, this.parent.Map, ThingPlaceMode.Direct, out t, null, null, default(Rot4));
				if (this.PropsSpawner.spawnForbidden)
				{
					t.SetForbidden(true, true);
				}
				if (this.PropsSpawner.showMessageIfOwned && this.parent.Faction == Faction.OfPlayer)
				{
					Messages.Message("MessageCompSpawnerSpawnedItem".Translate(this.PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06006CA2 RID: 27810 RVA: 0x00247840 File Offset: 0x00245A40
		public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(parent).InRandomOrder(null))
			{
				if (intVec.Walkable(parent.Map))
				{
					Building edifice = intVec.GetEdifice(parent.Map);
					if (edifice == null || !thingToSpawn.IsEdifice())
					{
						Building_Door building_Door = edifice as Building_Door;
						if ((building_Door == null || building_Door.FreePassage) && (parent.def.passability == Traversability.Impassable || GenSight.LineOfSight(parent.Position, intVec, parent.Map, false, null, 0, 0)))
						{
							bool flag = false;
							List<Thing> thingList = intVec.GetThingList(parent.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Thing thing = thingList[i];
								if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								result = intVec;
								return true;
							}
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06006CA3 RID: 27811 RVA: 0x0024797C File Offset: 0x00245B7C
		private void ResetCountdown()
		{
			this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x00247994 File Offset: 0x00245B94
		public override void PostExposeData()
		{
			string str = this.PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (this.PropsSpawner.saveKeysPrefix + "_");
			Scribe_Values.Look<int>(ref this.ticksUntilSpawn, str + "ticksUntilSpawn", 0, false);
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x002479E4 File Offset: 0x00245BE4
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Spawn " + this.PropsSpawner.thingToSpawn.label,
					icon = TexCommand.DesirePower,
					action = delegate()
					{
						this.ResetCountdown();
						this.TryDoSpawn();
					}
				};
			}
			yield break;
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x002479F4 File Offset: 0x00245BF4
		public override string CompInspectStringExtra()
		{
			if (this.PropsSpawner.writeTimeLeftToSpawn && (!this.PropsSpawner.requiresPower || this.PowerOn))
			{
				return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(this.PropsSpawner.thingToSpawn, null, this.PropsSpawner.spawnCount)).Resolve() + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor);
			}
			return null;
		}

		// Token: 0x04003C68 RID: 15464
		private int ticksUntilSpawn;
	}
}
