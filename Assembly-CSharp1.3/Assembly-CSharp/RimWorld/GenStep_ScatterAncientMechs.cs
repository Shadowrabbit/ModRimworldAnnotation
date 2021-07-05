using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C97 RID: 3223
	public class GenStep_ScatterAncientMechs : GenStep_Scatterer
	{
		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004B3A RID: 19258 RVA: 0x0018F885 File Offset: 0x0018DA85
		private static IEnumerable<ThingDef> MechThingDefs
		{
			get
			{
				yield return ThingDefOf.AncientWarwalkerTorso;
				yield return ThingDefOf.AncientMiniWarwalkerRemains;
				yield return ThingDefOf.AncientWarspiderRemains;
				yield break;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004B3B RID: 19259 RVA: 0x0018F88E File Offset: 0x0018DA8E
		public override int SeedPart
		{
			get
			{
				return 1034745625;
			}
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x0018F895 File Offset: 0x0018DA95
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient outdoor building"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.thingToPlace = GenStep_ScatterAncientMechs.MechThingDefs.RandomElement<ThingDef>();
			base.Generate(map, parms);
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x0018F8CC File Offset: 0x0018DACC
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			int num = Rand.RangeInclusive(1, 4);
			for (int i = 0; i < 4; i++)
			{
				this.rotation = new Rot4((i + num) % 4);
				if (this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x0018F922 File Offset: 0x0018DB22
		private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x0018F930 File Offset: 0x0018DB30
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToPlace, null), loc, map, this.rotation, WipeMode.Vanish, false);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.2f, 0, int.MaxValue, null);
			if (thing.def == ThingDefOf.AncientWarwalkerTorso)
			{
				if (Rand.Chance(0.75f))
				{
					int num = ThingDefOf.AncientWarwalkerTorso.size.z / 2 + ThingDefOf.AncientWarwalkerLeg.size.z / 2 + Rand.Range(1, 3);
					IntVec3 intVec = thing.Position + new IntVec3(Rand.Range(-3, 3), 0, -num).RotatedBy(this.rotation);
					if (this.CanPlaceThingAt(intVec, this.rotation, map, ThingDefOf.AncientWarwalkerLeg))
					{
						ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientWarwalkerLeg, null), intVec, map, this.rotation, WipeMode.Vanish, false), map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
					}
				}
				if (Rand.Chance(0.75f))
				{
					int newX = (ThingDefOf.AncientWarwalkerTorso.size.x / 2 + ThingDefOf.AncientWarwalkerClaw.size.x / 2 + Rand.Range(3, 5)) * (Rand.Bool ? -1 : 1);
					IntVec3 intVec2 = thing.Position + new IntVec3(newX, 0, Rand.Range(-1, 1)).RotatedBy(this.rotation);
					if (this.CanPlaceThingAt(intVec2, this.rotation, map, ThingDefOf.AncientWarwalkerClaw))
					{
						Thing thing2 = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientWarwalkerClaw, null), intVec2, map, this.rotation, WipeMode.Vanish, false);
						ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
						ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_OilSmear, 0.2f, 0, int.MaxValue, null);
					}
				}
			}
		}

		// Token: 0x04002D9A RID: 11674
		private const float MechClawChance = 0.75f;

		// Token: 0x04002D9B RID: 11675
		private const float MechLegChance = 0.75f;

		// Token: 0x04002D9C RID: 11676
		private const float OilSmearChance = 0.2f;

		// Token: 0x04002D9D RID: 11677
		private ThingDef thingToPlace;

		// Token: 0x04002D9E RID: 11678
		private Rot4 rotation;
	}
}
