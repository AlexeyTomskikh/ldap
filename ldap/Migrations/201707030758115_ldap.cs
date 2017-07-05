namespace ldap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ldap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescriptionEvent = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserOfRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Users", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOfRoles", "UserId", "dbo.Roles");
            DropForeignKey("dbo.UserOfRoles", "RoleId", "dbo.Users");
            DropIndex("dbo.UserOfRoles", new[] { "UserId" });
            DropIndex("dbo.UserOfRoles", new[] { "RoleId" });
            DropTable("dbo.UserOfRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Events");
        }
    }
}
