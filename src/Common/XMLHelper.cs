using System.Data;
//
using System.Xml;


namespace Common
{
    /// <summary>
    /// 辅助操作XML
    /// </summary>
    public class XMLHelper
    {
        XmlDocument xmlDoc = new XmlDocument();
        string _path = "";

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="xmlPath"></param>
        public XMLHelper(string xmlPath)
        {
            _path = xmlPath;
            if (!FileHelper.IsFileExists(_path))
            {
                FileHelper.WriteFile(_path, "");
            }
            xmlDoc.Load(_path);
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Append(XmlElement element)
        {
            return Append(element);
        }
        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Append(XmlNode node)
        {
            bool result = false;
            try
            {
                xmlDoc.AppendChild(node);
                xmlDoc.Save(_path);
                result = true;
            }
            catch { }
            return result;
        }


        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xpath">xpath 表达式：descendant::bk:book[bk:author/bk:last-name='Kingsolver']</param>
        /// <returns></returns>
        public XmlNode GetNode(string xpath)
        {
            try
            {
                XmlNode node = xmlDoc.SelectSingleNode(xpath);
                return node;
            }
            catch { }
            return null;
        }
        /// <summary>
        /// 根据父节点获取节点
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlNode GetNode(XmlNode parent, string name)
        {
            try
            {
                foreach (XmlNode item in parent.ChildNodes)
                {
                    if (item.Name == name) return item;
                    if (item.HasChildNodes) return GetNode(item, name);
                }
            }
            catch { }
            return null;
        }
        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xpath"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Insert(string xpath, XmlNode node)
        {
            bool result = false;
            try
            {
                XmlNode parent = xmlDoc.SelectSingleNode(xpath);
                parent.AppendChild(node);
                xmlDoc.Save(_path);
                result = true;

            }
            catch { }
            return result;
        }
        /// <summary>
        /// 插入到节点的指定位置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xpath"></param>
        /// <param name="node"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Insert(string xpath, XmlNode node, int index)
        {
            bool result = false;
            try
            {
                XmlNode parent = xmlDoc.SelectSingleNode(xpath);
                parent.InsertBefore(node, parent.ChildNodes[index]);
                xmlDoc.Save(_path);
                result = true;
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Update(XmlNode node)
        {
            bool result = false;
            try
            {
                XmlNode selectNode = GetNode(xmlDoc.FirstChild, node.Name);
                selectNode = node;
                xmlDoc.Save(_path);
                result = true;
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 更新节点值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Update(string name, string value)
        {
            bool result = false;
            try
            {
                XmlNode selectNode = GetNode(xmlDoc.FirstChild, name);
                selectNode.Value = value;
                xmlDoc.Save(_path);
                result = true;
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Delete(XmlNode node)
        {
            bool result = false;
            try
            {
                XmlNode selectNode = GetNode(xmlDoc.FirstChild, node.Name);
                selectNode = node;
                selectNode.RemoveAll();
                xmlDoc.Save(_path);
                result = true;
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Delete(string name)
        {
            bool result = false;
            try
            {
                XmlNode selectNode = GetNode(xmlDoc.FirstChild, name);
                selectNode.RemoveAll();
                xmlDoc.Save(_path);
                result = true;
            }
            catch { }
            return result;
        }




        /// <summary>
        /// 从XML转换成DS
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataSet XMLToDataSet(XmlDocument doc, string path)
        {
            DataSet ds = new DataSet();
            try
            {
                doc.Save(path);
                ds.ReadXml(path);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 将DS转换成XmlDocument
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlDocument DataSetToXML(DataSet ds, string path)
        {
            
            ds.WriteXml(path);
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }



    }
}
