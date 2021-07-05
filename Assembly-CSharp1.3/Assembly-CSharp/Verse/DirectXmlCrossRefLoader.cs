using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000310 RID: 784
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x00083540 File Offset: 0x00081740
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00083550 File Offset: 0x00081750
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, Type assumeFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object, FieldInfo, string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName, mayRequireMod, assumeFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00083598 File Offset: 0x00081798
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName, string mayRequireMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName, mayRequireMod, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000835EC File Offset: 0x000817EC
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, XmlNode parentNode, string mayRequireMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,XmlNode)");
			try
			{
				string text = mayRequireMod;
				if (mayRequireMod == null)
				{
					XmlAttributeCollection attributes = parentNode.Attributes;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
				}
				string mayRequireMod2 = text;
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), parentNode.Name, mayRequireMod2, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00083670 File Offset: 0x00081870
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null, string mayRequireMod = null)
		{
			DeepProfiler.Start("RegisterListWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterList, out wantedRef))
				{
					wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterList, wantedRefForList);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
				}
				else
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
				}
				wantedRefForList.AddWantedListEntry(targetDefName, mayRequireMod);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000836E0 File Offset: 0x000818E0
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("RegisterDictionaryWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterDict, out wantedRef))
				{
					wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterDict, wantedRefForDictionary);
				}
				else
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
				}
				wantedRefForDictionary.AddWantedDictEntry(entryNode);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00083750 File Offset: 0x00081950
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("TryResolveDef");
			T result;
			try
			{
				T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (failReportMode == FailMode.LogErrors)
					{
						string text = string.Concat(new object[]
						{
							"Could not resolve cross-reference to ",
							typeof(T),
							" named ",
							defName
						});
						if (debugWanterInfo != null)
						{
							text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
						}
						Log.Error(text);
					}
					result = default(T);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00083800 File Offset: 0x00081A00
		public static void Clear()
		{
			DeepProfiler.Start("Clear");
			try
			{
				DirectXmlCrossRefLoader.wantedRefs.Clear();
				DirectXmlCrossRefLoader.wantedListDictRefs.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00083844 File Offset: 0x00081A44
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			DeepProfiler.Start("ResolveAllWantedCrossReferences");
			try
			{
				HashSet<DirectXmlCrossRefLoader.WantedRef> resolvedRefs = new HashSet<DirectXmlCrossRefLoader.WantedRef>();
				object resolvedRefsLock = new object();
				DeepProfiler.enabled = false;
				GenThreading.ParallelForEach<DirectXmlCrossRefLoader.WantedRef>(DirectXmlCrossRefLoader.wantedRefs, delegate(DirectXmlCrossRefLoader.WantedRef wantedRef)
				{
					if (wantedRef.TryResolve(failReportMode))
					{
						object resolvedRefsLock = resolvedRefsLock;
						lock (resolvedRefsLock)
						{
							resolvedRefs.Add(wantedRef);
						}
					}
				}, -1);
				foreach (DirectXmlCrossRefLoader.WantedRef wantedRef2 in resolvedRefs)
				{
					wantedRef2.Apply();
				}
				DirectXmlCrossRefLoader.wantedRefs.RemoveAll((DirectXmlCrossRefLoader.WantedRef x) => resolvedRefs.Contains(x));
				DeepProfiler.enabled = true;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x04000FB5 RID: 4021
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x04000FB6 RID: 4022
		private static Dictionary<object, DirectXmlCrossRefLoader.WantedRef> wantedListDictRefs = new Dictionary<object, DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x02001A43 RID: 6723
		private abstract class WantedRef
		{
			// Token: 0x06009C41 RID: 40001
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x06009C42 RID: 40002 RVA: 0x0000313F File Offset: 0x0000133F
			public virtual void Apply()
			{
			}

			// Token: 0x0400648D RID: 25741
			public object wanter;
		}

		// Token: 0x02001A44 RID: 6724
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x170019A3 RID: 6563
			// (get) Token: 0x06009C44 RID: 40004 RVA: 0x00368E9F File Offset: 0x0036709F
			private bool BadCrossRefAllowed
			{
				get
				{
					return !this.mayRequireMod.NullOrEmpty() && !ModsConfig.IsActive(this.mayRequireMod);
				}
			}

			// Token: 0x06009C45 RID: 40005 RVA: 0x00368EBE File Offset: 0x003670BE
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, Type overrideFieldType = null)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
				this.mayRequireMod = mayRequireMod;
				this.overrideFieldType = overrideFieldType;
			}

			// Token: 0x06009C46 RID: 40006 RVA: 0x00368EEC File Offset: 0x003670EC
			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>());
					return false;
				}
				Type type = this.overrideFieldType ?? this.fi.FieldType;
				this.resolvedDef = GenDefDatabase.GetDefSilentFail(type, this.defName, true);
				if (this.resolvedDef == null)
				{
					if (failReportMode == FailMode.LogErrors && !this.BadCrossRefAllowed)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not resolve cross-reference: No ",
							type,
							" named ",
							this.defName.ToStringSafe<string>(),
							" found to give to ",
							this.wanter.GetType(),
							" ",
							this.wanter.ToStringSafe<object>()
						}));
					}
					return false;
				}
				SoundDef soundDef = this.resolvedDef as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Could not resolve cross-reference: No ",
						type,
						" named ",
						this.defName.ToStringSafe<string>(),
						" found to give to ",
						this.wanter.GetType(),
						" ",
						this.wanter.ToStringSafe<object>(),
						" (using undefined sound instead)"
					}));
				}
				this.fi.SetValue(this.wanter, this.resolvedDef);
				return true;
			}

			// Token: 0x0400648E RID: 25742
			public FieldInfo fi;

			// Token: 0x0400648F RID: 25743
			public string defName;

			// Token: 0x04006490 RID: 25744
			public Def resolvedDef;

			// Token: 0x04006491 RID: 25745
			public string mayRequireMod;

			// Token: 0x04006492 RID: 25746
			public Type overrideFieldType;
		}

		// Token: 0x02001A45 RID: 6725
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06009C47 RID: 40007 RVA: 0x00369056 File Offset: 0x00367256
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06009C48 RID: 40008 RVA: 0x00369078 File Offset: 0x00367278
			public void AddWantedListEntry(string newTargetDefName, string mayRequireMod = null)
			{
				if (!mayRequireMod.NullOrEmpty() && this.mayRequireMods == null)
				{
					this.mayRequireMods = new List<string>();
					for (int i = 0; i < this.defNames.Count; i++)
					{
						this.mayRequireMods.Add(null);
					}
				}
				this.defNames.Add(newTargetDefName);
				if (this.mayRequireMods != null)
				{
					this.mayRequireMods.Add(mayRequireMod);
				}
			}

			// Token: 0x06009C49 RID: 40009 RVA: 0x003690E4 File Offset: 0x003672E4
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					bool flag2 = this.mayRequireMods != null && i < this.mayRequireMods.Count && !this.mayRequireMods[i].NullOrEmpty() && !ModsConfig.IsActive(this.mayRequireMods[i]);
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], flag2 ? FailMode.Silent : failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
						this.defNames.RemoveAt(i);
						if (this.mayRequireMods != null && i < this.mayRequireMods.Count)
						{
							this.mayRequireMods.RemoveAt(i);
						}
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}

			// Token: 0x04006493 RID: 25747
			private List<string> defNames = new List<string>();

			// Token: 0x04006494 RID: 25748
			private List<string> mayRequireMods;

			// Token: 0x04006495 RID: 25749
			private object debugWanterInfo;
		}

		// Token: 0x02001A46 RID: 6726
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06009C4A RID: 40010 RVA: 0x003691C2 File Offset: 0x003673C2
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06009C4B RID: 40011 RVA: 0x003691EE File Offset: 0x003673EE
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x06009C4C RID: 40012 RVA: 0x003691FC File Offset: 0x003673FC
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					object first;
					if (flag)
					{
						first = DirectXmlCrossRefLoader.TryResolveDef<K>(xmlNode2.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						first = xmlNode2;
					}
					object second;
					if (flag2)
					{
						second = DirectXmlCrossRefLoader.TryResolveDef<V>(xmlNode3.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						second = xmlNode3;
					}
					this.makingData.Add(new Pair<object, object>(first, second));
				}
				return true;
			}

			// Token: 0x06009C4D RID: 40013 RVA: 0x003692F0 File Offset: 0x003674F0
			public override void Apply()
			{
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<object, object> pair in this.makingData)
				{
					try
					{
						object obj = pair.First;
						object obj2 = pair.Second;
						if (obj is XmlNode)
						{
							obj = DirectXmlToObject.ObjectFromXml<K>(obj as XmlNode, true);
						}
						if (obj2 is XmlNode)
						{
							obj2 = DirectXmlToObject.ObjectFromXml<V>(obj2 as XmlNode, true);
						}
						dictionary.Add((K)((object)obj), (V)((object)obj2));
					}
					catch
					{
						Log.Error(string.Concat(new object[]
						{
							"Failed to load key/value pair: ",
							pair.First,
							", ",
							pair.Second
						}));
					}
				}
			}

			// Token: 0x04006496 RID: 25750
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x04006497 RID: 25751
			private object debugWanterInfo;

			// Token: 0x04006498 RID: 25752
			private List<Pair<object, object>> makingData = new List<Pair<object, object>>();
		}
	}
}
