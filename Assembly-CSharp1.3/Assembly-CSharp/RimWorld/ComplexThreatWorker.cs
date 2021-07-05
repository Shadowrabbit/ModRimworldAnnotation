using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6B RID: 3179
	public abstract class ComplexThreatWorker
	{
		// Token: 0x06004A3C RID: 19004 RVA: 0x001887F0 File Offset: 0x001869F0
		public void Resolve(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings, StringBuilder debug = null)
		{
			try
			{
				bool flag = false;
				if (parms.triggerSignal.NullOrEmpty())
				{
					string triggerSignal;
					if (this.TryGetThingTriggerSignal(parms, out triggerSignal))
					{
						parms.triggerSignal = triggerSignal;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: Existing thing");
						}
					}
					else if (this.def.fallbackToRoomEnteredTrigger)
					{
						parms.triggerSignal = ComplexUtility.SpawnRoomEnteredTrigger(parms.room, parms.map);
						flag = true;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: Room entry");
						}
					}
					else
					{
						if (!this.def.allowPassive)
						{
							Log.Warning("Unable to generate a trigger for threat " + this.def.defName);
							return;
						}
						parms.triggerSignal = this.def.defName + Find.UniqueIDsManager.GetNextSignalTagID();
						parms.passive = true;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: None. Passive threat.");
						}
					}
				}
				float points = parms.points;
				if (!parms.passive && Rand.Chance(this.def.delayChance))
				{
					int num = this.def.delayTickOptions.RandomElement<int>();
					parms.points = points;
					parms.delayTicks = new int?(num);
					float num2 = this.def.threatFactorOverDelayTicksCurve.Evaluate((float)num);
					parms.points *= num2;
					if (debug != null)
					{
						debug.AppendLine(string.Format("--> Threat delay ticks: {0}", num));
						debug.AppendLine(string.Format("--> Threat delay points factor: {0}", num2));
						debug.AppendLine(string.Format("--> Threat points post delay factor: {0}", parms.points));
					}
				}
				if (!parms.passive && !flag && Rand.Chance(this.def.spawnInOtherRoomChance))
				{
					ComplexResolveParams parms2 = parms;
					foreach (List<CellRect> room in parms.allRooms.InRandomOrder(null))
					{
						parms2.room = room;
						if (this.def.Worker.CanResolve(parms2))
						{
							parms.room = room;
							break;
						}
					}
				}
				float num3 = 0f;
				this.ResolveInt(parms, ref num3, outSpawnedThings);
				if (parms.passive)
				{
					num3 *= this.def.postSpawnPassiveThreatFactor;
					if (debug != null)
					{
						debug.AppendLine(string.Format("--> Threat post spawn passive factor: {0}", this.def.postSpawnPassiveThreatFactor));
					}
				}
				if (debug != null)
				{
					debug.AppendLine(string.Format("--> Total points used: {0}", num3));
				}
				threatPointsUsed += num3;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception resolving ",
					base.GetType().Name,
					": ",
					ex
				}));
			}
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x00188B04 File Offset: 0x00186D04
		private bool TryGetThingTriggerSignal(ComplexResolveParams threatParams, out string triggerSignal)
		{
			if (threatParams.room == null || threatParams.spawnedThings.NullOrEmpty<Thing>())
			{
				triggerSignal = null;
				return false;
			}
			List<CellRect> room = threatParams.room;
			for (int i = 0; i < threatParams.spawnedThings.Count; i++)
			{
				Thing thing = threatParams.spawnedThings[i];
				if (room.Any((CellRect r) => r.Contains(thing.Position)))
				{
					CompHackable compHackable = thing.TryGetComp<CompHackable>();
					if (compHackable != null && !compHackable.IsHacked)
					{
						if (Rand.Bool)
						{
							if (compHackable.hackingStartedSignal == null)
							{
								compHackable.hackingStartedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
							}
							triggerSignal = compHackable.hackingStartedSignal;
						}
						else
						{
							if (compHackable.hackingCompletedSignal == null)
							{
								compHackable.hackingCompletedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
							}
							triggerSignal = compHackable.hackingCompletedSignal;
						}
						return true;
					}
					Building_Casket building_Casket;
					if ((building_Casket = (thing as Building_Casket)) != null && building_Casket.CanOpen)
					{
						if (building_Casket.openedSignal.NullOrEmpty())
						{
							building_Casket.openedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
						}
						triggerSignal = building_Casket.openedSignal;
						return true;
					}
				}
			}
			triggerSignal = null;
			return false;
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x00188C54 File Offset: 0x00186E54
		public bool CanResolve(ComplexResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x06004A3F RID: 19007
		protected abstract void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> spawnedThings);

		// Token: 0x06004A40 RID: 19008 RVA: 0x00188CB4 File Offset: 0x00186EB4
		protected virtual bool CanResolveInt(ComplexResolveParams parms)
		{
			return (float)this.def.minPoints <= parms.points && (!this.def.allowPassive || this.def.fallbackToRoomEnteredTrigger || !parms.triggerSignal.NullOrEmpty());
		}

		// Token: 0x04002D1F RID: 11551
		public ComplexThreatDef def;

		// Token: 0x04002D20 RID: 11552
		private const string ThreatTriggerSignal = "ThreatTriggerSignal";
	}
}
