﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApp.Repository.Migrations
{
    public partial class IdIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdentityId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Customers");
        }
    }
}
