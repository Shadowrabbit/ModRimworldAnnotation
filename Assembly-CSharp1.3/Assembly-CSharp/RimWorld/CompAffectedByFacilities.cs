using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F6 RID: 4342
	[StaticConstructorOnStartup]
	public class CompAffectedByFacilities : ThingComp
	{
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060067F9 RID: 26617 RVA: 0x00232A86 File Offset: 0x00230C86
		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
			}
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x00232A90 File Offset: 0x00230C90
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

		// Token: 0x060067FB RID: 26619 RVA: 0x00232AEC File Offset: 0x00230CEC
		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot) && CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot);
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x00232B1C File Offset: 0x00230D1C
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

		// Token: 0x060067FD RID: 26621 RVA: 0x00232BCC File Offset: 0x00230DCC
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
			if (compProperties.mustBePlacedAdjacentCardinalToBedHead || compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
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
					IntVec3 sleepingSlotPos = BedUtility.GetSleepingSlotPos(i, myPos, myRot, myDef.size);
					if (sleepingSlotPos.IsAdjacentToCardinalOrInside(other))
					{
						if (compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
						{
							if (other.MovedBy(facilityRot.FacingCell).Contains(sleepingSlotPos))
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!compProperties.mustBePlacedAdjacent && !compProperties.mustBePlacedAdjacentCardinalToBedHead && !compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
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

		// Token: 0x060067FE RID: 26622 RVA: 0x00232CF9 File Offset: 0x00230EF9
		public bool IsValidFacilityForMe(Thing facility)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, this.parent.def, this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x00232D28 File Offset: 0x00230F28
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

		// Token: 0x06006800 RID: 26624 RVA: 0x00232D94 File Offset: 0x00230F94
		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x00232DB8 File Offset: 0x00230FB8
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

		// Token: 0x06006802 RID: 26626 RVA: 0x00232E7C File Offset: 0x0023107C
		public void Notify_NewLink(Thing facility)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
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

		// Token: 0x06006803 RID: 26627 RVA: 0x00232F00 File Offset: 0x00231100
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
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x00232F49 File Offset: 0x00231149
		public void Notify_FacilityDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x00232F49 File Offset: 0x00231149
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x06006806 RID: 26630 RVA: 0x00232F49 File Offset: 0x00231149
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x06006807 RID: 26631 RVA: 0x00232F51 File Offset: 0x00231151
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x06006808 RID: 26632 RVA: 0x00232F59 File Offset: 0x00231159
		public override void PostDeSpawn(Map map)
		{
			this.UnlinkAll();
		}

		// Token: 0x06006809 RID: 26633 RVA: 0x00232F64 File Offset: 0x00231164
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
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
				}
			}
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x00232FEC File Offset: 0x002311EC
		private bool IsBetter(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, Thing thanThisFacility)
		{
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.");
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

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x0600680B RID: 26635 RVA: 0x00233089 File Offset: 0x00231289
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

		// Token: 0x0600680C RID: 26636 RVA: 0x00233099 File Offset: 0x00231299
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

		// Token: 0x0600680D RID: 26637 RVA: 0x002330C0 File Offset: 0x002312C0
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing t in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, t.TrueCenter());
			}
		}

		// Token: 0x0600680E RID: 26638 RVA: 0x0023312C File Offset: 0x0023132C
		public static void DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(float curX, ref float curY, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompAffectedByFacilities.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			int num = 0;
			foreach (Thing thing in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				num++;
				if (num == 1)
				{
					CompAffectedByFacilities.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
				}
				CompAffectedByFacilities.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x002331C4 File Offset: 0x002313C4
		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
			}
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x00233200 File Offset: 0x00231400
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

		// Token: 0x06006811 RID: 26641 RVA: 0x002332D8 File Offset: 0x002314D8
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

		// Token: 0x06006812 RID: 26642 RVA: 0x00233344 File Offset: 0x00231544
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

		// Token: 0x06006813 RID: 26643 RVA: 0x00232F51 File Offset: 0x00231151
		private void RelinkAll()
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x06006814 RID: 26644 RVA: 0x00233519 File Offset: 0x00231719
		public bool IsFacilityActive(Thing facility)
		{
			return facility.TryGetComp<CompFacility>().CanBeActive;
		}

		// Token: 0x06006815 RID: 26645 RVA: 0x00233528 File Offset: 0x00231728
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

		// Token: 0x06006816 RID: 26646 RVA: 0x002335A0 File Offset: 0x002317A0
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
			}
			this.linkedFacilities.Clear();
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x0023363C File Offset: 0x0023183C
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref float y, string text, ref CompAffectedByFacilities.<>c__DisplayClass26_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x04003A7F RID: 14975
		private List<Thing> linkedFacilities = new List<Thing>();

		// Token: 0x04003A80 RID: 14976
		public static Material InactiveFacilityLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x04003A81 RID: 14977
		private static Dictionary<ThingDef, int> alreadyReturnedCount = new Dictionary<ThingDef, int>();

		// Token: 0x04003A82 RID: 14978
		private List<ThingDef> alreadyUsed = new List<ThingDef>();
	}
}
