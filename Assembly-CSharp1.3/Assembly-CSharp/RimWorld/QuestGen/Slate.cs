using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001725 RID: 5925
	public class Slate
	{
		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06008896 RID: 34966 RVA: 0x003118F8 File Offset: 0x0030FAF8
		public string CurrentPrefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x06008897 RID: 34967 RVA: 0x00311900 File Offset: 0x0030FB00
		public T Get<T>(string name, T defaultValue = default(T), bool isAbsoluteName = false)
		{
			T result;
			if (this.TryGet<T>(name, out result, isAbsoluteName))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06008898 RID: 34968 RVA: 0x0031191C File Offset: 0x0030FB1C
		public bool TryGet<T>(string name, out T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				var = default(T);
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			object obj;
			if (!this.vars.TryGetValue(name, out obj))
			{
				var = default(T);
				return false;
			}
			if (obj == null)
			{
				var = default(T);
				return true;
			}
			if (obj is T)
			{
				var = (T)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<T>(obj))
			{
				var = ConvertHelper.Convert<T>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert slate variable \"",
				name,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(T).Name
			}));
			var = default(T);
			return false;
		}

		// Token: 0x06008899 RID: 34969 RVA: 0x00311A1C File Offset: 0x0030FC1C
		public void Set<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				Log.Error("Tried to set a variable with null name. var=" + var.ToStringSafe<T>());
				return;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			ISlateRef slateRef = var as ISlateRef;
			if (slateRef != null)
			{
				object value;
				slateRef.TryGetConvertedValue<object>(this, out value);
				this.vars[name] = value;
				return;
			}
			this.vars[name] = var;
		}

		// Token: 0x0600889A RID: 34970 RVA: 0x00311AAB File Offset: 0x0030FCAB
		public void SetIfNone<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (this.Exists(name, isAbsoluteName))
			{
				return;
			}
			this.Set<T>(name, var, isAbsoluteName);
		}

		// Token: 0x0600889B RID: 34971 RVA: 0x00311AC4 File Offset: 0x0030FCC4
		public bool Remove(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			return this.vars.Remove(name);
		}

		// Token: 0x0600889C RID: 34972 RVA: 0x00311B14 File Offset: 0x0030FD14
		public bool Exists(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			return this.vars.ContainsKey(name);
		}

		// Token: 0x0600889D RID: 34973 RVA: 0x00311B74 File Offset: 0x0030FD74
		private string TryResolveFirstAvailableName(string nameWithPrefix)
		{
			if (nameWithPrefix == null)
			{
				return null;
			}
			nameWithPrefix = QuestGenUtility.NormalizeVarPath(nameWithPrefix);
			if (this.vars.ContainsKey(nameWithPrefix))
			{
				return nameWithPrefix;
			}
			int num = nameWithPrefix.LastIndexOf('/');
			if (num < 0)
			{
				return nameWithPrefix;
			}
			string text = nameWithPrefix.Substring(num + 1);
			string text2 = nameWithPrefix.Substring(0, num);
			string text3;
			for (;;)
			{
				text3 = text;
				if (!text2.NullOrEmpty())
				{
					text3 = text2 + "/" + text3;
				}
				if (this.vars.ContainsKey(text3))
				{
					break;
				}
				if (text2.NullOrEmpty())
				{
					return nameWithPrefix;
				}
				int num2 = text2.LastIndexOf('/');
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				else
				{
					text2 = "";
				}
			}
			return text3;
		}

		// Token: 0x0600889E RID: 34974 RVA: 0x00311C14 File Offset: 0x0030FE14
		public void PushPrefix(string newPrefix, bool allowNonPrefixedLookup = false)
		{
			if (newPrefix.NullOrEmpty())
			{
				Log.Error("Tried to push a null prefix.");
				newPrefix = "unnamed";
			}
			if (!this.prefix.NullOrEmpty())
			{
				this.prefix += "/";
			}
			this.prefix += newPrefix;
			this.prevAllowNonPrefixedLookupStack.Push(this.allowNonPrefixedLookup);
			if (allowNonPrefixedLookup)
			{
				this.allowNonPrefixedLookup = true;
			}
		}

		// Token: 0x0600889F RID: 34975 RVA: 0x00311C8C File Offset: 0x0030FE8C
		public void PopPrefix()
		{
			int num = this.prefix.LastIndexOf('/');
			if (num >= 0)
			{
				this.prefix = this.prefix.Substring(0, num);
			}
			else
			{
				this.prefix = "";
			}
			if (this.prevAllowNonPrefixedLookupStack.Count != 0)
			{
				this.allowNonPrefixedLookup = this.prevAllowNonPrefixedLookupStack.Pop();
			}
		}

		// Token: 0x060088A0 RID: 34976 RVA: 0x00311CEC File Offset: 0x0030FEEC
		public Slate.VarRestoreInfo GetRestoreInfo(string name)
		{
			bool flag = this.allowNonPrefixedLookup;
			this.allowNonPrefixedLookup = false;
			Slate.VarRestoreInfo result;
			try
			{
				object value;
				bool exists = this.TryGet<object>(name, out value, false);
				result = new Slate.VarRestoreInfo(name, exists, value);
			}
			finally
			{
				this.allowNonPrefixedLookup = flag;
			}
			return result;
		}

		// Token: 0x060088A1 RID: 34977 RVA: 0x00311D38 File Offset: 0x0030FF38
		public void Restore(Slate.VarRestoreInfo varRestoreInfo)
		{
			if (varRestoreInfo.exists)
			{
				this.Set<object>(varRestoreInfo.name, varRestoreInfo.value, false);
				return;
			}
			this.Remove(varRestoreInfo.name, false);
		}

		// Token: 0x060088A2 RID: 34978 RVA: 0x00311D64 File Offset: 0x0030FF64
		public void SetAll(Slate otherSlate)
		{
			this.vars.Clear();
			foreach (KeyValuePair<string, object> keyValuePair in otherSlate.vars)
			{
				this.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060088A3 RID: 34979 RVA: 0x00311DD4 File Offset: 0x0030FFD4
		public void Reset()
		{
			this.vars.Clear();
		}

		// Token: 0x060088A4 RID: 34980 RVA: 0x00311DE4 File Offset: 0x0030FFE4
		public Slate DeepCopy()
		{
			Slate slate = new Slate();
			slate.prefix = this.prefix;
			foreach (KeyValuePair<string, object> keyValuePair in this.vars)
			{
				slate.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return slate;
		}

		// Token: 0x060088A5 RID: 34981 RVA: 0x00311E5C File Offset: 0x0031005C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in from x in this.vars
			orderby x.Key
			select x)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				string str = (keyValuePair.Value is IEnumerable && !(keyValuePair.Value is string)) ? ((IEnumerable)keyValuePair.Value).ToStringSafeEnumerable() : keyValuePair.Value.ToStringSafe<object>();
				stringBuilder.Append(keyValuePair.Key + "=" + str);
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append("(none)");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040056A6 RID: 22182
		private Dictionary<string, object> vars = new Dictionary<string, object>();

		// Token: 0x040056A7 RID: 22183
		private string prefix = "";

		// Token: 0x040056A8 RID: 22184
		private bool allowNonPrefixedLookup;

		// Token: 0x040056A9 RID: 22185
		private Stack<bool> prevAllowNonPrefixedLookupStack = new Stack<bool>();

		// Token: 0x040056AA RID: 22186
		public const char Separator = '/';

		// Token: 0x0200298F RID: 10639
		public struct VarRestoreInfo
		{
			// Token: 0x0600E1E4 RID: 57828 RVA: 0x004249F8 File Offset: 0x00422BF8
			public VarRestoreInfo(string name, bool exists, object value)
			{
				this.name = name;
				this.exists = exists;
				this.value = value;
			}

			// Token: 0x04009C30 RID: 39984
			public string name;

			// Token: 0x04009C31 RID: 39985
			public bool exists;

			// Token: 0x04009C32 RID: 39986
			public object value;
		}
	}
}
