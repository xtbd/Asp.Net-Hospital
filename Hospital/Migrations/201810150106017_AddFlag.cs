namespace _1._17.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Flag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Flag");
        }
    }
}
