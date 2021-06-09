using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001865 RID: 6245
	public class CompSpawnSubplant : ThingComp
	{
		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06008A87 RID: 35463 RVA: 0x0005CE76 File Offset: 0x0005B076
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06008A88 RID: 35464 RVA: 0x0005CE83 File Offset: 0x0005B083
		public List<Thing> SubplantsForReading
		{
			get
			{
				this.Cleanup();
				return this.subplants;
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x06008A89 RID: 35465 RVA: 0x00287328 File Offset: 0x00285528
		private float ProgressMultiplier
		{
			get
			{
				foreach (Pair<int, float> pair in CompSpawnSubplant.TicksToProgressMultipliers)
				{
					if (this.meditationTicksToday < pair.First)
					{
						return pair.Second;
					}
				}
				return CompSpawnSubplant.TicksToProgressMultipliers.Last<Pair<int, float>>().Second;
			}
		}

		// Token: 0x06008A8A RID: 35466 RVA: 0x0005CE91 File Offset: 0x0005B091
		[Obsolete("Only used for mod compatibility.")]
		public void AddProgress(float progress)
		{
			this.AddProgress_NewTmp(progress, false);
		}

		// Token: 0x06008A8B RID: 35467 RVA: 0x002873A0 File Offset: 0x002855A0
		public void AddProgress_NewTmp(float progress, bool ignoreMultiplier = false)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Subplant spawners are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 43254, false);
				return;
			}
			if (!ignoreMultiplier)
			{
				progress *= this.ProgressMultiplier;
			}
			this.progressToNextSubplant += progress;
			this.meditationTicksToday++;
			this.TryGrowSubplants();
		}

		// Token: 0x06008A8C RID: 35468 RVA: 0x0005CE9B File Offset: 0x0005B09B
		public override void CompTickLong()
		{
			if (GenLocalDate.DayTick(this.parent.Map) < 2000)
			{
				this.meditationTicksToday = 0;
			}
		}

		// Token: 0x06008A8D RID: 35469 RVA: 0x0005CEBB File Offset: 0x0005B0BB
		public void Cleanup()
		{
			this.subplants.RemoveAll((Thing p) => !p.Spawned);
		}

		// Token: 0x06008A8E RID: 35470 RVA: 0x002873F4 File Offset: 0x002855F4
		public override string CompInspectStringExtra()
		{
			return "TotalMeditationToday".Translate((this.meditationTicksToday / 2500).ToString() + "LetterHour".Translate(), this.ProgressMultiplier.ToStringPercent()) + "\n" + this.Props.subplant.LabelCap + ": " + this.SubplantsForReading.Count + " (" + "ProgressToNextSubplant".Translate(this.progressToNextSubplant.ToStringPercent()) + ")";
		}

		// Token: 0x06008A8F RID: 35471 RVA: 0x0005CEE8 File Offset: 0x0005B0E8
		private void TryGrowSubplants()
		{
			while (this.progressToNextSubplant >= 1f)
			{
				this.DoGrowSubplant();
				this.progressToNextSubplant -= 1f;
			}
		}

		// Token: 0x06008A90 RID: 35472 RVA: 0x002874BC File Offset: 0x002856BC
		private void DoGrowSubplant()
		{
			IntVec3 position = this.parent.Position;
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && WanderUtility.InSameRoom(position, intVec, this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					using (List<Thing>.Enumerator enumerator = thingList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def == this.Props.subplant)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag && this.Props.subplant.CanEverPlantAt_NewTemp(intVec, this.parent.Map, true))
					{
						for (int j = thingList.Count - 1; j >= 0; j--)
						{
							if (thingList[j].def.category == ThingCategory.Plant)
							{
								thingList[j].Destroy(DestroyMode.Vanish);
							}
						}
						this.subplants.Add(GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish));
						if (this.Props.spawnSound != null)
						{
							this.Props.spawnSound.PlayOneShot(new TargetInfo(this.parent));
						}
						Action action = this.onGrassGrown;
						if (action == null)
						{
							return;
						}
						action();
						return;
					}
				}
			}
		}

		// Token: 0x06008A91 RID: 35473 RVA: 0x0005CF11 File Offset: 0x0005B111
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Add 100% progress",
				action = delegate()
				{
					this.AddProgress_NewTmp(1f, true);
				}
			};
			yield break;
		}

		// Token: 0x06008A92 RID: 35474 RVA: 0x0028765C File Offset: 0x0028585C
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.progressToNextSubplant, "progressToNextSubplant", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.subplants, "subplants", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meditationTicksToday, "meditationTicksToday", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.subplants.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040058E5 RID: 22757
		private float progressToNextSubplant;

		// Token: 0x040058E6 RID: 22758
		private List<Thing> subplants = new List<Thing>();

		// Token: 0x040058E7 RID: 22759
		private int meditationTicksToday;

		// Token: 0x040058E8 RID: 22760
		public Action onGrassGrown;

		// Token: 0x040058E9 RID: 22761
		private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>>
		{
			new Pair<int, float>(30000, 1f),
			new Pair<int, float>(60000, 0.5f),
			new Pair<int, float>(120000, 0.25f),
			new Pair<int, float>(240000, 0.15f)
		};
	}
}
