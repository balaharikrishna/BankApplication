using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    BankName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "ReserveBankManagers",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Salt = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<short>(type: "Smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReserveBankManagers", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    BranchName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    BranchAddress = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BranchPhoneNumber = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                    table.ForeignKey(
                        name: "FK_Branches_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyCode = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyCode);
                    table.ForeignKey(
                        name: "FK_Currencies_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeadManagers",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    BankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Salt = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<short>(type: "Smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadManagers", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_HeadManagers_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    EmailId = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    AccountType = table.Column<short>(type: "Smallint", nullable: false),
                    Address = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Gender = table.Column<short>(type: "Smallint", nullable: false),
                    PassbookIssueDate = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Salt = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<short>(type: "Smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Customers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    BranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Salt = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<short>(type: "Smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Managers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    BranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Salt = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    HashedPassword = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<short>(type: "Smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Staffs_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RtgsSameBank = table.Column<short>(type: "Smallint", nullable: false),
                    RtgsOtherBank = table.Column<short>(type: "Smallint", nullable: false),
                    ImpsSameBank = table.Column<short>(type: "Smallint", nullable: false),
                    ImpsOtherBank = table.Column<short>(type: "Smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCharges_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerAccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false),
                    CustomerBankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    CustomerBranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    FromCustomerBankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    ToCustomerBankId = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    FromCustomerBranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true),
                    ToCustomerBranchId = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true),
                    FromCustomerAccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: true),
                    TransactionId = table.Column<string>(type: "varchar(23)", maxLength: 23, nullable: false),
                    TransactionType = table.Column<short>(type: "Smallint", nullable: false),
                    ToCustomerAccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: true),
                    TransactionDate = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    AccountId = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Customers",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BankId",
                table: "Branches",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_BankId",
                table: "Currencies",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BranchId",
                table: "Customers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadManagers_BankId",
                table: "HeadManagers",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_BranchId",
                table: "Managers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchId",
                table: "Staffs",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCharges_BranchId",
                table: "TransactionCharges",
                column: "BranchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "HeadManagers");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "ReserveBankManagers");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "TransactionCharges");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
