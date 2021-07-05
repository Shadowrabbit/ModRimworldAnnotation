using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C70 RID: 3184
	public class ComplexThreatWorker_SecurityCrate : ComplexThreatWorker
	{
		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004A50 RID: 19024 RVA: 0x00189664 File Offset: 0x00187864
		public IEnumerable<ComplexThreatDef> SubThreats
		{
			get
			{
				yield return ComplexThreatDefOf.SleepingInsects;
				yield return ComplexThreatDefOf.SleepingMechanoids;
				yield return ComplexThreatDefOf.Infestation;
				yield return ComplexThreatDefOf.CryptosleepPods;
				yield break;
			}
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x00189670 File Offset: 0x00187870
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			if (base.CanResolveInt(parms))
			{
				IntVec3 intVec;
				if (ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out intVec, 1, null))
				{
					return this.SubThreats.Any((ComplexThreatDef st) => st.Worker.CanResolve(parms));
				}
			}
			return false;
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x00189704 File Offset: 0x00187904
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out loc, 1, null);
			Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingDefOf.AncientSecurityCrate, loc, parms.map, WipeMode.Vanish);
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientComplex_SecurityCrate.root.Generate(parms2);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!building_Crate.TryAcceptThing(list[i], false))
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
			}
			outSpawnedThings.Add(building_Crate);
			parms.spawnedThings.Add(building_Crate);
			if (building_Crate.openedSignal.NullOrEmpty())
			{
				building_Crate.openedSignal = "CrateOpened" + Find.UniqueIDsManager.GetNextSignalTagID();
			}
			parms.triggerSignal = building_Crate.openedSignal;
			ComplexThreatDef complexThreatDef = (from st in this.SubThreats
			where st.Worker.CanResolve(parms)
			select st).RandomElement<ComplexThreatDef>();
			float num = 0f;
			complexThreatDef.Worker.Resolve(parms, ref num, outSpawnedThings, null);
			threatPointsUsed += num;
		}
	}
}
