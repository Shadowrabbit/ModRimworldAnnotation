using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000183 RID: 387
	public sealed class AutoSlaughterManager : IExposable
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0003BD74 File Offset: 0x00039F74
		public List<Pawn> AnimalsToSlaughter
		{
			get
			{
				if (this.cacheDirty)
				{
					try
					{
						this.animalsToSlaughterCached.Clear();
						foreach (AutoSlaughterConfig autoSlaughterConfig in this.configs)
						{
							if (autoSlaughterConfig.AnyLimit)
							{
								AutoSlaughterManager.tmpAnimals.Clear();
								AutoSlaughterManager.tmpAnimalsMale.Clear();
								AutoSlaughterManager.tmpAnimalsFemale.Clear();
								foreach (Pawn pawn in this.map.mapPawns.SpawnedColonyAnimals)
								{
									if (pawn.def == autoSlaughterConfig.animal && AutoSlaughterManager.CanEverAutoSlaughter(pawn))
									{
										AutoSlaughterManager.tmpAnimals.Add(pawn);
										if (pawn.gender == Gender.Male)
										{
											AutoSlaughterManager.tmpAnimalsMale.Add(pawn);
										}
										if (pawn.gender == Gender.Female)
										{
											AutoSlaughterManager.tmpAnimalsFemale.Add(pawn);
										}
									}
								}
								AutoSlaughterManager.tmpAnimals.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsMale.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsFemale.SortBy((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								if (autoSlaughterConfig.maxFemales != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsFemale.Count > autoSlaughterConfig.maxFemales)
									{
										Pawn item = AutoSlaughterManager.tmpAnimalsFemale.Pop<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item);
										this.animalsToSlaughterCached.Add(item);
									}
								}
								if (autoSlaughterConfig.maxMales != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsMale.Count > autoSlaughterConfig.maxMales)
									{
										Pawn item2 = AutoSlaughterManager.tmpAnimalsMale.Pop<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item2);
										this.animalsToSlaughterCached.Add(item2);
									}
								}
								if (autoSlaughterConfig.maxTotal != -1)
								{
									while (AutoSlaughterManager.tmpAnimals.Count > autoSlaughterConfig.maxTotal)
									{
										Pawn pawn2 = AutoSlaughterManager.tmpAnimals.Pop<Pawn>();
										if (pawn2.gender == Gender.Male)
										{
											AutoSlaughterManager.tmpAnimalsMale.Remove(pawn2);
										}
										if (pawn2.gender == Gender.Female)
										{
											AutoSlaughterManager.tmpAnimalsFemale.Remove(pawn2);
										}
										this.animalsToSlaughterCached.Add(pawn2);
									}
								}
							}
						}
						this.cacheDirty = false;
					}
					finally
					{
						AutoSlaughterManager.tmpAnimals.Clear();
						AutoSlaughterManager.tmpAnimalsMale.Clear();
						AutoSlaughterManager.tmpAnimalsFemale.Clear();
					}
				}
				return this.animalsToSlaughterCached;
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0003C058 File Offset: 0x0003A258
		public static bool CanEverAutoSlaughter(Pawn animal)
		{
			return animal.ageTracker.CurLifeStage.reproductive && animal.HomeFaction == Faction.OfPlayer && !animal.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0003C094 File Offset: 0x0003A294
		public AutoSlaughterManager(Map map)
		{
			this.map = map;
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.race != null && thingDef.race.Animal && thingDef.race.wildness < 1f)
				{
					this.configs.Add(new AutoSlaughterConfig
					{
						animal = thingDef
					});
				}
			}
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0003C13C File Offset: 0x0003A33C
		public void Notify_PawnDespawned()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0003C13C File Offset: 0x0003A33C
		public void Notify_PawnSpawned()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003C13C File Offset: 0x0003A33C
		public void Notify_PawnChangedFaction()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0003C13C File Offset: 0x0003A33C
		public void Notify_ConfigChanged()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0003C145 File Offset: 0x0003A345
		public void ExposeData()
		{
			Scribe_Collections.Look<AutoSlaughterConfig>(ref this.configs, "configs", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04000933 RID: 2355
		private static List<Pawn> tmpAnimals = new List<Pawn>();

		// Token: 0x04000934 RID: 2356
		private static List<Pawn> tmpAnimalsMale = new List<Pawn>();

		// Token: 0x04000935 RID: 2357
		private static List<Pawn> tmpAnimalsFemale = new List<Pawn>();

		// Token: 0x04000936 RID: 2358
		public Map map;

		// Token: 0x04000937 RID: 2359
		public List<AutoSlaughterConfig> configs = new List<AutoSlaughterConfig>();

		// Token: 0x04000938 RID: 2360
		private List<Pawn> animalsToSlaughterCached = new List<Pawn>();

		// Token: 0x04000939 RID: 2361
		private bool cacheDirty;
	}
}
