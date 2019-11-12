namespace _1._17.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Flag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Flag", c => c.Boolean(nullable: false));
        }
    }
}
