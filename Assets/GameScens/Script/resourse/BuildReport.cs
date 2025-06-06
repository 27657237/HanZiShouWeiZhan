using System.Collections.Generic;

using System.Xml;

/// <summary>
/// 构建报告类
/// </summary>
public class BuildReport
{
    public int InternalResourceVersion { get; set; }
    public string ApplicableGameVersion { get; set; }
    public List<ResourceInfo> Resources { get; set; } 

    /// <summary>
    /// 从XML解析构建报告
    /// </summary>
    public static BuildReport ParseFromXml(string xmlContent)
    {
        // 去除可能的BOM
        if (xmlContent.StartsWith("\uFEFF"))
        {
            xmlContent = xmlContent.Substring(1);
        }

        var report = new BuildReport();
        var doc = new XmlDocument();
        doc.LoadXml(xmlContent);

        // 解析摘要信息
        var summaryNode = doc.SelectSingleNode("//BuildReport/Summary");
        if (summaryNode != null)
        {
            var versionNode = summaryNode.SelectSingleNode("InternalResourceVersion");
            if (versionNode != null && int.TryParse(versionNode.InnerText, out var version))
            {
                report.InternalResourceVersion = version;
            }

            var gameVersionNode = summaryNode.SelectSingleNode("ApplicableGameVersion");
            if (gameVersionNode != null)
            {
                report.ApplicableGameVersion = gameVersionNode.InnerText;
            }
        }

        // 解析资源信息
        var resourcesNode = doc.SelectSingleNode("//BuildReport/Resources");
        if (resourcesNode != null)
        {
            foreach (XmlNode resourceNode in resourcesNode.SelectNodes("Resource"))
            {
                var resource = new ResourceInfo
                {
                    Name = resourceNode.Attributes["Name"]?.Value,
                    Extension = resourceNode.Attributes["Extension"]?.Value,
                    LoadType = int.TryParse(resourceNode.Attributes["LoadType"]?.Value, out var loadType) ? loadType : 0,
                    Packed = bool.TryParse(resourceNode.Attributes["Packed"]?.Value, out var packed) && packed
                };

                // 获取资源哈希值
                var webGLNode = resourceNode.SelectSingleNode("Codes/WebGL");
                if (webGLNode != null)
                {
                    resource.HashCode = webGLNode.Attributes["HashCode"]?.Value;
                }

                if (!string.IsNullOrEmpty(resource.Name)
                    && !string.IsNullOrEmpty(resource.HashCode))
                {
                    report.Resources.Add(resource);
                }
            }
        }

        return report;
    }
}