namespace Helper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Info : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "BibliographicDescription", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "BibliographicDescription", c => c.String(maxLength: 255));
        }
    }
}
