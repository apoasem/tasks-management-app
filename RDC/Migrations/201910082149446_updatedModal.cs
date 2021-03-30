namespace RDC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedModal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Priority", c => c.String());
            AddColumn("dbo.Tasks", "TaskId", c => c.Int());
            AddColumn("dbo.Tasks", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Tasks", "TaskId");
            AddForeignKey("dbo.Tasks", "TaskId", "dbo.Tasks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "TaskId", "dbo.Tasks");
            DropIndex("dbo.Tasks", new[] { "TaskId" });
            DropColumn("dbo.Tasks", "Discriminator");
            DropColumn("dbo.Tasks", "TaskId");
            DropColumn("dbo.Tasks", "Priority");
        }
    }
}
