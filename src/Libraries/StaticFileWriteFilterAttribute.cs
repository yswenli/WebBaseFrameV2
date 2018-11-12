using System.IO;
using System.Web;
//
using System.Web.Mvc;

namespace Libraries
{
    /// <summary>
    /// 通过mvc提供的过滤器扩展点实现页面内容静态化
    /// </summary>
    public class StaticFileWriteFilterAttribute : FilterAttribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //定义输出内容
            filterContext.HttpContext.Response.Filter = new StaticFileWriteResponseFilterWrapper(filterContext);
        }

        /// <summary>
        /// 静态文件写操作类
        /// </summary>
        class StaticFileWriteResponseFilterWrapper : System.IO.Stream
        {
            private System.IO.Stream inner;
            private ControllerContext context;
            /// <summary>
            /// 构造方法
            /// </summary>
            /// <param name="context"></param>
            public StaticFileWriteResponseFilterWrapper(ControllerContext context)
            {
                this.context = context;
                this.inner = context.HttpContext.Response.Filter;
            }
            #region 重写Stream方法
            public override bool CanRead
            {
                get { return inner.CanRead; }
            }

            public override bool CanSeek
            {
                get { return inner.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return inner.CanWrite; }
            }

            public override void Flush()
            {
                inner.Flush();
            }

            public override long Length
            {
                get { return inner.Length; }
            }

            public override long Position
            {
                get
                {
                    return inner.Position;
                }
                set
                {
                    inner.Position = value;
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return inner.Read(buffer, offset, count);
            }

            public override long Seek(long offset, System.IO.SeekOrigin origin)
            {
                return inner.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                inner.SetLength(value);
            }
            #endregion
            /// <summary>
            /// 生成文件方法
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="offset"></param>
            /// <param name="count"></param>
            public override void Write(byte[] buffer, int offset, int count)
            {
                inner.Write(buffer, offset, count);
                try
                {
                    string p = context.HttpContext.Server.MapPath(HttpContext.Current.Request.Path);

                    if (Path.HasExtension(p))
                    {
                        string dir = Path.GetDirectoryName(p);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        if (File.Exists(p))
                        {
                            File.Delete(p);
                        }
                        File.AppendAllText(p, System.Text.Encoding.UTF8.GetString(buffer));
                    }
                }
                catch
                {

                }
            }
        }
    }
}