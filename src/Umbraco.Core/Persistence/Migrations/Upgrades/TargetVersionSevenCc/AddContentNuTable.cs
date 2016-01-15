﻿using System.Data;
using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Umbraco.Core.Persistence.Migrations.Upgrades.TargetVersionSevenCc
{
    [Migration("7.4.0", 1000, GlobalSettings.UmbracoMigrationName)]
    class AddContentNuTable : MigrationBase
    {
        public AddContentNuTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
            : base(sqlSyntax, logger)
        { }

        public override void Up()
        {
            var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
            if (tables.InvariantContains("cmsContentNu")) return;

            var textType = SqlSyntax.GetSpecialDbType(SpecialDbTypes.NTEXT);

            Create.Table("cmsContentNu")
                .WithColumn("nodeId").AsInt32().NotNullable()
                .WithColumn("published").AsBoolean().NotNullable()
                .WithColumn("data").AsCustom(textType).NotNullable()
                .WithColumn("rv").AsInt64().NotNullable().WithDefaultValue(0);

            Create.PrimaryKey("PK_cmsContentNu")
                .OnTable("cmsContentNu")
                .Columns(new[] { "nodeId", "published" });

            Create.ForeignKey("FK_cmsContentNu_umbracoNode_id")
                .FromTable("cmsContentNu")
                .ForeignColumn("nodeId")
                .ToTable("umbracoNode")
                .PrimaryColumn("id")
                .OnDelete(Rule.Cascade)
                .OnUpdate(Rule.None);
        }

        public override void Down()
        { }
    }
}
