namespace Mvc5UserCrudManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCountry",
                c => new
                    {
                        UserCountryId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CountryId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserCountryId)
                .ForeignKey("dbo.UserCountry", t => t.CountryId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCourse",
                c => new
                    {
                        UserCourseId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.String(nullable: false, maxLength: 128),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserCourseId)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDescription",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCountry", "CountryId", "dbo.Country");
            DropForeignKey("dbo.UserDescription", "UserId", "dbo.User");
            DropForeignKey("dbo.UserCourse", "UserId", "dbo.User");
            DropForeignKey("dbo.UserCourse", "CourseId", "dbo.Course");
            DropForeignKey("dbo.UserCountry", "UserId", "dbo.User");
            DropForeignKey("dbo.UserCountry", "CountryId", "dbo.UserCountry");
            DropIndex("dbo.UserDescription", new[] { "UserId" });
            DropIndex("dbo.UserCourse", new[] { "CourseId" });
            DropIndex("dbo.UserCourse", new[] { "UserId" });
            DropIndex("dbo.UserCountry", new[] { "CountryId" });
            DropIndex("dbo.UserCountry", new[] { "UserId" });
            DropTable("dbo.UserDescription");
            DropTable("dbo.Course");
            DropTable("dbo.UserCourse");
            DropTable("dbo.User");
            DropTable("dbo.UserCountry");
            DropTable("dbo.Country");
        }
    }
}
