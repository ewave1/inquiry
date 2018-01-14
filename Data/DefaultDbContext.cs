using Data.Entities;
using System.Data.Entity;

namespace SmartSSO.Entities
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    /// <summary>
    /// 默认数据上下文
    /// </summary>
    public class DefaultDbContext : DbContext
    {
        /// <summary>
        /// 系统管理用户表
        /// </summary>
        public DbSet<ManageUser> ManageUser { get; set; }

        /// <summary>
        /// 应用用户表
        /// </summary>
        public DbSet<AppUser> AppUser { get; set; }

        /// <summary>
        /// 应用信息
        /// </summary>
        public DbSet<AppInfo> AppInfo { get; set; }

        /// <summary>
        /// 用户授权操作
        /// </summary>
        public DbSet<UserAuthOperate> UserAuthOperate { get; set; }

        /// <summary>
        /// 用户授权会话
        /// </summary>
        public DbSet<UserAuthSession> UserAuthSession { get; set; }

        /// <summary>
        /// 通用的折扣设置
        /// </summary>
        public DbSet<DiscountSet> DiscountSet { get; set; }
        public DbSet<Product> Product { get; set; }
        /// <summary>
        /// 编码（No use)
        /// </summary>
        public DbSet<SealCode> SealCode { get; set; }

        /// <summary>
        /// 询价记录
        /// </summary>
        public DbSet<InquiryLog> InquiryLog { get; set; }


        /// <summary>
        /// 上传的文件
        /// </summary>
        public DbSet<UploadFile> UploadFile { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public DbSet<Customer> Customer { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public DbSet<Storage> Storage { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public DbSet<Material> Material { get; set; }

        /// <summary>
        /// 物性，颜色
        /// </summary>
        public DbSet<MaterialFeature> MaterialFeature { get; set; }
        /// <summary>
        /// 比重
        /// </summary>
        public DbSet<MaterialGravity> MaterialGravity { get; set; }
        /// <summary>
        /// 孔数
        /// </summary>
        public DbSet<MaterialHole> MaterialHole { get; set; }
        /// <summary>
        /// 时数
        /// </summary>
        public DbSet<MaterialHour> MaterialHour { get; set; }
        /// <summary>
        /// 利用率和不良率
        /// </summary>
        public DbSet<MaterialRate> MaterialRate { get; set; } 


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InquiryLog>().Property(p => p.Price).HasPrecision(18, 3);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 3);
        }
    }
}