using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A8 RID: 5288
	public static class ShortCircuitUtility
	{
		// Token: 0x06007E84 RID: 32388 RVA: 0x002CDC11 File Offset: 0x002CBE11
		public static IEnumerable<Building> GetShortCircuitablePowerConduits(Map map)
		{
			ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			try
			{
				List<Thing> conduits = map.listerThings.ThingsOfDef(ThingDefOf.PowerConduit);
				int num;
				for (int i = 0; i < conduits.Count; i = num + 1)
				{
					Building building = (Building)conduits[i];
					CompPower powerComp = building.PowerComp;
					if (powerComp != null)
					{
						bool hasActivePowerSource;
						if (!ShortCircuitUtility.tmpPowerNetHasActivePowerSource.TryGetValue(powerComp.PowerNet, out hasActivePowerSource))
						{
							hasActivePowerSource = powerComp.PowerNet.HasActivePowerSource;
							ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Add(powerComp.PowerNet, hasActivePowerSource);
						}
						if (hasActivePowerSource)
						{
							yield return building;
						}
					}
					num = i;
				}
				conduits = null;
			}
			finally
			{
				ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06007E85 RID: 32389 RVA: 0x002CDC24 File Offset: 0x002CBE24
		public static void DoShortCircuit(Building culprit)
		{
			PowerNet powerNet = culprit.PowerComp.PowerNet;
			Map map = culprit.Map;
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			if (powerNet.batteryComps.Any((CompPowerBattery x) => x.StoredEnergy > 20f))
			{
				ShortCircuitUtility.DrainBatteriesAndCauseExplosion(powerNet, culprit, out num, out num2);
			}
			else
			{
				flag = ShortCircuitUtility.TryStartFireNear(culprit);
			}
			string value;
			if (culprit.def == ThingDefOf.PowerConduit)
			{
				value = "AnElectricalConduit".Translate();
			}
			else
			{
				value = Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(culprit.Label, false, false);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				stringBuilder.Append("ShortCircuitStartedFire".Translate(value));
			}
			else
			{
				stringBuilder.Append("ShortCircuit".Translate(value));
			}
			if (num > 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitDischargedEnergy".Translate(num.ToString("F0")));
			}
			if (num2 > 5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasLarge".Translate());
			}
			if (num2 > 8f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasHuge".Translate());
			}
			Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(culprit.Position, map, false), null, null, null, null);
		}

		// Token: 0x06007E86 RID: 32390 RVA: 0x002CDDEC File Offset: 0x002CBFEC
		public static bool TryShortCircuitInRain(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if ((compPowerTrader != null && compPowerTrader.PowerOn && compPowerTrader.Props.shortCircuitInRain) || (thing.TryGetComp<CompPowerBattery>() != null && thing.TryGetComp<CompPowerBattery>().StoredEnergy > 100f))
			{
				TaggedString taggedString = "ShortCircuitRain".Translate(thing.Label, thing);
				TargetInfo target = new TargetInfo(thing.Position, thing.Map, false);
				if (thing.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), taggedString, LetterDefOf.NegativeEvent, target, null, null, null, null);
				}
				else
				{
					Messages.Message(taggedString, target, MessageTypeDefOf.NeutralEvent, true);
				}
				GenExplosion.DoExplosion(thing.OccupiedRect().RandomCell, thing.Map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x06007E87 RID: 32391 RVA: 0x002CDF04 File Offset: 0x002CC104
		private static void DrainBatteriesAndCauseExplosion(PowerNet net, Building culprit, out float totalEnergy, out float explosionRadius)
		{
			totalEnergy = 0f;
			for (int i = 0; i < net.batteryComps.Count; i++)
			{
				CompPowerBattery compPowerBattery = net.batteryComps[i];
				totalEnergy += compPowerBattery.StoredEnergy;
				compPowerBattery.DrawPower(compPowerBattery.StoredEnergy);
			}
			explosionRadius = Mathf.Sqrt(totalEnergy) * 0.05f;
			explosionRadius = Mathf.Clamp(explosionRadius, 1.5f, 14.9f);
			GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			if (explosionRadius > 3.5f)
			{
				GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius * 0.3f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			}
		}

		// Token: 0x06007E88 RID: 32392 RVA: 0x002CE008 File Offset: 0x002CC208
		private static bool TryStartFireNear(Building b)
		{
			ShortCircuitUtility.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(3f);
			CellRect startRect = b.OccupiedRect();
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = b.Position + GenRadial.RadialPattern[i];
				if (GenSight.LineOfSight(b.Position, intVec, b.Map, startRect, CellRect.SingleCell(intVec), null) && FireUtility.ChanceToStartFireIn(intVec, b.Map) > 0f)
				{
					ShortCircuitUtility.tmpCells.Add(intVec);
				}
			}
			return ShortCircuitUtility.tmpCells.Any<IntVec3>() && FireUtility.TryStartFireIn(ShortCircuitUtility.tmpCells.RandomElement<IntVec3>(), b.Map, Rand.Range(0.1f, 1.75f));
		}

		// Token: 0x04004EC2 RID: 20162
		private static Dictionary<PowerNet, bool> tmpPowerNetHasActivePowerSource = new Dictionary<PowerNet, bool>();

		// Token: 0x04004EC3 RID: 20163
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
