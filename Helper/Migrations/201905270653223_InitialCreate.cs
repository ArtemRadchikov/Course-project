namespace Helper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        MidleName = c.String(nullable: false),
                        SecondName = c.String(nullable: false),
                        Book_BookID = c.Int(),
                        Book_BookID1 = c.Int(),
                        Book1_BookID = c.Int(),
                    })
                .PrimaryKey(t => t.AuthorID)
                .ForeignKey("dbo.Books", t => t.Book_BookID)
                .ForeignKey("dbo.Books", t => t.Book_BookID1)
                .ForeignKey("dbo.Books", t => t.Book1_BookID)
                .Index(t => t.Book_BookID)
                .Index(t => t.Book_BookID1)
                .Index(t => t.Book1_BookID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        PublishDate = c.Int(nullable: false),
                        Publisher = c.String(nullable: false),
                        BibliographicDescription = c.String(maxLength: 255),
                        Description = c.String(nullable: false, maxLength: 255),
                        Url = c.String(nullable: false),
                        KeyWordItem_KeyWordItemID = c.Int(),
                        Author_AuthorID = c.Int(),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.KeyWordItems", t => t.KeyWordItem_KeyWordItemID)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorID)
                .Index(t => t.KeyWordItem_KeyWordItemID)
                .Index(t => t.Author_AuthorID);
            
            CreateTable(
                "dbo.KeyWordItems",
                c => new
                    {
                        KeyWordItemID = c.Int(nullable: false, identity: true),
                        Value = c.String(maxLength: 200),
                        Book_BookID = c.Int(),
                        Book_BookID1 = c.Int(),
                    })
                .PrimaryKey(t => t.KeyWordItemID)
                .ForeignKey("dbo.Books", t => t.Book_BookID)
                .ForeignKey("dbo.Books", t => t.Book_BookID1)
                .Index(t => t.Book_BookID)
                .Index(t => t.Book_BookID1);
            
            CreateTable(
                "dbo.Coefficient_a0",
                c => new
                    {
                        DecisionID = c.Int(nullable: false),
                        SymbolicValue = c.String(nullable: false),
                        LaTeXValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID)
                .ForeignKey("dbo.Decisions", t => t.DecisionID)
                .Index(t => t.DecisionID);
            
            CreateTable(
                "dbo.Decisions",
                c => new
                    {
                        DecisionID = c.Int(nullable: false, identity: true),
                        InputedValue = c.String(),
                        LowerSegmentValue = c.String(nullable: false),
                        UpperSegmentValue = c.String(nullable: false),
                        HalfPeriod = c.String(nullable: false),
                        СreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID);
            
            CreateTable(
                "dbo.Coefficient_an",
                c => new
                    {
                        DecisionID = c.Int(nullable: false),
                        SymbolicValue = c.String(nullable: false),
                        LaTeXValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID)
                .ForeignKey("dbo.Decisions", t => t.DecisionID)
                .Index(t => t.DecisionID);
            
            CreateTable(
                "dbo.Coefficient_bn",
                c => new
                    {
                        DecisionID = c.Int(nullable: false),
                        SymbolicValue = c.String(nullable: false),
                        LaTeXValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID)
                .ForeignKey("dbo.Decisions", t => t.DecisionID)
                .Index(t => t.DecisionID);
            
            CreateTable(
                "dbo.FourierSeries",
                c => new
                    {
                        DecisionID = c.Int(nullable: false),
                        SymbolicValue = c.String(nullable: false),
                        LaTeXValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID)
                .ForeignKey("dbo.Decisions", t => t.DecisionID)
                .Index(t => t.DecisionID);
            
            CreateTable(
                "dbo.OriginalValues",
                c => new
                    {
                        DecisionID = c.Int(nullable: false),
                        SymbolicValue = c.String(nullable: false),
                        LaTeXValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID)
                .ForeignKey("dbo.Decisions", t => t.DecisionID)
                .Index(t => t.DecisionID);
            
            CreateTable(
                "dbo.PartialSum_k",
                c => new
                    {
                        DecisionID = c.Int(nullable: false),
                        SymbolicValue = c.String(nullable: false),
                        LaTeXValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DecisionID)
                .ForeignKey("dbo.Decisions", t => t.DecisionID)
                .Index(t => t.DecisionID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Rool = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coefficient_a0", "DecisionID", "dbo.Decisions");
            DropForeignKey("dbo.PartialSum_k", "DecisionID", "dbo.Decisions");
            DropForeignKey("dbo.OriginalValues", "DecisionID", "dbo.Decisions");
            DropForeignKey("dbo.FourierSeries", "DecisionID", "dbo.Decisions");
            DropForeignKey("dbo.Coefficient_bn", "DecisionID", "dbo.Decisions");
            DropForeignKey("dbo.Coefficient_an", "DecisionID", "dbo.Decisions");
            DropForeignKey("dbo.Books", "Author_AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Authors", "Book1_BookID", "dbo.Books");
            DropForeignKey("dbo.Authors", "Book_BookID1", "dbo.Books");
            DropForeignKey("dbo.KeyWordItems", "Book_BookID1", "dbo.Books");
            DropForeignKey("dbo.Books", "KeyWordItem_KeyWordItemID", "dbo.KeyWordItems");
            DropForeignKey("dbo.KeyWordItems", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.Authors", "Book_BookID", "dbo.Books");
            DropIndex("dbo.PartialSum_k", new[] { "DecisionID" });
            DropIndex("dbo.OriginalValues", new[] { "DecisionID" });
            DropIndex("dbo.FourierSeries", new[] { "DecisionID" });
            DropIndex("dbo.Coefficient_bn", new[] { "DecisionID" });
            DropIndex("dbo.Coefficient_an", new[] { "DecisionID" });
            DropIndex("dbo.Coefficient_a0", new[] { "DecisionID" });
            DropIndex("dbo.KeyWordItems", new[] { "Book_BookID1" });
            DropIndex("dbo.KeyWordItems", new[] { "Book_BookID" });
            DropIndex("dbo.Books", new[] { "Author_AuthorID" });
            DropIndex("dbo.Books", new[] { "KeyWordItem_KeyWordItemID" });
            DropIndex("dbo.Authors", new[] { "Book1_BookID" });
            DropIndex("dbo.Authors", new[] { "Book_BookID1" });
            DropIndex("dbo.Authors", new[] { "Book_BookID" });
            DropTable("dbo.Users");
            DropTable("dbo.PartialSum_k");
            DropTable("dbo.OriginalValues");
            DropTable("dbo.FourierSeries");
            DropTable("dbo.Coefficient_bn");
            DropTable("dbo.Coefficient_an");
            DropTable("dbo.Decisions");
            DropTable("dbo.Coefficient_a0");
            DropTable("dbo.KeyWordItems");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
