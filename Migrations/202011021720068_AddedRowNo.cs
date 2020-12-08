namespace ToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoes", "RowNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoes", "RowNo");
        }
    }
}
