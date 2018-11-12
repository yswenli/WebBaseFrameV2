using Common.Echarts.ChartData;
/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common
 *文件名：  EchartsHelper
 *版本号：  V1.0.0.0
 *唯一标识：8705414e-9e9c-4dad-a95c-7d5eee805db4
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/7/15 11:10:51
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/15 11:10:51
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Common
{
    public class EchartsHelper
    {
        private static string EchartsJsSrc =
            @"<script src='/Areas/Admin/Content/js/echarts/esl.js'></script>
    <script type='text/javascript'>
    $(function () {{
        require.config({{
            paths: {{
                'macarons': '/Areas/Admin/Content/js/echarts/theme/macarons',
                'echarts': '/Areas/Admin/Content/js/echarts/echarts'
            }}
        }});
        var currentTheme;
        require(['macarons'],function(ct){{
            currentTheme=ct;
            echartsCurrentTheme=ct;
        }});
        require(
            [
                'echarts',
                'echarts/chart/{0}'
            ],
            function (ec) {{
                // 基于准备好的dom，初始化echarts图表
                var myChart = ec.init(document.getElementById('echartstable'));

                var option = {1};

                // 为echarts对象加载数据 
                myChart.setOption(option);
                myChart.setTheme(currentTheme);
                echartsInstance=ec;
                myEcharts=myChart;
            }}
        );

    }});
    </script>";

        #region HorizontalBar

        private static string HorizontalBarOption = @" 
                {{
                    title: {{
                        text: '{0}',
                        subtext: '{1}',
                        x: 'center'
                    }},
                    tooltip: {{
                        trigger: 'axis'
                    }},
                    legend: {{
                        orient: 'horizontal',
                        x: 'left',
                        data: [{2}],
                    }},
                    toolbox: {{
                        show: true,
                        feature: {{
                            mark: {{ show: false }},
                            dataView: {{ show: false, readOnly: false }},
                            magicType: {{ show: true, type: ['line', 'bar'{5}] }},
                            restore: {{ show: false }},
                            saveAsImage: {{ show: true }}
                        }}
                    }},
                    calculable: true,
                    xAxis: [
                        {{
                            type: 'value'
                        }}
                    ],
                    yAxis: [
                        {{
                            type: 'category',
                            data: [{3}],
                            axisLabel: {{
                                rotate: 0 // 文字倾斜
                            }}
                        }}
                    ],
                    grid: {{
                        x: 160, // 图表距离Container的左边距
                        x2: 60, // 图表距离Container的右边距
                        y: 60, // 图片距离上方描述文字的距离 默认为60
                        y2: 60 // X轴距离Container底部的高度
                    }},
                    series: [{4}]
                }}";

        public static MvcHtmlString FillHorizontalBarEcharts(IList<BarChartsData> list, string text, string subtext)
        {
            return MvcHtmlString.Create(string.Format(EchartsJsSrc, "bar", FillHorizontalBarEchartsOption(list, text, subtext)));
        }

        public static MvcHtmlString FillHorizontalBarEcharts(IList<BaseChartsData> list, string text, string subtext, string legend)
        {
            return MvcHtmlString.Create(string.Format(EchartsJsSrc, "bar", FillHorizontalBarEchartsOption(list, text, subtext, legend)));
        }

        public static string FillHorizontalBarEchartsOption(IList<BarChartsData> list, string text, string subtext)
        {
            var yAxisData = string.Join(",", list.Select(c => "'" + c.X + "'").ToList());
            var seriesList = new List<string>();
            var legendDataList = new List<string>();
            foreach (var l in list.GroupBy(c => c.Legend))
            {
                legendDataList.Add("'" + l.Key + "'");
                seriesList.Add(string.Format("{{type:'bar',barWidth:30,name:'{0}',data:[{1}]}}", l.Key,
                                             string.Join(",", list.Where(c => c.Legend == l.Key).Select(c => c.Y))));
            }

            return string.Format(HorizontalBarOption, text, subtext, string.Join(",", legendDataList), yAxisData, string.Join(",", seriesList), "");
        }

        public static string FillHorizontalBarEchartsOption(IList<BaseChartsData> list, string text, string subtext, string legend)
        {
            if (list.Count == 0)
            {
                return string.Format(HorizontalBarOption, text, subtext, "'" + legend + "'", "''", string.Format("{{type:'bar',barWidth:30,name:'{0}',data:[{1}]}}", legend, 0), "");
            }
            var yAxisData = string.Join(",", list.Select(c => "'" + c.X + "'").ToList());
            var seriesList = string.Format(
                "{{type:'bar',barWidth:30,name:'{0}',data:[{1}]}}",
                legend, string.Join(",", list.Select(c => c.Y)));

            return string.Format(HorizontalBarOption, text, subtext, "'" + legend + "'", yAxisData, seriesList, "");
        }

        public static MvcHtmlString FillHorizontalBarStackEcharts(IList<BarStackChartsData> list, string text, string subtext)
        {
            return MvcHtmlString.Create(string.Format(EchartsJsSrc, "bar", FillHorizontalBarStackEchartsOption(list, text, subtext)));
        }

        public static string FillHorizontalBarStackEchartsOption(IList<BarStackChartsData> list, string text, string subtext)
        {
            if (list.Count == 0)
            {
                return string.Format(HorizontalBarOption, text, subtext, "''", "''", "{type:'bar',barWidth:30,name:'',data:['-']}", "");
            }
            var yAxisData = string.Join(",", list.Select(c => "'" + c.X + "'").Distinct().OrderByDescending(c => c).ToList());
            var seriesList = new List<string>();
            var legendDataList = new List<string>();
            var ls = (from l in list
                      group l by new { l.Legend, l.Stack } into l1
                      select new { l1.Key.Legend, l1.Key.Stack });

            foreach (var l in ls)
            {
                legendDataList.Add("'" + l.Legend + "'");
                seriesList.Add(string.Format("{{type:'bar',barWidth:30,name:'{0}',data:[{1}],stack :'{2}'}}", l.Legend,
                                             string.Join(",", list.Where(c => c.Legend == l.Legend && c.Stack == l.Stack).OrderByDescending(c => c.X).Select(c => c.Y)), l.Stack));
            }

            return string.Format(HorizontalBarOption, text, subtext, string.Join(",", legendDataList), yAxisData, string.Join(",", seriesList), ", 'stack', 'tiled'");
        }
        #endregion

        #region VerticalBar

        private static string VericalBarOptionSrc = @"               
                {{
                    title: {{
                        text: '{0}',
                        subtext: '{1}',
                        x: 'center'
                    }},
                    tooltip: {{
                        trigger: 'axis'
                    }},
                    legend: {{
                        orient: 'horizontal',
                        x: 'center',
                        y: '50px',
                        data: [{2}],
                    }},
                    toolbox: {{
                        show: true,
                        feature: {{
                            mark: {{ show: false }},
                            dataView: {{ show: false, readOnly: false }},
                            magicType: {{ show: false, type: ['line', 'bar'{5}] }},
                            restore: {{ show: false }},
                            saveAsImage: {{ show: true }}
                        }}
                    }},
                    calculable: false,
                    xAxis: [
                        {{
                            type: 'category',
                            data: [{3}],
                            axisLabel: {{
                                rotate: 30 // 文字倾斜
                            }}
                        }}
                    ],
                    yAxis: [
                        {{
                            type: 'value'
                        }},
                        {{
                            type:'value',
                            axisLabel:{{formatter: '{{value}} %'}}
                        }}
                    ],
                    grid: {{
                        x: 60, // 图表距离Container的左边距
                        x2: 60, // 图表距离Container的右边距
                        y: 80, // 图片距离上方描述文字的距离 默认为60
                        y2: 100 // X轴距离Container底部的高度
                    }},
                    series: [{4}]
                }}";


        public static MvcHtmlString FillVerticalBarEcharts(IList<BarChartsData> list, string text, string subtext)
        {
            return MvcHtmlString.Create(string.Format(EchartsJsSrc, "bar", FillVerticalBarEchartsOption(list, text, subtext)));
        }

        public static string FillVerticalBarEchartsOption(IList<BarChartsData> list, string text, string subtext)
        {
            if (list.Count == 0)
            {
                return string.Format(VericalBarOptionSrc, text, subtext, "''", "''", "{type:'bar',name:'',data:['-']}", "");
            }
            var xAxisData = string.Join(",", list.OrderBy(c => c.Sort).Select(c => "'" + c.X + "'").Distinct().ToList());
            var seriesList = new List<string>();
            var legendDataList = new List<string>();

            #region 柱形图
            var barData = list.Where(c => c.Type == ChartType.Bar && !c.Hidden).ToList();
            foreach (var l in barData.GroupBy(c => c.Legend))
            {
                if (!string.IsNullOrWhiteSpace(l.Key))
                    legendDataList.Add("'" + l.Key + "'");
                seriesList.Add(string.Format("{{type:'bar',barWidth:20,name:'{0}',data:[{1}]}}", l.Key,
                                             string.Join(",", barData.Where(c => c.Legend == l.Key).OrderBy(c => c.Sort).Select(c => c.Y))));
            }
            #endregion

            #region 堆积图
            var magicType = "";
            var stackList = list.Where(c => c.Type == ChartType.StackBar).Select(c => c as BarStackChartsData).ToList();
            if (stackList.Count > 0)
            {
                magicType = ", 'stack', 'tiled'";
                var ls = (from l in stackList
                          group l by new { l.Legend, l.Stack, l.Hidden } into l1
                          select new { l1.Key.Legend, l1.Key.Stack, l1.Key.Hidden });

                foreach (var l in ls)
                {
                    if (!string.IsNullOrWhiteSpace(l.Legend))
                        legendDataList.Add("'" + l.Legend + "'");
                    seriesList.Add(string.Format("{{type:'bar',barWidth:20,name:'{0}',data:[{1}],stack :'{2}'{3}}}", l.Legend,
                                                 string.Join(",", stackList.Where(c => c.Legend == l.Legend && c.Stack == l.Stack).OrderBy(c => c.Sort).Select(c => c.Y)), l.Stack,
                                                 l.Hidden ? ",tooltip:{trigger:'item',show:false},itemStyle:{normal:{label:{ show:false},borderColor:'rgba(0,0,0,0)',color:'rgba(0,0,0,0)'},emphasis:{borderColor:'rgba(0,0,0,0)',color:'rgba(0,0,0,0)'}}" : ""));
                }
            }
            #endregion

            #region 折线图
            var lineData = list.Where(c => c.Type == ChartType.Line).ToList();
            foreach (var l in lineData.GroupBy(c => c.Legend))
            {
                if (!string.IsNullOrWhiteSpace(l.Key))
                    legendDataList.Add("'" + l.Key + "'");
                var f = lineData.FirstOrDefault(c => c.Legend == l.Key) as LineChartsData;
                seriesList.Add(
                    string.Format(
                        "{{type:'line',name:'{0}',data:[{1}]{2},tooltip:{{trigger:'item',formatter:'{{a}}<br/>{{b}}：{{c}}{3}'}}{4}{5}}}",
                        l.Key,
                        string.Join(",", lineData.Where(c => c.Legend == l.Key).OrderBy(c => c.Sort).Select(c => c.Y)),
                        f.yAxisIndex > 0 ? ",yAxisIndex:1" : "",
                        f.yAxisIndex > 0 ? "%" : "",
                        f.ShowAverage ? ",markLine: {data: [{ type: 'average', name: '平均值' }]}" : "",
                        f.LineType != LineStyleType.Solid ? string.Format(",smooth:false,itemStyle:{{normal:{{lineStyle:{{type: '{0}'}}}}}}", f.LineType.ToString().ToLower()) : ",smooth:true"));
            }
            #endregion

            #region 散点 气泡图
            var scatterData = list.Where(c => c.Type == ChartType.Scatter);
            const string t = @"{{            
                            type: 'scatter',
                            {1}
                            itemStyle: {{
                                normal: {{
                                    color: 'red',
                                    label: {{ show: true }}
                                }}
                            }},
                            tooltip: {{
                                trigger: 'item',
                                formatter: function (value) {{
                                    return value[2][2];
                                }}
                            }},
                            data: [{0}]
                      }}";
            var slist = new List<string>();
            var yAxisIndex = 0;
            foreach (var s in scatterData)
            {
                var ss = s as ScatterChartsData;
                if (ss == null) continue;
                var v = string.Format("['{0}',{1},'{2}']", ss.X, ss.Y, ss.Description);
                slist.Add(v);
                if (s.yAxisIndex > 0)
                    yAxisIndex = s.yAxisIndex;
            }
            if (slist.Count > 0)
                seriesList.Add(string.Format(t, string.Join(",", slist), yAxisIndex > 0 ? "'yAxisIndex':1," : ""));
            #endregion

            return string.Format(VericalBarOptionSrc, text, subtext, string.Join(",", legendDataList), xAxisData, string.Join(",", seriesList), magicType);
        }

        #endregion

        #region Pie

        private static string PieJsOptionSrc = @"
                 {{
                    title : {{
                        text: '{0}',
                        subtext: '{1}',
                        x:'center'
                    }},
                    tooltip : {{
                        trigger: 'item',
                        formatter: '{{a}} <br/>{{b}} : {{c}} ({{d}}%)'
                    }},
                    legend: {{
                        orient : 'vertical',
                        x : 'left',
                        data:[{2}]
                    }},
                    toolbox: {{
                        show : true,
                        feature: {{
                            mark: {{ show: false }},
                            dataView: {{ show: false, readOnly: false }},
                            restore: {{ show: false }},
                            saveAsImage: {{ show: true }}
                        }}
                    }},
                    calculable : true,
                    series : [
                        {{
                            name:'{3}',
                            type:'pie',
                            radius : '55%',
                            center: ['50%', '60%'],
                            itemStyle:{{normal:{{label:{{show: true,formatter: '{{b}} : {{c}} ({{d}}%)'}}}}}},
                            data:[{4}]
                        }}
                    ]
                }}";

        public static MvcHtmlString FillPieEcharts(IList<PieChartsData> list, string text, string subtext, string seriesname)
        {
            return MvcHtmlString.Create(string.Format(EchartsJsSrc, "pie", FillPieEchartsOption(list, text, subtext, seriesname)));
        }

        public static string FillPieEchartsOption(IList<PieChartsData> list, string text, string subtext, string seriesname)
        {
            var seriesList = new List<string>();
            var legendDataList = new List<string>();
            foreach (var l in list)
            {
                legendDataList.Add("'" + l.X + "'");
                seriesList.Add(string.Format("{{name:'{0}',value:{1}}}", l.X, l.Y));
            }

            return string.Format(PieJsOptionSrc, text, subtext, string.Join(",", legendDataList), seriesname, string.Join(",", seriesList));
        }

        #endregion
    }
}

namespace Common.Echarts.ChartData
{
    public class BaseChartsData
    {
        private String _X;
        [Column("X", false, false, false)]
        public String X { get { return _X; } set { _X = value; } }

        private Int32 _Y;
        [Column("Y", false, false, false)]
        public Int32 Y { get { return _Y; } set { _Y = value; } }

        public ChartType Type { get; set; }

        public int Sort { get; set; }
    }

    public enum ChartType
    {
        None = 0,

        /// <summary>
        /// 柱状图
        /// </summary>
        Bar = 1,

        /// <summary>
        /// 柱状堆积图
        /// </summary>
        StackBar = 2,

        /// <summary>
        /// 饼图
        /// </summary>
        Pie = 3,

        /// <summary>
        /// 折线
        /// </summary>
        Line = 4,

        /// <summary>
        /// 散点，气泡图
        /// </summary>
        Scatter = 5
    }

    public class BarChartsData : BaseChartsData
    {
        public BarChartsData()
        {
            Type = ChartType.Bar;
        }

        private String _Legend;
        [Column("Legend", false, false, false)]
        public String Legend { get { return _Legend; } set { _Legend = value; } }

        public int yAxisIndex { get; set; }

        public bool Hidden { get; set; }
    }

    public class BarStackChartsData : BarChartsData
    {
        public BarStackChartsData()
        {
            Type = ChartType.StackBar;
        }

        private String _Stack;
        [Column("Stack", false, false, false)]
        public String Stack { get { return _Stack; } set { _Stack = value; } }
    }

    public class LineChartsData : BarChartsData
    {
        public LineChartsData()
        {
            ShowAverage = false;
            yAxisIndex = 1;
            Type = ChartType.Line;
            LineType = LineStyleType.Solid;
        }

        public bool ShowAverage { get; set; }

        public LineStyleType LineType { get; set; }
    }

    public enum LineStyleType
    {
        Solid,

        Dotted,

        Dashed
    }

    public class PieChartsData : BaseChartsData
    {
        public PieChartsData()
        {
            Type = ChartType.Pie;
        }
    }

    public class ScatterChartsData : BarChartsData
    {
        public ScatterChartsData()
        {
            Type = ChartType.Scatter;
            yAxisIndex = 1;
        }

        public ScatterChartsData(string x, int y, string desc)
            : this()
        {
            X = x;
            Y = y;
            Description = string.Format("{0}：{1} %", desc, y);
        }

        public string Description { get; set; }
    }
}