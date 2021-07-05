using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200119C RID: 4508
	public class CompSpawnSubplant : ThingComp
	{
		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06006C84 RID: 27780 RVA: 0x00246D86 File Offset: 0x00244F86
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06006C85 RID: 27781 RVA: 0x00246D93 File Offset: 0x00244F93
		public List<Thing> SubplantsForReading
		{
			get
			{
				this.Cleanup();
				return this.subplants;
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06006C86 RID: 27782 RVA: 0x00246DA4 File Offset: 0x00244FA4
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

		// Token: 0x06006C87 RID: 27783 RVA: 0x00246E1C File Offset: 0x0024501C
		public void AddProgress(float progress, bool ignoreMultiplier = false)
		{
			if (!ModLister.CheckRoyalty("Subplant spawning"))
			{
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

		// Token: 0x06006C88 RID: 27784 RVA: 0x00246E5A File Offset: 0x0024505A
		public override void CompTickLong()
		{
			if (GenLocalDate.DayTick(this.parent.Map) < 2000)
			{
				this.meditationTicksToday = 0;
			}
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x00246E7A File Offset: 0x0024507A
		public void Cleanup()
		{
			this.subplants.RemoveAll((Thing p) => !p.Spawned);
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x00246EA8 File Offset: 0x002450A8
		public override string CompInspectStringExtra()
		{
			return "TotalMeditationToday".Translate((this.meditationTicksToday / 2500).ToString() + "LetterHour".Translate(), this.ProgressMultiplier.ToStringPercent()) + "\n" + this.Props.subplant.LabelCap + ": " + this.SubplantsForReading.Count + " (" + "ProgressToNextSubplant".Translate(this.progressToNextSubplant.ToStringPercent()) + ")";
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x00246F6D File Offset: 0x0024516D
		private void TryGrowSubplants()
		{
			while (this.progressToNextSubplant >= 1f)
			{
				this.DoGrowSubplant();
				this.progressToNextSubplant -= 1f;
			}
		}

		// Token: 0x06006C8C RID: 27788 RVA: 0x00246F98 File Offset: 0x00245198
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
					if (!flag && this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, true))
					{
						for (int j = thingList.Count - 1; j >= 0; j--)
						{
							if (thingList[j].def.category == ThingCategory.Plant)
							{
								thingList[j].Destroy(DestroyMode.Vanish);
							}
						}
						Plant plant = (Plant)GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish);
						this.subplants.Add(plant);
						if (this.Props.initialGrowthRange != null)
						{
							plant.Growth = this.Props.initialGrowthRange.Value.RandomInRange;
						}
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

		// Token: 0x06006C8D RID: 27789 RVA: 0x00247174 File Offset: 0x00245374
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
					this.AddProgress(1f, true);
				}
			};
			yield break;
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x00247184 File Offset: 0x00245384
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

		// Token: 0x04003C58 RID: 15448
		private float progressToNextSubplant;

		// Token: 0x04003C59 RID: 15449
		private List<Thing> subplants = new List<Thing>();

		// Token: 0x04003C5A RID: 15450
		private int meditationTicksToday;

		// Token: 0x04003C5B RID: 15451
		public Action onGrassGrown;

		// Token: 0x04003C5C RID: 15452
		private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>>
		{
			new Pair<int, float>(30000, 1f),
			new Pair<int, float>(60000, 0.5f),
			new Pair<int, float>(120000, 0.25f),
			new Pair<int, float>(240000, 0.15f)
		};
	}
}
