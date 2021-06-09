using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001997 RID: 6551
	public class Designator_Cancel : Designator
	{
		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x060090C7 RID: 37063 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060090C8 RID: 37064 RVA: 0x0029AF48 File Offset: 0x00299148
		public Designator_Cancel()
		{
			this.defaultLabel = "DesignatorCancel".Translate();
			this.defaultDesc = "DesignatorCancelDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Cancel;
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
			this.tutorTag = "Cancel";
		}

		// Token: 0x060090C9 RID: 37065 RVA: 0x0029AFD4 File Offset: 0x002991D4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (this.CancelableDesignationsAt(c).Count<Designation>() > 0)
			{
				return true;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060090CA RID: 37066 RVA: 0x0029B050 File Offset: 0x00299250
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Designation designation in this.CancelableDesignationsAt(c).ToList<Designation>())
			{
				if (designation.def.designateCancelable)
				{
					base.Map.designationManager.RemoveDesignation(designation);
				}
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x060090CB RID: 37067 RVA: 0x0029B108 File Offset: 0x00299308
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t) != null)
			{
				using (IEnumerator<Designation> enumerator = base.Map.designationManager.AllDesignationsOn(t).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.designateCancelable)
						{
							return true;
						}
					}
				}
			}
			if (t.def.mineable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
			{
				return true;
			}
			if (t.def.IsSmoothable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.SmoothWall) != null)
			{
				return true;
			}
			return t.Faction == Faction.OfPlayer && (t is Frame || t is Blueprint);
		}

		// Token: 0x060090CC RID: 37068 RVA: 0x0029B210 File Offset: 0x00299410
		public override void DesignateThing(Thing t)
		{
			if (t is Frame || t is Blueprint)
			{
				t.Destroy(DestroyMode.Cancel);
				return;
			}
			base.Map.designationManager.RemoveAllDesignationsOn(t, true);
			if (t.def.mineable)
			{
				Designation designation = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine);
				if (designation != null)
				{
					base.Map.designationManager.RemoveDesignation(designation);
				}
			}
			if (t.def.IsSmoothable)
			{
				Designation designation2 = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.SmoothWall);
				if (designation2 != null)
				{
					base.Map.designationManager.RemoveDesignation(designation2);
				}
			}
		}

		// Token: 0x060090CD RID: 37069 RVA: 0x0006127F File Offset: 0x0005F47F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x060090CE RID: 37070 RVA: 0x00061286 File Offset: 0x0005F486
		private IEnumerable<Designation> CancelableDesignationsAt(IntVec3 c)
		{
			return from x in base.Map.designationManager.AllDesignationsAt(c)
			where x.def != DesignationDefOf.Plan
			select x;
		}

		// Token: 0x060090CF RID: 37071 RVA: 0x0029B2C4 File Offset: 0x002994C4
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			Designator_Cancel.seenThings.Clear();
			int i = 0;
			while (i < dragCells.Count)
			{
				if (!base.Map.designationManager.HasMapDesignationAt(dragCells[i]))
				{
					goto IL_76;
				}
				Graphics.DrawMesh(MeshPool.plane10, dragCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
				if (base.Map.designationManager.DesignationAt(dragCells[i], DesignationDefOf.Mine) == null)
				{
					goto IL_76;
				}
				IL_FF:
				i++;
				continue;
				IL_76:
				List<Thing> thingList = dragCells[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing = thingList[j];
					if (!Designator_Cancel.seenThings.Contains(thing) && this.CanDesignateThing(thing).Accepted)
					{
						Vector3 drawPos = thing.DrawPos;
						drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
						Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, DesignatorUtility.DragHighlightThingMat, 0);
						Designator_Cancel.seenThings.Add(thing);
					}
				}
				goto IL_FF;
			}
			Designator_Cancel.seenThings.Clear();
		}

		// Token: 0x04005C06 RID: 23558
		private static HashSet<Thing> seenThings = new HashSet<Thing>();
	}
}
