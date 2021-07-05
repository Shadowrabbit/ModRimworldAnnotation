using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000DD RID: 221
	public class BodyDef : Def
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0000B74D File Offset: 0x0000994D
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0000B755 File Offset: 0x00009955
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0000B75D File Offset: 0x0000995D
		public IEnumerable<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
		{
			int num;
			for (int i = 0; i < this.AllParts.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = this.AllParts[i];
				if (bodyPartRecord.def.tags.Contains(tag))
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0000B774 File Offset: 0x00009974
		public IEnumerable<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
		{
			int num;
			for (int i = 0; i < this.AllParts.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = this.AllParts[i];
				if (bodyPartRecord.def == def)
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0008F9B8 File Offset: 0x0008DBB8
		public bool HasPartWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				if (this.AllParts[i].def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0000B78B File Offset: 0x0000998B
		public BodyPartRecord GetPartAtIndex(int index)
		{
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				return null;
			}
			return this.cachedAllParts[index];
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0008F9FC File Offset: 0x0008DBFC
		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0000B7AD File Offset: 0x000099AD
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty<BodyPartRecord>())
			{
				yield return "no parts vulnerable to frostbite";
			}
			foreach (BodyPartRecord bodyPartRecord in this.AllParts)
			{
				if (bodyPartRecord.def.conceptual && bodyPartRecord.coverageAbs != 0f)
				{
					yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", bodyPartRecord);
				}
				else if (Prefs.DevMode && !bodyPartRecord.def.conceptual)
				{
					float num = 0f;
					foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.parts)
					{
						num += bodyPartRecord2.coverage;
					}
					if (num >= 1f)
					{
						Log.Warning(string.Concat(new string[]
						{
							"BodyDef ",
							this.defName,
							" has BodyPartRecord of ",
							bodyPartRecord.def.defName,
							" whose children have more or equal coverage than 100% (",
							(num * 100f).ToString("0.00"),
							"%)"
						}), false);
					}
				}
			}
			List<BodyPartRecord>.Enumerator enumerator2 = default(List<BodyPartRecord>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0008FA34 File Offset: 0x0008DC34
		public override void ResolveReferences()
		{
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0f)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0008FAA4 File Offset: 0x0008DCA4
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + this, false);
				return;
			}
			node.body = this;
			for (int i = 0; i < node.parts.Count; i++)
			{
				node.parts[i].parent = node;
			}
			if (node.parent != null)
			{
				node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
			}
			else
			{
				node.coverageAbsWithChildren = 1f;
			}
			float num = 1f;
			for (int j = 0; j < node.parts.Count; j++)
			{
				num -= node.parts[j].coverage;
			}
			if (Mathf.Abs(num) < 1E-05f)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				num = 0f;
			}
			node.coverageAbs = node.coverageAbsWithChildren * num;
			if (node.height == BodyPartHeight.Undefined)
			{
				node.height = BodyPartHeight.Middle;
			}
			if (node.depth == BodyPartDepth.Undefined)
			{
				node.depth = BodyPartDepth.Outside;
			}
			for (int k = 0; k < node.parts.Count; k++)
			{
				if (node.parts[k].height == BodyPartHeight.Undefined)
				{
					node.parts[k].height = node.height;
				}
				if (node.parts[k].depth == BodyPartDepth.Undefined)
				{
					node.parts[k].depth = node.depth;
				}
			}
			this.cachedAllParts.Add(node);
			for (int l = 0; l < node.parts.Count; l++)
			{
				this.CacheDataRecursive(node.parts[l]);
			}
		}

		// Token: 0x04000368 RID: 872
		public BodyPartRecord corePart;

		// Token: 0x04000369 RID: 873
		[Unsaved(false)]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		// Token: 0x0400036A RID: 874
		[Unsaved(false)]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;
	}
}
