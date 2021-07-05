using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018F RID: 399
	public class Designation : IExposable
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0003CEA4 File Offset: 0x0003B0A4
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0003CEB1 File Offset: 0x0003B0B1
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000033AC File Offset: 0x000015AC
		public Designation()
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003CEBA File Offset: 0x0003B0BA
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0003CED0 File Offset: 0x0003B0D0
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == DesignationDefOf.Haul && !this.target.HasThing)
			{
				Log.Error("Haul designation has no target! Deleting.");
				this.Delete();
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003CF2F File Offset: 0x0003B12F
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003CF59 File Offset: 0x0003B159
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003CF90 File Offset: 0x0003B190
		public virtual void DesignationDraw()
		{
			if (this.target.HasThing && !this.target.Thing.Spawned)
			{
				return;
			}
			Vector3 position = default(Vector3);
			if (this.target.HasThing)
			{
				position = this.target.Thing.DrawPos;
				position.y = this.DesignationDrawAltitude;
			}
			else
			{
				position = this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
			}
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, this.def.iconMat, 0);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003D028 File Offset: 0x0003B228
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003D03C File Offset: 0x0003B23C
		public override string ToString()
		{
			return string.Format(string.Concat(new object[]
			{
				"(",
				this.def.defName,
				" target=",
				this.target,
				")"
			}), Array.Empty<object>());
		}

		// Token: 0x04000956 RID: 2390
		public DesignationManager designationManager;

		// Token: 0x04000957 RID: 2391
		public DesignationDef def;

		// Token: 0x04000958 RID: 2392
		public LocalTargetInfo target;

		// Token: 0x04000959 RID: 2393
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
