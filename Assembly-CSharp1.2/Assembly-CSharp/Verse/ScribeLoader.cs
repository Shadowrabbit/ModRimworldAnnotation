﻿using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020004AF RID: 1199
	public class ScribeLoader
	{
		// Token: 0x06001DDB RID: 7643 RVA: 0x000F874C File Offset: 0x000F694C
		public void InitLoading(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoading() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			if (this.curParent != null)
			{
				Log.Error("Current parent is not null in InitLoading", false);
				this.curParent = null;
			}
			if (this.curPathRelToParent != null)
			{
				Log.Error("Current path relative to parent is not null in InitLoading", false);
				this.curPathRelToParent = null;
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						this.curXmlParent = xmlDocument.DocumentElement;
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading file: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000F8858 File Offset: 0x000F6A58
		public void InitLoadingMetaHeaderOnly(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoadingMetaHeaderOnly() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							using (XmlReader xmlReader = xmlTextReader.ReadSubtree())
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.Load(xmlReader);
								XmlElement xmlElement = xmlDocument.CreateElement("root");
								xmlElement.AppendChild(xmlDocument.DocumentElement);
								this.curXmlParent = xmlElement;
								goto IL_82;
							}
							goto IL_80;
							IL_82:
							goto IL_8E;
						}
						IL_80:
						return;
					}
					IL_8E:;
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading meta header: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000F8970 File Offset: 0x000F6B70
		public void FinalizeLoading()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called FinalizeLoading() but current mode is " + Scribe.mode, false);
				return;
			}
			try
			{
				Scribe.ExitNode();
				this.curXmlParent = null;
				this.curParent = null;
				this.curPathRelToParent = null;
				Scribe.mode = LoadSaveMode.Inactive;
				this.crossRefs.ResolveAllCrossReferences();
				this.initer.DoAllPostLoadInits();
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg, false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000F8A04 File Offset: 0x000F6C04
		public bool EnterNode(string nodeName)
		{
			if (this.curXmlParent != null)
			{
				XmlNode xmlNode = this.curXmlParent[nodeName];
				if (xmlNode == null && char.IsDigit(nodeName[0]))
				{
					xmlNode = this.curXmlParent.ChildNodes[int.Parse(nodeName)];
				}
				if (xmlNode == null)
				{
					return false;
				}
				this.curXmlParent = xmlNode;
			}
			this.curPathRelToParent = this.curPathRelToParent + "/" + nodeName;
			return true;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000F8A74 File Offset: 0x000F6C74
		public void ExitNode()
		{
			if (this.curXmlParent != null)
			{
				this.curXmlParent = this.curXmlParent.ParentNode;
			}
			if (this.curPathRelToParent != null)
			{
				int num = this.curPathRelToParent.LastIndexOf('/');
				this.curPathRelToParent = ((num > 0) ? this.curPathRelToParent.Substring(0, num) : null);
			}
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000F8ACC File Offset: 0x000F6CCC
		public void ForceStop()
		{
			this.curXmlParent = null;
			this.curParent = null;
			this.curPathRelToParent = null;
			this.crossRefs.Clear(false);
			this.initer.Clear();
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04001548 RID: 5448
		public CrossRefHandler crossRefs = new CrossRefHandler();

		// Token: 0x04001549 RID: 5449
		public PostLoadIniter initer = new PostLoadIniter();

		// Token: 0x0400154A RID: 5450
		public IExposable curParent;

		// Token: 0x0400154B RID: 5451
		public XmlNode curXmlParent;

		// Token: 0x0400154C RID: 5452
		public string curPathRelToParent;
	}
}
