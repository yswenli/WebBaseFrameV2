/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common
 *文件名：  HighchartsHelper
 *版本号：  V1.0.0.0
 *唯一标识：e41eea13-882f-4c5b-b03c-410caa98ce11
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/7/15 10:57:12
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/15 10:57:12
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
    public class HighchartsHelper
    {
        #region private

        public static string highchartsJsSrc =
            @"<script src='/Areas/Admin/Content/js/highcharts/highcharts.js'></script>
<script src='/Areas/Admin/Content/js/highcharts/modules/exporting.js'></script>";
        private const string Temp = @"
<script type='text/javascript'>
$(function () {{
    var chart;
var categories=[{3}];
    $(document).ready(function() {{
        chart = new Highcharts.Chart({{
            chart: {{
                renderTo: '{0}',
                margin: [ 50, 50, 100, 80]
            }},
            credits:{{enabled:false }},
            title: {{
                text: '{1}',
                x: -20 //center
            }},
            subtitle: {{
                text: '{2}',
                x: -20
            }},
            xAxis: [{{
                categories:categories ,
                labels: {{
                    rotation: -35,
                    align: 'right'
                    }},
			    reversed: false
            }}{10}],
            yAxis: {{
                title: {{
                    text: '{4}'
                }},
			    {9}
                plotLines: [{{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }}],
            stackLabels: {{
                    enabled: {5},
                    style: {{
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }}
                }}
            }},
            tooltip: {{
                formatter: function() {{
                        return {6};
                }}
            }},
            plotOptions: {{
                {7}: {{
                    stacking: 'normal',
                    dataLabels: {{
                        enabled: {11},
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                        formatter: function() {{
                            return '<b>'+ this.y +'</b> ';
                        }}
                    }}
                }},
                pie: {{
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {{
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        formatter: function() {{
                            return '<b>'+ this.point.name +'</b>: '+ Math.round(this.percentage*100)/100 +' %';
                        }}
                    }},showInLegend: true
                }}
            }},
            exporting: {{
            url: '~/HighchartsExport.axd',
            filename: 'MyChart',
            width: 1200
            }},
            legend: {{
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -10,
                y: 100,
                borderWidth: 0
            }},
            series: {8}
        }});
    }});
    
}});
		</script>
<div id='{0}' style='width: {12}; height: {13}; margin: 0 auto;float:left'></div>";

        private const string TempLevelsPie = @"<div id='{0}' style='min-width: 400px; height: 400px; margin: 0 auto'></div>
  <script>
  var chart;
$(document).ready(function() {{

	var colors = Highcharts.getOptions().colors,
		categories = [{3}],
		name = 'brands',
		data = [{5}];


	// Build the data arrays
	var categoryData = [];
	var subCategoryData = [];
	for (var i = 0; i < data.length; i++) {{

		// add browser data
		categoryData.push({{
			name: categories[i],
			y: data[i].y,
			color: data[i].color
		}});

		// add version data
		for (var j = 0; j < data[i].drilldown.data.length; j++) {{
			var brightness = 0.2 - (j / data[i].drilldown.data.length) / 5 ;
			subCategoryData.push({{
				name: data[i].drilldown.categories[j],
				y: data[i].drilldown.data[j],
				color: Highcharts.Color(data[i].color).brighten(brightness).get()
			}});
		}}
	}}

	// Create the chart
	chart = new Highcharts.Chart({{
		chart: {{
			renderTo: '{0}',
			type: 'pie'
		}},
        credits:{{enabled:false }},
		title: {{
			text: '{1}'
		}},
        subtitle: {{
			text: '{2}'
		}},
		yAxis: {{
			title: {{
				text: '{4}'
			}}
		}},
		plotOptions: {{
			pie: {{
				shadow: false
			}}
		}},
		tooltip: {{
			formatter: function() {{
				return '<b>'+ this.point.name +'</b>: '+this.y+' , '+ Math.round(this.percentage*100)/100 +' %';
			}}
		}},
		series: [{{
			name: 'Category',
			data: categoryData,
			size: '60%',
			dataLabels: {{
				formatter: function() {{
					return this.y > 0 ? this.point.name : null;
				}},
				color: 'white',
				distance: -30
			}}
		}}, {{
			name: 'SubCategory',
			data: subCategoryData,
			innerSize: '60%',
			dataLabels: {{
				formatter: function() {{
					// display only if larger than 1
					return this.y > 0 ? '<b>'+ this.point.name +':</b> '+ Math.round(this.percentage*100)/100 +'%' : null;
				}}
			}}
		}}]
	}});
}});
  </script>";
        private static HighchartsHelper _highchartsHelper;
        #endregion
        public static HighchartsHelper Helper()
        {
            return _highchartsHelper = _highchartsHelper ?? new HighchartsHelper();
        }

        /// <summary>
        /// 通过实体list生成报表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entityList">实体list</param>
        /// <param name="containerId">图表DIV的ID</param>
        /// <param name="highchartsType">图表类型</param>
        /// <param name="xCategoryColumnName">X轴标记列名</param>
        /// <param name="stackColumnName">分组列名，如果没有分组，则使用null</param>
        /// <param name="dataColumnName">数据列名</param>
        /// <param name="sortBy">按哪列排序</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="unit">单位</param>
        /// <param name="showStackTotal">是否显示堆数据的总和</param>
        /// <param name="stackFor">哪种类型的分组累计？对应图表类型就会出现分组累计效果，不对应则没有分组累计效果，比如像要3月上海北京的总数，则X轴是月份，分组列是大区，我们使用bar图表，分组累计类型也要用bar</param>
        /// <param name="needAbs">是否使用绝对值样式，可以让条形图左右（柱状图上下）分开显示不同组别的数据</param>
        /// <param name="showLabel">是否显示数据标签</param>
        /// <param name="drawTable">是否在图表下面画table</param>
        /// <param name="width">宽 例400px</param>
        /// <param name="height">高 例400px</param>
        /// <returns>highcharts的html代码</returns>
        public string FillHighchartsByEntityList<T>(IList<T> entityList, string containerId, HighchartsType highchartsType, string xCategoryColumnName
            , string stackColumnName, string dataColumnName, string sortBy, string title, string subTitle, string unit, string showStackTotal, string stackFor, bool needAbs, bool showLabel, bool drawTable, string width, string height)
        {
            var dataTable = new System.Data.DataTable();
            if (entityList.Count == 0)
            { return ""; }
            #region 获取类型的属性
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                { continue; }//去掉系统自带的属性
                dataTable.Columns.Add(propertyInfo.Name);
            }
            #endregion
            #region 把对象转换成datatable
            foreach (T entity in entityList)
            {
                DataRow data = dataTable.NewRow();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                    { continue; }//去掉系统自带的属性
                    data[propertyInfo.Name] = propertyInfo.GetValue(entity, null);
                }
                dataTable.Rows.Add(data);
            }
            #endregion
            return FillHighchartsByDataTable(dataTable, containerId, highchartsType, xCategoryColumnName, stackColumnName, dataColumnName, sortBy, title, subTitle, unit, showStackTotal, stackFor, needAbs, showLabel, drawTable, width, height);
        }
        /// <summary>
        /// 通过实体list生成报表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entityList">实体list</param>
        /// <param name="highchartsType">图表类型</param>
        /// <param name="xCategoryColumnName">X轴标记列名</param>
        /// <param name="stackColumnName">分组列名，如果没有分组，则使用null</param>
        /// <param name="dataColumnName">数据列名</param>
        /// <param name="sortBy">按哪列排序</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="unit">单位</param>
        /// <param name="showStackTotal">是否显示堆数据的总和</param>
        /// <param name="stackFor">哪种类型的分组累计？对应图表类型就会出现分组累计效果，不对应则没有分组累计效果，比如像要3月上海北京的总数，则X轴是月份，分组列是大区，我们使用bar图表，分组累计类型也要用bar</param>
        /// <param name="needAbs">是否使用绝对值样式，可以让条形图左右（柱状图上下）分开显示不同组别的数据</param>
        /// <param name="showLabel">是否显示数据标签</param>
        /// <param name="drawTable">是否在图表下面画table</param>
        /// <returns>highcharts的html代码</returns>
        public string FillHighchartsByEntityList<T>(IList<T> entityList, HighchartsType highchartsType, string xCategoryColumnName
           , string stackColumnName, string dataColumnName, string sortBy, string title, string subTitle, string unit, string showStackTotal, string stackFor, bool needAbs, bool showLabel, bool drawTable)
        {
            var dataTable = new System.Data.DataTable();
            if (entityList.Count == 0)
            { return ""; }
            #region 获取类型的属性
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                { continue; }//去掉系统自带的属性
                dataTable.Columns.Add(propertyInfo.Name);
            }
            #endregion
            #region 把对象转换成datatable
            foreach (T entity in entityList)
            {
                DataRow data = dataTable.NewRow();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                    { continue; }//去掉系统自带的属性
                    data[propertyInfo.Name] = propertyInfo.GetValue(entity, null);
                }
                dataTable.Rows.Add(data);
            }
            #endregion
            return FillHighchartsByDataTable(dataTable, "container", highchartsType, xCategoryColumnName, stackColumnName, dataColumnName, sortBy, title, subTitle, unit, showStackTotal, stackFor, needAbs, showLabel, drawTable, "400px", "400px");

        }

        /// <summary>
        /// 根据datatable画highcharts图表
        /// </summary>
        /// <param name="dataTable">数据源</param>
        /// <param name="containerId">图表DIV的ID</param>
        /// <param name="highchartsType">图表类型</param>
        /// <param name="xCategoryColumnName">X轴标记列名</param>
        /// <param name="stackColumnName">分组列名，如果没有分组，则使用null</param>
        /// <param name="dataColumnName">数据列名</param>
        /// <param name="sortBy">按哪列排序</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="unit">单位</param>
        /// <param name="showStackTotal">是否显示堆数据的总和</param>
        /// <param name="stackFor">哪种类型的分组累计？对应图表类型就会出现分组累计效果，不对应则没有分组累计效果，比如像要3月上海北京的总数，则X轴是月份，分组列是大区，我们使用bar图表，分组累计类型也要用bar</param>
        /// <param name="needAbs">是否使用绝对值样式，可以让条形图左右（柱状图上下）分开显示不同组别的数据</param>
        /// <param name="showLabel">是否显示数据标签</param>
        /// <param name="drawTable">是否在图表下面画table</param>
        /// <param name="width">宽 例400px</param>
        /// <param name="height">高 例400px</param>
        /// <returns>highcharts的html代码</returns>
        public string FillHighchartsByDataTable(System.Data.DataTable dataTable, string containerId, HighchartsType highchartsType, string xCategoryColumnName, string stackColumnName, string dataColumnName, string sortBy
            , string title, string subTitle, string unit, string showStackTotal, string stackFor, bool needAbs, bool showLabel, bool drawTable, string width, string height)
        {
            #region 准备数据
            var hsCategory = new HashSet<string>();
            var hsStack = new HashSet<string>();
            dataTable.DefaultView.Sort = sortBy;// xCategoryColumnName;//排序，让图表和table表按序排列
            for (int i = 0; i < dataTable.DefaultView.Count; i++)
            {
                string xName = dataTable.DefaultView[i][xCategoryColumnName].ToString();
                xName = xName == "" ? " " : xName;//如果分组数据为空字符串，则用空格代替，因为后面创建datatable的列的时候，空字符串会被自动改成Colum1，为了防止这种问题，这里暂时用空格代替
                if (!hsCategory.Contains(xName))
                {
                    hsCategory.Add(xName);//获取X轴数据列表
                }
                if (stackColumnName == null)//如果只有一行数据的，我们就传null进来，这里就会知道只保存一行数据
                {
                    if (!hsStack.Contains(null))
                        hsStack.Add(null);
                }
                else
                {
                    if (!hsStack.Contains(dataTable.DefaultView[i][stackColumnName].ToString()))
                    {
                        hsStack.Add(dataTable.DefaultView[i][stackColumnName].ToString());//如果X轴数据要堆分组的话，这里获取堆分组的数据列表
                    }
                }
            }

            string xcategory = "";//传给highcharts的X轴数据
            foreach (string hscate in hsCategory)
            {
                xcategory += "'" + hscate + "',";
            }
            xcategory = xcategory.Substring(0, xcategory.Length - 1);
            string series = "";
            string tooltip;
            if (highchartsType == HighchartsType.pie)//暂时这样处理，因为pie与其他类型的相关性不大，所以分开做series数据与tootip鼠标掩盖提示
            {
                #region pie类型
                series += "[";
                const string dataTemplate = "['{0}',{1}],";
                series += "{type: 'pie',name: 'Browser share',data: [";
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    series += string.Format(dataTemplate, dataRow[xCategoryColumnName], dataRow[dataColumnName]);
                }
                series = series.Substring(0, series.Length - 1) + "]}]";
                tooltip = "'<b>'+ this.point.name +'</b>: '+this.y+' , '+ Math.round(this.percentage*100)/100 +' %'";//这里鼠标提示类别名称，值，百分比
                #endregion
            }
            else
            {
                #region 非pie类型
                series += "[";
                foreach (string stack in hsStack)
                {
                    series += "{type:'" + highchartsType + "',name:'" + (stack ?? unit) + "',data:[";//如果没有分组数据，分组列名就用单位名称吧
                    foreach (string cate in hsCategory)
                    {
                        DataRow[] rows;
                        if (stack == null)
                        { rows = dataTable.Select(xCategoryColumnName + "='" + cate + "'"); }//没有堆分组的时候，只有一列数据
                        else
                        {
                            rows = dataTable.Select(stackColumnName + "='" + stack + "' and " + xCategoryColumnName + "='" + cate + "'");
                        }

                        if (rows.Length == 0)
                        { series += "0,"; }
                        else
                        { series += rows[0][dataColumnName] + ","; }
                    }
                    series = series.Substring(0, series.Length - 1);
                    series += "],stack:0},";
                }
                series = series.Substring(0, series.Length - 1) + "]";
                if (needAbs)
                { tooltip = "'<b>'+ this.series.name +'</b><br/>'+this.x +': '+ (Math.abs(this.y)) +'" + unit + "'"; }//使用绝对值
                else
                { tooltip = "'<b>'+ this.series.name +'</b><br/>'+this.x +': '+ this.y +'" + unit + "'"; }
                #endregion
            }
            string abs = "";
            string mirror = "";//目前做法是，如果选择使用了绝对值，则默认制造镜像效果，比如数轴左边是男，右边是女
            string dataLabel = showLabel ? "true" : "false";
            if (needAbs)
            {
                abs = @"labels: {
				    formatter: function(){
					    return (Math.abs(this.value));
				    }
			    },";
                mirror =
                    @", { // mirror axis on right side
			opposite: true,
			reversed: false,
			categories: categories,
			linkedTo: 0
		}";
            }
            #endregion
            string highcharts = FormatSimpleHighcharts(containerId, title, subTitle, xcategory, unit, showStackTotal, tooltip, stackFor, series, abs, mirror, dataLabel, width, height);
            StringBuilder sbTable = new StringBuilder();
            #region 画table
            if (drawTable)
            {
                #region 画表头,并重新做一个用来显示table用的临时数据表,给临时数据表添加列
                sbTable.AppendLine("<table id='tableContainer' class='list-table'>");//id固定是这个，为了将来导出excel做准备
                sbTable.AppendLine("<thead><tr><th>表头</th>");
                DataTable showTable = new DataTable();
                showTable.Columns.Add("stack");//默认添加一列分组数据
                foreach (string cate in hsCategory)
                {
                    showTable.Columns.Add(cate);//给新表添加列
                    sbTable.Append("<th>");
                    sbTable.Append(cate);
                    sbTable.Append("</th>");
                }
                sbTable.AppendLine("</tr></thead>");
                #endregion
                #region 给临时表添加数据
                foreach (string stack in hsStack)
                {
                    DataRow showRow = showTable.NewRow();
                    showRow["stack"] = stack;
                    foreach (string cate in hsCategory)
                    {
                        DataRow[] drs =
                            dataTable.Select(xCategoryColumnName + "='" + cate +
                                             (stack == null ? "" : ("' and " + stackColumnName + "='" + stack)) + "'");
                        string tableData = ""; //如果没数据，则添空字符串
                        if (drs.Length > 0)
                        {
                            tableData = drs[0][dataColumnName].ToString();
                            if (needAbs) //如果会用绝对值了，则表格也要使用绝对值
                                tableData = Math.Abs(Convert.ToDouble(tableData)).ToString();
                        }
                        showRow[cate] = tableData;
                    }
                    showTable.Rows.Add(showRow);
                }
                #endregion
                #region 把新表数据填充到字符串中
                int columnNumber = showTable.Columns.Count;
                foreach (DataRow dataRow in showTable.Rows)
                {
                    for (int i = 0; i < columnNumber; i++)
                    {
                        sbTable.Append("<td>");
                        sbTable.Append(dataRow[i].ToString());
                        sbTable.Append("</td>");
                    }
                    sbTable.AppendLine("</tr>");
                }
                sbTable.Append("</table>");
                #endregion
            }
            #endregion
            #region 原始表
            ////表格暂时没想到好办法
            //sbTable.AppendLine("<table border=1>");
            //int columnNumber = dataTable.Columns.Count;
            //foreach (DataRow dataRow in dataTable.Rows)
            //{
            //    sbTable.Append("<tr>");
            //    for (int i = 0; i < columnNumber; i++)
            //    {
            //        sbTable.Append("<td>");
            //        sbTable.Append(dataRow[i].ToString());
            //        sbTable.Append("</td>");
            //    }
            //    sbTable.AppendLine("</tr>");
            //}
            //sbTable.Append("</table>");
            #endregion
            return highcharts + sbTable;
        }
        /// <summary>
        /// 生成一个highcharts的图表
        /// </summary>
        /// <param name="containerId">图表DIV的ID</param>
        /// <param name="title">图表标题</param>
        /// <param name="subtitle">子标题</param>
        /// <param name="xAxisData">X轴数据，例子'北京','上海'</param>
        /// <param name="yAxisTitle">Y轴标题</param>
        /// <param name="showStackTotal">是否显示分类数据的总和</param>
        /// <param name="tooltip">鼠标提示，bar例子'<b>'+ this.series.name +'</b><br/>'+this.x +': '+ this.y +'人' ，pie例子'<b>'+ this.point.name +'</b>: '+ this.percentage +' %'</param>
        /// <param name="stackFor">给谁做堆数据，例子scatter，line，spline，area，areaspline，column，bar，pie，series，如果不想堆数据，可以选择一个不是当前图表类型的并且也不是series，因为series意思是对所有类型做堆数据</param>
        /// <param name="series">数据，bar例子{name: '北京',data: [70, 69]}, {name: '上海',data: [20, 18]} ，pie例子{type: 'pie',name: 'Browser share',data: [['Firefox',   45.0],['IE',       26.8],{name: 'Chrome',y: 12.8,sliced: true,selected: true},['Safari',    8.5],['Opera',     6.2],['Others',   0.7]]}</param>
        /// <param name="abs">使用绝对值样式</param>
        /// <param name="mirror">镜像数据，，可以让条形图左右（柱状图上下）分开显示不同组别的数据</param>
        /// <param name="dataLabel">数据标签样子</param>
        /// <param name="width">宽 例400px</param>
        /// <param name="height">高 例400px</param>
        /// <returns>highcharts的html代码</returns> 
        public string FormatSimpleHighcharts(string containerId, string title, string subtitle, string xAxisData, string yAxisTitle, string showStackTotal, string tooltip, string stackFor, string series, string abs, string mirror, string dataLabel, string width, string height)
        {
            string result = string.Format(Temp, containerId, title, subtitle, xAxisData, yAxisTitle, showStackTotal, tooltip, stackFor, series, abs, mirror, dataLabel, width, height);
            return result;
        }
        /// <summary>
        /// 生成highcharts的2级饼图，暂时独立代码与其他图使用的模板不同
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="containerId">图表DIV的ID</param>
        /// <param name="entityList">实体ilist</param>
        /// <param name="xCategoryColumnName">X轴的数据列名</param>
        /// <param name="stackColumnName">堆数据的列名，或者叫做分组的列名</param>
        /// <param name="dataColumnName">数据所在列的列名</param>
        /// <param name="sortBy">按哪个列名排序，通常是与x轴为相同的，饼图似乎没太大意义，总之先放这里好了</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="unit">数据单位</param>
        /// <param name="drawTable">是否在图表后跟table表格</param>
        /// <returns>highcharts的html代码</returns>
        public string FillLevelsPieHighchartsByEntityList<T>(IList<T> entityList, string containerId, string xCategoryColumnName
            , string stackColumnName, string dataColumnName, string sortBy, string title, string subTitle, string unit, bool drawTable)
        {
            var dataTable = new System.Data.DataTable();
            if (entityList.Count == 0)
            { return ""; }
            #region 获取类型的属性
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                { continue; }//去掉系统自带的属性
                dataTable.Columns.Add(propertyInfo.Name);
            }
            #endregion
            #region 把对象转换成datatable
            foreach (T entity in entityList)
            {
                DataRow data = dataTable.NewRow();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                    { continue; }//去掉系统自带的属性
                    data[propertyInfo.Name] = propertyInfo.GetValue(entity, null);
                }
                dataTable.Rows.Add(data);
            }
            #endregion
            return FillLevelsPieHighchartsByDataTable(dataTable, containerId, xCategoryColumnName, stackColumnName, dataColumnName, sortBy, title, subTitle, unit, drawTable);
        }
        /// <summary>
        /// 生成highcharts的2级饼图，暂时独立代码与其他图使用的模板不同
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="entityList">实体ilist</param>
        /// <param name="xCategoryColumnName">X轴的数据列名</param>
        /// <param name="stackColumnName">堆数据的列名，或者叫做分组的列名</param>
        /// <param name="dataColumnName">数据所在列的列名</param>
        /// <param name="sortBy">按哪个列名排序，通常是与x轴为相同的，饼图似乎没太大意义，总之先放这里好了</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="unit">数据单位</param>
        /// <param name="drawTable">是否在图表后跟table表格</param>
        /// <returns>highcharts的html代码</returns>
        public string FillLevelsPieHighchartsByEntityList<T>(IList<T> entityList, string xCategoryColumnName
            , string stackColumnName, string dataColumnName, string sortBy, string title, string subTitle, string unit, bool drawTable)
        {
            var dataTable = new System.Data.DataTable();
            if (entityList.Count == 0)
            { return ""; }
            #region 获取类型的属性
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                { continue; }//去掉系统自带的属性
                dataTable.Columns.Add(propertyInfo.Name);
            }
            #endregion
            #region 把对象转换成datatable
            foreach (T entity in entityList)
            {
                DataRow data = dataTable.NewRow();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType.FullName == "System.Data.EntityState" || propertyInfo.PropertyType.FullName == "System.Data.EntityKey")
                    { continue; }//去掉系统自带的属性
                    data[propertyInfo.Name] = propertyInfo.GetValue(entity, null);
                }
                dataTable.Rows.Add(data);
            }
            #endregion
            return FillLevelsPieHighchartsByDataTable(dataTable, "container", xCategoryColumnName, stackColumnName, dataColumnName, sortBy, title, subTitle, unit, drawTable);
        }
        /// <summary>
        /// 生成highcharts的2级饼图，暂时独立代码与其他图使用的模板不同
        /// </summary>
        /// <param name="dataTable">数据源</param>
        /// <param name="containerId">图表DIV的ID</param>
        /// <param name="xCategoryColumnName">X轴的数据列名</param>
        /// <param name="stackColumnName">堆数据的列名，或者叫做分组的列名</param>
        /// <param name="dataColumnName">数据所在列的列名</param>
        /// <param name="sortBy">按哪个列名排序，通常是与x轴为相同的，饼图似乎没太大意义，总之先放这里好了</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="unit">数据单位</param>
        /// <param name="drawTable">是否在图表后跟table表格</param>
        /// <returns>highcharts的html代码</returns>
        public string FillLevelsPieHighchartsByDataTable(System.Data.DataTable dataTable, string containerId, string xCategoryColumnName, string stackColumnName
            , string dataColumnName, string sortBy, string title, string subTitle, string unit, bool drawTable)
        {
            Dictionary<string, Dictionary<string, string>> cateDic = new Dictionary<string, Dictionary<string, string>>();
            HashSet<string> hsStack = new HashSet<string>();
            dataTable.DefaultView.Sort = sortBy;// xCategoryColumnName;//排序，让图表和table表按序排列
            for (int i = 0; i < dataTable.DefaultView.Count; i++)
            {
                string cate = dataTable.DefaultView[i][xCategoryColumnName].ToString();
                cate = cate == "" ? " " : cate;//如果分组数据为空字符串，则用空格代替，因为后面创建datatable的列的时候，空字符串会被自动改成Colum1，为了防止这种问题，这里暂时用空格代替
                if (!cateDic.Keys.Contains(cate))
                {
                    cateDic.Add(cate, new Dictionary<string, string>());//获取X轴数据列表
                }
                if (!cateDic[cate].Keys.Contains(dataTable.DefaultView[i][stackColumnName].ToString()))
                {
                    cateDic[cate].Add(dataTable.DefaultView[i][stackColumnName].ToString(), dataTable.DefaultView[i][dataColumnName].ToString());//获取X轴数据列下面的子列的列表
                }
                if (!hsStack.Contains(dataTable.DefaultView[i][stackColumnName].ToString()))
                {
                    hsStack.Add(dataTable.DefaultView[i][stackColumnName].ToString());//为后面画table准备X轴子数据列的数据列表
                }
            }
            const string data = @"{{
				y: {0},
				color: colors[{4}],
				drilldown: {{
					name: '{1}',
					categories: [{2}],
					data: [{3}],
					color: colors[{4}]
				}}
			}},";
            string dataAll = "";
            string xCate = "";
            int colorId = 0;//每组数据的颜色
            foreach (string key in cateDic.Keys)
            {
                double y = 0;
                xCate += "'" + key + "',";
                string subCates = "";//子数据的名字
                string subValues = "";//子数据的值
                foreach (string subCate in cateDic[key].Keys)
                {
                    subCates += "'" + subCate + "',";
                    subValues += cateDic[key][subCate] + ",";
                    y += Convert.ToDouble(cateDic[key][subCate]);//X轴数据值
                }
                subCates = subCates.Substring(0, subCates.Length - 1);
                subValues = subValues.Substring(0, subValues.Length - 1);
                dataAll += string.Format(data, y, key, subCates, subValues, colorId);
                colorId++;
            }
            xCate = xCate.Substring(0, xCate.Length - 1);
            dataAll = dataAll.Substring(0, dataAll.Length - 1);

            string highcharts = FormatLevelsPieHighcharts(containerId, title, subTitle, xCate, unit, dataAll);
            StringBuilder sbTable = new StringBuilder();
            #region 画table
            if (drawTable)
            {
                sbTable.AppendLine("<table id='tableContainer' class='list-table'>");//id固定是这个，为了将来导出excel做准备
                sbTable.AppendLine("<thead><tr><th>表头</th>");
                DataTable showTable = new DataTable();
                showTable.Columns.Add("stack");
                foreach (string cate in cateDic.Keys)
                {
                    showTable.Columns.Add(cate);
                    sbTable.Append("<th>");
                    sbTable.Append(cate);
                    sbTable.Append("</th>");
                }
                sbTable.AppendLine("</tr></thead>");
                foreach (string stack in hsStack)
                {
                    DataRow showRow = showTable.NewRow();
                    showRow["stack"] = stack;
                    foreach (string cate in cateDic.Keys)
                    {
                        DataRow[] drs =
                            dataTable.Select(xCategoryColumnName + "='" + cate + "' and " + stackColumnName + "='" + stack + "'");
                        string tableData = ""; //如果没数据，则添空字符串
                        if (drs.Length > 0)
                        {
                            tableData = drs[0][dataColumnName].ToString();
                        }
                        showRow[cate] = tableData;
                    }
                    showTable.Rows.Add(showRow);
                }

                int columnNumber = showTable.Columns.Count;
                foreach (DataRow dataRow in showTable.Rows)
                {
                    for (int i = 0; i < columnNumber; i++)
                    {
                        sbTable.Append("<td>");
                        sbTable.Append(dataRow[i].ToString());
                        sbTable.Append("</td>");
                    }
                    sbTable.AppendLine("</tr>");
                }
                sbTable.Append("</table>");
            }
            #endregion
            return highcharts + sbTable;
        }
        /// <summary>
        /// 生成highcharts的2级饼图，暂时独立代码与其他图使用的模板不同
        /// </summary>
        /// <param name="containerId">图表DIV的ID</param>
        /// <param name="title">标题</param>
        /// <param name="subTitle">子标题</param>
        /// <param name="xCate">X轴字段</param>
        /// <param name="unit">数据单位</param>
        /// <param name="dataAll">数据字符</param>
        /// <returns>highcharts的html代码</returns>
        public string FormatLevelsPieHighcharts(string containerId, string title, string subTitle, string xCate, string unit, string dataAll)
        {
            string result = string.Format(TempLevelsPie, containerId, title, subTitle, xCate, unit, dataAll);
            return result;
        }
    }
    /// <summary>
    /// 图表类型
    /// </summary>
    public enum HighchartsType
    {
        line, spline, area, areaspline, column, bar, pie, scatter

    }
}
