using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000083 RID: 131
	public class BodyDef : Def
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000199DA File Offset: 0x00017BDA
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x000199E2 File Offset: 0x00017BE2
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x000199EA File Offset: 0x00017BEA
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

		// Token: 0x060004D8 RID: 1240 RVA: 0x00019A01 File Offset: 0x00017C01
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

		// Token: 0x060004D9 RID: 1241 RVA: 0x00019A18 File Offset: 0x00017C18
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

		// Token: 0x060004DA RID: 1242 RVA: 0x00019A5C File Offset: 0x00017C5C
		public BodyPartRecord GetPartAtIndex(int index)
		{
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				return null;
			}
			return this.cachedAllParts[index];
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00019A80 File Offset: 0x00017C80
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

		// Token: 0x060004DC RID: 1244 RVA: 0x00019AB5 File Offset: 0x00017CB5
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
						}));
					}
				}
			}
			List<BodyPartRecord>.Enumerator enumerator2 = default(List<BodyPartRecord>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00019AC8 File Offset: 0x00017CC8
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

		// Token: 0x060004DE RID: 1246 RVA: 0x00019B38 File Offset: 0x00017D38
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + this);
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

		// Token: 0x040001BA RID: 442
		public BodyPartRecord corePart;

		// Token: 0x040001BB RID: 443
		[Unsaved(false)]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		// Token: 0x040001BC RID: 444
		[Unsaved(false)]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;
	}
}
