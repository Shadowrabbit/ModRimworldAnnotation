using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024C RID: 588
	public class Designation : IExposable
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x000113A8 File Offset: 0x0000F5A8
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x000113B5 File Offset: 0x0000F5B5
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00006B8B File Offset: 0x00004D8B
		public Designation()
		{
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000113BE File Offset: 0x0000F5BE
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000B48C0 File Offset: 0x000B2AC0
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == DesignationDefOf.Haul && !this.target.HasThing)
			{
				Log.Error("Haul designation has no target! Deleting.", false);
				this.Delete();
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000113D4 File Offset: 0x0000F5D4
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000113FE File Offset: 0x0000F5FE
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000B4920 File Offset: 0x000B2B20
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

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00011435 File Offset: 0x0000F635
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000B49B8 File Offset: 0x000B2BB8
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

		// Token: 0x04000C47 RID: 3143
		public DesignationManager designationManager;

		// Token: 0x04000C48 RID: 3144
		public DesignationDef def;

		// Token: 0x04000C49 RID: 3145
		public LocalTargetInfo target;

		// Token: 0x04000C4A RID: 3146
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
