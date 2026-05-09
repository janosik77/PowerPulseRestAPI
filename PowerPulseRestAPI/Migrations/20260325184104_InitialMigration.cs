using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PowerPulseRestAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    postal_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    city = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    building_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    apartment_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    full_text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    address_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.CheckConstraint("ck_address_building_number_not_empty", "building_number <> ''");
                    table.CheckConstraint("ck_address_city_not_empty", "city <> ''");
                    table.CheckConstraint("ck_address_country_not_empty", "country <> ''");
                    table.CheckConstraint("ck_address_street_not_empty", "street <> ''");
                    table.CheckConstraint("ck_address_updated_at", "updated_at >= created_at");
                });

            migrationBuilder.CreateTable(
                name: "knowledge_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "material_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_categories", x => x.id);
                    table.CheckConstraint("ck_material_category_name_not_empty", "name <> ''");
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                    table.CheckConstraint("ck_role_name_not_empty", "name <> ''");
                });

            migrationBuilder.CreateTable(
                name: "text_templates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    channel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    title_template = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    body_template = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_text_templates", x => x.id);
                    table.CheckConstraint("ck_text_template_body_not_empty", "body_template <> ''");
                    table.CheckConstraint("ck_text_template_key_not_empty", "[key] <> ''");
                    table.CheckConstraint("ck_text_template_updated_at", "updated_at >= created_at");
                });

            migrationBuilder.CreateTable(
                name: "tool_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_categories", x => x.id);
                    table.CheckConstraint("ck_tool_category_not_self_parent", "parent_id IS NULL OR parent_id <> id");
                    table.ForeignKey(
                        name: "FK_tool_categories_tool_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tool_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    plate_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    vin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    make = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    current_mileage = table.Column<int>(type: "int", nullable: false),
                    last_service_at = table.Column<DateOnly>(type: "date", nullable: true),
                    last_service_mileage = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.id);
                    table.CheckConstraint("ck_vehicle_last_service_mileage_nonneg", "last_service_mileage IS NULL OR last_service_mileage >= 0");
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    avatar_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    address_id = table.Column<long>(type: "bigint", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.id);
                    table.CheckConstraint("ck_person_email_not_empty", "email <> ''");
                    table.CheckConstraint("ck_person_first_name_not_empty", "first_name <> ''");
                    table.CheckConstraint("ck_person_last_name_not_empty", "last_name <> ''");
                    table.CheckConstraint("ck_person_updated_at", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "FK_persons_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    default_unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.id);
                    table.CheckConstraint("ck_material_currency_not_empty", "currency <> ''");
                    table.CheckConstraint("ck_material_default_unit_not_empty", "default_unit <> ''");
                    table.CheckConstraint("ck_material_name_not_empty", "name <> ''");
                    table.CheckConstraint("ck_material_price_nonneg", "price >= 0");
                    table.CheckConstraint("ck_material_url_not_empty", "url <> ''");
                    table.ForeignKey(
                        name: "FK_materials_material_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "material_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tools",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    model = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    serial_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    condition = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    purchase_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tools", x => x.id);
                    table.CheckConstraint("ck_tool_name_not_empty", "name <> ''");
                    table.ForeignKey(
                        name: "FK_tools_tool_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "tool_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tax_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    avatar_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    contact_person_id = table.Column<long>(type: "bigint", nullable: false),
                    address_id = table.Column<long>(type: "bigint", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                    table.CheckConstraint("ck_customer_company_name_not_empty", "company_name <> ''");
                    table.CheckConstraint("ck_customer_updated_at", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "FK_customers_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customers_persons_contact_person_id",
                        column: x => x.contact_person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    person_id = table.Column<long>(type: "bigint", nullable: false),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: false),
                    terminated_at = table.Column<DateOnly>(type: "date", nullable: true),
                    department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    job_title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    hourly_wage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    remaining_vacation_days = table.Column<int>(type: "int", nullable: false),
                    vacation_days_per_year = table.Column<int>(type: "int", nullable: false),
                    account_encrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    account_last4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.CheckConstraint("ck_employee_job_title_not_empty", "job_title <> ''");
                    table.CheckConstraint("ck_employee_terminated_after_hire", "terminated_at IS NULL OR terminated_at >= hire_date");
                    table.CheckConstraint("ck_employee_vacation_nonneg", "remaining_vacation_days >= 0 AND vacation_days_per_year >= 0");
                    table.ForeignKey(
                        name: "FK_employees_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "person_identifiers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    person_id = table.Column<long>(type: "bigint", nullable: false),
                    value_encrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    last4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_identifiers", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_identifiers_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    person_id = table.Column<long>(type: "bigint", nullable: false),
                    last_password_update = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_login_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("ck_user_email_not_empty", "email <> ''");
                    table.CheckConstraint("ck_user_login_not_empty", "login <> ''");
                    table.CheckConstraint("ck_user_password_hash_not_empty", "password_hash <> ''");
                    table.ForeignKey(
                        name: "FK_users_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "low_stock_notes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_by_employee_id = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_low_stock_notes", x => x.id);
                    table.CheckConstraint("ck_low_stock_note_updated_at", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "FK_low_stock_notes_employees_created_by_employee_id",
                        column: x => x.created_by_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    avatar_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_by_employee_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    responsible_employee_id = table.Column<long>(type: "bigint", nullable: true),
                    address_id = table.Column<long>(type: "bigint", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.CheckConstraint("ck_project_code_not_empty", "code <> ''");
                    table.CheckConstraint("ck_project_dates", "end_date IS NULL OR start_date IS NULL OR end_date >= start_date");
                    table.CheckConstraint("ck_project_name_not_empty", "name <> ''");
                    table.ForeignKey(
                        name: "FK_projects_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_employees_created_by_employee_id",
                        column: x => x.created_by_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_employees_responsible_employee_id",
                        column: x => x.responsible_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    returned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_assignments", x => x.id);
                    table.CheckConstraint("ck_vehicle_assignment_return_after_assign", "returned_at IS NULL OR returned_at >= assigned_at");
                    table.ForeignKey(
                        name: "FK_vehicle_assignments_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_assignments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_articles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_articles", x => x.id);
                    table.ForeignKey(
                        name: "FK_knowledge_articles_knowledge_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "knowledge_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_knowledge_articles_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tool_id = table.Column<long>(type: "bigint", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    returned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_assignments", x => x.id);
                    table.CheckConstraint("ck_tool_assignment_return_after_assign", "returned_at IS NULL OR returned_at >= assigned_at");
                    table.ForeignKey(
                        name: "FK_tool_assignments_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_issues",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    reported_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_issues", x => x.id);
                    table.ForeignKey(
                        name: "FK_vehicle_issues_users_reported_by_user_id",
                        column: x => x.reported_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_issues_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_number = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    issue_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subtotal_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    billing_period_start = table.Column<DateOnly>(type: "date", nullable: false),
                    billing_period_end = table.Column<DateOnly>(type: "date", nullable: false),
                    customer_name_snapshot = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    customer_tax_id_snapshot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    billing_address_snapshot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.id);
                    table.CheckConstraint("ck_invoice_amounts_nonneg", "subtotal_amount >= 0 AND tax_amount >= 0 AND total_amount >= 0");
                    table.CheckConstraint("ck_invoice_billing_period", "billing_period_end >= billing_period_start");
                    table.CheckConstraint("ck_invoice_currency_not_empty", "currency <> ''");
                    table.CheckConstraint("ck_invoice_due_after_issue", "due_date >= issue_date");
                    table.CheckConstraint("ck_invoice_number_not_empty", "invoice_number <> ''");
                    table.CheckConstraint("ck_invoice_total_match", "total_amount = subtotal_amount + tax_amount");
                    table.ForeignKey(
                        name: "FK_invoices_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoices_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoices_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_access",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    is_enabled = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_access", x => x.id);
                    table.ForeignKey(
                        name: "FK_project_access_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_access_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    attachment_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_by_employee_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_attachments", x => x.id);
                    table.CheckConstraint("ck_project_attachment_url_not_empty", "url <> ''");
                    table.ForeignKey(
                        name: "FK_project_attachments_employees_created_by_employee_id",
                        column: x => x.created_by_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_attachments_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_notes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    note_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_by_employee_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_notes", x => x.id);
                    table.CheckConstraint("ck_project_note_content_not_empty", "content <> ''");
                    table.CheckConstraint("ck_project_notes_updated_at", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "FK_project_notes_employees_created_by_employee_id",
                        column: x => x.created_by_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_notes_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_tasks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    due_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    estimated_minutes = table.Column<int>(type: "int", nullable: true),
                    created_by_employee_id = table.Column<long>(type: "bigint", nullable: false),
                    assigned_to_employee_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_tasks", x => x.id);
                    table.CheckConstraint("ck_project_task_estimate_positive", "estimated_minutes IS NULL OR estimated_minutes >= 0");
                    table.CheckConstraint("ck_project_task_priority", "priority IN ('LOW', 'MEDIUM', 'HIGH', 'URGENT')");
                    table.CheckConstraint("ck_project_task_title_not_empty", "title <> ''");
                    table.ForeignKey(
                        name: "FK_project_tasks_employees_assigned_to_employee_id",
                        column: x => x.assigned_to_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_tasks_employees_created_by_employee_id",
                        column: x => x.created_by_employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_tasks_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_article_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    attachment_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_article_attachments", x => x.id);
                    table.CheckConstraint("ck_knowledge_attachment_sort_nonneg", "sort_order >= 0");
                    table.ForeignKey(
                        name: "FK_knowledge_article_attachments_knowledge_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "knowledge_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_article_favorites",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_article_favorites", x => x.id);
                    table.ForeignKey(
                        name: "FK_knowledge_article_favorites_knowledge_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "knowledge_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_knowledge_article_favorites_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_article_reads",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_article_reads", x => x.id);
                    table.ForeignKey(
                        name: "FK_knowledge_article_reads_knowledge_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "knowledge_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_knowledge_article_reads_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_labor_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    line_subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    line_tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    line_total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_labor_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_labor_items_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_material_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_id = table.Column<long>(type: "bigint", nullable: false),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    line_subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    line_tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    line_total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_material_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_material_items_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoice_material_items_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_movements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    movement_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: true),
                    operation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    occurred_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    invoice_id = table.Column<long>(type: "bigint", nullable: true),
                    invoiced_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_movements", x => x.id);
                    table.CheckConstraint("ck_material_movement_invoiced_at_requires_invoice", "invoice_id IS NOT NULL OR invoiced_at IS NULL");
                    table.CheckConstraint("ck_material_movement_qty_positive", "quantity > 0");
                    table.CheckConstraint("ck_material_movement_unit_not_empty", "unit <> ''");
                    table.ForeignKey(
                        name: "FK_material_movements_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_material_movements_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "work_sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    invoice_id = table.Column<long>(type: "bigint", nullable: true),
                    invoiced_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_sessions", x => x.id);
                    table.CheckConstraint("ck_work_session_end_after_start", "ended_at IS NULL OR ended_at >= started_at");
                    table.ForeignKey(
                        name: "FK_work_sessions_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_work_sessions_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_work_sessions_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "Manager", "ADMIN" },
                    { 2L, "USER", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_address_type",
                table: "addresses",
                column: "address_type");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_country_postal_code_city",
                table: "addresses",
                columns: new[] { "country", "postal_code", "city" });

            migrationBuilder.CreateIndex(
                name: "IX_customers_address_id",
                table: "customers",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_company_name",
                table: "customers",
                column: "company_name");

            migrationBuilder.CreateIndex(
                name: "IX_customers_contact_person_id",
                table: "customers",
                column: "contact_person_id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_status",
                table: "customers",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_customers_tax_id",
                table: "customers",
                column: "tax_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_person_id",
                table: "employees",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_status",
                table: "employees",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_labor_items_invoice_id",
                table: "invoice_labor_items",
                column: "invoice_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoice_material_items_invoice_id",
                table: "invoice_material_items",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_material_items_invoice_id_material_id",
                table: "invoice_material_items",
                columns: new[] { "invoice_id", "material_id" });

            migrationBuilder.CreateIndex(
                name: "IX_invoice_material_items_material_id",
                table: "invoice_material_items",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_created_by_user_id",
                table: "invoices",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_customer_id",
                table: "invoices",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_customer_id_issue_date",
                table: "invoices",
                columns: new[] { "customer_id", "issue_date" });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_number",
                table: "invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoices_issue_date",
                table: "invoices",
                column: "issue_date");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_project_id",
                table: "invoices",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_status",
                table: "invoices",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_attachments_article_id",
                table: "knowledge_article_attachments",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_attachments_article_id_sort_order",
                table: "knowledge_article_attachments",
                columns: new[] { "article_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_favorites_article_id_user_id",
                table: "knowledge_article_favorites",
                columns: new[] { "article_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_favorites_user_id",
                table: "knowledge_article_favorites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_article_id_user_id",
                table: "knowledge_article_reads",
                columns: new[] { "article_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_read_at",
                table: "knowledge_article_reads",
                column: "read_at");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_user_id",
                table: "knowledge_article_reads",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_category_id",
                table: "knowledge_articles",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_created_at",
                table: "knowledge_articles",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_created_by_user_id",
                table: "knowledge_articles",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_status",
                table: "knowledge_articles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_categories_name",
                table: "knowledge_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_low_stock_notes_created_at",
                table: "low_stock_notes",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_low_stock_notes_created_by_employee_id",
                table: "low_stock_notes",
                column: "created_by_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_low_stock_notes_created_by_employee_id_created_at",
                table: "low_stock_notes",
                columns: new[] { "created_by_employee_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_low_stock_notes_priority",
                table: "low_stock_notes",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "IX_material_categories_name",
                table: "material_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_created_at",
                table: "material_movements",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_created_by_user_id",
                table: "material_movements",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_invoice_id",
                table: "material_movements",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_invoiced_at",
                table: "material_movements",
                column: "invoiced_at");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_material_id",
                table: "material_movements",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_material_id_project_id",
                table: "material_movements",
                columns: new[] { "material_id", "project_id" });

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_movement_type",
                table: "material_movements",
                column: "movement_type");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_occurred_at",
                table: "material_movements",
                column: "occurred_at");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_operation_id",
                table: "material_movements",
                column: "operation_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_operation_id_material_id",
                table: "material_movements",
                columns: new[] { "operation_id", "material_id" });

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_project_id",
                table: "material_movements",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_materials_barcode",
                table: "materials",
                column: "barcode",
                unique: true,
                filter: "[barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_materials_category_id",
                table: "materials",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_materials_is_active",
                table: "materials",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_materials_name",
                table: "materials",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_person_identifiers_person_id",
                table: "person_identifiers",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_address_id",
                table: "persons",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_email",
                table: "persons",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_persons_last_name",
                table: "persons",
                column: "last_name");

            migrationBuilder.CreateIndex(
                name: "IX_project_access_employee_id",
                table: "project_access",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_access_project_id",
                table: "project_access",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_access_project_id_employee_id",
                table: "project_access",
                columns: new[] { "project_id", "employee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_created_by_employee_id",
                table: "project_attachments",
                column: "created_by_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_project_id",
                table: "project_attachments",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_project_id_created_at",
                table: "project_attachments",
                columns: new[] { "project_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_created_by_employee_id",
                table: "project_notes",
                column: "created_by_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_project_id",
                table: "project_notes",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_project_id_created_at",
                table: "project_notes",
                columns: new[] { "project_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_assigned_to_employee_id",
                table: "project_tasks",
                column: "assigned_to_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_created_by_employee_id",
                table: "project_tasks",
                column: "created_by_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_due_at",
                table: "project_tasks",
                column: "due_at");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_project_id",
                table: "project_tasks",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_project_id_status",
                table: "project_tasks",
                columns: new[] { "project_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_status",
                table: "project_tasks",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_projects_address_id",
                table: "projects",
                column: "address_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_projects_code",
                table: "projects",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_projects_created_by_employee_id",
                table: "projects",
                column: "created_by_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_customer_id",
                table: "projects",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_end_date",
                table: "projects",
                column: "end_date");

            migrationBuilder.CreateIndex(
                name: "IX_projects_name",
                table: "projects",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_projects_responsible_employee_id",
                table: "projects",
                column: "responsible_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_start_date",
                table: "projects",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "IX_projects_status",
                table: "projects",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_text_templates_channel",
                table: "text_templates",
                column: "channel");

            migrationBuilder.CreateIndex(
                name: "IX_text_templates_key_channel",
                table: "text_templates",
                columns: new[] { "key", "channel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_assigned_at",
                table: "tool_assignments",
                column: "assigned_at");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_created_by_user_id",
                table: "tool_assignments",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_employee_id",
                table: "tool_assignments",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_employee_id_returned_at",
                table: "tool_assignments",
                columns: new[] { "employee_id", "returned_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_returned_at",
                table: "tool_assignments",
                column: "returned_at");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_tool_id",
                table: "tool_assignments",
                column: "tool_id",
                unique: true,
                filter: "[returned_at] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tool_categories_name",
                table: "tool_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tool_categories_parent_id",
                table: "tool_categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tools_category_id",
                table: "tools",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tools_is_active",
                table: "tools",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_tools_name",
                table: "tools",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tools_serial_number",
                table: "tools",
                column: "serial_number",
                unique: true,
                filter: "[serial_number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_is_active",
                table: "users",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_person_id",
                table: "users",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_assignments_employee_id",
                table: "vehicle_assignments",
                column: "employee_id",
                unique: true,
                filter: "[returned_at] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_assignments_vehicle_id",
                table: "vehicle_assignments",
                column: "vehicle_id",
                unique: true,
                filter: "[returned_at] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_assignments_vehicle_id_assigned_at",
                table: "vehicle_assignments",
                columns: new[] { "vehicle_id", "assigned_at" });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_created_at",
                table: "vehicle_issues",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_reported_by_user_id",
                table: "vehicle_issues",
                column: "reported_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_status",
                table: "vehicle_issues",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_vehicle_id",
                table: "vehicle_issues",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_vehicle_id_status",
                table: "vehicle_issues",
                columns: new[] { "vehicle_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_plate_number",
                table: "vehicles",
                column: "plate_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_status",
                table: "vehicles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_vin",
                table: "vehicles",
                column: "vin",
                unique: true,
                filter: "[vin] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_employee_id",
                table: "work_sessions",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_employee_id_project_id_started_at",
                table: "work_sessions",
                columns: new[] { "employee_id", "project_id", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_invoice_id",
                table: "work_sessions",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_invoiced_at",
                table: "work_sessions",
                column: "invoiced_at");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_project_id",
                table: "work_sessions",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_started_at",
                table: "work_sessions",
                column: "started_at");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_status",
                table: "work_sessions",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoice_labor_items");

            migrationBuilder.DropTable(
                name: "invoice_material_items");

            migrationBuilder.DropTable(
                name: "knowledge_article_attachments");

            migrationBuilder.DropTable(
                name: "knowledge_article_favorites");

            migrationBuilder.DropTable(
                name: "knowledge_article_reads");

            migrationBuilder.DropTable(
                name: "low_stock_notes");

            migrationBuilder.DropTable(
                name: "material_movements");

            migrationBuilder.DropTable(
                name: "person_identifiers");

            migrationBuilder.DropTable(
                name: "project_access");

            migrationBuilder.DropTable(
                name: "project_attachments");

            migrationBuilder.DropTable(
                name: "project_notes");

            migrationBuilder.DropTable(
                name: "project_tasks");

            migrationBuilder.DropTable(
                name: "text_templates");

            migrationBuilder.DropTable(
                name: "tool_assignments");

            migrationBuilder.DropTable(
                name: "vehicle_assignments");

            migrationBuilder.DropTable(
                name: "vehicle_issues");

            migrationBuilder.DropTable(
                name: "work_sessions");

            migrationBuilder.DropTable(
                name: "knowledge_articles");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "tools");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "knowledge_categories");

            migrationBuilder.DropTable(
                name: "material_categories");

            migrationBuilder.DropTable(
                name: "tool_categories");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
