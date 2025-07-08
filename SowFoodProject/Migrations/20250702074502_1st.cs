using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SowFoodProject.Migrations
{
    /// <inheritdoc />
    public partial class _1st : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SowFoodCompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetVerified = table.Column<bool>(type: "bit", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanyCustomers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SowFoodCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RegisteredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanyCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyCustomers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyCustomers_SowFoodCompanies_SowFoodCompanyId",
                        column: x => x.SowFoodCompanyId,
                        principalTable: "SowFoodCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanyStaff",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SowFoodCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanyStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyStaff_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyStaff_SowFoodCompanies_SowFoodCompanyId",
                        column: x => x.SowFoodCompanyId,
                        principalTable: "SowFoodCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanyProductionItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfProduction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SowFoodCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanyProductionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyProductionItems_SowFoodCompanies_SowFoodCompanyId",
                        column: x => x.SowFoodCompanyId,
                        principalTable: "SowFoodCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyProductionItems_SowFoodCompanyStaff_AddedById",
                        column: x => x.AddedById,
                        principalTable: "SowFoodCompanyStaff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanyShelfItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SowFoodCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanyShelfItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyShelfItems_SowFoodCompanies_SowFoodCompanyId",
                        column: x => x.SowFoodCompanyId,
                        principalTable: "SowFoodCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyShelfItems_SowFoodCompanyStaff_AddedById",
                        column: x => x.AddedById,
                        principalTable: "SowFoodCompanyStaff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanyStaffAppraisers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SowFoodCompanyStaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanyStaffAppraisers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyStaffAppraisers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyStaffAppraisers_SowFoodCompanyStaff_SowFoodCompanyStaffId",
                        column: x => x.SowFoodCompanyStaffId,
                        principalTable: "SowFoodCompanyStaff",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanyStaffAttendances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SowFoodCompanyStaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogonTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedTimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanyStaffAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyStaffAttendances_AspNetUsers_ConfirmedByUserId",
                        column: x => x.ConfirmedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanyStaffAttendances_SowFoodCompanyStaff_SowFoodCompanyStaffId",
                        column: x => x.SowFoodCompanyStaffId,
                        principalTable: "SowFoodCompanyStaff",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SowFoodCompanySalesRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SowFoodCompanyProductItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SowFoodCompanyShelfItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SowFoodCompanyCustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SowFoodCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SowFoodCompanyStaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SowFoodCompanySalesRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SowFoodCompanySalesRecords_SowFoodCompanies_SowFoodCompanyId",
                        column: x => x.SowFoodCompanyId,
                        principalTable: "SowFoodCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanySalesRecords_SowFoodCompanyCustomers_SowFoodCompanyCustomerId",
                        column: x => x.SowFoodCompanyCustomerId,
                        principalTable: "SowFoodCompanyCustomers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanySalesRecords_SowFoodCompanyProductionItems_SowFoodCompanyProductItemId",
                        column: x => x.SowFoodCompanyProductItemId,
                        principalTable: "SowFoodCompanyProductionItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanySalesRecords_SowFoodCompanyShelfItems_SowFoodCompanyShelfItemId",
                        column: x => x.SowFoodCompanyShelfItemId,
                        principalTable: "SowFoodCompanyShelfItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SowFoodCompanySalesRecords_SowFoodCompanyStaff_SowFoodCompanyStaffId",
                        column: x => x.SowFoodCompanyStaffId,
                        principalTable: "SowFoodCompanyStaff",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanies_UserId",
                table: "SowFoodCompanies",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyCustomers_SowFoodCompanyId",
                table: "SowFoodCompanyCustomers",
                column: "SowFoodCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyCustomers_UserId",
                table: "SowFoodCompanyCustomers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyProductionItems_AddedById",
                table: "SowFoodCompanyProductionItems",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyProductionItems_SowFoodCompanyId",
                table: "SowFoodCompanyProductionItems",
                column: "SowFoodCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanySalesRecords_SowFoodCompanyCustomerId",
                table: "SowFoodCompanySalesRecords",
                column: "SowFoodCompanyCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanySalesRecords_SowFoodCompanyId",
                table: "SowFoodCompanySalesRecords",
                column: "SowFoodCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanySalesRecords_SowFoodCompanyProductItemId",
                table: "SowFoodCompanySalesRecords",
                column: "SowFoodCompanyProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanySalesRecords_SowFoodCompanyShelfItemId",
                table: "SowFoodCompanySalesRecords",
                column: "SowFoodCompanyShelfItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanySalesRecords_SowFoodCompanyStaffId",
                table: "SowFoodCompanySalesRecords",
                column: "SowFoodCompanyStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyShelfItems_AddedById",
                table: "SowFoodCompanyShelfItems",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyShelfItems_SowFoodCompanyId",
                table: "SowFoodCompanyShelfItems",
                column: "SowFoodCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyStaff_SowFoodCompanyId",
                table: "SowFoodCompanyStaff",
                column: "SowFoodCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyStaff_UserId",
                table: "SowFoodCompanyStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyStaffAppraisers_SowFoodCompanyStaffId",
                table: "SowFoodCompanyStaffAppraisers",
                column: "SowFoodCompanyStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyStaffAppraisers_UserId",
                table: "SowFoodCompanyStaffAppraisers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyStaffAttendances_ConfirmedByUserId",
                table: "SowFoodCompanyStaffAttendances",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SowFoodCompanyStaffAttendances_SowFoodCompanyStaffId",
                table: "SowFoodCompanyStaffAttendances",
                column: "SowFoodCompanyStaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SowFoodCompanySalesRecords");

            migrationBuilder.DropTable(
                name: "SowFoodCompanyStaffAppraisers");

            migrationBuilder.DropTable(
                name: "SowFoodCompanyStaffAttendances");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "SowFoodCompanyCustomers");

            migrationBuilder.DropTable(
                name: "SowFoodCompanyProductionItems");

            migrationBuilder.DropTable(
                name: "SowFoodCompanyShelfItems");

            migrationBuilder.DropTable(
                name: "SowFoodCompanyStaff");

            migrationBuilder.DropTable(
                name: "SowFoodCompanies");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
