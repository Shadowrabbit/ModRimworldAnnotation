using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112B RID: 4395
	public class CompFacility : ThingComp
	{
		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x0600699E RID: 27038 RVA: 0x0023995C File Offset: 0x00237B5C
		public bool CanBeActive
		{
			get
			{
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				return compPowerTrader == null || compPowerTrader.PowerOn;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x0600699F RID: 27039 RVA: 0x00239983 File Offset: 0x00237B83
		public List<Thing> LinkedBuildings
		{
			get
			{
				return this.linkedBuildings;
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x060069A0 RID: 27040 RVA: 0x0023998B File Offset: 0x00237B8B
		public CompProperties_Facility Props
		{
			get
			{
				return (CompProperties_Facility)this.props;
			}
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x00239998 File Offset: 0x00237B98
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						GenDraw.DrawLineBetween(a, thing.TrueCenter());
						compAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility(myDef, myPos, myRot);
					}
				}
			}
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x00239A5C File Offset: 0x00237C5C
		public static void DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(float curX, ref float curY, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompFacility.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			int num = 0;
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						num++;
						if (num == 1)
						{
							CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|8_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
						}
						CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|8_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
					}
				}
			}
			if (num == 0)
			{
				CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|8_0(ref curY, "FacilityNoPotentialLinks".Translate(), ref CS$<>8__locals1);
			}
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x00239B60 File Offset: 0x00237D60
		public void Notify_NewLink(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
					return;
				}
			}
			this.linkedBuildings.Add(thing);
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x00239BAC File Offset: 0x00237DAC
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					this.linkedBuildings.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x00239BF5 File Offset: 0x00237DF5
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x00239BF5 File Offset: 0x00237DF5
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x00239BFD File Offset: 0x00237DFD
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x00239C08 File Offset: 0x00237E08
		public override void PostDeSpawn(Map map)
		{
			this.thingsToNotify.Clear();
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.thingsToNotify.Add(this.linkedBuildings[i]);
			}
			this.UnlinkAll();
			foreach (Thing thing in this.thingsToNotify)
			{
				thing.TryGetComp<CompAffectedByFacilities>().Notify_FacilityDespawned();
			}
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x00239C9C File Offset: 0x00237E9C
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
				}
			}
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x00239D2C File Offset: 0x00237F2C
		public override string CompInspectStringExtra()
		{
			CompProperties_Facility props = this.Props;
			if (props.statOffsets == null)
			{
				return null;
			}
			bool flag = this.AmIActiveForAnyone();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < props.statOffsets.Count; i++)
			{
				StatModifier statModifier = props.statOffsets[i];
				StatDef stat = statModifier.stat;
				stringBuilder.Append(stat.LabelCap);
				stringBuilder.Append(": ");
				stringBuilder.Append(statModifier.value.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
				if (!flag)
				{
					stringBuilder.Append(" (");
					stringBuilder.Append("InactiveFacility".Translate());
					stringBuilder.Append(")");
				}
				if (i < props.statOffsets.Count - 1)
				{
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x00239BFD File Offset: 0x00237DFD
		private void RelinkAll()
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x00239E14 File Offset: 0x00238014
		private void LinkToNearbyBuildings()
		{
			this.UnlinkAll();
			CompProperties_Facility props = this.Props;
			if (props.linkableBuildings == null)
			{
				return;
			}
			for (int i = 0; i < props.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(props.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanLinkTo(this.parent))
					{
						this.linkedBuildings.Add(thing);
						compAffectedByFacilities.Notify_NewLink(this.parent);
					}
				}
			}
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x00239EE4 File Offset: 0x002380E4
		private bool AmIActiveForAnyone()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x00239F28 File Offset: 0x00238128
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().Notify_LinkRemoved(this.parent);
			}
			this.linkedBuildings.Clear();
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x00239F90 File Offset: 0x00238190
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|8_0(ref float y, string text, ref CompFacility.<>c__DisplayClass8_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x04003B04 RID: 15108
		private List<Thing> linkedBuildings = new List<Thing>();

		// Token: 0x04003B05 RID: 15109
		private HashSet<Thing> thingsToNotify = new HashSet<Thing>();
	}
}
