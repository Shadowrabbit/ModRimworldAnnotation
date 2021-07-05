using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200024D RID: 589
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x00011448 File Offset: 0x0000F648
		public DesignationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000B4A10 File Offset: 0x000B2C10
		public void ExposeData()
		{
			Scribe_Collections.Look<Designation>(ref this.allDesignations, "allDesignations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.", false);
				}
				if (this.allDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.", false);
				}
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					this.allDesignations[i].designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.allDesignations.Count - 1; j >= 0; j--)
				{
					TargetType targetType = this.allDesignations[j].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType == TargetType.Cell)
						{
							if (!this.allDesignations[j].target.Cell.IsValid)
							{
								Log.Error("Cell-needing designation " + this.allDesignations[j] + " had no cell target. Removing...", false);
								this.allDesignations.RemoveAt(j);
							}
						}
					}
					else if (!this.allDesignations[j].target.HasThing)
					{
						Log.Error("Thing-needing designation " + this.allDesignations[j] + " had no thing target. Removing...", false);
						this.allDesignations.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x000B4BB4 File Offset: 0x000B2DB4
		public void DrawDesignations()
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				if (!this.allDesignations[i].target.HasThing || this.allDesignations[i].target.Thing.Map == this.map)
				{
					this.allDesignations[i].DesignationDraw();
				}
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x000B4C24 File Offset: 0x000B2E24
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target, false);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target, false);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing)
			{
				newDes.target.Thing.SetForbidden(false, false);
			}
			this.allDesignations.Add(newDes);
			newDes.designationManager = this;
			newDes.Notify_Added();
			Map map = newDes.target.HasThing ? newDes.target.Thing.Map : this.map;
			if (map != null)
			{
				MoteMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x000B4D28 File Offset: 0x000B2F28
		public Designation DesignationOn(Thing t)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x000B4D6C File Offset: 0x000B2F6C
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.", false);
				return null;
			}
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t && designation.def == def)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x000B4DDC File Offset: 0x000B2FDC
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.", false);
				return null;
			}
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00011462 File Offset: 0x0000F662
		public IEnumerable<Designation> AllDesignationsOn(Thing t)
		{
			int count = this.allDesignations.Count;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				if (this.allDesignations[i].target.Thing == t)
				{
					yield return this.allDesignations[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00011479 File Offset: 0x0000F679
		public IEnumerable<Designation> AllDesignationsAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Designation designation = this.allDesignations[i];
				if ((!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
				{
					yield return designation;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x000B4E78 File Offset: 0x000B3078
		public bool HasMapDesignationAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (!designation.target.HasThing && designation.target.Cell == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00011490 File Offset: 0x0000F690
		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					yield return designation;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000B4ED0 File Offset: 0x000B30D0
		public bool AnySpawnedDesignationOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x000114A7 File Offset: 0x0000F6A7
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000B4F34 File Offset: 0x000B3134
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000B4F54 File Offset: 0x000B3154
		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if ((!standardCanceling || designation.def.designateCancelable) && designation.target.Thing == t)
				{
					designation.Notify_Removing();
				}
			}
			this.allDesignations.RemoveAll((Designation d) => (!standardCanceling || d.def.designateCancelable) && d.target.Thing == t);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x000B4FE0 File Offset: 0x000B31E0
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000B5000 File Offset: 0x000B3200
		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				if (this.allDesignations[i].def == def)
				{
					this.allDesignations[i].Notify_Removing();
					this.allDesignations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000B5058 File Offset: 0x000B3258
		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				Designation designation = this.allDesignations[i];
				if (cellRect.Contains(designation.target.Cell) && designation.def.removeIfBuildingDespawned)
				{
					this.RemoveDesignation(designation);
				}
			}
		}

		// Token: 0x04000C4B RID: 3147
		public Map map;

		// Token: 0x04000C4C RID: 3148
		public List<Designation> allDesignations = new List<Designation>();
	}
}
