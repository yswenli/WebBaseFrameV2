
namespace Common
{
    public class EnumHelper
    {
        /// <summary>
        /// 表基本字段
        /// </summary>
        public enum BaseField
        {
            ID,
            CreateUserID,
            LastUpdateUserID,
            CreateDate,
            LastUpdateDate,
            IsDeleted,
            Sort
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public enum EnumCheck
        {
            待审核 = 1,
            审核通过 = 2,
            被驳回 = 3
        }
        /// <summary>
        /// 数据字典分类
        /// </summary>
        public enum EnumDictionaryType
        {
            审核状态=1,
            会议预约类型=2,
            部门管理 = 3,
            会议详细状态=4,
            页面分类 = 5,
            资料库附件类型 = 6
        }
        /// <summary>
        /// 页面分类
        /// </summary>
        public enum EnumPageType
        {
            列表页 = 35,
            表单页 = 36,
            明细页 = 37
        }
        /// <summary>
        /// 系统角色
        /// </summary>
        public enum EnumRole
        { 
            /// <summary>
            /// 超级管理员
            /// </summary>
            Admin=1,
            /// <summary>
            /// 技术支持
            /// </summary>
            Tech=2,
            /// <summary>
            /// 普通用户
            /// </summary>
            User=3
        }
    }
}
