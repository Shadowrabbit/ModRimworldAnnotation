using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000190 RID: 400
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x06000B46 RID: 2886 RVA: 0x0003D092 File Offset: 0x0003B292
		public DesignationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003D0AC File Offset: 0x0003B2AC
		public void ExposeData()
		{
			Scribe_Collections.Look<Designation>(ref this.allDesignations, "allDesignations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.");
				}
				if (this.allDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.");
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
								Log.Error("Cell-needing designation " + this.allDesignations[j] + " had no cell target. Removing...");
								this.allDesignations.RemoveAt(j);
							}
						}
					}
					else if (!this.allDesignations[j].target.HasThing)
					{
						Log.Error("Thing-needing designation " + this.allDesignations[j] + " had no thing target. Removing...");
						this.allDesignations.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003D24C File Offset: 0x0003B44C
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

		// Token: 0x06000B49 RID: 2889 RVA: 0x0003D2BC File Offset: 0x0003B4BC
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target);
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
				FleckMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0003D3C0 File Offset: 0x0003B5C0
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

		// Token: 0x06000B4B RID: 2891 RVA: 0x0003D404 File Offset: 0x0003B604
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.");
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

		// Token: 0x06000B4C RID: 2892 RVA: 0x0003D474 File Offset: 0x0003B674
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.");
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

		// Token: 0x06000B4D RID: 2893 RVA: 0x0003D50C File Offset: 0x0003B70C
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

		// Token: 0x06000B4E RID: 2894 RVA: 0x0003D523 File Offset: 0x0003B723
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

		// Token: 0x06000B4F RID: 2895 RVA: 0x0003D53C File Offset: 0x0003B73C
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

		// Token: 0x06000B50 RID: 2896 RVA: 0x0003D594 File Offset: 0x0003B794
		public bool HasMapDesignationOn(Thing t)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.HasThing && designation.target.Thing == t)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0003D5E4 File Offset: 0x0003B7E4
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

		// Token: 0x06000B52 RID: 2898 RVA: 0x0003D5FC File Offset: 0x0003B7FC
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

		// Token: 0x06000B53 RID: 2899 RVA: 0x0003D65F File Offset: 0x0003B85F
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0003D674 File Offset: 0x0003B874
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0003D694 File Offset: 0x0003B894
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

		// Token: 0x06000B56 RID: 2902 RVA: 0x0003D720 File Offset: 0x0003B920
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0003D740 File Offset: 0x0003B940
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

		// Token: 0x06000B58 RID: 2904 RVA: 0x0003D798 File Offset: 0x0003B998
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

		// Token: 0x0400095A RID: 2394
		public Map map;

		// Token: 0x0400095B RID: 2395
		public List<Designation> allDesignations = new List<Designation>();
	}
}
