using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001787 RID: 6023
	[StaticConstructorOnStartup]
	public class CompAffectedByFacilities : ThingComp
	{
		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x060084DA RID: 34010 RVA: 0x00059052 File Offset: 0x00057252
		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
			}
		}

		// Token: 0x060084DB RID: 34011 RVA: 0x00274B74 File Offset: 0x00272D74
		public bool CanLinkTo(Thing facility)
		{
			if (!this.CanPotentiallyLinkTo(facility.def, facility.Position, facility.Rotation))
			{
				return false;
			}
			if (!this.IsValidFacilityForMe(facility))
			{
				return false;
			}
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060084DC RID: 34012 RVA: 0x0005905A File Offset: 0x0005725A
		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot) && CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot);
		}

		// Token: 0x060084DD RID: 34013 RVA: 0x00274BD0 File Offset: 0x00272DD0
		public bool CanPotentiallyLinkTo(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation))
			{
				return false;
			}
			if (!this.IsPotentiallyValidFacilityForMe(facilityDef, facilityPos, facilityRot))
			{
				return false;
			}
			int num = 0;
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					num++;
					if (this.IsBetter(facilityDef, facilityPos, facilityRot, this.linkedFacilities[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return true;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			return num + 1 <= compProperties.maxSimultaneous;
		}

		// Token: 0x060084DE RID: 34014 RVA: 0x00274C80 File Offset: 0x00272E80
		public static bool CanPotentiallyLinkTo_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (compProperties.mustBePlacedAdjacent)
			{
				CellRect rect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
				CellRect rect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				if (!GenAdj.AdjacentTo8WayOrInside(rect, rect2))
				{
					return false;
				}
			}
			if (compProperties.mustBePlacedAdjacentCardinalToBedHead)
			{
				if (!myDef.IsBed)
				{
					return false;
				}
				CellRect other = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				bool flag = false;
				int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(myDef.size);
				for (int i = 0; i < sleepingSlotsCount; i++)
				{
					if (BedUtility.GetSleepingSlotPos(i, myPos, myRot, myDef.size).IsAdjacentToCardinalOrInside(other))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!compProperties.mustBePlacedAdjacent && !compProperties.mustBePlacedAdjacentCardinalToBedHead)
			{
				Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
				Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
				if (Vector3.Distance(a, b) > compProperties.maxDistance)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060084DF RID: 34015 RVA: 0x00059088 File Offset: 0x00057288
		public bool IsValidFacilityForMe(Thing facility)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, this.parent.def, this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x060084E0 RID: 34016 RVA: 0x00274D70 File Offset: 0x00272F70
		private bool IsPotentiallyValidFacilityForMe(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map))
			{
				return false;
			}
			if (facilityDef.GetCompProperties<CompProperties_Facility>().canLinkToMedBedsOnly)
			{
				Building_Bed building_Bed = this.parent as Building_Bed;
				if (building_Bed == null || !building_Bed.Medical)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060084E1 RID: 34017 RVA: 0x000590B6 File Offset: 0x000572B6
		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

		// Token: 0x060084E2 RID: 34018 RVA: 0x00274DDC File Offset: 0x00272FDC
		private static bool IsPotentiallyValidFacilityForMe_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CellRect cellRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			CellRect cellRect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
			bool result = false;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					for (int k = cellRect2.minZ; k <= cellRect2.maxZ; k++)
					{
						for (int l = cellRect2.minX; l <= cellRect2.maxX; l++)
						{
							IntVec3 start = new IntVec3(j, 0, i);
							IntVec3 end = new IntVec3(l, 0, k);
							if (GenSight.LineOfSight(start, end, map, cellRect, cellRect2, null))
							{
								return true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060084E3 RID: 34019 RVA: 0x00274EA0 File Offset: 0x002730A0
		public void Notify_NewLink(Thing facility)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					Log.Error("Notify_NewLink was called but the link is already here.", false);
					return;
				}
			}
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facility.def, facility.Position, facility.Rotation);
			if (potentiallySupplantedFacility != null)
			{
				potentiallySupplantedFacility.TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
				this.linkedFacilities.Remove(potentiallySupplantedFacility);
			}
			this.linkedFacilities.Add(facility);
		}

		// Token: 0x060084E4 RID: 34020 RVA: 0x00274F24 File Offset: 0x00273124
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == thing)
				{
					this.linkedFacilities.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.", false);
		}

		// Token: 0x060084E5 RID: 34021 RVA: 0x000590D8 File Offset: 0x000572D8
		public void Notify_FacilityDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x060084E6 RID: 34022 RVA: 0x000590D8 File Offset: 0x000572D8
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x060084E7 RID: 34023 RVA: 0x000590D8 File Offset: 0x000572D8
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x060084E8 RID: 34024 RVA: 0x000590E0 File Offset: 0x000572E0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x060084E9 RID: 34025 RVA: 0x000590E8 File Offset: 0x000572E8
		public override void PostDeSpawn(Map map)
		{
			this.UnlinkAll();
		}

		// Token: 0x060084EA RID: 34026 RVA: 0x00274F70 File Offset: 0x00273170
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
				}
			}
		}

		// Token: 0x060084EB RID: 34027 RVA: 0x00274FF0 File Offset: 0x002731F0
		private bool IsBetter(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, Thing thanThisFacility)
		{
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.", false);
				return false;
			}
			Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
			Vector3 a = this.parent.TrueCenter();
			float num = Vector3.Distance(a, b);
			float num2 = Vector3.Distance(a, thanThisFacility.TrueCenter());
			if (num != num2)
			{
				return num < num2;
			}
			if (facilityPos.x != thanThisFacility.Position.x)
			{
				return facilityPos.x < thanThisFacility.Position.x;
			}
			return facilityPos.z < thanThisFacility.Position.z;
		}

		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x060084EC RID: 34028 RVA: 0x000590F0 File Offset: 0x000572F0
		private IEnumerable<Thing> ThingsICanLinkTo
		{
			get
			{
				if (!this.parent.Spawned)
				{
					yield break;
				}
				IEnumerable<Thing> enumerable = CompAffectedByFacilities.PotentialThingsToLinkTo(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map);
				foreach (Thing thing in enumerable)
				{
					if (this.CanLinkTo(thing))
					{
						yield return thing;
					}
				}
				IEnumerator<Thing> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060084ED RID: 34029 RVA: 0x00059100 File Offset: 0x00057300
		public static IEnumerable<Thing> PotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompAffectedByFacilities.alreadyReturnedCount.Clear();
			CompProperties_AffectedByFacilities compProperties = myDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (compProperties.linkableFacilities == null)
			{
				yield break;
			}
			IEnumerable<Thing> enumerable = Enumerable.Empty<Thing>();
			for (int i = 0; i < compProperties.linkableFacilities.Count; i++)
			{
				enumerable = enumerable.Concat(map.listerThings.ThingsOfDef(compProperties.linkableFacilities[i]));
			}
			Vector3 myTrueCenter = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			IOrderedEnumerable<Thing> orderedEnumerable = from x in enumerable
			orderby Vector3.Distance(myTrueCenter, x.TrueCenter()), x.Position.x, x.Position.z
			select x;
			foreach (Thing thing in orderedEnumerable)
			{
				if (CompAffectedByFacilities.CanPotentiallyLinkTo_Static(thing, myDef, myPos, myRot))
				{
					CompProperties_Facility compProperties2 = thing.def.GetCompProperties<CompProperties_Facility>();
					if (CompAffectedByFacilities.alreadyReturnedCount.ContainsKey(thing.def))
					{
						if (CompAffectedByFacilities.alreadyReturnedCount[thing.def] >= compProperties2.maxSimultaneous)
						{
							continue;
						}
					}
					else
					{
						CompAffectedByFacilities.alreadyReturnedCount.Add(thing.def, 0);
					}
					Dictionary<ThingDef, int> dictionary = CompAffectedByFacilities.alreadyReturnedCount;
					ThingDef def = thing.def;
					int num = dictionary[def];
					dictionary[def] = num + 1;
					yield return thing;
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060084EE RID: 34030 RVA: 0x00275090 File Offset: 0x00273290
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing t in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, t.TrueCenter());
			}
		}

		// Token: 0x060084EF RID: 34031 RVA: 0x002750FC File Offset: 0x002732FC
		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
			}
		}

		// Token: 0x060084F0 RID: 34032 RVA: 0x00275134 File Offset: 0x00273334
		private Thing GetPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing thing = null;
			int num = 0;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					if (thing == null)
					{
						thing = this.linkedFacilities[i];
					}
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (num + 1 <= compProperties.maxSimultaneous)
			{
				return null;
			}
			Thing thing2 = thing;
			for (int j = 0; j < this.linkedFacilities.Count; j++)
			{
				if (facilityDef == this.linkedFacilities[j].def && this.IsBetter(thing2.def, thing2.Position, thing2.Rotation, this.linkedFacilities[j]))
				{
					thing2 = this.linkedFacilities[j];
				}
			}
			return thing2;
		}

		// Token: 0x060084F1 RID: 34033 RVA: 0x0027520C File Offset: 0x0027340C
		public float GetStatOffset(StatDef stat)
		{
			float num = 0f;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					CompProperties_Facility compProperties = this.linkedFacilities[i].def.GetCompProperties<CompProperties_Facility>();
					if (compProperties.statOffsets != null)
					{
						num += compProperties.statOffsets.GetStatOffsetFromList(stat);
					}
				}
			}
			return num;
		}

		// Token: 0x060084F2 RID: 34034 RVA: 0x00275278 File Offset: 0x00273478
		public void GetStatsExplanation(StatDef stat, StringBuilder sb)
		{
			this.alreadyUsed.Clear();
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				bool flag2 = false;
				for (int j = 0; j < this.alreadyUsed.Count; j++)
				{
					if (this.alreadyUsed[j] == this.linkedFacilities[i].def)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && this.IsFacilityActive(this.linkedFacilities[i]))
				{
					CompProperties_Facility compProperties = this.linkedFacilities[i].def.GetCompProperties<CompProperties_Facility>();
					if (compProperties.statOffsets != null)
					{
						float num = compProperties.statOffsets.GetStatOffsetFromList(stat);
						if (num != 0f)
						{
							if (!flag)
							{
								flag = true;
								sb.AppendLine();
								sb.AppendLine("StatsReport_Facilities".Translate() + ":");
							}
							int num2 = 0;
							for (int k = 0; k < this.linkedFacilities.Count; k++)
							{
								if (this.IsFacilityActive(this.linkedFacilities[k]) && this.linkedFacilities[k].def == this.linkedFacilities[i].def)
								{
									num2++;
								}
							}
							num *= (float)num2;
							sb.Append("    ");
							if (num2 != 1)
							{
								sb.Append(num2.ToString() + "x ");
							}
							sb.AppendLine(this.linkedFacilities[i].LabelCap + ": " + num.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
							this.alreadyUsed.Add(this.linkedFacilities[i].def);
						}
					}
				}
			}
		}

		// Token: 0x060084F3 RID: 34035 RVA: 0x000590E0 File Offset: 0x000572E0
		private void RelinkAll()
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x060084F4 RID: 34036 RVA: 0x00059125 File Offset: 0x00057325
		public bool IsFacilityActive(Thing facility)
		{
			return facility.TryGetComp<CompFacility>().CanBeActive;
		}

		// Token: 0x060084F5 RID: 34037 RVA: 0x00275450 File Offset: 0x00273650
		private void LinkToNearbyFacilities()
		{
			this.UnlinkAll();
			if (this.parent.Spawned)
			{
				foreach (Thing thing in this.ThingsICanLinkTo)
				{
					this.linkedFacilities.Add(thing);
					thing.TryGetComp<CompFacility>().Notify_NewLink(this.parent);
				}
			}
		}

		// Token: 0x060084F6 RID: 34038 RVA: 0x002754C8 File Offset: 0x002736C8
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
			}
			this.linkedFacilities.Clear();
		}

		// Token: 0x040055FB RID: 22011
		private List<Thing> linkedFacilities = new List<Thing>();

		// Token: 0x040055FC RID: 22012
		public static Material InactiveFacilityLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x040055FD RID: 22013
		private static Dictionary<ThingDef, int> alreadyReturnedCount = new Dictionary<ThingDef, int>();

		// Token: 0x040055FE RID: 22014
		private List<ThingDef> alreadyUsed = new List<ThingDef>();
	}
}
